using dotnet_voyage_log.Context;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Repository;

public class UserRepository : IUserRepository
{
    private DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public List<User> RetrieveAllUsers() {
        try {
            List<User> users = _context.Users.ToList();
            return users;
        } catch (Exception e) {
            throw new Exception("Internal server error");
        }
    }

    public User RetrieveSingleUserById(long id) {
        try {
            return _context.Users.Where(x => x.Id == id).First();
        } catch (Exception e) {
            throw new Exception("Internal server error");
        }
    }

    public void InsertUser(User newUser){
       try {
            _context.Users.Add(newUser);
            _context.SaveChanges();
        } catch (Exception e) {
            throw new Exception("Internal server error");
        }
    }

    public void UpdateAllFields(User updatedUser) {
        try {
            _context.Users.Update(updatedUser);
            _context.SaveChanges();
        } catch (Exception e) {
            throw new Exception("Internal server error");
        }
    }

    public void DeleteUser(User user) {
        try {
            _context.Users.Remove(user);
            _context.SaveChanges();
        } catch (Exception e) {
            throw new Exception("Internal server error");
        }
    }

}