using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Newtonsoft.Json;

namespace dotnet_voyage_log.Service;

public class VoyageService : IVoyageService
{
    private IVoyageRepository _repository;
    private ILogger<IVoyageService> _logger;

    private IAuthentication _auth;

    public VoyageService(IVoyageRepository repository, IAuthentication auth, ILogger<IVoyageService> logger){
        _repository = repository;
        _auth = auth;
        _logger = logger;
    }

    public List<Voyage> GetAll()
    {
        return _repository.GetAll();
    }

    public void CreateVoyage(long userId, Voyage newVoyage)
    {
        newVoyage.UserId = userId;
        newVoyage.CheckVoyage();
        _repository.CreateVoyage(newVoyage);
    }


    public void UpdateVoyage(long userId, long voyageId, Voyage updatedVoyage)
    {
        Voyage? oldRecord = _repository.GetById(voyageId);
        if (oldRecord == null) {
            throw new Exception("Voyage not found");
        }
        bool isOwner = _auth.IsOwner(userId, oldRecord);
        if (!isOwner) {
            throw new UnauthorizedAccessException();
        }

        UpdateFields(oldRecord, updatedVoyage);
        try{
            _repository.UpdateVoyage(updatedVoyage);
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }


    public void DeleteVoyage(long userId, long voyageId)
    {
        Voyage? record = _repository.GetById(voyageId);
        if (record == null) {
            throw new Exception("Voyage not found");
        }
        bool isOwner = _auth.IsOwner(userId, record);
        System.Diagnostics.Debug.WriteLine(userId);
        System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(record, Formatting.Indented));
        if (!isOwner) {
            throw new UnauthorizedAccessException();
        }

        try {
            _repository.DeleteVoyage(record);
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    private void UpdateFields(Voyage oldRecord, Voyage updatedVoyage) {
        oldRecord.Topic = updatedVoyage.Topic;
        oldRecord.Description = updatedVoyage.Description;
        oldRecord.Notes = updatedVoyage.Notes;
        oldRecord.UpdatedAt = DateTime.Now;
        oldRecord.LocationLongitude = updatedVoyage.LocationLongitude;
        oldRecord.LocationLatitude = updatedVoyage.LocationLatitude;

    }


   
}

