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
using NWOpen.Net;
using NWOpen.Net.Exceptions;
using NWOpen.Net.Models;
using NWOpen.Net.Services;

namespace NWOpen.Tests;

public class NwOpenQueryBuilderTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly NWOpenService _nwOpenService;

    public NwOpenQueryBuilderTests()
    {
        _httpMessageHandlerMock = new();

        HttpClient httpClient = new(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new("https://api.nwopen.example"),
        };

        var optionsMock = new Mock<IOptions<NWOpenServiceOptions>>();
        optionsMock
            .Setup(o => o.Value)
            .Returns(new NWOpenServiceOptions
            {
                BaseUrl = "https://api.nwopen.example",
                JsonSerializerOptions = new()
                {
                    PropertyNameCaseInsensitive = true,
                },
            });

        var loggerMock = new Mock<ILogger<NWOpenService>>();

        _nwOpenService = new(
            httpClient,
            optionsMock.Object,
            loggerMock.Object
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldReturnOrganizationsResult_WhenResponseIsValid()
    {
        // Arrange
        Project organization = new()
        {
            ProjectId = "20447",
            Title = "Hybrid protein-lipid nanoparticles for targeted oligonucleotide delivery in endometriosis (HYPNODE)",
            FundingSchemeId = 4851,
            FundingScheme = "Open technologieprogramma OTP 2022 2022-9",
            Department = "Toegepaste en Technische Wetenschappen",
            SubDepartment = "Toegepaste en Technische Wetenschappen",
            StartDate = new DateTime(2024, 3, 1, 0,0,0, DateTimeKind.Utc),
            SummaryNl = "summarynl",
            SummaryEn = "summaryen",
        };

        Metadata metadata = new()
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
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(organizationResult),
            });

        INWOpenQueryBuilder queryBuilder = _nwOpenService
            .Query()
            .WithTitle("Test")
            .WithNumberOfResults(1);

        // Act
        NWOpenResult? result = await queryBuilder.ExecuteAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Single(result.Projects);
        Assert.Equal("20447", result.Projects[0].ProjectId);
        Assert.Equal(
            "Hybrid protein-lipid nanoparticles for targeted oligonucleotide delivery in endometriosis (HYPNODE)",
            result.Projects[0].Title
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowOnInvalidJson()
    {
        // Arrange: valid status but invalid JSON body
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("Not JSON"),
            });

        INWOpenQueryBuilder queryBuilder = _nwOpenService
            .Query()
            .WithTitle("Test")
            .WithNumberOfResults(1);

        // Act & Assert
        await Assert.ThrowsAsync<NWOpenException>(()
            => queryBuilder.ExecuteAsync()
        );
    }

    [Fact]
    public async Task ExecuteAsync_ShouldThrowAndLog_OnHttpRequestException()
    {
        // Arrange: handler throws
        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException("network gone"));

        INWOpenQueryBuilder queryBuilder = _nwOpenService
            .Query()
            .WithTitle("Test")
            .WithNumberOfResults(1);

        // Act & Assert
        await Assert.ThrowsAsync<NWOpenException>(()
            => queryBuilder.ExecuteAsync()
        );
    }

    [Fact]
    public void WithNumberOfResults_ShouldThrow_WhenZero()
    {
        NWOpenQueryBuilder queryBuilder = _nwOpenService.Query();
        Assert.Throws<ArgumentException>(() => queryBuilder.WithNumberOfResults(0));
    }

    [Fact]
    public void WithNumberOfResults_ShouldThrow_WhenCalledTwice()
    {
        INWOpenQueryBuilder queryBuilder = _nwOpenService
            .Query()
            .WithNumberOfResults(1);

        Assert.Throws<InvalidOperationException>(() => queryBuilder.WithNumberOfResults(2));
    }

    [Fact]
    public void WithTitle_ShouldThrow_WhenCalledTwice()
    {
        INWOpenQueryBuilder queryBuilder = _nwOpenService
            .Query()
            .WithTitle("Test");

        Assert.Throws<InvalidOperationException>(() => queryBuilder.WithTitle("Test2"));
    }

    [Fact]
    public void BuildQueries_ShouldReturnExpectedString()
    {
        INWOpenQueryBuilder queryBuilder = _nwOpenService
            .Query()
            .WithOrganization("Test")
            .WithTitle("Test")
            .WithNumberOfResults(1)
            .WithMemberLastName("Doe")
            .WithStartDateFrom(new(2020, 1, 1, 0,0,0, DateTimeKind.Utc))
            .WithStartDateUntil(new(2021, 1, 1, 0,0,0, DateTimeKind.Utc))
            .WithEndDateFrom(new(2021, 1, 1, 0,0,0, DateTimeKind.Utc))
            .WithEndDateUntil(new(2022, 1, 1, 0,0,0, DateTimeKind.Utc))
            .WithRole("role");

        List<string> queries = queryBuilder.BuildQueries();

        Assert.Single(queries);
        Assert.Equal(
            "per_page=1&organisation=%22Test%22&title=%22Test%22&role=%22role%22&last_name=%22Doe%22&rs_start_date=2020-01-01&re_start_date=2021-01-01&rs_end_date=2021-01-01&re_end_date=2022-01-01",
            queries[0]
        );
    }
}