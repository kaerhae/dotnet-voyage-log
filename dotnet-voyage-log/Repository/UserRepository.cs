using dotnet_voyage_log.Context;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;

namespace dotnet_voyage_log.Repository;

public class UserRepository : IUserRepository
{
    private DataContext _context;
    private readonly ILogger<UserRepository> _logger;


    public UserRepository(DataContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all users from the database with voyages field one-to-many relationship
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// List of User
    /// </returns>
    public List<User> RetrieveAllUsers() {
        try {
            return _context.Users.Select(u => new User{
            Id = u.Id,
            Username = u.Username,
            AppRole = u.AppRole,
            Voyages = u.Voyages
                    .Select(v => new Voyage{
                        Id = v.Id,
                        Topic = v.Topic,
                        Description = v.Description,
                        Notes = v.Notes,
                        Images = v.Images,
                        CreatedAt = v.CreatedAt,
                        UpdatedAt = v.UpdatedAt,
                        LocationLatitude = v.LocationLatitude,
                        LocationLongitude = v.LocationLongitude,
                        RegionId = v.RegionId,
                        }).ToList()

        }).ToList();
        } catch (Exception e) {
             _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    /// <summary>
    /// Retrieves single user by id from database.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// Nullable User
    /// </returns>
    public User? RetrieveSingleUserById(long id) {
        try {
            return _context.Users.Where(x => x.Id == id).FirstOrDefault();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    /// <summary>
    /// Retrieves single user by name from database.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// Nullable User
    /// </returns>
    public User? RetrieveSingleUserByUsername(string username) {
        try {
            return _context.Users.Where(x => x.Username.ToLower() == username.ToLower()).FirstOrDefault();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception($"Internal server error");
        }
    }

    /// <summary>
    /// Takes User object and insert it to database.
    /// </summary>
    /// <exception cref="Exception" />
    public void InsertUser(User newUser){
       try {
            _context.Users.Add(newUser);
            _context.SaveChanges();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    /// <summary>
    /// Takes User object and inserts with updated fields to existing user.
    /// </summary>
    /// <exception cref="Exception" />
    public void UpdateAllFields(User updatedUser) {
        try {
            _context.Users.Update(updatedUser);
            _context.SaveChanges();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

    /// <summary>
    /// Takes User object and removes it from database.
    /// </summary>
    /// <exception cref="Exception" />
    public void DeleteUser(User user) {
        try {
            _context.Users.Remove(user);
            _context.SaveChanges();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Internal server error");
        }
    }

}