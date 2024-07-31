using dotnet_voyage_log.Context;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Newtonsoft.Json;

namespace dotnet_voyage_log.Service;
public class UserService : IUserService {

    private readonly IUserRepository _repository;
    public UserService(IUserRepository repository) {
        _repository = repository;
    }

    public List<User> GetAll() {
        try {
            return _repository.RetrieveAllUsers();
        } catch (Exception e) {
            System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(e, Formatting.Indented));
            Console.WriteLine(JsonConvert.SerializeObject(e, Formatting.Indented));

            throw new Exception("Error on fetching all users");
        }
    }

    public User GetById(long id) {
        User user = _repository.RetrieveSingleUserById(id);
        if (user != null) {
            return user;
        }

        throw new Exception("User not found");
    }

    public User CreateUser(User newUser) {
        ValidateUserInput(newUser);
        _repository.InsertUser(newUser);
        return newUser;

    }

    public User UpdateUser(long userId, User updatedUser)
    {
        ValidateUserInput(updatedUser);
        User oldRecord = _repository.RetrieveSingleUserById(userId);
        if(oldRecord != null) {
            UpdateFields(oldRecord, updatedUser);
            _repository.UpdateAllFields(oldRecord);
            return oldRecord;
        }

        throw new Exception("User not found");
    }

    public User DeleteUser(long userId) 
    {
        User user = _repository.RetrieveSingleUserById(userId);
        if (user != null) {
            _repository.DeleteUser(user);
            return user;
        }

        throw new Exception("User not found");
    }

    private void ValidateUserInput(User newUser) {
        if(newUser.Username == "") {
            throw new Exception("Username missing");
        }
        if(newUser.PasswdHash == "") {
            throw new Exception("Username missing");
        }
    }

    private void UpdateFields(User oldRecord, User updatedUser) {
        oldRecord.Username = updatedUser.Username;
        oldRecord.Email = updatedUser.Email;
        oldRecord.AppRole = updatedUser.AppRole;
        oldRecord.PasswdHash = updatedUser.PasswdHash;
    }
}