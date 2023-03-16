using System.IdentityModel.Tokens.Jwt;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Models.Identity;
using FoxtaurServer.Services.Abstract;
using FoxtaurServer.Services.Abstract.Models;
using FoxtaurServer.Services.Abstract.Models.Enums;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using FoxtaurServer.Constants;
using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using FoxtaurServer.Dao.Models.Enums;
using FoxtaurServer.Models.Api;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace FoxtaurServer.Services.Implementations;

public class AccountsService : IAccountsService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfigurationService _configurationService;
    private readonly IProfilesDao _profilesDao;

    public AccountsService(UserManager<User> userManager,
        IConfigurationService configurationService,
        IProfilesDao profilesDao)
    {
        _userManager = userManager;
        _configurationService = configurationService;
        _profilesDao = profilesDao;
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
        
        // Preparing new, empty profile for user
        var newProfile = new Profile()
        {
            Id = user.Id,
            FirstName = GlobalConstants.NewProfileFirstName,
            MiddleName = GlobalConstants.NewProfileMiddleName,
            LastName = GlobalConstants.NewProfileLastName,
            Sex = GlobalConstants.NewProfileSex,
            DateOfBirth = GlobalConstants.NewProfileDateOfBirth,
            Phone = GlobalConstants.NewProfilePhone,
            Team = null,
            Category = GlobalConstants.NewProfileCategory,
            ColorR = GlobalConstants.NewProfileColorR,
            ColorG = GlobalConstants.NewProfileColorG,
            ColorB = GlobalConstants.NewProfileColorB,
            ColorA = GlobalConstants.NewProfileColorA
        };

        await _profilesDao.CreateAsync(newProfile);

        return UserRegistrationResult.OK;
    }

    public async Task<LoginResult> LoginAsync(LoginRequest request)
    {
        if (request == null)
        {
            return new LoginResult(false, String.Empty, DateTime.MinValue);
        }
        
        var user = await _userManager.FindByNameAsync(request.Login);
        if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))  
        {  
            var userRoles = await _userManager.GetRolesAsync(user);  
  
            var authClaims = new List<Claim>  
            {  
                new Claim(ClaimTypes.Name, user.UserName),  
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  
            };  
  
            foreach (var userRole in userRoles)  
            {  
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));  
            }  
  
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(await _configurationService.GetConfigurationString(GlobalConstants.JwtSecretSettingName)));  
  
            var token = new JwtSecurityToken(
                await _configurationService.GetConfigurationString(GlobalConstants.JwtValidIssuerSettingName),
                await _configurationService.GetConfigurationString(GlobalConstants.JwtValidAudienceSettingName),
                expires: DateTime.UtcNow.AddHours(GlobalConstants.JwtLifetime),  
                claims: authClaims,  
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)  
            );  
  
            return new LoginResult(true, new JwtSecurityTokenHandler().WriteToken(token), token.ValidTo);
        }  
        
        return new LoginResult(false, String.Empty, DateTime.MinValue);
    }

    public async Task<UserInfoDto> GetUserInfoAsync(string login)
    {
        var user = await _userManager.FindByNameAsync(login);

        if (user == null)
        {
            return null;
        }

        return new UserInfoDto(Guid.Parse(user.Id), user.UserName, user.Email);
    }
}