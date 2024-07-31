using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dotnet_voyage_log.Models;

[Table("users")]
public class User {
    [Key]
    public long Id { get;set; }
    [Required]
    [Column("username")]
    public string Username { get;set; }
    [Column("email")]
    public string? Email { get;set; }
    [Column("app_role")]
    public string AppRole { get;set; }
    [JsonIgnore]
    [Required]
    [Column("password_hash")]
    public string PasswdHash { get;set; }
    [Required]
    [Column("created_at")]
    public DateTime CreatedAt { get;set; }
}