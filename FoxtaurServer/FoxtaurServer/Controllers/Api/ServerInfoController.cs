using FoxtaurServer.Constants;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller, returning basic server information
/// </summary>
[Authorize]
[ApiController]
public class ServerInfoController : ControllerBase
{
    private readonly IConfigurationService _configurationService;

    public ServerInfoController(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
    }
    
    /// <summary>
    /// Get server info
    /// </summary>
    [AllowAnonymous]
    [Route("api/ServerInfo")]
    [Route("api/ServerInfo/Index")]
    [HttpGet]
    public async Task<ServerInfoDto> Index()
    {
        var serverName = await _configurationService.GetConfigurationString(GlobalConstants.ServerNameSettingName);
        var serverInfo = new ServerInfoDto(serverName, GlobalConstants.ProtocolVersion);

        return serverInfo;
    }
}