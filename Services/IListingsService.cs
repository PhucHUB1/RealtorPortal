using RealEstate.Dto.Request;
using RealEstate.Dto.Response;

namespace RealEstate.Services;

public interface IListingsService
{
    Task<IEnumerable<ListingResponseDto>> GetAllAsync();
    Task<ListingResponseDto> GetByIdAsync(int id);
    Task<ListingResponseDto> CreateAsync(ListingRequestDto request);
    Task<ListingResponseDto> UpdateAsync(int id, ListingRequestDto request);
    Task<bool> DeleteAsync(int id);
}