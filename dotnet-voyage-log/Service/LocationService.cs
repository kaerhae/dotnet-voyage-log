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
    public List<Country> GetAllCountries()
    {
        return _repository.GetAllCountries();
    }

    public List<Region> GetAllRegions()
    {
        return _repository.GetAllRegions();
    }

    public Country GetSingleCountry(string name)
    {
        Country? c = _repository.GetSingleCountry(name);
        if (c != null) {
            return c;
        }
        
        throw new Exception("Country not found");
    }

    public Region GetSingleRegion(string name)
    {
        Region? r = _repository.GetSingleRegion(name);
        if (r != null) {
            return r;
        }

        throw new Exception("Region not found");
    }
}