using System.Text.Json.Serialization;

namespace NWOpen.Net.Models;

public class Author
{
    /// <summary>
    /// Last name of the project member.
    /// </summary>
    [JsonPropertyName("last_name")]
    public required string LastName { get; set; }

    /// <summary>
    /// Initials of the project member.
    /// </summary>
    [JsonPropertyName("initials")]
    public string? Initials { get; set; }

    /// <summary>
    /// First name project member.
    /// </summary>
    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    /// <summary>
    /// Insert before the name.
    /// </summary>
    [JsonPropertyName("prefix")]
    public string? Prefix { get; set; }

    [JsonPropertyName("role")]
    public required string Role { get; set; }

    /// <summary>
    /// Index number of the author.
    /// </summary>
    [JsonPropertyName("index_number")]
    public int? IndexNumber { get; set; }
}
