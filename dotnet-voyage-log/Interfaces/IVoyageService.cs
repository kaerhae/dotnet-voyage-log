using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Interfaces;

public interface IVoyageService {
    public List<Voyage> GetAll();
    public void CreateVoyage(long userId, Voyage newVoyage);
    public void UpdateVoyage(long userId, long voyageId, Voyage updatedVoyage);
    public void DeleteVoyage(long userId, long voyageId);
}