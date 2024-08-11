using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Service;

public class LocationService : ILocationService
{
    private ILocationRepository _repository;
    private readonly ILogger<ILocationService> _logger;


    public LocationService(ILocationRepository repository, ILogger<ILocationService> logger) {
        _repository = repository;
        _logger = logger;
    }

    /// <summary>
    /// Retrives all countries from database.
    /// </summary>
    /// <returns>
    /// List of countries
    /// </returns>
    public List<Country> GetAllCountries()
    {
        return _repository.GetAllCountries();
    }

    /// <summary>
    /// Returns all regions from the database.
    /// </summary>
    /// <returns>
    /// List of regions
    /// </returns>
    public List<Region> GetAllRegions()
    {
        return _repository.GetAllRegions();
    }

    /// <summary>
    /// Takes country name and retuns it by name. Throws error if not exist.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// Country
    /// </returns>
    public Country GetSingleCountry(string name)
    {
        Country? c = _repository.GetSingleCountry(name);
        if (c != null) {
            return c;
        }
        
        throw new Exception("Country not found");
    }

    /// <summary>
    /// Takes region name and retuns it by name. Throws error if not exist.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// Region
    /// </returns>
    public Region GetSingleRegion(string name)
    {
        Region? r = _repository.GetSingleRegion(name);
        if (r != null) {
            return r;
        }

        throw new Exception("Region not found");
    }
}