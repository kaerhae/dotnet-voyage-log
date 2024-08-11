
using Amazon.S3;
using Amazon.S3.Model;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace dotnet_voyage_log.Tests.Service;

public class S3ServiceTests
{
    private IS3Service _service;

    private Mock<IAmazonS3> _client;
    private Mock<ILogger<IS3Service>> _logger;

    public S3ServiceTests()
    {
        _client = new Mock<IAmazonS3>();
        _logger = new Mock<ILogger<IS3Service>>();
        _service = new S3Service(_client.Object, "bucket", _logger.Object);
    }

    [Fact]
    public async void GetImageList_ShouldReturnImages()
    {
        List<S3Object> data = new List<S3Object>(){
            new S3Object(){ Key="Test" },
            new S3Object(){ Key="Test2" },
        };
        var mockRequest = new ListObjectsV2Request();
        var mockSignRequest = new GetPreSignedUrlRequest() {
            BucketName = "voyage-bucket",
            Key = "Test",
            Expires = DateTime.UtcNow.AddMinutes(30),
        };
        var mockS3Object = new ListObjectsV2Response
        {
            S3Objects = data
        };
        _client.Setup(client => client.ListObjectsV2Async(
                It.IsAny<ListObjectsV2Request>(),
                It.IsAny<CancellationToken>()
            )).Callback<ListObjectsV2Request, CancellationToken>((request, token) =>
            {
                if (!String.IsNullOrEmpty(request.BucketName))
                {
                    Assert.Equal("bucket", request.BucketName);
                }
            }).Returns((ListObjectsV2Request r, CancellationToken token) =>
            {
                return Task.FromResult(new ListObjectsV2Response()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    S3Objects = data
                });
            });

        _client.Setup(x => x.GetPreSignedURL(mockSignRequest)).Returns("signedUrl");

        var result = await _service.GetImageList();
        Assert.Equal("Test", result[0].Name);
        Assert.Equal("Test2", result[1].Name);
    }

    [Fact]
    public async void UploadImage_ShouldThrowExceptionIfFileIsNull()
    {
        _client.Setup(client => client.PutObjectAsync(
                It.IsAny<PutObjectRequest>(),
                It.IsAny<CancellationToken>()
            )).Callback<PutObjectRequest, CancellationToken>((request, token) =>
            {
                if (!String.IsNullOrEmpty(request.BucketName))
                {
                    Assert.Equal("bucket", request.BucketName);
                }
            }).Returns((PutObjectRequest r, CancellationToken token) =>
            {
                return Task.FromResult(new PutObjectResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK
                });
            });

        var exception = await Assert.ThrowsAsync<FileNotFoundException>(async() => await _service.UploadImage(null));
        Assert.Equal("Unable to find the specified file.", exception.Message);
    }


    [Fact]
    public async void GetSingleImage_ShouldReturnImage()
    {
        var mockSignRequest = new GetPreSignedUrlRequest() {
            BucketName = "bucket",
            Key = "Test",
            Expires = DateTime.UtcNow.AddMinutes(30),
        };

        _client.Setup(client => client.GetObjectAsync(
                It.IsAny<GetObjectRequest>(),
                It.IsAny<CancellationToken>()
            )).Callback<GetObjectRequest, CancellationToken>((request, token) =>
            {
                if (!String.IsNullOrEmpty(request.BucketName))
                {
                    Assert.Equal("bucket", request.BucketName);
                    Assert.Equal("Test", request.Key);
                }
            }).Returns((GetObjectRequest r, CancellationToken token) =>
            {
                return Task.FromResult(new GetObjectResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Key = "Test"
                });
            });

        _client.Setup(x => x.GetPreSignedURL(mockSignRequest)).Returns("signedUrl");

        var result = await _service.GetSingleImage("Test");
        Assert.Equal("Test", result.Name);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void GetSingleImage_ShouldThrowErrorIfKeyNull(string objectKey)
    {
        var mockSignRequest = new GetPreSignedUrlRequest() {
            BucketName = "bucket",
            Key = "Test",
            Expires = DateTime.UtcNow.AddMinutes(30),
        };

        _client.Setup(client => client.GetObjectAsync(
                It.IsAny<GetObjectRequest>(),
                It.IsAny<CancellationToken>()
            )).Callback<GetObjectRequest, CancellationToken>((request, token) =>
            {
                if (!String.IsNullOrEmpty(request.BucketName))
                {
                    Assert.Equal("bucket", request.BucketName);
                    Assert.Equal("Test", request.Key);
                }
            }).Returns((GetObjectRequest r, CancellationToken token) =>
            {
                return Task.FromResult(new GetObjectResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK,
                    Key = "Test"
                });
            });

        _client.Setup(x => x.GetPreSignedURL(mockSignRequest)).Returns("signedUrl");

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.GetSingleImage(objectKey));
        Assert.Equal("Value cannot be null.", exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async void DeleteImage_ShouldThrowExceptionIfKeyIsNull(string objectId)
    {
        _client.Setup(client => client.PutObjectAsync(
                It.IsAny<PutObjectRequest>(),
                It.IsAny<CancellationToken>()
            )).Callback<PutObjectRequest, CancellationToken>((request, token) =>
            {
                if (!String.IsNullOrEmpty(request.BucketName))
                {
                    Assert.Equal("bucket", request.BucketName);
                }
            }).Returns((PutObjectRequest r, CancellationToken token) =>
            {
                return Task.FromResult(new PutObjectResponse()
                {
                    HttpStatusCode = System.Net.HttpStatusCode.OK
                });
            });

        var exception = await Assert.ThrowsAsync<ArgumentNullException>(async() => await _service.DeleteImage(objectId));
        Assert.Equal("Value cannot be null.", exception.Message);
    }
}