using System.Security.Claims;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_voyage_log.Controllers;

[ApiController]
[Authorize(Policy = "AllUsers")]
[Route("[controller]")]
public class VoyageController : ControllerBase
{

    private readonly ILogger<VoyageController> _logger;
    private readonly IVoyageService _service;

    public VoyageController(ILogger<VoyageController> logger, IVoyageService service) {
        _logger = logger;
        _service = service;
    }

    [HttpGet("")]
    public List<Voyage> Get() {
        return _service.GetAll();
    }

    [HttpPost("")]
    public IActionResult Create([FromBody] Voyage newVoyage) {
        string id = GetUserId();
        _service.CreateVoyage(Int64.Parse(id), newVoyage);
        return Ok(new { message = "Voyage created" });
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] Voyage updatedVoyage) {
        string userId = GetUserId();
        _service.UpdateVoyage(Int64.Parse(userId), id, updatedVoyage);
        return Ok(new { message = "Voyage updated"});
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id) {
        string userId = GetUserId();
        _service.DeleteVoyage(Int64.Parse(userId), id);
        return Ok(new { message = "Voyage deleted"});
    }

    [NonAction]
    private string GetUserId() {
        string? id = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id == null) {
            throw new UnauthorizedAccessException();
        }

        return id;

    }

}