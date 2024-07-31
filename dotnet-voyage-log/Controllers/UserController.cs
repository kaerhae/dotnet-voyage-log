using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_voyage_log.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{

    private readonly ILogger<UserController> _logger;
    private readonly IUserService _service;

    public UserController(ILogger<UserController> logger, IUserService service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("")]
    public List<User> Get()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public User GetById(long id)
    {
        return _service.GetById(id);
    }

    [HttpPost("")]
    public IActionResult Post([FromBody] User newUser)
    {
        User u = _service.CreateUser(newUser);
        return Ok($"User {u.Username} successfully created");
    }

    [HttpPut("{id}")]
    public IActionResult Update(long id, [FromBody] User updatedUser) 
    {
        _service.UpdateUser(id, updatedUser);
        return Ok("User successfully updated");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(long id)
    {
        _service.DeleteUser(id);
        return Ok("User succesfully updated");
    }
}
