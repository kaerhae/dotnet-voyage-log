using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_voyage_log.Models;

[Table("voyages")]
public class Voyage {
    [Key]
    [Column("voyage_id")]
    public long Id { get;set; }
    [Required]
    [Column("topic")]
    public string Topic { get;set; }
    [Column("description")]
    public string? Description { get;set; }
    [Column("notes")]
    public string? Notes { get;set; }
    [Column("images")]
    public List<string>? Images { get;set; }
    [Column("created_at")]
    public DateTime CreatedAt { get;set; }
    [Column("updated_at")]
    public DateTime UpdatedAt { get;set; }
    [Column("location_longitude")]
    public double? LocationLongitude { get;set; }
    [Column("location_latitude")]
    public double? LocationLatitude { get;set; }
    [Column("countryFK")]
    [ForeignKey("City")]
    public long CountryId { get; set; }
    [Column("regionFK")]
    [ForeignKey("Region")]
    public long RegionId { get; set; }
}