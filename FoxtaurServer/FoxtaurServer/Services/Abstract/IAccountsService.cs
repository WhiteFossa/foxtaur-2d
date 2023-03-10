using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Services.Abstract.Models;
using FoxtaurServer.Services.Abstract.Models.Enums;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with accounts, like register/login and so on
/// </summary>
public interface IAccountsService
{
    /// <summary>
    /// Register a new user
    /// </summary>
    Task<UserRegistrationResult> RegisterUserAsync(RegistrationRequest request);

    /// <summary>
    /// Logs user in
    /// </summary>
    Task<LoginResult> LoginAsync(LoginRequest request);
}