// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using NWOpen.Net.Exceptions;
using NWOpen.Net.Models;
using NWOpen.Net.Services;

namespace NWOpen.Tests;

public class NwOpenServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly NWOpenService _nwOpenService;

    public NwOpenServiceTests()
    {
        _httpMessageHandlerMock = new();

        HttpClient httpClient = new(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new("https://api.nwopen.example")
        };

        var optionsMock = new Mock<IOptions<NWOpenServiceOptions>>();
        optionsMock
            .Setup(o => o.Value)
            .Returns(new NWOpenServiceOptions
            {
                BaseUrl = "https://api.nwopen.example",
                JsonSerializerOptions = new()
                {
                    PropertyNameCaseInsensitive = true
                }
            });

        var loggerMock = new Mock<ILogger<NWOpenService>>();

        _nwOpenService = new(
            httpClient,
            optionsMock.Object,
            loggerMock.Object
        );
    }

    [Fact]
    public async Task GetOrganization_ShouldReturnOrganization_WhenResponseIsValid()
    {
        Project project = new()
        {
            ProjectId = "20447",
            GrantId = null,
            ParentProjectId = null,
            Title =
                "Hybrid protein-lipid nanoparticles for targeted oligonucleotide delivery in endometriosis (HYPNODE)",
            FundingSchemeId = 4851,
            FundingScheme = "Open technologieprogramma OTP 2022 2022-9",
            Department = "Toegepaste en Technische Wetenschappen",
            SubDepartment = "Toegepaste en Technische Wetenschappen",
            StartDate = new DateTime(2024, 3, 1, 0,0,0, DateTimeKind.Utc),
            EndDate = null,
            SummaryNl = "summarynl",
            SummaryEn = "summaryen",
            SummaryUpdates = [],
            ProjectMembers =
            [
                new ProjectMember
                {
                    Role = "Researcher",
                    MemberId = 557530,
                    Orcid = null,
                    LastName = "Mathur",
                    Initials = "A.",
                    FirstName = "Aanchal",
                    Organisation = "Radboud universitair medisch centrum",
                    OrganisationId = 59105,
                    Active = true,
                    DegreePreNominal = null,
                    DegreePostNominal = null,
                    Dai = null,
                    Ror = null,
                },
            ],
            Products =
            [
                new Product
                {
                    Title = "The Posttransit Tail of WASP-107b Observed at 10830 Å",
                    Year = 2021,
                    Edition = "Volume 162, Issue 6",
                    Start = null,
                    End = null,
                    Type = "Wetenschappelijk artikel",
                    UrlOpenAccess = "10.3847/1538-3881/ac178a",
                    JournalTitle = "The Astronomical Journal",
                    Authors =
                    [
                        new Author
                        {
                            LastName = "Spake",
                            Initials = "J.J.",
                            FirstName = null,
                            Prefix = null,
                            Role = "Auteur",
                            IndexNumber = null,
                        },
                    ],
                    City = null,
                    Publisher = null,
                    SubTitle = null,
                    Isbn = null,
                    Doi = null,
                },
            ],
        };

        NWOpenResult mockResult = new()
        {
            Projects = [project],
            Metadata = new()
            {
                ApiType = "NWO Projects API",
                Version = "1.0.1",
                ReleaseDate = new DateTime(2024, 5, 2, 0,0,0, DateTimeKind.Utc),
                Funder = "501100003246",
                RorId = "https://ror.org/04jsz6e67",
                Date = new DateTime(2024, 6, 4, 0,0,0, DateTimeKind.Utc),
                Count = 26,
                PerPage = 10,
                Pages = 3,
                Page = 2,
            },
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(mockResult),
            });

        Project? result = await _nwOpenService.GetProjectAsync("20447");

        Assert.NotNull(result);
        Assert.Equal("20447", result.ProjectId);
        Assert.Equal(
            "Hybrid protein-lipid nanoparticles for targeted oligonucleotide delivery in endometriosis (HYPNODE)",
            result.Title);
    }

    [Fact]
    public async Task GetOrganization_ShouldLogError_WhenRequestExceptionIsThrown()
    {
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Failed to get results from NWOpen"));
        
        await Assert.ThrowsAsync<NWOpenException>(() => _nwOpenService.GetProjectAsync("123"));
    }

    [Fact]
    public async Task GetOrganization_ShouldLogError_WhenResponseIsInvalid()
    {
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Invalid JSON"),
            });
        
        await Assert.ThrowsAsync<NWOpenException>(() => _nwOpenService.GetProjectAsync("123"));
    }
}
