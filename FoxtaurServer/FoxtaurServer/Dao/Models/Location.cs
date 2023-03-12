using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

    /// <summary>
    /// This location acts as start in those distances
    /// </summary>
    public List<Distance> AsStartInDistances { get; set; }
    
    /// <summary>
    /// This location acts as finish corridor entrances in those distances
    /// </summary>
    public List<Distance> AsFinishCorridorEntranceInDistances { get; set; }
    
    /// <summary>
    /// This location acts as finish in those distances
    /// </summary>
    public List<Distance> AsFinishLocationInDistances { get; set; }
    
    /// <summary>
    /// This location acts as fox in those distances
    /// </summary>
    public List<Distance> AsFoxLocationInDistances { get; set; }
    
    /// <summary>
    /// This location acts as expected fox order location in those distances
    /// </summary>
    public List<Distance> AsExpectedFoxOrderLocationInDistances { get; set; }
}