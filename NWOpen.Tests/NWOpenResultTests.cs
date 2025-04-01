// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using NWOpen.Net.Models;

namespace NWOpen.Tests;

public class NWOpenResultTests
{
    private readonly NWOpenResult _result1 = new()
    {
        Metadata = new()
        {
            ApiType = "NWO Projects API",
            Version = "1.0.1",
            ReleaseDate = new DateTime(2024, 5, 2),
            Funder = "501100003246",
            RorId = "https://ror.org/04jsz6e67",
            Date = new DateTime(2024, 6, 4),
            Count = 26,
            PerPage = 10,
            Pages = 3,
            Page = 2,
        },
        Projects = [],
    };

    private readonly NWOpenResult _result2 = new()
    {
        Metadata = new()
        {
            ApiType = "NWO Projects API",
            Version = "1.0.1",
            ReleaseDate = new DateTime(2024, 5, 2),
            Funder = "501100003246",
            RorId = "https://ror.org/04jsz6e67",
            Date = new DateTime(2024, 6, 4),
            Count = 9,
            PerPage = 10,
            Pages = 1,
            Page = 1,
        },
        Projects = [],
    };

    [Fact]
    public void Combine_ShouldCombineMetadata()
    {
        NWOpenResult result = _result1.Combine(_result2);

        Assert.Equal(35, result.Metadata.Count);
    }
}
