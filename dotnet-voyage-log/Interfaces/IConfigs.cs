namespace dotnet_voyage_log.Interfaces;


public interface IConfigs {
    public string GetConnectionString();
    public string GetSecretKey() ;
}