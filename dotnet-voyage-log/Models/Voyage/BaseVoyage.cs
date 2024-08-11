using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_voyage_log.Models;

public class BaseVoyage {
    [Column("topic")]
    public required string Topic { get;set; }
    [Column("description")]
    public string? Description { get;set; }
    [Column("notes")]
    public string? Notes { get;set; }
    [Column("location_longitude")]
    public double? LocationLongitude { get;set; }
    [Column("location_latitude")]
    public double? LocationLatitude { get;set; }
    [Column("region_fk")]
    public long RegionId { get; set; }
    [Column("user_fk")]
    public long UserId {get;set;}
    public void CheckVoyage(){
        if(this.Topic == "" || this.RegionId == 0 || this.UserId == 0 ){
            throw new Exception("Malformatted voyage");
        }
    }
}