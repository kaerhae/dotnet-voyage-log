using dotnet_voyage_log.Context;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace dotnet_voyage_log.Repository;

public class VoyageRepository : IVoyageRepository
{
    private DataContext _context;
    private ILogger<IVoyageRepository> _logger;

    public VoyageRepository(DataContext context, ILogger<IVoyageRepository> logger) {
        _context = context;
        _logger = logger;
    }
    /// <summary>
    /// Retrieves all voyage-logs from database.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// List of Voyage
    /// </returns>
    public List<Voyage> GetAll()
    {
        return [.. _context.Voyages.Select(v => new Voyage{
            Id = v.Id,
            Topic = v.Topic,
            Description = v.Description,
            Notes = v.Notes,
            Images = v.Images,
            CreatedAt = v.CreatedAt,
            UpdatedAt = v.UpdatedAt,
            LocationLatitude = v.LocationLatitude,
            LocationLongitude = v.LocationLongitude,
            UserId = v.UserId,
            RegionId = v.RegionId,
        })];
    }

    /// <summary>
    /// Retrieves single Voyage from database.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// Nullable Voyage
    /// </returns>
    public Voyage? GetById(long id) {
        try {
            return _context.Voyages.Where(x => x.Id == id).FirstOrDefault();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Voyage not found");
        }
    }

    /// <summary>
    /// Takes voyage object and inserts it to database.
    /// </summary>
    /// <exception cref="Exception" />
    public void CreateVoyage(Voyage newVoyage)
    {
        try {
            _context.Voyages.Add(newVoyage);
            _context.SaveChanges();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            _logger.LogError($"Inner: {e.InnerException?.Message}");
            throw new Exception("Internal server error");
        }
    }

    /// <summary>
    /// Takes Voyage object and updates its fields in database.
    /// </summary>
    /// <exception cref="Exception" />
    public void UpdateVoyage(Voyage updatedVoyage)
    {
        try {
            _context.Voyages.Update(updatedVoyage);
            _context.SaveChanges();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    /// <summary>
    /// Takes Voyage object and deletes it from database.
    /// </summary>
    /// <exception cref="Exception" />
    public void DeleteVoyage(Voyage voyage)
    {
        try {
            _context.Voyages.Remove(voyage);
            _context.SaveChanges();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }
}