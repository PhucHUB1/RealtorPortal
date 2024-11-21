using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Models;

[ApiController]
[Route("api/[controller]")]
public class ReferenceDataController : ControllerBase
{
    private readonly RealEstateContext _context;

    public ReferenceDataController(RealEstateContext context)
    {
        _context = context;
    }

    [AllowAnonymous]
    [HttpGet("cities")]
    public async Task<IActionResult> GetCities()
    {
        var cities = await _context.Cities.ToListAsync();
        return Ok(cities);
    }

    [AllowAnonymous]
    [HttpGet("districts")]
    public async Task<IActionResult> GetDistricts()
    {
        var districts = await _context.Districts.ToListAsync();
        return Ok(districts);
    }

    [AllowAnonymous]
    [HttpGet("wards")]
    public async Task<IActionResult> GetWards()
    {
        var wards = await _context.Wards.ToListAsync();
        return Ok(wards);
    }

    [Authorize]
    [HttpGet("juridicals")]
    public async Task<ActionResult<IEnumerable<Juridical>>> GetJuridicals()
    {
        var juridicals = await _context.Juridicals.ToListAsync();
        return Ok(juridicals);
    }

    [Authorize]
    [HttpGet("directions")]
    public async Task<ActionResult<IEnumerable<Direction>>> GetDirections()
    {
        var directions = await _context.Directions.ToListAsync();
        return Ok(directions);
    }

    [AllowAnonymous]
    [HttpGet("propertytypes")]
    public async Task<ActionResult<IEnumerable<PropertyType>>> GetPropertyTypes()
    {
        var propertyTypes = await _context.PropertyTypes.ToListAsync();
        return Ok(propertyTypes);
    }

    [Authorize]
    [HttpGet("VipPackage")]
    public async Task<ActionResult<IEnumerable<ListingsVipPackage>>> GetVipPackage()
    {
        var vipPackage = await _context.ListingsVipPackages.ToListAsync();
        return Ok(vipPackage);
    }

    // Get Districts by City
    [AllowAnonymous]
    [HttpGet("city/{cityId}/districts")]
    public async Task<IActionResult> GetDistrictsByCity(int cityId)
    {
        var city = await _context.Cities.Include(c => c.Districts)
                                        .FirstOrDefaultAsync(c => c.Id == cityId);
        if (city == null) return NotFound("City not found");

        var result = new
        {
            City = new { city.Id, city.Name },
            Districts = city.Districts.Select(d => new { d.Id, d.Name })
        };

        return Ok(result);
    }

    // Get Wards by District
    [AllowAnonymous]
    [HttpGet("district/{districtId}/wards")]
    public async Task<IActionResult> GetWardsByDistrict(int districtId)
    {
        var district = await _context.Districts.Include(d => d.Wards)
                                               .FirstOrDefaultAsync(d => d.Id == districtId);
        if (district == null) return NotFound("District not found");

        var result = new
        {
            District = new { district.Id, district.Name },
            Wards = district.Wards.Select(w => new { w.Id, w.Name })
        };

        return Ok(result);
    }

    // Get Districts and Wards by City
    [AllowAnonymous]
    [HttpGet("city/{cityId}/districts-and-wards")]
    public async Task<IActionResult> GetDistrictsAndWardsByCity(int cityId)
    {
        var city = await _context.Cities.Include(c => c.Districts)
                                        .ThenInclude(d => d.Wards)
                                        .FirstOrDefaultAsync(c => c.Id == cityId);
        if (city == null) return NotFound("City not found");

        var result = new
        {
            City = new { city.Id, city.Name },
            Districts = city.Districts.Select(d => new
            {
                District = new { d.Id, d.Name },
                Wards = d.Wards.Select(w => new { w.Id, w.Name })
            })
        };

        return Ok(result);
    }
}
