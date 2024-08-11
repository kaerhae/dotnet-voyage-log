using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Interfaces;

public interface IS3Service {
    Task<List<S3ObjectData>> GetImageList();
    Task UploadImage(IFormFile? file);
    Task<S3ObjectData> GetSingleImage(string key);
    Task DeleteImage(string key);
}