namespace LibWebClient.Models.Enums;

/// <summary>
/// Result for registration on a distance
/// </summary>
public enum RegistrationOnDistanceResult
{
    Success = 0,
    
    AlreadyRegistered = 1,
    
    DistanceNotFound = 2,
    
    Failure = 3
}