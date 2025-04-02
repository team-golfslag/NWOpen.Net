// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using System.Text.Json.Serialization;

namespace NWOpen.Net.Models;

public class Author
{
    /// <summary>
    /// Last name of the project member.
    /// </summary>
    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

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
    public string? Role { get; set; }

    /// <summary>
    /// Index number of the author.
    /// </summary>
    [JsonPropertyName("index_number")]
    public int? IndexNumber { get; set; }
}
