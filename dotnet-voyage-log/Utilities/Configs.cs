using dotnet_voyage_log.Interfaces;

namespace dotnet_voyage_log.Utilities;

public class Configs : IConfigs {
    private IConfiguration _config;

    public Configs(IConfiguration config) {
        _config = config;
    }
    public string GetConnectionString() {
        string? conn = _config["ConnectionStrings:PostgresConn"];
        if (conn != null) {
            return conn;
        }
        throw new Exception("Connection string missing");
    }

    public string GetSecretKey() {
        string? secret = _config["Secrets:SecretKey"];
        if (secret != null) {
            return secret;
        }

        throw new Exception("Secret key is missing");

    }
}