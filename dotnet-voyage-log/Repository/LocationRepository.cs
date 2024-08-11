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

    /// <summary>
    /// Retrieves all countries from database and returns with list of region fields one-to-many relationship.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// List of countries
    /// </returns>
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

    /// <summary>
    /// Retrieves all regions from database and returns with country field foreign key relationship.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// List of regions
    /// </returns>
    public List<Region> GetAllRegions()
    {
        try {
            return _context.Regions.Select(x => new Region {
                Id = x.Id,
                Name = x.Name,
                CountryId = x.CountryId,
                Country = _context.Countries
                    .Where(c => c.Id == x.CountryId)
                    .Select(c => new Country{
                        Id = c.Id,
                        Name = c.Name
                    }).FirstOrDefault()
            }).ToList();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    /// <summary>
    /// Retrieves single country from database and returns with list of region fields one-to-many relationship.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// Nullable Country
    /// </returns>
    public Country? GetSingleCountry(string name)
    {
        try {
            return _context.Countries.Where(x => x.Name.ToLower() == name.ToLower()).Select(c => new Country() {
                Id = c.Id,
                Name = c.Name,
                Regions = c.Regions.Select(r => new Region(){
                    Id = r.Id,
                    Name = r.Name,
                    CountryId = r.CountryId,
                }).ToList()
            }).FirstOrDefault();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    /// <summary>
    /// Retrieves single region from database and returns with country fields foreign key relationship.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// Nullable Region
    /// </returns>
    public Region? GetSingleRegion(string name)
    {
        try {
            return _context.Regions.Where(x => x.Name.ToLower() == name.ToLower()).Select(x => new Region {
                Id = x.Id,
                Name = x.Name,
                CountryId = x.CountryId,
                Country = _context.Countries
                    .Where(c => c.Id == x.CountryId).Select(c => new Country{
                        Id = c.Id,
                        Name = c.Name
                    }).FirstOrDefault(),
                Voyages = x.Voyages
                    .Select(v => new Voyage{
                        Id = v.Id,
                        Topic = v.Topic,
                        Description = v.Description,
                        Notes = v.Notes,
                        Images = v.Images,
                        CreatedAt = v.CreatedAt,
                        UpdatedAt = v.UpdatedAt,
                        LocationLatitude = v.LocationLatitude,
                        LocationLongitude = v.LocationLongitude,
                        RegionId = v.RegionId,
                        }).ToList()
            }).FirstOrDefault();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }
}