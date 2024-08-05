using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Service;

public class VoyageService : IVoyageService
{
    private IVoyageRepository _repository;
    private IUserRepository _userRepository;

    public VoyageService(IVoyageRepository repository, IUserRepository userRepository){
        _repository = repository;
        _userRepository = userRepository;
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
        bool isOwner = IsOwner(userId, oldRecord);
        if (!isOwner) {
            throw new UnauthorizedAccessException();
        }

        UpdateFields(oldRecord, updatedVoyage);
        try{
            _repository.UpdateVoyage(updatedVoyage);
        } catch (Exception e) {
            throw new Exception("Internal server error");
        }
    }


    public void DeleteVoyage(long userId, long voyageId)
    {
        Voyage? record = _repository.GetById(voyageId);
        if (record == null) {
            throw new Exception("Voyage not found");
        }
        bool isOwner = IsOwner(userId, record);
        if (!isOwner) {
            throw new UnauthorizedAccessException();
        }

        try {
            _repository.DeleteVoyage(record);
        } catch (Exception e) {
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

    private bool IsOwner(long userId, Voyage voyage) {
        User user = _userRepository.RetrieveSingleUserById(userId);
        if (user.AppRole == "admin") {
            return true;
        }
        if (voyage.UserId == user.Id) {
            return true;
        }

        return false;
    }

   
}

