using RealEstate.Dto.Request;
using RealEstate.Dto.Response;

namespace RealEstate.Services
{
    public interface IListingsService
    {
        Task<ListingResponseDto> CreateTemporaryListingAsync(ListingRequestDto request, int userId);
        Task<ListingResponseDto> AddVipPackageToListingAsync(VipPackageSelectionDto request, int userId);
        Task<ListingResponseDto> UpdateListingAsync(int id, ListingRequestDto request, int userId);
        Task<bool> DeleteListingAsync(int id, int userId);
        Task<IEnumerable<ListingResponseDto>> GetAllAsync();
        Task<ListingResponseDto?> GetByIdAsync(int id);
    }
}