namespace FoxtaurServer.Models.Api.Enums;

/// <summary>
/// Result for registration on distance
/// </summary>
public enum RegistrationOnDistanceResult
{
    Success = 0,
    
    AlreadyRegistered = 1,
    
    DistanceNotFound = 2,
    
    Failure = 3
}