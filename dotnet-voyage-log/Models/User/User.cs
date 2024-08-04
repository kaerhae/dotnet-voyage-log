using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dotnet_voyage_log.Models;

[Table("users")]
public class User {

    [Key]
    [Column("user_id")]
    public long Id { get;set; }
    [Column("username")]
    public required string Username { get; set; }
    [Column("email")]
    public string? Email { get;set; }
    [Column("app_role")]
    public required string AppRole { get;set; }
    [JsonIgnore]
    [Column("password_hash")]
    public string PasswdHash { get;set; }
    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get;set; }
    public void CheckUser() {
        if(this.Username == "" 
        || this.PasswdHash == "" 
        || this.Email == ""
        || this.AppRole == "") {
            throw new Exception("Malformatted user: One or more user field empty");
        }
    }
}