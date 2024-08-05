using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Interfaces;

public interface ILocationRepository {
    public List<Country> GetAllCountries();
    public Country? GetSingleCountry(string name);
    public List<Region> GetAllRegions();
    public Region? GetSingleRegion(string name);


}