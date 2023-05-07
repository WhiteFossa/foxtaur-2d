namespace FoxtaurServer.Models.Api;

public class MapFileDto
{
    /// <summary>
    /// Map file Id
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// User-friendly file name
    /// </summary>
    public string Name { get; }

    public MapFileDto
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