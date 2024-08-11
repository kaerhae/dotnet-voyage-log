
using dotnet_voyage_log.Interfaces;

namespace dotnet_voyage_log.Utilities;

public class Configs : IConfigs {
    private IConfiguration _config;

    public Configs(IConfiguration config) {
        _config = config;
    }
    /// <summary>
    /// Returns connection string from either POSTGRES_CONNECTION_STRING env or ConnectionStrings:PostgresConn secret.
    /// </summary>
    /// <returns>
    /// string: Connection string
    /// </returns>
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

    /// <summary>
    /// Returns secret key from either SECRET_KEY env or JwtSettings:SecretKey secret.
    /// </summary>
    /// <returns>
    /// string: Secret key
    /// </returns>
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

    /// <summary>
    /// Returns JWT Audience string from either JWT_AUDIENCE env or JwtSettings:Audience secret.
    /// </summary>
    /// <returns>
    /// string: JWT Audience
    /// </returns>
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

    /// <summary>
    /// Returns JWT Issuer string from either JWT_ISSUER env or JwtSettings:Issuer secret.
    /// </summary>
    /// <returns>
    /// string: JWT Issuer
    /// </returns>
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


    /// <summary>
    /// Returns initial admin username from either INIT_ADMIN_USERNAME env or InitAdmin:Username secret.
    /// </summary>
    /// <returns>
    /// string: Initial admin username
    /// </returns>
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

    /// <summary>
    /// Returns initial admin email from either INIT_ADMIN_EMAIL env or InitAdmin:Email secret.
    /// </summary>
    /// <returns>
    /// string: Initial admin email
    /// </returns>
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

    /// <summary>
    /// Returns initial admin password from either INIT_ADMIN_PASSWORD env or InitAdmin:Password secret.
    /// </summary>
    /// <returns>
    /// string: Initial admin password
    /// </returns>
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

    public string GetImageBucket()
    {
        string? bucketName;
        bucketName = Environment.GetEnvironmentVariable("S3_BUCKET_NAME");
        if(bucketName != null) {
            return bucketName;
        }

        bucketName = _config["AWS:S3BucketName"];
        if (bucketName != null) {
            return bucketName;
        }

        throw new Exception("Voyage bucket name is missing");
    }
}