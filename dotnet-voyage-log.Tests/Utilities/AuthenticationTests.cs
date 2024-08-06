using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Moq;

namespace dotnet_voyage_log.Utilities;


public class AuthenticationTests {
    private IAuthentication _auth;
    private Mock<IUserRepository> _userRepository;

    public AuthenticationTests()
    {
        _userRepository = new Mock<IUserRepository>();
        _auth = new Authentication(_userRepository.Object);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("Test123")]
    [InlineData("?=(%=#())")]
    public void HashPassword_ShouldReturnHash(string a)
    {
        string h = _auth.HashPassword(a);
        Assert.NotEmpty(h);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("Test123")]
    [InlineData("?=(%=#())")]
    public void IsValidPassword_ShouldReturnTrueWhenValid(string a)
    {
        string h = _auth.HashPassword(a);
        bool v = _auth.IsValidPassword(h, a);
        Assert.True(v);
    }

    [Theory]
    [InlineData("test")]
    [InlineData("Test123")]
    [InlineData("?=(%=#())")]
    public void IsValidPassword_ShouldReturnFalseWhenInvalid(string a)
    {
        string h = _auth.HashPassword(a);
        bool v = _auth.IsValidPassword(h, $"{a}wrong");
        Assert.False(v);
    }

    [Fact]
    public void IsOwner_ShouldBeOk() {
        Voyage v = new Voyage() {
            Topic = "Test",
            UserId = 1,
            RegionId = 1
        };
        _userRepository.Setup(x => x.RetrieveSingleUserById(1)).Returns(new User() { Id=1, Username = "test", AppRole = "user"});
        bool i = _auth.IsOwner(1, v);
        Assert.True(i);
    }

    [Fact]
    public void IsOwner_ShouldBeOkAsAdmin() {
        Voyage v = new Voyage() {
            Topic = "Test",
            UserId = 2,
            RegionId = 2
        };
        _userRepository.Setup(x => x.RetrieveSingleUserById(1)).Returns(new User() { Id=1, Username = "test", AppRole = "admin"});
        bool i = _auth.IsOwner(1, v);
        Assert.True(i);
    }

    [Fact]
    public void IsOwner_ShouldThrowErrorWhenWrongUser() {
        Voyage v = new Voyage() {
            Topic = "Test",
            UserId = 2,
            RegionId = 2
        };
        _userRepository.Setup(x => x.RetrieveSingleUserById(1)).Returns(new User() { Id=1, Username = "test", AppRole = "user"});
        bool i = _auth.IsOwner(1, v);
        Assert.False(i);
    }
}