using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Dto.Request;
using RealEstate.Dto.Response;
using RealEstate.Dto.Update;
using RealEstate.Mapper;
using RealEstate.Models;

namespace RealEstate.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly RealEstateContext _context;
        private readonly string _jwtSecret;

        public UserService(RealEstateContext context, IConfiguration config)
        {
            _context = context;
            _jwtSecret = config["Jwt:Secret"];
        }
        
        public async Task<AuthResponseDto?> AuthenticateAsync(UserLoginDto loginDto)
        {
            var user = await _context.Users
                .Include(u => u.Role) 
                .FirstOrDefaultAsync(u =>
                    u.Username == loginDto.UsernameOrEmail || u.Email == loginDto.UsernameOrEmail);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role.Name)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthResponseDto
            {
                Token = tokenHandler.WriteToken(token),
                Username = user.Username
            };
        }

        // Get all users (Admin only)
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
        {
            var users = await _context.Users
                .Include(u => u.Role)
                .ToListAsync();

            return users.Select(u => u.ToResponse());
        }

        // Get a single user by ID
        public async Task<UserResponseDto> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            return user.ToResponse();
        }

        // Create a new user (AllowAnonymous)
        [AllowAnonymous]
        public async Task<UserResponseDto> CreateAsync(UserRequestDto request)
        {
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

            if (existingUser != null)
                throw new InvalidOperationException($"Username or Email is already taken.");

            var userEntity = request.ToEntity();
            userEntity.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Assign default role
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User")
                              ?? new Role { Name = "User", CreatedAt = DateTime.Now };

            userEntity.RoleId = defaultRole.Id;

            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.ToResponse();
        }

        // Update existing user
        public async Task<UserResponseDto> UpdateAsync(int id, UserUpdateDto request)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {id} not found.");

            user.Email = request.Email ?? user.Email;
            user.Username = request.Username ?? user.Username;

            if (!string.IsNullOrWhiteSpace(request.Password))
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);

            user.Name = request.Name ?? user.Name;
            user.Phone = request.Phone ?? user.Phone;
            user.UpdatedAt = DateTime.UtcNow;

            if (request.AvatarFile != null)
                user.Avatar = request.AvatarFile.FileName;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return user.ToResponse();
        }

        // Delete a user (Admin only)
        [Authorize(Roles = "Admin")]
        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
