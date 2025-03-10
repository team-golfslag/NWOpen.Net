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
            Projects = Projects.Concat(other.Projects).ToList()
        };
}
