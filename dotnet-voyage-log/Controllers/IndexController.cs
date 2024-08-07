using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_voyage_log.Controllers;

[ApiController]
[Route("")]
public class IndexController : ControllerBase
{
    [HttpGet("")]
    public string Get(){
        return "Server up and running";
    }
}