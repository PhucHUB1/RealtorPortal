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
            ImageUrls = listing.Images.Select(img => img.Url).ToList() 
        };
    }

    public static Listing ToEntity(this ListingRequestDto request)
    {
        return new Listing
        {
            UserId = request.UserId,
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
            VipPackageId = request.VipPackageId,
            VipExpiryDate = request.VipExpiryDate,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}