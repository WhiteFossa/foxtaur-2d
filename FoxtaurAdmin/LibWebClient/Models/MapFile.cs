namespace LibWebClient.Models;

/// <summary>
/// Map file model
/// </summary>
public class MapFile
{
    /// <summary>
    /// Map file Id
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// User-friendly file name
    /// </summary>
    public string Name { get; }

    public MapFile
    (
        Guid id,
        string name
    )
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        Id = id;
        Name = name;
    }
}