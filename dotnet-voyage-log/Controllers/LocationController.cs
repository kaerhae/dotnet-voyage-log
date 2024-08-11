using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_voyage_log.Controllers;

[ApiController]
[Route("[controller]")]
public class LocationController : ControllerBase
{

    private readonly ILogger<LoginController> _logger;
    private readonly ILocationService _service;

    public LocationController(ILogger<LoginController> logger, ILocationService service)
    {
        _logger = logger;
        _service = service;
    }

    [AllowAnonymous]
    [HttpGet("Countries")]
    public List<Country> GetAllCountries()
    {
        return _service.GetAllCountries();
    }

    [AllowAnonymous]
    [HttpGet("Regions")]
    public List<Region> GetAllRegions()
    {
        return _service.GetAllRegions();
    }

    [AllowAnonymous]
    [HttpGet("Countries/{name}")]
    public Country GetCountry(string name)
    {
        return _service.GetSingleCountry(name);
    }

    [AllowAnonymous]
    [HttpGet("Regions/{name}")]
    public Region GetRegion(string name)
    {
        return _service.GetSingleRegion(name);
    }

}
