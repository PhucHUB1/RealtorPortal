using Microsoft.EntityFrameworkCore;
using RealEstate.Dto.Request;
using RealEstate.Dto.Response;
using RealEstate.Mapper;
using RealEstate.Models;

namespace RealEstate.Services.Implement;

public class ListingsService(RealEstateContext context) : IListingsService
{
    public async Task<IEnumerable<ListingResponseDto>> GetAllAsync()
    {
        var listings = await context.Listings.ToListAsync();
        return listings.Select(l => l.ToResponse());
    }

    public async Task<ListingResponseDto> CreateAsync(ListingRequestDto request)
    {
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            var listing = request.ToEntity();
            context.Listings.Add(listing);
            await context.SaveChangesAsync();
            
            if (request.Images != null && request.Images.Any())
            {
                var imageEntities = new List<Image>();

                foreach (var image in request.Images)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                    var filePath = Path.Combine("wwwroot/images", fileName);


                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }

                  
                    imageEntities.Add(new Image
                    {
                        ListingId = listing.Id,
                        Url = $"/images/{fileName}",
                        CreatedAt = DateTime.UtcNow
                    });
                }

                context.Images.AddRange(imageEntities);
                await context.SaveChangesAsync();
            }

            await transaction.CommitAsync();
            return listing.ToResponse();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<ListingResponseDto> GetByIdAsync(int id)
    {
        var listing = await context.Listings
            .Include(l => l.Images) 
            .FirstOrDefaultAsync(l => l.Id == id);

        if (listing == null)
            throw new KeyNotFoundException($"Listing with ID {id} not found.");

        return listing.ToResponse();
    }


    public async Task<ListingResponseDto> UpdateAsync(int id, ListingRequestDto request)
    {
        var listing = await context.Listings.FindAsync(id);
        if (listing == null) throw new KeyNotFoundException($"Listing with ID {id} not found.");

        listing.Title = request.Title ?? listing.Title;
        listing.Description = request.Description ?? listing.Description;
        listing.Price = request.Price != 0 ? request.Price : listing.Price;
        listing.Area = request.Area != 0 ? request.Area : listing.Area;
        listing.Address = request.Address ?? listing.Address;
        listing.UpdatedAt = DateTime.Now;

        context.Listings.Update(listing);
        await context.SaveChangesAsync();
        return listing.ToResponse();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var listing = await context.Listings.FindAsync(id);
        if (listing == null) return false;

        context.Listings.Remove(listing);
        await context.SaveChangesAsync();
        return true;
    }
}