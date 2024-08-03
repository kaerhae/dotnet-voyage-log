using dotnet_voyage_log.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace dotnet_voyage_log.Utilities;

public class Authentication : IAuthentication
{
    public string  HashPassword(string plain)
    {
        string hash = BC.HashPassword(plain);
        return hash;
    }

    public bool IsValidPassword(string hash, string plain)
    {
        bool isAuthenticated = BC.Verify(plain, hash);
        if (!isAuthenticated) {
            return false;
        }

        return true;
    }
}