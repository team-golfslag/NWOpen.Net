using NWOpen.Net.Models;

namespace NWOpen.Testing;

public class MetadataTests
{
    private readonly Metadata _metadata1 = new()
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
    };

    private readonly Metadata _metadata2 = new()
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
    };

    [Fact]
    public void Combine_ShouldCombineCount()
    {
        Metadata metadata = _metadata1.Combine(_metadata2);

        Assert.Equal(35, metadata.Count);
    }
}
