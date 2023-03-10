using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Models.Identity;
using FoxtaurServer.Services.Abstract;
using FoxtaurServer.Services.Abstract.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller for registration, login and so on
/// </summary>
[ApiController]
public class AccountsController : ControllerBase
{
    private readonly IAccountsService _accountsService;

    public AccountsController(IAccountsService accountsService)
    {
        _accountsService = accountsService;
    }

    /// <summary>
    /// User registration
    /// </summary>
    [HttpPost]  
    [Route("api/Accounts/Register")]  
    public async Task<IActionResult> Register([FromBody]RegistrationRequest request)  
    {
        if (request == null)
        {
            return BadRequest();
        }

        var result = await _accountsService.RegisterUserAsync(request);

        switch (result)
        {
            case UserRegistrationResult.OK:
                return Ok("User created.");
            
            case UserRegistrationResult.ErrLoginIsTaken:
                return UnprocessableEntity($"Login {request.Login} is already taken.");
            
            case UserRegistrationResult.ErrWeakPassword:
                return UnprocessableEntity($"Password is too weak.");
            
            case UserRegistrationResult.ErrGenericError:
                return StatusCode(StatusCodes.Status500InternalServerError, $"Generic error during user registration.");
                
            default:
                throw new ArgumentException("Invalid UserRegistrationResult.");
        }
    } 
}