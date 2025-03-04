using System.Text.Json.Serialization;

namespace NWOpen.Net.Models;


public class Metadata : ICombinable<Metadata>
{
    /// <summary>
    /// Name of the call.
    /// </summary>
    [JsonPropertyName("api_type")]
    public required string ApiType { get; set; }

    /// <summary>
    /// Version of the API.
    /// </summary>
    [JsonPropertyName("version")]
    public required string Version { get; set; }

    /// <summary>
    /// API release date.
    /// </summary>
    [JsonPropertyName("release_date")]
    public DateTime? ReleaseDate { get; set; }

    /// <summary>
    /// Crossref-ID of NWO.
    /// </summary>
    [JsonPropertyName("funder")]
    public required string Funder { get; set; }

    /// <summary>
    /// RORid of NWO.
    /// </summary>
    [JsonPropertyName("ror_id")]
    public required string RorId { get; set; }

    /// <summary>
    /// Date the API call was made.
    /// </summary>
    [JsonPropertyName("date")]
    public DateTime? Date { get; set; }

    /// <summary>
    /// Number of rows that make up the result.
    /// </summary>
    [JsonPropertyName("count")]
    public int? Count { get; set; }

    /// <summary>
    /// The maximum number of rows returned per call.
    /// </summary>
    [JsonPropertyName("per_page")]
    public int? PerPage { get; set; }

    /// <summary>
    /// Number of pages that make up the result.
    /// </summary>
    [JsonPropertyName("pages")]
    public int? Pages { get; set; }

    /// <summary>
    /// The returned page number.
    /// </summary>
    [JsonPropertyName("page")]
    public int? Page { get; set; }

    public Metadata Combine(Metadata other) => new()
    {
        ApiType = other.ApiType,
        Version = other.Version,
        ReleaseDate = other.ReleaseDate,
        Funder = other.Funder,
        RorId = other.RorId,
        Date = other.Date,
        Count = other.Count + Count,
        PerPage = other.PerPage,
        Pages = other.Pages,
        Page = other.Page
    };
}