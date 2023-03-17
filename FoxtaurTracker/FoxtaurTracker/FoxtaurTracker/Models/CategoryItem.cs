using LibWebClient.Models.Enums;

namespace FoxtaurTracker.Models;

public class CategoryItem
{
    /// <summary>
    /// Category Id
    /// </summary>
    public Category Id { get; }

    /// <summary>
    /// Description
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Index in list
    /// </summary>
    public int Index { get; }
    
    public CategoryItem(Category id, string description, int index)
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