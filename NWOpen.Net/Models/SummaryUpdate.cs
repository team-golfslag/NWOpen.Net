// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

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
    public string? UpdateEn { get; set; }

    /// <summary>
    /// Update of the public summary of the project in English.
    /// </summary>
    [JsonPropertyName("update_nl")]
    public string? UpdateNl { get; set; }
}
