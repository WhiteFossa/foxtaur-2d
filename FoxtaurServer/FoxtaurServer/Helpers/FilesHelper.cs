using System.Security.Cryptography;

namespace FoxtaurServer.Helpers;

/// <summary>
/// Useful stuff to work with files
/// </summary>
public static class FilesHelper
{
    /// <summary>
    /// Calculate SHA512 hash of file contents (result is Base64 encoded)
    /// </summary>
    public static string CalculateSHA512(byte[] data)
    {
        var calculator = new SHA512Managed();
        var resultBytes = calculator.ComputeHash(data);

        return Convert.ToBase64String(resultBytes, Base64FormattingOptions.None);
    }
}