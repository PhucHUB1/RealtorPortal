namespace RealEstate.Dto.Request;

public class ListingRequestDto
{
    public int? UserId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int PropertyTypeId { get; set; }
    public decimal Price { get; set; }
    public double Area { get; set; }
    public string? Address { get; set; }
    public int? JuridicalId { get; set; }
    public int? DirectionId { get; set; }
    public int CityId { get; set; }
    public int DistrictId { get; set; }
    public int? WardId { get; set; }
    public int? VipPackageId { get; set; }
    public DateOnly? VipExpiryDate { get; set; }
    public List<IFormFile>? Images { get; set; } 
}