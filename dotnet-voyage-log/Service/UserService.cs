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

    /// <summary>
    /// Takes LoginUser object, checks if user exists, then validates password, and finally
    /// generates token.
    /// </summary>
    /// <exception cref="Exception" />
    /// <exception cref="UnauthorizedAccessException"/>
    /// <returns>
    /// Token as string
    /// </returns>
    public string LoginUser(LoginUser user) {
        user.CheckLoginUser();
        User? isUser = _repository.RetrieveSingleUserByUsername(user.Username);
        if (isUser == null) {
            throw new Exception("User not found");
        }

        bool isCorrectPassword = _auth.IsValidPassword(isUser.PasswdHash, user.Password);
        if (!isCorrectPassword) {
            throw new UnauthorizedAccessException();
        }
        
        return _generator.GenerateToken(isUser);
    }
    /// <summary>
    /// Retrieves all existing users from database.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// Users as list
    /// </returns>
    public List<User> GetAll() {
        try {
            return _repository.RetrieveAllUsers();
        } catch (Exception e) {
            _logger.LogError($"Error: {e.Message}");
            throw new Exception("Error on fetching all users");
        }
    }
    /// <summary>
    /// Takes id and retrieves single user by id. If exists, will return it. If not, returns exception.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// User
    /// </returns>
    public User GetById(long id) {
        User? user = _repository.RetrieveSingleUserById(id);
        if (user != null) {
            return user;
        }

        throw new Exception("User not found");
    }
    /// <summary>
    /// Takes SignupUser and creates new admin user. Throws error if username exists.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// User
    /// </returns>
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
    /// <summary>
    /// Takes SignupUser and creates new normal user. Throws error if username exists.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// User
    /// </returns>
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

    /// <summary>
    /// Takes userId and User object. If user exists, updates fields with parameter User properties.
    /// Throws error if username does not exist.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// User
    /// </returns>
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

    /// <summary>
    /// Takes userId and checks if it exists. If it exists, will delete it. 
    /// Throws error if username does not exist.
    /// </summary>
    /// <exception cref="Exception" />
    /// <returns>
    /// User
    /// </returns>
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