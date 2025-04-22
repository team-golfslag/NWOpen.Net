// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using System.Net;
using System.Net.Http.Json;
using System.Web;
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
    
    [Fact]
        public void WithOrganization_CalledTwice_ThrowsInvalidOperationException()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            builder.WithOrganization("TestOrg");
            Assert.Throws<InvalidOperationException>(() => builder.WithOrganization("OtherOrg"));
        }

        [Fact]
        public void WithTitle_CalledTwice_ThrowsInvalidOperationException()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            builder.WithTitle("MyTitle");
            Assert.Throws<InvalidOperationException>(() => builder.WithTitle("AnotherTitle"));
        }

        [Fact]
        public void WithRole_CalledTwice_ThrowsInvalidOperationException()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            builder.WithRole("RoleA");
            Assert.Throws<InvalidOperationException>(() => builder.WithRole("RoleB"));
        }

        [Fact]
        public void WithMemberLastName_CalledTwice_ThrowsInvalidOperationException()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            builder.WithMemberLastName("Smith");
            Assert.Throws<InvalidOperationException>(() => builder.WithMemberLastName("Jones"));
        }

        [Fact]
        public void WithStartDateFrom_CalledTwice_ThrowsInvalidOperationException()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            builder.WithStartDateFrom(new(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            Assert.Throws<InvalidOperationException>(() => builder.WithStartDateFrom(new(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        }

        [Fact]
        public void WithStartDateUntil_CalledTwice_ThrowsInvalidOperationException()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            builder.WithStartDateUntil(new(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            Assert.Throws<InvalidOperationException>(() => builder.WithStartDateUntil(new(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        }

        [Fact]
        public void WithEndDateFrom_CalledTwice_ThrowsInvalidOperationException()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            builder.WithEndDateFrom(new(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            Assert.Throws<InvalidOperationException>(() => builder.WithEndDateFrom(new(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        }

        [Fact]
        public void WithEndDateUntil_CalledTwice_ThrowsInvalidOperationException()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            builder.WithEndDateUntil(new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            Assert.Throws<InvalidOperationException>(() => builder.WithEndDateUntil(new DateTime(2021, 1, 1, 0, 0, 0, DateTimeKind.Utc)));
        }

        [Fact]
        public void WithNumberOfResults_Zero_ThrowsArgumentException()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            Assert.Throws<ArgumentException>(() => builder.WithNumberOfResults(0));
        }

        [Fact]
        public void WithNumberOfResults_CalledTwice_ThrowsInvalidOperationException()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            builder.WithNumberOfResults(5);
            Assert.Throws<InvalidOperationException>(() => builder.WithNumberOfResults(10));
        }

        [Fact]
        public void BuildQueries_ResultLessThanPageSize_UsesPerPage()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            var queries = builder
                .WithNumberOfResults(20)
                .BuildQueries();

            Assert.Single(queries);
            string q = queries[0];
            Assert.StartsWith("per_page=20", q);
        }

        [Fact]
        public void BuildQueries_ExactPageSize_UsesPageOne()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            var queries = builder
                .WithNumberOfResults(100)
                .BuildQueries();

            Assert.Single(queries);
            Assert.Equal("page=1", queries.Single());
        }

        [Fact]
        public void BuildQueries_MoreThanPageSize_GeneratesMultiplePageQueries()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            const int total = 250;
            var queries = builder
                .WithNumberOfResults(total)
                .BuildQueries();

            Assert.Equal(3, queries.Count);
            Assert.Contains("page=1", queries);
            Assert.Contains("page=2", queries);
            Assert.Contains("page=3", queries);
        }

        [Fact]
        public void EncodedFields_AppearInQueryString()
        {
            NWOpenQueryBuilder builder = _nwOpenService.Query();
            const string org = "Org Name";
            const string title = "Title Value";

            var queries = builder
                .WithOrganization(org)
                .WithTitle(title)
                .WithNumberOfResults(1)
                .BuildQueries();

            string q = queries.Single();
            // Check that the quotes around the values are URL-encoded
            string encodedOrg = HttpUtility.UrlEncode("\"" + org + "\"");
            string encodedTitle = HttpUtility.UrlEncode("\"" + title + "\"");

            Assert.Contains($"organisation={encodedOrg}", q);
            Assert.Contains($"title={encodedTitle}", q);
        }
}