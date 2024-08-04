using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Interfaces;
public interface IUserService {
    string LoginUser(LoginUser user);
    public List<User> GetAll();
    public User GetById(long id);
    public User CreateAdminUser(SignupUser newUser);
    public User CreateNormalUser(SignupUser newUser);
    public User UpdateUser(long userId, User updatedUser);
    public User DeleteUser(long userId);
}