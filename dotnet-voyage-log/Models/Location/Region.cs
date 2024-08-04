using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_voyage_log.Models;

[Table("regions")]
public class Region {
    [Key]
    [Column("region_id")]
    public long Id { get; set; }
    [Column("region_name")]
    public required string Name { get; set; }
    [Column("country")]
    public Country Country {get;set;}
    [ForeignKey("Country")]
    [Column("country_fk")]
    public long CountryId { get;set; }

}