using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Dto.Request;
using RealEstate.Services;

namespace RealEstate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingsController(IListingsService listingService) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var listings = await listingService.GetAllAsync();
        return Ok(listings);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var listing = await listingService.GetByIdAsync(id);
            return Ok(listing);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpPost]
    [Consumes("multipart/form-data")] 
    public async Task<IActionResult> CreateListing([FromForm] ListingRequestDto request)
    {
        try
        {
            var listing = await listingService.CreateAsync(request);
            return CreatedAtAction(nameof(CreateListing), new { id = listing.Id }, listing);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ListingRequestDto request)
    {
        try
        {
            var updatedListing = await listingService.UpdateAsync(id, request);
            return Ok(updatedListing);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await listingService.DeleteAsync(id);
        if (!result) return NotFound($"Listing with ID {id} not found.");
        return NoContent();
    }
}
