using dotnet_voyage_log.Interfaces;

namespace dotnet_voyage_log.Utilities;


public class AuthenticationTests {
    private IAuthentication _auth;

    public AuthenticationTests()
    {
        _auth = new Authentication();
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
}