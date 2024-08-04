using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace dotnet_voyage_log.Models;

public class SignupUser {
    [JsonProperty("username")]
    public required string Username { get; set; }
    [JsonProperty("email")]
    public string? Email { get;set; }
    [JsonProperty("password")]
    public required string Password {get;set;}
}