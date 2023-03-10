using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Models.Identity;
using FoxtaurServer.Services.Abstract;
using FoxtaurServer.Services.Abstract.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace FoxtaurServer.Services.Implementations;

public class AccountsService : IAccountsService
{
    private readonly UserManager<User> _userManager;

    public AccountsService(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<UserRegistrationResult> RegisterUserAsync(RegistrationRequest request)
    {
        if (request == null)
        {
            return UserRegistrationResult.ErrGenericError;
        }
        
        var existingUser = await _userManager.FindByNameAsync(request.Login);
        if (existingUser != null)
        {
            return UserRegistrationResult.ErrLoginIsTaken;
        }

        var user = new User()  
        {  
            UserName = request.Login,
            Email = request.Email,
            SecurityStamp = Guid.NewGuid().ToString() // TODO: Is it secure?
        };  
        
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return UserRegistrationResult.ErrWeakPassword; // Most probably reason.
        }

        return UserRegistrationResult.OK;
    }
}