using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace dotnet_voyage_log.Models;

public class LoginUser {
    [JsonProperty("username")]
    public required string Username { get; set; }
    
    [JsonProperty("password")]
    public required string Password { get; set; }
    public void CheckLoginUser() {
        if(this.Username == "" || this.Password == "") {
            throw new Exception("Malformatted login");
        }
    }
}