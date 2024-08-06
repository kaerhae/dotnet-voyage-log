using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Interfaces;

public interface IVoyageRepository {
    public List<Voyage> GetAll();
    public Voyage? GetById(long id);
    public void CreateVoyage(Voyage newVoyage);
    public void UpdateVoyage(Voyage updatedVoyage);
    public void DeleteVoyage(Voyage voyage);
}