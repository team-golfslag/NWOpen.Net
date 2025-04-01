// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using System.Text.Json.Serialization;

namespace NWOpen.Net.Models;

public class NWOpenResult : ICombinable<NWOpenResult>
{
    [JsonPropertyName("meta")]
    public required Metadata Metadata { get; set; }

    [JsonPropertyName("projects")]
    public required List<Project> Projects { get; set; }

    public NWOpenResult Combine(NWOpenResult other) =>
        new()
        {
            Metadata = Metadata.Combine(other.Metadata),
            Projects = Projects.Concat(other.Projects).ToList(),
        };
}
