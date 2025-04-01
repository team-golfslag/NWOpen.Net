// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using System.Text.Json.Serialization;

namespace NWOpen.Net.Models;

//project_id String (100) File number of the project. A unique combination of numbers
// and/or letters and/or punctuation marks by which the file is
// identified
// grant_id String (100)
// parent_project_id String (100) Programme, when there is a programme with underlying
// projects.
// title String (255) Title of the project.
// funding_scheme_id String (50) ID of the call under which the project falls.
// funding_scheme String (255) Name of the call under which the project falls.
// department String (255) NWO Domain under which the funding has been allocated to
// the project.
// sub_department String (255) NWO Sub-domain under which the funding has been allocated
// to the project.
//start_date Date (YYYYMM-DD)
// Actual start date of the project.
// end_date Date (YYYYMM-DD)
// Actual completion date of the project.
// summary_nl String (8000) Scientific summary of the project in Dutch.
// summary_en String (8000) Scientific summary of the project in English.
// summary_update Array Array details described in the next section, 2.2.3.1.
// project_members Array Array with details of the project mebers. Details in section 0.
// products Array Array with details of products orginated from the project.
// Details in section 0.
public class Project
{
    /// <summary>
    /// File number of the project. A unique combination of numbers and/or
    /// letters and/or punctuation marks by which the file is identified
    /// </summary>
    [JsonPropertyName("project_id")]
    public required string ProjectId { get; set; }

    [JsonPropertyName("grant_id")]
    public string? GrantId { get; set; }

    /// <summary>
    /// Programme, when there is a programme with underlying projects.
    /// </summary>
    [JsonPropertyName("parent_project_id")]
    public string? ParentProjectId { get; set; }

    /// <summary>
    /// Title of the project.
    /// </summary>
    [JsonPropertyName("title")]
    public required string Title { get; set; }

    /// <summary>
    /// ID of the call under which the project falls.
    /// </summary>
    [JsonPropertyName("funding_scheme_id")]
    public int? FundingSchemeId { get; set; }

    /// <summary>
    /// Name of the call under which the project falls.
    /// </summary>
    [JsonPropertyName("funding_scheme")]
    public required string FundingScheme { get; set; }

    /// <summary>
    /// NWO Domain under which the funding has been allocated to the project.
    /// </summary>
    [JsonPropertyName("department")]
    public required string Department { get; set; }

    /// <summary>
    /// NWO Sub-domain under which the funding has been allocated to the project.
    /// </summary>
    [JsonPropertyName("sub_department")]
    public required string SubDepartment { get; set; }

    /// <summary>
    /// Actual start date of the project.
    /// </summary>
    [JsonPropertyName("start_date")]
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Actual completion date of the project.
    /// </summary>
    [JsonPropertyName("end_date")]
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Scientific summary of the project in Dutch.
    /// </summary>
    [JsonPropertyName("summary_nl")]
    public required string SummaryNl { get; set; }

    /// <summary>
    /// Scientific summary of the project in English.
    /// </summary>
    [JsonPropertyName("summary_en")]
    public required string SummaryEn { get; set; }

    [JsonPropertyName("summary_updates")]
    public List<SummaryUpdate>? SummaryUpdates { get; set; }

    [JsonPropertyName("project_members")]
    public List<ProjectMember>? ProjectMembers { get; set; }

    [JsonPropertyName("products")]
    public List<Product>? Products { get; set; }
}
