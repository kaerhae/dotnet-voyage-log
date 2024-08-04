using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_voyage_log.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{

    private readonly ILogger<LoginController> _logger;
    private readonly IUserService _service;

    public LoginController(ILogger<LoginController> logger, IUserService service)
    {
        _logger = logger;
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost("")]
    public IActionResult Post([FromBody] LoginUser user)
    {
        string token = _service.LoginUser(user);
        return Ok(token); 
    }

}
