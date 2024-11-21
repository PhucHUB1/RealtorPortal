using Microsoft.EntityFrameworkCore;
using RealEstate.Dto.Request;
using RealEstate.Dto.Response;
using RealEstate.Mapper;
using RealEstate.Models;

namespace RealEstate.Services.Implement
{
    public class ListingsService : IListingsService
    {
        private readonly RealEstateContext _context;

        public ListingsService(RealEstateContext context)
        {
            _context = context;
        }

        // Tạo Listing tạm thời
        public async Task<ListingResponseDto> CreateTemporaryListingAsync(ListingRequestDto request, int userId)
        {
            var listing = request.ToEntity();
            listing.UserId = userId;
            listing.CreatedAt = DateTime.UtcNow;
            listing.UpdatedAt = DateTime.UtcNow;

            // Lưu Listing vào DB
            _context.Listings.Add(listing);
            await _context.SaveChangesAsync();

            // Nếu có ảnh, xử lý lưu ảnh
            if (request.Images != null && request.Images.Any())
            {
                var imageEntities = await SaveImagesAsync(request.Images, listing.Id);
                _context.Images.AddRange(imageEntities);
                await _context.SaveChangesAsync();
            }

            return listing.ToResponse();
        }

        // Cập nhật Listing
        public async Task<ListingResponseDto> UpdateListingAsync(int id, ListingRequestDto request, int userId)
        {
            var listing = await _context.Listings.Include(l => l.Images).FirstOrDefaultAsync(l => l.Id == id);

            if (listing == null || listing.UserId != userId)
            {
                throw new UnauthorizedAccessException("Listing not found or unauthorized.");
            }

            // Cập nhật thông tin cơ bản
            listing.Title = request.Title ?? listing.Title;
            listing.Description = request.Description ?? listing.Description;
            listing.PropertyTypeId = request.PropertyTypeId != 0 ? request.PropertyTypeId : listing.PropertyTypeId;
            listing.Price = request.Price != 0 ? request.Price : listing.Price;
            listing.Area = request.Area != 0 ? request.Area : listing.Area;
            listing.Address = request.Address ?? listing.Address;
            listing.CityId = request.CityId != 0 ? request.CityId : listing.CityId;
            listing.DistrictId = request.DistrictId != 0 ? request.DistrictId : listing.DistrictId;
            listing.WardId = request.WardId ?? listing.WardId;
            listing.UpdatedAt = DateTime.UtcNow;

            // Nếu có ảnh mới, thêm ảnh vào Listing
            if (request.Images != null && request.Images.Any())
            {
                var imageEntities = await SaveImagesAsync(request.Images, listing.Id);
                _context.Images.AddRange(imageEntities);
            }

            await _context.SaveChangesAsync();

            return listing.ToResponse();
        }
    
        // Gán gói VIP cho Listing
        public async Task<ListingResponseDto> AddVipPackageToListingAsync(VipPackageSelectionDto request, int userId)
        {
            var listing = await _context.Listings.FindAsync(request.ListingId);
            if (listing == null || listing.UserId != userId)
            {
                throw new UnauthorizedAccessException("Listing not found or unauthorized.");
            }

            var vipPackage = await _context.ListingsVipPackages.FindAsync(request.VipPackageId);
            if (vipPackage == null)
            {
                throw new KeyNotFoundException("VIP package not found.");
            }

            listing.VipPackageId = request.VipPackageId;
            listing.VipExpiryDate = listing.CreatedAt?.AddDays(vipPackage.DurationDays);
            listing.UpdatedAt = DateTime.UtcNow;

            _context.Listings.Update(listing);
            await _context.SaveChangesAsync();

            return listing.ToResponse();
        }
    
        public async Task<IEnumerable<ListingResponseDto>> GetAllAsync()
        {
            var listings = await _context.Listings
                .Where(l => l.VipPackageId != null && l.VipExpiryDate > DateTime.UtcNow)
                .Include(l => l.Images) 
                .ToListAsync();

            return listings.Select(l => l.ToResponse());
        }

     
        public async Task<ListingResponseDto?> GetByIdAsync(int id)
        {
            var listing = await _context.Listings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.Id == id && l.VipPackageId != null && l.VipExpiryDate > DateTime.UtcNow);

            return listing?.ToResponse();
        }

        // Xóa Listing
        public async Task<bool> DeleteListingAsync(int id, int userId)
        {
            var listing = await _context.Listings.FindAsync(id);

            if (listing == null || listing.UserId != userId)
            {
                throw new UnauthorizedAccessException("Listing not found or unauthorized.");
            }

            // Xóa Listing
            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync();

            return true;
        }

        // Lưu ảnh vào thư mục và cơ sở dữ liệu
        private async Task<List<Image>> SaveImagesAsync(List<IFormFile> images, int listingId)
        {
            var imageEntities = new List<Image>();

            foreach (var image in images)
            {
                // Đặt tên file duy nhất
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
                var filePath = Path.Combine("wwwroot/images", fileName);

                // Lưu file vào thư mục
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Thêm thông tin vào Image entity
                imageEntities.Add(new Image
                {
                    ListingId = listingId,
                    Url = $"/images/{fileName}",
                    CreatedAt = DateTime.UtcNow
                });
            }

            return imageEntities;
        }
    }
}
