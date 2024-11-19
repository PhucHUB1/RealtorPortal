using System.ComponentModel.DataAnnotations;


namespace RealEstate.Dto.Request;

public class UserRequestDto
{
    [Required]
    public string? Email { get; set; }  
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
    public string? Name { get; set; }
    public string? Phone { get; set; }
    public IFormFile? AvatarFile { get; set; }
}
