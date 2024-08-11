namespace dotnet_voyage_log.Models;

public class VoyageWithFiles : BaseVoyage {
    public List<IFormFile>? Images { get; set; }
}