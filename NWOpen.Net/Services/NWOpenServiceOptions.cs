// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using System.Text.Json;
using System.Text.Json.Serialization;

namespace NWOpen.Net.Services;

public class NWOpenServiceOptions
{
    /// <summary>
    /// The base URL for the NWOpen API.
    /// </summary>
    /// <value>The default value is "https://nwopen-api.nwo.nl/NWOpen-API/api/Projects".</value>
    public string BaseUrl { get; set; } = "https://nwopen-api.nwo.nl/NWOpen-API/api/Projects";

    /// <summary>
    /// The options for the JSON serializer.
    /// </summary>
    /// <value>
    /// The default value is a new instance of <see cref="JsonSerializerOptions" /> with the following settings:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <see cref="System.Text.Json.JsonSerializerOptions.PropertyNamingPolicy" /> =
    /// <see cref="JsonNamingPolicy.SnakeCaseLower" />
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <see cref="JsonSerializerOptions.Converters" /> = { new <see cref="JsonStringEnumConverter" /> (
    /// <see cref="JsonNamingPolicy.SnakeCaseLower" />) }
    /// </description>
    /// </item>
    /// </list>
    /// </value>
    public JsonSerializerOptions JsonSerializerOptions { get; set; } = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower),
        },
    };
}
