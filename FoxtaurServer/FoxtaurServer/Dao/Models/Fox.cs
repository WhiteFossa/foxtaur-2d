namespace FoxtaurServer.Dao.Models;

/// <summary>
/// Fox in database
/// </summary>
public class Fox
{
    /// <summary>
    /// Fox ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Fox name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Fox frequency in Hz
    /// </summary>
    public double Frequency { get; set; }

    /// <summary>
    /// Fox code
    /// </summary>
    public string Code { get; set; }
}