using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Newtonsoft.Json;

namespace dotnet_voyage_log.Service;

public class VoyageService : IVoyageService
{
    private IVoyageRepository _repository;
    private ILogger<IVoyageService> _logger;

    private IAuthentication _auth;
    private IS3Service _s3Service;
    public VoyageService(IVoyageRepository repository, IAuthentication auth, IS3Service s3Service, ILogger<IVoyageService> logger){
        _repository = repository;
        _auth = auth;
        _s3Service = s3Service;
        _logger = logger;
    }

    public List<Voyage> GetAll()
    {
        return _repository.GetAll();
    }
    /// <summary>
    /// Takes a VoyageWithFiles object and if it contains images --> Uploads images and sets image keys to list.
    /// Then creates a insertable Voyage object with image key lists.
    /// </summary>
    /// <exception cref="Exception"></exception>
    /// <returns>
    /// List of keys
    /// </returns>
    public async Task<Voyage> CreateVoyage(long userId, VoyageWithFiles voyageWithFiles)
    {
        // Set ownership of voyage to uploader
        voyageWithFiles.UserId = userId;
        Voyage insertableVoyage = await InsertImagesAndCreateInsertableVoyage(voyageWithFiles);
        insertableVoyage.CheckVoyage();
        _repository.CreateVoyage(insertableVoyage);
        return insertableVoyage;
    }

    /// <summary>
    /// Takes userId, voyageId, and voyage-log, which will be updated. First checks if voyage-log exists,
    /// then checks if userId is owner of the voyage-log. Finally updates voyage-log.
    /// </summary>
    /// <exception cref="Exception"/>
    /// <exception cref="UnauthorizedAccessException"/>
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

    /// <summary>
    /// Takes userId and voyageId. Checks if userId is owner of the voyage-log. If is, deletes voyage-log.
    /// </summary>
    /// <exception cref="Exception">Not found</exception>
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

    /// <summary>
    /// Takes a VoyageWithFiles object and if it contains images --> Uploads images and sets image keys to list.
    /// Then creates a insertable Voyage object with image key lists.
    /// </summary>
    /// <exception cref="Exception"></exception>
    /// <returns>
    /// Voyage
    /// </returns>
    private async Task<Voyage> InsertImagesAndCreateInsertableVoyage(VoyageWithFiles voyageWithFiles)
    {
        List<string> imageKeys = new();
        if(voyageWithFiles.Images != null && voyageWithFiles.Images.Count > 0) {
           try {
            imageKeys = await InsertImagesToS3(voyageWithFiles.Images);
           } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
           }
        }

        return new Voyage {
            Topic = voyageWithFiles.Topic,
            Description = voyageWithFiles.Description,
            Notes = voyageWithFiles.Notes,
            LocationLatitude = voyageWithFiles.LocationLatitude,
            LocationLongitude = voyageWithFiles.LocationLongitude,
            RegionId = voyageWithFiles.RegionId,
            UserId = voyageWithFiles.UserId,
            Images = imageKeys
        };
        
    }

    private void UpdateFields(Voyage oldRecord, Voyage updatedVoyage) {
        oldRecord.Topic = updatedVoyage.Topic;
        oldRecord.Description = updatedVoyage.Description;
        oldRecord.Notes = updatedVoyage.Notes;
        oldRecord.UpdatedAt = DateTime.Now;
        oldRecord.LocationLongitude = updatedVoyage.LocationLongitude;
        oldRecord.LocationLatitude = updatedVoyage.LocationLatitude;
    }

    /// <summary>
    /// Inserts Voyage images to S3 storage. Returns imageKey list for fetching.
    /// </summary>
    /// <returns>
    /// List of keys
    /// </returns>
    private async Task<List<string>> InsertImagesToS3(List<IFormFile> files) {
        List<string> imageIds = new();
        foreach(IFormFile file in files) {
            try {
                var key = await _s3Service.UploadImage(file);
                imageIds.Add(key);
            } catch (Exception e) {
                _logger.LogError($"Error: {e.Message}");
                throw new Exception("Internal server error");
            }
        }

        return imageIds;
    }


   
}

