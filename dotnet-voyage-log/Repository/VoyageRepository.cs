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

    public Voyage? GetById(long id) {
        try {
            return _context.Voyages.Where(x => x.Id == id).FirstOrDefault();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Voyage not found");
        }
    }

    public void CreateVoyage(Voyage newVoyage)
    {
        try {
            _context.Voyages.Add(newVoyage);
            _context.SaveChanges();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
        }
    }

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