using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Utilities;


public class LoginUserTests {


    [Theory]
    [InlineData("test", "")]
    [InlineData("", "testpassword")]
    [InlineData("", "")]
    
    public void LoginUser_CheckLoginUser_ShouldNotThrowErrorWhenCorrect(string a, string b)
    {
        var user = new LoginUser() {
            Username = a,
            Password = b
        };
        var exception = Assert.Throws<Exception>(() => user.CheckLoginUser());
        Assert.Equal("Malformatted login", exception.Message);
    }

    [Theory]
    [InlineData("test", "t")]
    [InlineData("t", "testpassword")]
    [InlineData("test", "testpassword")]
    public void LoginUser_CheckLoginUser_ShouldThrowErrorWhenFieldsMissing(string a, string b)
    {
        var user = new LoginUser() {
            Username = a,
            Password = b
        };

        user.CheckLoginUser();
    }
}