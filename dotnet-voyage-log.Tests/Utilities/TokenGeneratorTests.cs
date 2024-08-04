using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using dotnet_voyage_log.Utilities;
using Moq;

namespace dotnet_voyage_log.Tests.Utilities;

public class TokenGeneratorTests
{
    private readonly ITokenGenerator _generator;
    private Mock<IConfigs> _config;


    public TokenGeneratorTests() {
        _config = new Mock<IConfigs>();
        _generator = new TokenGenerator(_config.Object);
    }
    
    [Fact]
    public void GenerateToken_ShouldNotBeNull()
    {

        _config
        .Setup(x => x.GetSecretKey())
        .Returns("123456789101112131415161718192021222324");

        User u = new User(){
            Username = "test",
            PasswdHash = "1234",
            AppRole = "admin"
        };

        string s = _generator.GenerateToken(u);

        Assert.NotEmpty(s);
    }
}