namespace FoxtaurServer.Services.Abstract.Models.Enums;

/// <summary>
/// Possible user registration results
/// </summary>
public enum UserRegistrationResult
{
    OK,
    
    ErrLoginIsTaken,
    
    ErrWeakPassword,
    
    ErrGenericError
}