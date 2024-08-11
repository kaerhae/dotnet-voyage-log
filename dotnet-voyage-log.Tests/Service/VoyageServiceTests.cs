
using System.Reflection;
using Castle.Core.Logging;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using dotnet_voyage_log.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace dotnet_voyage_log.Tests.Service;

public class VoyageServiceTests
{
    private IVoyageService _service;

    private Mock<IVoyageRepository> _repository;
    private Mock<IUserRepository> _userRepository;
    private Mock<IAuthentication> _auth;
    private Mock<IS3Service> _s3Service;
    private Mock<ILogger<IVoyageService>> _logger;

    public VoyageServiceTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _repository = new Mock<IVoyageRepository>();
        _auth = new Mock<IAuthentication>();
        _logger = new Mock<ILogger<IVoyageService>>();
        _s3Service = new Mock<IS3Service>();
        _service = new VoyageService(_repository.Object, _auth.Object, _s3Service.Object, _logger.Object);
    }

    [Fact]
    public void GetAllCountries_ShouldReturnCountries()
    {
        List<Voyage> voyages = new List<Voyage>(){
            new Voyage { Topic = "Test", UserId = 1, RegionId = 1}
        };
        _repository.Setup(x => x.GetAll()).Returns(voyages);

        var result = _service.GetAll();

        Assert.Equal(voyages[0].Id, result[0].Id);
        Assert.Equal(voyages[0].Topic, result[0].Topic);
        Assert.Equal(voyages[0].UserId, result[0].UserId);
        Assert.Equal(voyages[0].RegionId, result[0].RegionId);
    }

    [Fact]
    public async void CreateVoyage_ShouldBeOk()
    {
        VoyageWithFiles vf = new VoyageWithFiles(){
            Topic = "Test",
            UserId = 1,
            RegionId = 1,
        };

        Voyage v = new Voyage(){
            Topic = "Test",
            UserId = 1,
            RegionId = 1
        };

        var content = "test content";
        var fileName = "test.pdf";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        IFormFile mockFile = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

        _s3Service.Setup(x => x.UploadImage(mockFile)).ReturnsAsync("key");
        _repository.Setup(x => x.CreateVoyage(v));

        Voyage result = await _service.CreateVoyage(1, vf);

        Assert.Equal(v.Topic, result.Topic);
        Assert.Equal(v.UserId, result.UserId);
        Assert.Equal(v.RegionId, result.RegionId);

    }

    [Fact]
    public async void CreateVoyage_ShouldBeOkWithEmptyImages()
    {
        VoyageWithFiles vf = new VoyageWithFiles(){
            Topic = "Test",
            UserId = 1,
            RegionId = 1,
            Images = []
        };

        Voyage v = new Voyage(){
            Topic = "Test",
            UserId = 1,
            RegionId = 1
        };

        _repository.Setup(x => x.CreateVoyage(v));

        Voyage result = await _service.CreateVoyage(1, vf);

        Assert.Equal(v.Topic, result.Topic);
        Assert.Equal(v.UserId, result.UserId);
        Assert.Equal(v.RegionId, result.RegionId);

    }

    [Fact]
    public async void CreateVoyage_ShouldBeOkWithImageList()
    {
        var content = "test content";
        var fileName = "test.jpg";
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(content);
        writer.Flush();
        stream.Position = 0;

        IFormFile mockFile = new FormFile(stream, 0, stream.Length, "test", fileName);

        VoyageWithFiles vf = new VoyageWithFiles(){
            Topic = "Test",
            UserId = 1,
            RegionId = 1,
            Images = [mockFile]
        };

        Voyage v = new Voyage(){
            Topic = "Test",
            UserId = 1,
            RegionId = 1
        };

        _repository.Setup(x => x.CreateVoyage(v));
        _s3Service.Setup(x => x.UploadImage(mockFile)).ReturnsAsync("key");

        Voyage result = await _service.CreateVoyage(1, vf);

        Assert.Equal(v.Topic, result.Topic);
        Assert.Equal(v.UserId, result.UserId);
        Assert.Equal(v.RegionId, result.RegionId);
        Assert.Equal(new List<string>(){ "key" }, result.Images);

    }

    [Theory]
    [InlineData("", 1)]
    [InlineData("foo", 0)]

    public async void CreateVoyage_ShouldThrowErrorWhenInvalidVoyage(string a, long b)
    {
        VoyageWithFiles v = new VoyageWithFiles(){
            Topic = a,
            RegionId = b,

        };

        var exception = await Assert.ThrowsAsync<Exception>(async () => await _service.CreateVoyage(1, v));

        Assert.Equal("Malformatted voyage", exception.Message);
    }

    [Fact]
    public async void CreateVoyage_ShouldThrowErrorWhenInvalidUserId()
    {
        VoyageWithFiles v = new VoyageWithFiles(){
            Topic = "foo",
            RegionId = 1,

        };

        var exception = await Assert.ThrowsAsync<Exception>(async () => await _service.CreateVoyage(0, v));

        Assert.Equal("Malformatted voyage", exception.Message);
    }

    [Fact]
    public void UpdateVoyage_ShouldBeOk()
    {
        long userId = 1;
        Voyage vNew = new Voyage(){
            Topic = "foo",
            RegionId = 1,
        };

        Voyage vOld = new Voyage(){
            Topic = "bar",
            UserId = 1,
            RegionId = 1
        };

        User u = new User(){ Id = 1, Username = "user", AppRole = "user"};

        _repository.Setup(x => x.GetById(2)).Returns(vOld);
        _userRepository.Setup(x => x.RetrieveSingleUserById(userId)).Returns(u);
        _auth.Setup(x => x.IsOwner(userId, vOld)).Returns(true);
        _repository.Setup(x => x.UpdateVoyage(vNew));
        _service.UpdateVoyage(userId, 2, vNew);
    }

    [Fact]
    public void UpdateVoyage_ShouldBeOkAsAdmin()
    {
        long userId = 144444;
        Voyage vNew = new Voyage(){
            Topic = "foo",
            RegionId = 1,
        };

        Voyage vOld = new Voyage(){
            Topic = "bar",
            UserId = 1,
            RegionId = 1
        };

        User u = new User(){ Username = "user", AppRole = "admin"};

        _repository.Setup(x => x.GetById(2)).Returns(vOld);
        _userRepository.Setup(x => x.RetrieveSingleUserById(userId)).Returns(u);
        _auth.Setup(x => x.IsOwner(userId, vOld)).Returns(true);
        _repository.Setup(x => x.UpdateVoyage(vNew));
        _service.UpdateVoyage(userId, 2, vNew);
    }
    
    [Fact]
    public void UpdateVoyage_ShouldThrowErrorWhenInvalidUser()
    {
        long userId = 200000;
        Voyage vNew = new Voyage(){
            Topic = "foo",
            RegionId = 1,
        };

        Voyage vOld = new Voyage(){
            Topic = "bar",
            UserId = 1,
            RegionId = 1
        };

        User u = new User(){ Id = 1, Username = "user", AppRole = "user"};
        
        _repository.Setup(x => x.GetById(2)).Returns(vOld);
        _userRepository.Setup(x => x.RetrieveSingleUserById(userId)).Returns(u);
        _repository.Setup(x => x.UpdateVoyage(vNew));
        
        var exception = Assert.Throws<UnauthorizedAccessException>(() => _service.UpdateVoyage(userId, 2, vNew));

        Assert.Equal("Attempted to perform an unauthorized operation.", exception.Message);
    }
   

    [Fact]
    public void UpdateVoyage_ShouldThrowErrorWhenVoyageIsNull()
    {
        long userId = 200000;
        Voyage vNew = new Voyage(){
            Topic = "foo",
            RegionId = 1,
        };


        User u = new User(){ Id = 1, Username = "user", AppRole = "user"};
        
        _repository.Setup(x => x.GetById(0)).Returns(null as Voyage);
        _repository.Setup(x => x.UpdateVoyage(vNew));
        
        var exception = Assert.Throws<Exception>(() => _service.UpdateVoyage(userId, 2, vNew));

        Assert.Equal("Voyage not found", exception.Message);
    }

    [Fact]
    public void DeleteVoyage_ShouldBeOk()
    {
        long userId = 1;

        Voyage record = new Voyage(){
            Topic = "bar",
            UserId = 1,
            RegionId = 1
        };

        User u = new User(){ Id = 1, Username = "user", AppRole = "user"};

        _repository.Setup(x => x.GetById(1)).Returns(record);
        _userRepository.Setup(x => x.RetrieveSingleUserById(userId)).Returns(u);
        _auth.Setup(x => x.IsOwner(userId, record)).Returns(true);
        _repository.Setup(x => x.DeleteVoyage(record));
        _service.DeleteVoyage(1, 1);
    }

    
    [Fact]
    public void DeleteVoyage_ShouldBeOkAsAdmin()
    {
        long userId = 144444;

        Voyage record = new Voyage(){
            Topic = "bar",
            UserId = 1,
            RegionId = 1
        };

        User u = new User(){
            Id = userId, 
            Username = "user", 
            AppRole = "admin"
            };

        _repository.Setup(x => x.GetById(2)).Returns(record);
        _userRepository.Setup(x => x.RetrieveSingleUserById(userId)).Returns(u);
        _auth.Setup(x => x.IsOwner(userId, record)).Returns(true);
        _repository.Setup(x => x.DeleteVoyage(record));
        _service.DeleteVoyage(userId, 2);
    }

    [Fact]
    public void DeleteVoyage_ShouldThrowErrorWhenInvalidUser()
    {
        Voyage record = new Voyage(){
            Id = 2,
            Topic = "bar",
            UserId = 1,
            RegionId = 1
        };

        User u = new User(){ Id = 1, Username = "user", AppRole = "user"};
        
        _repository.Setup(x => x.GetById(2)).Returns(record);
        _userRepository.Setup(x => x.RetrieveSingleUserById(22222)).Returns(u);
        _repository.Setup(x => x.DeleteVoyage(record));
        
        var exception = Assert.Throws<UnauthorizedAccessException>(() => _service.DeleteVoyage(22222, 2));

        Assert.Equal("Attempted to perform an unauthorized operation.", exception.Message);
    }

    [Fact]
    public void DeleteVoyage_ShouldThrowErrorWhenVoyageIsNull()
    {
        long userId = 200000;
        Voyage record = new Voyage(){
            Topic = "foo",
            RegionId = 1,
        };


        User u = new User(){ Id = 1, Username = "user", AppRole = "user"};
        
        _repository.Setup(x => x.GetById(0)).Returns(null as Voyage);
        _repository.Setup(x => x.DeleteVoyage(record));
        
        var exception = Assert.Throws<Exception>(() => _service.DeleteVoyage(userId, 2));

        Assert.Equal("Voyage not found", exception.Message);
    }

}