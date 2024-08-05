using dotnet_voyage_log.Context;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Repository;


public class LocationRepository : ILocationRepository
{

    private DataContext _context;
    private readonly ILogger<ILocationRepository> _logger;


    public LocationRepository(DataContext context, ILogger<LocationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    public List<Country> GetAllCountries()
    {
        try {
            return _context.Countries.Select(c => new Country(){
                Id = c.Id,
                Name = c.Name,
                Regions = c.Regions
                    .Select(e => new Region { 
                        Id = e.Id, 
                        Name = e.Name,
                        CountryId = e.CountryId
                        }).ToList()
            }).ToList();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    public List<Region> GetAllRegions()
    {
        try {
            return _context.Regions.ToList();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    public Country? GetSingleCountry(string name)
    {
        try {
            return _context.Countries.Where(x => x.Name.ToLower() == name.ToLower()).Select(c => new Country() {
                Id = c.Id,
                Name = c.Name,
                Regions = c.Regions.Select(r => new Region(){
                    Id = r.Id,
                    Name = r.Name,
                    CountryId = r.CountryId
                }).ToList()
            }).FirstOrDefault();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    public Region? GetSingleRegion(string name)
    {
        try {
            return _context.Regions.Where(x => x.Name.ToLower() == name.ToLower()).FirstOrDefault();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }
}