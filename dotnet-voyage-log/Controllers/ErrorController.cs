using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_voyage_log.Controllers;

[ApiController]
public class ErrorController : ControllerBase
{
    [Route("error")]
    public IActionResult Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
        var exception = context?.Error;


        return Problem(
            detail: exception?.Message,
            statusCode: 500,
            title: "An error occurred while processing request.");
    }

    [Route("error/{statusCode}")]
    public IActionResult ErrorHandlerByStatusCode(int statusCode)
    {
        string message = statusCode switch
        {
            404 => "Resource not found",
            401 => "Unauthorized",
            403 => "Forbidden",
            500 => "Internal server error",
            _ => "An unexpected error occurred"
        };

        return StatusCode(statusCode, new { error = message });
    }
}