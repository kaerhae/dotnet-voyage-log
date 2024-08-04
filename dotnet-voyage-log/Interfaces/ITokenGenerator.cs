using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Interfaces;

public interface ITokenGenerator {
    public string GenerateToken(User user);
}