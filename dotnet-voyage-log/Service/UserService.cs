using BCrypt.Net;
using dotnet_voyage_log.Context;
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Newtonsoft.Json;


namespace dotnet_voyage_log.Service;

public class UserService : IUserService {

    private readonly ILogger<IUserService> _logger;
    private readonly IUserRepository _repository;
    private readonly ITokenGenerator _generator;
    private readonly IAuthentication _auth;
    public UserService(IUserRepository repository, ITokenGenerator generator, IAuthentication auth, ILogger<IUserService> logger) {
        _repository = repository;
        _generator = generator;
        _auth = auth;
        _logger = logger;
    }

    public string LoginUser(LoginUser user) {
        user.CheckLoginUser();
        User? isUser = _repository.RetrieveSingleUserByUsername(user.Username);
        if (isUser == null) {
            throw new Exception("User not found");
        }

        bool isCorrectPassword = _auth.IsValidPassword(isUser.PasswdHash, user.Password);
        if (!isCorrectPassword) {
            throw new Exception("Incorrect password");
        }
        
        return _generator.GenerateToken(isUser);
    }

    public List<User> GetAll() {
        try {
            return _repository.RetrieveAllUsers();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Error on fetching all users");
        }
    }

    public User GetById(long id) {
        User? user = _repository.RetrieveSingleUserById(id);
        if (user != null) {
            return user;
        }

        throw new Exception("User not found");
    }

    public User CreateAdminUser(SignupUser newUser) {
        /* Check if user exists already */
        User? isExisting = _repository.RetrieveSingleUserByUsername(newUser.Username);
        if (isExisting != null) {
            throw new Exception($"Username {newUser.Username} already exists");
        }
        /* In username field, removing case-insensitivity */
        User insertableUser = new User(){
            Username = newUser.Username.ToLower(),
            AppRole = "admin",
            Email = newUser.Email,
            PasswdHash = _auth.HashPassword(newUser.Password),
        };
        /* Check that contains all necessary fields */
        insertableUser.CheckUser();
        _repository.InsertUser(insertableUser);
        return insertableUser;
    }

    public User CreateNormalUser(SignupUser newUser) {
        /* Check if user exists already */
        User? isExisting = _repository.RetrieveSingleUserByUsername(newUser.Username);
        if (isExisting != null) {
            throw new Exception($"Username {newUser.Username} already exists");
        }
        /* In username field, removing case-insensitivity */
        User insertableUser = new User(){
            Username = newUser.Username.ToLower(),
            AppRole = "user",
            Email = newUser.Email,
            PasswdHash = _auth.HashPassword(newUser.Password),
        };
        /* Check that contains all necessary fields */
        insertableUser.CheckUser();
        _repository.InsertUser(insertableUser);
        return insertableUser;
    }



    public User UpdateUser(long userId, User updatedUser)
    {
        updatedUser.CheckUser();
        User? oldRecord = _repository.RetrieveSingleUserById(userId);
        if(oldRecord != null) {
            UpdateFields(oldRecord, updatedUser);
            _repository.UpdateAllFields(oldRecord);
            return oldRecord;
        }

        throw new Exception("User not found");
    }

    public User DeleteUser(long userId) 
    {
        User? user = _repository.RetrieveSingleUserById(userId);
        if (user != null) {
            _repository.DeleteUser(user);
            return user;
        }

        throw new Exception("User not found");
    }

    private void UpdateFields(User oldRecord, User updatedUser) {
        oldRecord.Username = updatedUser.Username.ToLower();
        oldRecord.Email = updatedUser.Email != null ? updatedUser.Email.ToLower() : "";
        oldRecord.AppRole = updatedUser.AppRole;
        oldRecord.PasswdHash = updatedUser.PasswdHash;
    }
}