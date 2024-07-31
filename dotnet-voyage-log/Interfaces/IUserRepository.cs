using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Interfaces;

public interface IUserRepository {
    public List<User> RetrieveAllUsers();
    public User RetrieveSingleUserById(long id);
    public void InsertUser(User newUser);
    public void UpdateAllFields(User updatedUser);
    public void DeleteUser(User user);
}
