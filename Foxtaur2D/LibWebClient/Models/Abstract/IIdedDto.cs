namespace LibWebClient.Models.Abstract;

/// <summary>
/// DTO with GUID ID
/// </summary>
public interface IIdedDto
{
    // ID
    Guid Id { get; set; }
}