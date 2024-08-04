
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using dotnet_voyage_log.Service;
using Moq;

namespace dotnet_voyage_log.Tests.Service;

public class UserServiceTests
{
    private IUserService _service;

    private Mock<IUserRepository> _repository;
    private Mock<ITokenGenerator> _generator;
    private Mock<IAuthentication> _auth;

    public UserServiceTests()
    {
        _repository = new Mock<IUserRepository>();
        _generator = new Mock<ITokenGenerator>();
        _auth = new Mock<IAuthentication>();
        _service = new UserService(_repository.Object, _generator.Object, _auth.Object);
    }

    [Fact]
    public void LoginUser_ShouldReturnToken()
    {
        User user = new User() {
                Id = 0,
                Username = "Test",
                PasswdHash = "1234"
            };
        LoginUser lUser = new LoginUser(){
            Username = "Test",
            Password = "1234"
        };
        string token = "1234567890123456789";
        _repository.Setup(x => x.RetrieveSingleUserByUsername(lUser.Username)).Returns(user);
        _auth.Setup(x => x.IsValidPassword(user.PasswdHash,lUser.Password)).Returns(true);
        _generator.Setup(x => x.GenerateToken(user)).Returns(token);

        string result = _service.LoginUser(lUser);

        Assert.Equal(token, result);
    }

    [Fact]
    public void LoginUser_ShouldThrowErrorWhenUserNotFound()
    {
        User user = new User() {
                Id = 0,
                Username = "Test",
                PasswdHash = "1234"
            };
        LoginUser lUser = new LoginUser(){
            Username = "Test",
            Password = "1234"
        };
        _repository.Setup(x => x.RetrieveSingleUserByUsername(lUser.Username)).Returns((User)null);

        var exception = Assert.Throws<Exception>(() => _service.LoginUser(lUser));
        Assert.Equal("User not found", exception.Message);
    }

    [Fact]
    public void LoginUser_ShouldThrowErrorWhenInvalidPassword()
    {
        User user = new User() {
                Id = 0,
                Username = "Test",
                PasswdHash = "1234"
            };
        LoginUser lUser = new LoginUser(){
            Username = "Test",
            Password = "1234"
        };
        _repository.Setup(x => x.RetrieveSingleUserByUsername(lUser.Username)).Returns(user);
        _auth.Setup(x => x.IsValidPassword(user.PasswdHash,lUser.Password)).Returns(false);

        var exception = Assert.Throws<Exception>(() => _service.LoginUser(lUser));
        Assert.Equal("Incorrect password", exception.Message);
    }

    [Fact]
    public void GetAll_ShouldReturnUsers()
    {
        List<User> users = new List<User>()
        {
            new User()
            {
                Id = 0,
                Username = "Test",
                PasswdHash = "1234"
            },
            new User()
            {
                Id = 1,
                Username = "Test2",
                PasswdHash = "1234"
            }
        };

        _repository.Setup(x => x.RetrieveAllUsers()).Returns(users);

        var result = _service.GetAll();

        Assert.True(result.Count() == 2);
        Assert.Equal(0, result[0].Id);
        Assert.Equal(1, result[1].Id);
    }


    [Fact]
    public void GetById_ShouldReturnUser()
    {
        User user = new User() {
                Id = 0,
                Username = "Test",
                PasswdHash = "1234"
            };

        _repository.Setup(x => x.RetrieveSingleUserById(0)).Returns(user);

        var result = _service.GetById(0);

        Assert.Equal(0, result.Id);
        Assert.Equal(user.Username, result.Username);
    }

    [Fact]
    public void GetById_ShouldThrowException()
    {
        _repository.Setup(x => x.RetrieveSingleUserById(0)).Returns((User)null);

        var exception = Assert.Throws<Exception>(() => _service.GetById(0));
        Assert.Equal("User not found", exception.Message);
    }

    [Fact]
    public void CreateUser_ShouldBeOk()
    {
        User u = new User(){
            Username = "test",
            Email = "test@test.com",
            PasswdHash = "1234",
            AppRole = "1234",
        };
        _repository.Setup(x => x.InsertUser(u));

        User result = _service.CreateUser(u);

        Assert.Equal(u.Username, result.Username);
        Assert.Equal(u.Email, result.Email);
        Assert.Equal(u.PasswdHash, result.PasswdHash);
        Assert.Equal(u.AppRole, result.AppRole);
    }

    [Fact]
    public void UpdateUser_ShouldBeOk()
    {
        User oldUser = new User(){
            Id = 0,
            Username = "test",
            Email = "test@test.com",
            PasswdHash = "1234",
            AppRole = "1234",
        };
        User newUser = new User() {
            Username = "updated",
            Email = "updated",
            PasswdHash = "updated",
            AppRole = "updated"
        };
        _repository.Setup(x => x.UpdateAllFields(newUser));
        _repository.Setup(x => x.RetrieveSingleUserById(0)).Returns(oldUser);

        User result = _service.UpdateUser(0, newUser);

        Assert.Equal(newUser.Username, result.Username);
        Assert.Equal(newUser.Email, result.Email);
        Assert.Equal(newUser.PasswdHash, result.PasswdHash);
        Assert.Equal(newUser.AppRole, result.AppRole);
    }

    [Fact]
    public void UpdateUser_ShouldReturnErrorWhenUserDoesNotExist()
    {
        User newUser = new User() {
            Username = "updated",
            Email = "updated",
            PasswdHash = "updated",
            AppRole = "updated"
        };
        _repository.Setup(x => x.UpdateAllFields(newUser));
        _repository.Setup(x => x.RetrieveSingleUserById(0)).Returns((User)null);
        var exception = Assert.Throws<Exception>(() => _service.UpdateUser(0, newUser));
        Assert.Equal("User not found", exception.Message);
    }

    [Fact]
    public void DeleteUser_ShouldBeOk()
    {
        User user = new User(){
            Id = 0,
            Username = "test",
            Email = "test@test.com",
            PasswdHash = "1234",
            AppRole = "1234",
        };
        _repository.Setup(x => x.DeleteUser(user));
        _repository.Setup(x => x.RetrieveSingleUserById(0)).Returns(user);

        User result = _service.DeleteUser(0);

        Assert.Equal(user.Username, result.Username);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.PasswdHash, result.PasswdHash);
        Assert.Equal(user.AppRole, result.AppRole);
    }

    [Fact]
    public void DeleteUser_ShouldReturnErrorWhenUserDoesNotExist()
    {
        User user = new User() {
            Username = "updated",
            Email = "updated",
            PasswdHash = "updated",
            AppRole = "updated"
        };
        _repository.Setup(x => x.DeleteUser(user));
        _repository.Setup(x => x.RetrieveSingleUserById(0)).Returns((User)null);
        var exception = Assert.Throws<Exception>(() => _service.DeleteUser(0));
        Assert.Equal("User not found", exception.Message);
    }
}