using LibWebClient.Models.Enums;

namespace FoxtaurTracker.Models;

/// <summary>
/// Body sex
/// </summary>
public class BodySexItem
{
    /// <summary>
    /// Sex id
    /// </summary>
    public BodySex Id { get; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Index in list
    /// </summary>
    public int Index { get; }

    public BodySexItem(BodySex id, string description, int index)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException(nameof(description));
        }

        Id = id;
        Description = description;
        Index = index;
    }
}