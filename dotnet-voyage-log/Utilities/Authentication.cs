using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using Newtonsoft.Json;
using BC = BCrypt.Net.BCrypt;

namespace dotnet_voyage_log.Utilities;
/// <summary>
/// Authentication. Constructor requires IUserRepository.
/// </summary>
public class Authentication : IAuthentication
{

    private IUserRepository _userRepository;

    public Authentication(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Password hash function.
    /// </summary>
    /// <returns>
    /// string: Hashed password
    /// </returns>
    public string  HashPassword(string plain)
    {
        string hash = BC.HashPassword(plain);
        return hash;
    }

    /// <summary>
    /// Check that plain password is valid by comparing hash from database.
    /// </summary>
    /// <returns>
    /// True or false
    /// </returns>
    public bool IsValidPassword(string hash, string plain)
    {
        bool isAuthenticated = BC.Verify(plain, hash);
        if (!isAuthenticated) {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Checks that user is the owner of Voyage object. Is checking that Voyage object contains userId.
    /// </summary>
    /// <returns>
    /// True or false
    /// </returns>
    /// /// <exception cref="UnauthorizedAccessException">
    /// </exception>
    public bool IsOwner(long userId, Voyage voyage) {
        User? user = _userRepository.RetrieveSingleUserById(userId);
        System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(user, Formatting.Indented));
        if (user == null) {
            throw new UnauthorizedAccessException();
        }
        if (user.AppRole == "admin") {
            return true;
        }
        if (voyage.UserId == user.Id) {
            return true;
        }

        return false;
    }
}