using Amazon.S3;
using Amazon.S3.Model;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Service;

public class S3Service : IS3Service {
    private IAmazonS3 _client;
    private string _bucketName;
    private readonly ILogger<IS3Service> _logger;

    public S3Service(IAmazonS3 client, string bucketName, ILogger<IS3Service> logger){
        _client = client;
        _bucketName = bucketName;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves S3ObjectData list from S3 storage. 
    /// Function retrieves images and returns image name and pre-signed URL
    /// </summary>
    /// <returns>
    /// string: List<S3ObjectData>
    /// </returns>
    public async Task<List<S3ObjectData>> GetImageList(){
        var request = new ListObjectsV2Request()
        {
            BucketName = _bucketName,
        };

        var result = await _client.ListObjectsV2Async(request);
        List<S3ObjectData> s3Objects = result.S3Objects.Select(s => {
             
            return new S3ObjectData()
            {
                Name = s.Key.ToString(),
                Url = GenerateUrl(_bucketName, s.Key, DateTime.UtcNow.AddMinutes(30)),
            };
        }).ToList();

        return s3Objects;

    }

    /// <summary>
    /// Uploads IFormFile to S3 storage.
    /// </summary>
    /// <exception cref="FileNotFoundException">
    /// </exception>
    public async Task UploadImage(IFormFile? file)
    {
        if (file == null) {
            throw new FileNotFoundException();
        }
        var fileName = $"voyage-image-{new Random().Next(100)}.jpg";
        try {
            using var newMemoryStream = new MemoryStream();
            file.CopyTo(newMemoryStream);
            var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = newMemoryStream
                };
            await _client.PutObjectAsync(request);
            _logger.LogInformation($"Image uploaded with id: {fileName}");
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    /// <summary>
    /// Retrieves single image by key from S3 storage
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// </exception>
    public async Task<S3ObjectData> GetSingleImage(string key)
    {
        if (key == null || key == "") {
            throw new ArgumentNullException();
        }
        var request = new GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = key,
                };
        try {
            var result = await _client.GetObjectAsync(request);
            if (result.HttpStatusCode == System.Net.HttpStatusCode.NotFound) {
                throw new FileNotFoundException();
            }

            return new S3ObjectData()
            {
                Name = result.Key.ToString(),
                Url = GenerateUrl(_bucketName, result.Key, DateTime.UtcNow.AddMinutes(30)),
            };
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    /// <summary>
    /// Deletes object by key from S3 storage.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// </exception>
    public async Task DeleteImage(string key)
    {
        if(key == null || key == "") {
            throw new ArgumentNullException();
        }

        var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
            };

        try {
            await _client.DeleteObjectAsync(request);
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }
    /// <summary>
    /// Generates signed URL by bucket name, key, and expiration date
    /// </summary>
    /// <returns>
    /// string: Generated url
    /// </returns>
    private string GenerateUrl(string bucketName, string key, DateTime expiresAt) {
        var urlRequest = new GetPreSignedUrlRequest() {
                BucketName = bucketName,
                Key = key,
                Expires = expiresAt,
            };

        return _client.GetPreSignedURL(urlRequest);
    }

}