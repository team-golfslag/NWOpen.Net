using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NWOpen.Net;
using NWOpen.Net.Models;
using NWOpen.Net.Services;

namespace NWOpen.Testing;

public class NwOpenQueryBuilderTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly Mock<ILogger<NWOpenService>> _loggerMock;
    private readonly NWOpenService _nwOpenService;

    public NwOpenQueryBuilderTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        HttpClient httpClient = new(_httpMessageHandlerMock.Object);
        _loggerMock = new Mock<ILogger<NWOpenService>>();
        _nwOpenService = new NWOpenService(httpClient, _loggerMock.Object);
    }

    [Fact]
    public async Task Execute_ShouldReturnOrganizationsResult_WhenResponseIsValid()
    {
        Project organization = new()
        {
            ProjectId = "20447",
            GrantId = null,
            ParentProjectId = null,
            Title = "Hybrid protein-lipid nanoparticles for targeted oligonucleotide delivery in endometriosis (HYPNODE)",
            FundingSchemeId = 4851,
            FundingScheme = "Open technologieprogramma OTP 2022 2022-9",
            Department = "Toegepaste en Technische Wetenschappen",
            SubDepartment = "Toegepaste en Technische Wetenschappen",
            StartDate = new DateTime(2024, 3, 1),
            EndDate = null,
            SummaryNl = "summarynl",
            SummaryEn = "summaryen",
            SummaryUpdates = [],
            ProjectMembers = [],
            Products = [],
        };

        Metadata metadata = new()
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

        NWOpenResult organizationResult = new()
        {
            Metadata = metadata,
            Projects = [organization],
        };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(organizationResult),
            });

        NwOpenQueryBuilder queryBuilder = _nwOpenService
            .Query()
            .WithTitle("Test")
            .WithNumberOfResults(1);

        NWOpenResult? result = await queryBuilder.Execute();

        Assert.NotNull(result);
        Assert.Single(result.Projects);
        Assert.Equal("20447", result.Projects[0].ProjectId);
        Assert.Equal("Hybrid protein-lipid nanoparticles for targeted oligonucleotide delivery in endometriosis (HYPNODE)", result.Projects[0].Title);
    }

    [Fact]
    public async Task Execute_ShouldReturnNull_WhenResponseIsInvalid()
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

        NwOpenQueryBuilder queryBuilder = _nwOpenService
            .Query()
            .WithTitle("Test")
            .WithNumberOfResults(1);

        NWOpenResult? result = await queryBuilder.Execute();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task Execute_ShouldLogError_WhenRequestExceptionIsThrown()
    {
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException("Failed to get results from NWOpen"));

        NwOpenQueryBuilder queryBuilder = _nwOpenService
            .Query()
            .WithTitle("Test")
            .WithNumberOfResults(1);

        NWOpenResult? result = await queryBuilder.Execute();

        Assert.Null(result);
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to get results from NWOpen")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }

    [Fact]
    public void BuildQuery_ShouldThrowArgumentException_WhenNumberOfResultsIsZero()
    {
        NwOpenQueryBuilder queryBuilder = _nwOpenService.Query();

        Assert.Throws<ArgumentException>(() => queryBuilder.WithNumberOfResults(0));
    }

    [Fact]
    public void BuildQuery_ShouldThrowArgumentException_WhenNumberOfResultsIsSetMultipleTimes()
    {
        NwOpenQueryBuilder queryBuilder = _nwOpenService
            .Query()
            .WithNumberOfResults(1);

        Assert.Throws<InvalidOperationException>(() => queryBuilder.WithNumberOfResults(2));
    }

    [Fact]
    public void BuildQuery_ShouldThrowArgumentException_WhenQueryIsSetMultipleTimes()
    {
        NwOpenQueryBuilder queryBuilder = _nwOpenService
            .Query()
            .WithTitle("Test");

        Assert.Throws<InvalidOperationException>(() => queryBuilder.WithTitle("Test2"));
    }

    [Fact]
    public void BuildQuery_ShouldReturnCorrectQueryString()
    {
        NwOpenQueryBuilder queryBuilder = _nwOpenService
            .Query()
            .WithOrganization("Test")
            .WithTitle("Test")
            .WithNumberOfResults(1)
            .WithMemberLastName("Doe")
            .WithStartDateFrom(new DateTime(2020, 1, 1))
            .WithStartDateUntil(new DateTime(2021, 1, 1))
            .WithEndDateFrom(new DateTime(2021, 1, 1))
            .WithEndDateUntil(new DateTime(2022, 1, 1))
            .WithRole("role");

        string query = queryBuilder.BuildQueries()[0];

        Assert.Equal("per_page=1&organisation=%22Test%22&title=%22Test%22&role=%22role%22&last_name=%22Doe%22&rs_start_date=2020-01-01&re_start_date=2021-01-01&rs_end_date=2021-01-01&re_end_date=2022-01-01", query);
    }
}
