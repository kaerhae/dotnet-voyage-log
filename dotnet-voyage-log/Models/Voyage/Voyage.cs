using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dotnet_voyage_log.Models;

[Table("voyages")]
public class Voyage : BaseVoyage {
    [Key]
    [Column("voyage_id")]
    public long Id { get;set; }
    
    [Column("images")]
    public List<string>? Images { get;set; }
    [Column("created_at")]
    public DateTime CreatedAt { get;set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get;set; }
    
    [JsonIgnore]
    public Region? Region { get; set; }

    [JsonIgnore]
    public User? User { get;set; }

    
}