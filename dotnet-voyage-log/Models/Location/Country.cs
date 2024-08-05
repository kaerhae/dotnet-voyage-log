using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_voyage_log.Models;

[Table("countries")]
public class Country {
    [Key]
    [Column("country_id")]
    public long Id { get; set; }
    [Column("country_name")]
    public required string Name { get; set; }
    [Column("regions")]
    public List<Region> Regions { get; set; }

}