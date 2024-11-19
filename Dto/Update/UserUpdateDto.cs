

namespace RealEstate.Dto.Update;

public class UserUpdateDto {
    
    public string? Email { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

  

    public string? Phone { get; set; }

    public IFormFile? AvatarFile { get; set; }
    

}

