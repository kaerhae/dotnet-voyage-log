namespace dotnet_voyage_log.Interfaces;

public interface IConfigs {
    public string GetConnectionString();
    public string GetAdminUsername();
    public string GetAdminEmail();
    public string GetAdminPassword();
    public string GetSecretKey();
    public string GetAudience();
    public string GetIssuer();
}