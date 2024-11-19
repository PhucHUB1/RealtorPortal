using RealEstate.Dto.Request;
using RealEstate.Dto.Response;
using RealEstate.Dto.Update;

namespace RealEstate.Services;

public interface IUserService
{
    Task<AuthResponseDto?> AuthenticateAsync(UserLoginDto loginDto);
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto> GetByIdAsync(int id);
    Task<UserResponseDto> CreateAsync(UserRequestDto request);
    Task<UserResponseDto> UpdateAsync(int id, UserUpdateDto request);
    Task<bool> DeleteAsync(int id);
}
