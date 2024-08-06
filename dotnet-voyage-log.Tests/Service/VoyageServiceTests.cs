
using Castle.Core.Logging;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using dotnet_voyage_log.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace dotnet_voyage_log.Tests.Service;

public class VoyageServiceTests
{
    private IVoyageService _service;

    private Mock<IVoyageRepository> _repository;
    private Mock<IUserRepository> _userRepository;
    private Mock<IAuthentication> _auth;
    private Mock<ILogger<IVoyageService>> _logger;

    public VoyageServiceTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _repository = new Mock<IVoyageRepository>();
        _auth = new Mock<IAuthentication>();
        _logger = new Mock<ILogger<IVoyageService>>();
        _service = new VoyageService(_repository.Object, _auth.Object, _logger.Object);
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
    public void CreateVoyage_ShouldBeOk()
    {
        Voyage v = new Voyage(){
            Topic = "Test",
            UserId = 1,
            RegionId = 1
        };

        _repository.Setup(x => x.CreateVoyage(v));

        _service.CreateVoyage(1, v);

    }

    

    [Theory]
    [InlineData("", 1)]
    [InlineData("foo", 0)]

    public void CreateVoyage_ShouldThrowErrorWhenInvalidVoyage(string a, long b)
    {
        Voyage v = new Voyage(){
            Topic = a,
            RegionId = b,

        };

        var exception = Assert.Throws<Exception>(() => _service.CreateVoyage(1, v));

        Assert.Equal("Malformatted voyage", exception.Message);
    }

    [Fact]
    public void CreateVoyage_ShouldThrowErrorWhenInvalidUserId()
    {
        Voyage v = new Voyage(){
            Topic = "foo",
            RegionId = 1,

        };

        var exception = Assert.Throws<Exception>(() => _service.CreateVoyage(0, v));

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