using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_voyage_log.Controllers;

[ApiController]
[Route("[controller]")]
public class SignUpController : ControllerBase
{

    private readonly ILogger<LoginController> _logger;
    private readonly IUserService _service;

    public SignUpController(ILogger<LoginController> logger, IUserService service)
    {
        _logger = logger;
        _service = service;
    }

    [AllowAnonymous]
    [HttpPost("")]
    public IActionResult Post([FromBody] SignupUser user)
    {
        User u = _service.CreateNormalUser(user);
        return Ok($"User {u.Username} successfully created");
    }

}
