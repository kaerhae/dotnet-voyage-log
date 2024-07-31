using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Interfaces;
public interface IUserService {
    public List<User> GetAll();
    public User GetById(long id);
    public User CreateUser(User newUser);
    public User UpdateUser(long userId, User updatedUser);
    public User DeleteUser(long userId);
}