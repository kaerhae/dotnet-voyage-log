using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace dotnet_voyage_log.Models;

[Table("regions")]
public class Region {
    [Key]
    [Column("region_id")]
    public long Id { get; set; }
    [Column("region_name")]
    public required string Name { get; set; }
    [Column("country_fk")]
    public long CountryId { get;set; }
    [JsonIgnore]
     public Country Country { get; set; } = null!; 

}