namespace LibWebClient.Models;

/// <summary>
/// Map
/// </summary>
public class Map
{
    /// <summary>
    /// Map Id
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Bounds - north latitude
    /// </summary>
    public double NorthLat { get; }

    /// <summary>
    /// Bounds - south latitude
    /// </summary>
    public double SouthLat { get; }

    /// <summary>
    /// Bounds - east longitude
    /// </summary>
    public double EastLon { get; }

    /// <summary>
    /// Bounds - west longitude
    /// </summary>
    public double WestLon { get; }

    /// <summary>
    /// Map file ID
    /// </summary>
    public Guid FileId { get; }
    
    public Map(Guid id,
        string name,
        double northLat,
        double southLat,
        double eastLon,
        double westLon,
        Guid fileId
    )
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        Id = id;
        Name = name;
        NorthLat = northLat;
        SouthLat = southLat;
        EastLon = eastLon;
        WestLon = westLon;
        FileId = fileId;
    }
}