using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstate.Models;

namespace RealEstate.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReferenceDataController(RealEstateContext context) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("cities")]
    public async Task<IActionResult> GetCities()
    {
        var cities = await context.Cities.ToListAsync();
        return Ok(cities);
    }
    [AllowAnonymous]
    [HttpGet("districts")]
    public async Task<IActionResult> GetDistricts()
    {
        var districts = await context.Districts.ToListAsync();
        return Ok(districts);
    }
    [AllowAnonymous]
    [HttpGet("wards")]
    public async Task<IActionResult> GetWards()
    {
        var wards = await context.Wards.ToListAsync();
        return Ok(wards);
    }
    [Authorize]
    [HttpGet("juridicals")]
    public async Task<ActionResult<IEnumerable<Juridical>>> GetJuridicals()
    {
        var juridicals = await context.Juridicals.ToListAsync();
        return Ok(juridicals);
    }
    [Authorize]
    [HttpGet("directions")]
    public async Task<ActionResult<IEnumerable<Direction>>> GetDirections()
    {
        var directions = await context.Directions.ToListAsync();
        return Ok(directions);
    }

    [AllowAnonymous]
    [HttpGet("propertytypes")]
    public async Task<ActionResult<IEnumerable<PropertyType>>> GetPropertyTypes()
    {
        var propertyTypes = await context.PropertyTypes.ToListAsync();
        return Ok(propertyTypes);
    }
    
    [Authorize]
    [HttpGet("VipPackage")]
    public async Task<ActionResult<IEnumerable<ListingsVipPackage>>> GetVipPackage()
    {
        var vipPackage = await context.ListingsVipPackages.ToListAsync();
        return Ok(vipPackage);
    }
}
