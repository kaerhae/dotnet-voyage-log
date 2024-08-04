
using dotnet_voyage_log.Interfaces;

namespace dotnet_voyage_log.Utilities;

public class Configs : IConfigs {
    private IConfiguration _config;

    public Configs(IConfiguration config) {
        _config = config;
    }
    public string GetConnectionString() {
        string? conn;
        conn = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
        if(conn != null) {
            return conn;
        }
        conn = _config["ConnectionStrings:PostgresConn"];
        if(conn != null) {
            return conn;
        }
        throw new Exception("Connection string missing");
    }

    public string GetSecretKey() {
        string? secret;
        secret = Environment.GetEnvironmentVariable("SECRET_KEY");
        if(secret != null) {
            return secret;
        }
        secret = _config["JwtSettings:SecretKey"];
        if (secret != null) {
            return secret;
        }

        throw new Exception("Secret key is missing");
    }

    public string GetAudience() {
        string? audience;
        audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        if(audience != null) {
            return audience;
        }
        
        audience = _config["JwtSettings:Audience"];
        if (audience != null) {
            return audience;
        }

        throw new Exception("Jwt Audience is missing");
    }

    public string GetIssuer() {
        string? issuer;
        issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        if(issuer != null) {
            return issuer;
        }

        issuer = _config["JwtSettings:Issuer"];
        if (issuer != null) {
            return issuer;
        }

        throw new Exception("Jwt Issuer is missing");
    }


    public string GetAdminUsername()
    {
        string? adminUser;
        adminUser = Environment.GetEnvironmentVariable("INIT_ADMIN_USERNAME");
        if(adminUser != null) {
            return adminUser;
        }
        
        adminUser = _config["InitAdmin:Username"];
        if (adminUser != null) {
            return adminUser;
        }

        throw new Exception("adminUser is missing");
    }

    public string GetAdminEmail()
    {
        string? adminEmail;
        adminEmail = Environment.GetEnvironmentVariable("INIT_ADMIN_EMAIL");
        if(adminEmail != null) {
            return adminEmail;
        }

        adminEmail = _config["InitAdmin:Email"];
        if (adminEmail != null) {
            return adminEmail;
        }

        throw new Exception("adminEmail is missing");
    }

    public string GetAdminPassword()
    {
        string? adminPass;
        adminPass = Environment.GetEnvironmentVariable("INIT_ADMIN_PASSWORD");
        if(adminPass != null) {
            return adminPass;
        }

        adminPass = _config["InitAdmin:Password"];
        if (adminPass != null) {
            return adminPass;
        }

        throw new Exception("adminPass is missing");
    }
}