// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using System.Text.Json.Serialization;

namespace NWOpen.Net.Models;

public class Product
{
    /// <summary>
    /// Title of publication.
    /// </summary>
    [JsonPropertyName("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Year of publication.
    /// </summary>
    [JsonPropertyName("year")]
    public int? Year { get; set; }

    /// <summary>
    /// Edition of the publication.
    /// </summary>
    [JsonPropertyName("edition")]
    public string? Edition { get; set; }

    /// <summary>
    /// Page in medium where the publication starts.
    /// </summary>
    [JsonPropertyName("start")]
    public int? Start { get; set; }

    /// <summary>
    /// Page in medium with the last page of the publication.
    /// </summary>
    [JsonPropertyName("end")]
    public int? End { get; set; }

    /// <summary>
    /// Type of the publication.
    /// </summary>
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Open access link to publication.
    /// </summary>
    [JsonPropertyName("url_open_access")]
    public string? UrlOpenAccess { get; set; }

    /// <summary>
    /// Title of the journal of publication.
    /// </summary>
    [JsonPropertyName("journal_title")]
    public string? JournalTitle { get; set; }

    /// <summary>
    /// An array with details of the authors of a product.
    /// </summary>
    [JsonPropertyName("authors")]
    public List<Author>? Authors { get; set; }

    /// <summary>
    /// City of publication.
    /// </summary>
    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("publisher")]
    public string? Publisher { get; set; }

    /// <summary>
    /// Subtitle of the publication.
    /// </summary>
    [JsonPropertyName("sub_title")]
    public string? SubTitle { get; set; }

    /// <summary>
    /// ISBN of the product.
    /// </summary>
    [JsonPropertyName("isbn")]
    public string? Isbn { get; set; }

    /// <summary>
    /// DOI of the product. DOI will become available when NWO data is enriched with DOI’s.
    /// </summary>
    [JsonPropertyName("doi")]
    public string? Doi { get; set; }
}
