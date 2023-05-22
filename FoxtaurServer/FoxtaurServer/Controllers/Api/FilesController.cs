using FoxtaurServer.Models;
using FoxtaurServer.Models.Api.Enums;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with files
/// </summary>
[Authorize]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IFilesService _filesService;

    public FilesController(IFilesService filesService)
    {
        _filesService = filesService;
    }
    
    /// <summary>
    /// Download given file
    /// </summary>
    [Route("api/Files/Download")]
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> DownloadAsync(Guid fileId, DownloadFileType type)
    {
        var file = await _filesService.GetFileToDownloadAsync(fileId, type);

        if (!file.IsFound)
        {
            return NotFound();
        }

        return File(file.Content,
            "application/zstd",
            file.LastModified,
            new EntityTagHeaderValue($"\"{ file.Hash }\""));
    }
}