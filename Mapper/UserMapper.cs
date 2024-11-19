using RealEstate.Dto.Request;
using RealEstate.Dto.Response;
using RealEstate.Dto.Update;
using RealEstate.Models;

namespace RealEstate.Mapper;

public static class UserMapper
{
    public static UserResponseDto ToResponse(this User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            Name = user.Name,
            Phone = user.Phone,
            Avatar = user.Avatar,
            Status = user.Status,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }


    public static User ToEntity(this UserRequestDto request)
    {
        return new User
        {
            Email = request.Email,
            Username = request.Username,
            Password = request.Password,
            Name = request.Name,
            Phone = request.Phone,
            CreatedAt = DateTime.Now,
            Avatar = request.AvatarFile != null ? SaveAvatar(request.AvatarFile) : null
        };
    }
    public static void UpdateEntity(this UserUpdateDto request, User user)
    {
        user.Email = request.Email ?? user.Email;
        user.Username = request.Username ?? user.Username;
        user.Password = request.Password ?? user.Password;
        user.Name = request.Name ?? user.Name;
        user.Phone = request.Phone ?? user.Phone;
        user.Avatar = request.AvatarFile != null ? SaveAvatar(request.AvatarFile) : null;
        user.UpdatedAt = DateTime.Now;  
    }

    private static string SaveAvatar(IFormFile avatarFile)
    {
        if (avatarFile == null) return null;

   
        var uploadPath = Path.Combine("wwwroot", "uploads");
        var filePath = Path.Combine(uploadPath, avatarFile.FileName);

        if (!Directory.Exists(uploadPath))
        {
            Directory.CreateDirectory(uploadPath);
        }
        using var stream = new FileStream(filePath, FileMode.Create);
        avatarFile.CopyTo(stream);

        return filePath; 
    }

}
