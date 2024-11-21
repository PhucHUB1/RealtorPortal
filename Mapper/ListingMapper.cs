using RealEstate.Dto.Request;
using RealEstate.Dto.Response;
using RealEstate.Models;

namespace RealEstate.Mapper;

public static class ListingMapper
{
    public static ListingResponseDto ToResponse(this Listing listing)
    {
        return new ListingResponseDto
        {
            Id = listing.Id,
            UserId = listing.UserId,
            Title = listing.Title,
            Description = listing.Description,
            PropertyTypeId = listing.PropertyTypeId,
            Price = listing.Price,
            Area = listing.Area,
            Address = listing.Address,
            JuridicalId = listing.JuridicalId,
            DirectionId = listing.DirectionId,
            CityId = listing.CityId,
            DistrictId = listing.DistrictId,
            WardId = listing.WardId,
            VipPackageId = listing.VipPackageId,
            VipExpiryDate = listing.VipExpiryDate,
            Status = listing.Status,
            CreatedAt = listing.CreatedAt,
            UpdatedAt = listing.UpdatedAt,
            ImageUrls = listing.Images?.Select(img => img.Url).ToList() ?? new List<string>()
        };
    }

    public static Listing ToEntity(this ListingRequestDto request)
    {
        return new Listing
        {
            Title = request.Title,
            Description = request.Description,
            PropertyTypeId = request.PropertyTypeId,
            Price = request.Price,
            Area = request.Area,
            Address = request.Address,
            JuridicalId = request.JuridicalId,
            DirectionId = request.DirectionId,
            CityId = request.CityId,
            DistrictId = request.DistrictId,
            WardId = request.WardId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static void UpdateVipDetails(this Listing listing, int vipPackageId, DateTime vipExpiryDate)
    {
        listing.VipPackageId = vipPackageId;
        listing.VipExpiryDate = vipExpiryDate;
        listing.UpdatedAt = DateTime.UtcNow;
    }
}
