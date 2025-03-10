using System.Text.Json.Serialization;

namespace NWOpen.Net.Models;

public class SummaryUpdate
{
    /// <summary>
    /// Date of submission summary update.
    /// </summary>
    [JsonPropertyName("submission_date")]
    public DateTime? SubmissionDate { get; set; }

    /// <summary>
    /// Update of the public summary of the project in Dutch. In the case de public summary is empty, the scientific summary
    /// will be returned.
    /// </summary>
    [JsonPropertyName("update_en")]
    public required string UpdateEn { get; set; }

    /// <summary>
    /// Update of the public summary of the project in English.
    /// </summary>
    [JsonPropertyName("update_nl")]
    public required string UpdateNl { get; set; }
}
