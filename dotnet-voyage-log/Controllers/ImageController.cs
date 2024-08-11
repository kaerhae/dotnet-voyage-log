using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_voyage_log.Controllers;

[ApiController]
[Authorize(Policy = "Admins")]
[Route("[controller]")]
//[NonController]
public class ImageController : ControllerBase
{

    private readonly ILogger<ImageController> _logger;
    private readonly IS3Service _service;

    public ImageController(ILogger<ImageController> logger, IS3Service service)
    {
        _logger = logger;
        _service = service;
    }

    [HttpGet("")]
    public async Task<List<S3ObjectData>> Get()
    {
        return await _service.GetImageList();
    }

    [HttpGet("{key}")]
    public async Task<S3ObjectData> GetSingle(string key)
    {
        return await _service.GetSingleImage(key);
    }


    [HttpPut("")]
    public async Task<ActionResult> Put([FromForm] Image images)
    {
        if(images.Files.Count == 0) {
            return BadRequest(new { message = "No files provided"});
        }
        foreach (IFormFile image in images.Files) {
            await _service.UploadImage(image);
        }
        return CreatedAtAction(nameof(Put), new { message = "Images uploaded succesfully"} );
    }

    [HttpDelete("{keys}")]
    public async Task<ActionResult> Delete(List<string> keys)
    {
        if(keys.Count == 0) {
            return BadRequest(new { message = "No keys provided"});
        }
        foreach(string key in keys) {
            await _service.DeleteImage(key);
        }

        return Ok(new { message = "Deleted succesfully"});
    }
}
