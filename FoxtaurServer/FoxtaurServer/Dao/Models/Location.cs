using System.ComponentModel.DataAnnotations;
using FoxtaurServer.Dao.Models.Enums;

namespace FoxtaurServer.Dao.Models;

/// <summary>
/// Location, such as start, fox or finish
/// </summary>
public class Location
{
    /// <summary>
    /// Location ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Location name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Location type
    /// </summary>
    public LocationType Type { get; set; }

    /// <summary>
    /// Latitude
    /// </summary>
    public double Lat { get; set; }

    /// <summary>
    /// Longitude
    /// </summary>
    public double Lon { get; set; }

    /// <summary>
    /// Fox, installed in this location.
    /// Valid only if Type == Fox, otherwise must be null.
    /// </summary>
    public Fox Fox { get; set; }
}