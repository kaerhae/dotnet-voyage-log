namespace dotnet_voyage_log.Interfaces;

public interface IAuthentication {
    public string HashPassword(string plain);
    public bool IsValidPassword(string hash, string plain);
}