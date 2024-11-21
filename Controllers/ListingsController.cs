using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Dto.Request;
using RealEstate.Services;
using System.Security.Claims;

namespace RealEstate.Controllers;

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ListingsController : ControllerBase
    {
        private readonly IListingsService _listingService;

        public ListingsController(IListingsService listingService)
        {
            _listingService = listingService;
        }

        // Tạo Listing tạm thời
        [Authorize]
        [HttpPost("create")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreateTemporaryListing([FromForm] ListingRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User not authenticated." });
            }

            var listing = await _listingService.CreateTemporaryListingAsync(request, int.Parse(userId));
            return CreatedAtAction(nameof(GetById), new { id = listing.Id }, listing);
        }


      
        [Authorize]
        [HttpPost("select-vip")]
        [Consumes("application/json")] 
        public async Task<IActionResult> SelectVipPackage([FromBody] VipPackageSelectionDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User not authenticated." });
            }

            try
            {
                var updatedListing = await _listingService.AddVipPackageToListingAsync(request, int.Parse(userId));
                return Ok(updatedListing);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        

     
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var listings = await _listingService.GetAllAsync();
            return Ok(listings);
        }

       
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var listing = await _listingService.GetByIdAsync(id);
            if (listing == null)
            {
                return NotFound(new { Message = "Listing not found." });
            }

            return Ok(listing);
        }
        
        // Update Listing
        [Authorize]
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateListing(int id, [FromForm] ListingRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User not authenticated." });
            }

            try
            {
                var updatedListing = await _listingService.UpdateListingAsync(id, request, int.Parse(userId));
                return Ok(updatedListing);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        // Delete Listing
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteListing(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "User not authenticated." });
            }

            try
            {
                var success = await _listingService.DeleteListingAsync(id, int.Parse(userId));
                if (!success)
                {
                    return NotFound(new { Message = "Listing not found." });
                }

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }

