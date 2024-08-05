using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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
    [Column("region_fk")]
    public long RegionId { get; set; }
    [JsonIgnore]
    public Region? Region { get; set; }

    [Column("user_fk")]
    public long UserId {get;set;}
    [JsonIgnore]
    public User? User { get;set; }

    public void CheckVoyage(){
        if(this.Topic == "" || this.RegionId == 0 || this.UserId == 0 ){
            throw new Exception("Malformatted voyage");
        }
    }
}