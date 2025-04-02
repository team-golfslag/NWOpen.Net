// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using System.Text.Json.Serialization;

namespace NWOpen.Net.Models;

public class ProjectMember
{
    /// <summary>
    /// Role of the project member within the project. Possible returned
    /// </summary>
    [JsonPropertyName("role")]
    public string? Role { get; set; }

    /// <summary>
    /// ID for the project member.
    /// </summary>
    [JsonPropertyName("member_id")]
    public int? MemberId { get; set; }

    /// <summary>
    /// ORCID of the projectmember. Will become available as soon as
    /// NWO’s data is enriched with ORCID.
    /// </summary>
    [JsonPropertyName("orcid")]
    public string? Orcid { get; set; }

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
    [JsonPropertyName("organisation")]
    public string? Organisation { get; set; }

    /// <summary>
    /// Organization where the project member is working for the project.
    /// </summary>
    [JsonPropertyName("organisation_id")]
    public int? OrganisationId { get; set; }

    /// <summary>
    /// Indication whether the project member is still working on the project.
    /// </summary>
    [JsonPropertyName("active")]
    public bool? Active { get; set; }

    /// <summary>
    /// Pre nominal titles.
    /// </summary>
    [JsonPropertyName("degree_pre_nominal")]
    public string? DegreePreNominal { get; set; }

    /// <summary>
    /// Insert before the name.
    /// </summary>
    [JsonPropertyName("prefix")]
    public string? Prefix { get; set; }

    /// <summary>
    /// Post nominal titles
    /// </summary>
    [JsonPropertyName("degree_post_nominal")]
    public string? DegreePostNominal { get; set; }

    /// <summary>
    /// ROR-id of the organisation the projectmember is attending the project for.
    /// </summary>
    [JsonPropertyName("dai")]
    public string? Dai { get; set; }

    /// <summary>
    /// ROR-id of the organisation the projectmember is attending the project for.
    /// </summary>
    [JsonPropertyName("ror")]
    public string? Ror { get; set; }
}
