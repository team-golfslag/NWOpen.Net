// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using System.Web;
using NWOpen.Net.Models;
using NWOpen.Net.Services;

namespace NWOpen.Net;

/// <summary>
/// A builder class to create a query to search for projects.
/// </summary>
public class NwOpenQueryBuilder
{
    /// <summary>
    /// The number of results per page.
    /// </summary>
    private const int PageSize = 100;

    /// <summary>
    /// The service to use to perform the query.
    /// </summary>
    private readonly NWOpenService _service;

    private DateTime? _endDateFrom;
    private DateTime? _endDateUntil;

    /// <summary>
    /// The last name to filter by.
    /// </summary>
    private string? _lastNameQuery;

    /// <summary>
    /// The amount of results to return.
    /// </summary>
    private int? _numberOfResults;

    /// <summary>
    /// The organization to filter by.
    /// </summary>
    private string? _organizationQuery;

    /// <summary>
    /// The role to filter by.
    /// </summary>
    private string? _roleQuery;

    private DateTime? _startDateFrom;
    private DateTime? _startDateUntil;

    /// <summary>
    /// Whether the title should be an exact match.
    /// </summary>
    private bool? _titleExact;

    /// <summary>
    /// The title to filter by.
    /// </summary>
    private string? _titleQuery;

    internal NwOpenQueryBuilder(NWOpenService service)
    {
        _service = service;
    }

    /// <summary>
    /// Filter by project organization.
    /// </summary>
    /// <param name="organization">The organization name to filter by.</param>
    /// <remarks> Can only be called once. </remarks>
    /// <exception cref="InvalidOperationException"> Organization can only be set once </exception>
    public NwOpenQueryBuilder WithOrganization(string organization)
    {
        if (_organizationQuery != null)
            throw new InvalidOperationException("Organization can only be set once");
        _organizationQuery = HttpUtility.UrlEncode("\"" + organization + "\"");
        return this;
    }

    /// <summary>
    /// Filter by project title.
    /// </summary>
    /// <param name="title">The title to filter by.</param>
    /// <param name="exact">Whether the title should be an exact match.</param>
    /// <exception cref="InvalidOperationException"> Title can only be set once </exception>
    public NwOpenQueryBuilder WithTitle(string title, bool exact = false)
    {
        if (_titleQuery != null)
            throw new InvalidOperationException("Title can only be set once");
        _titleExact = exact;
        _titleQuery = HttpUtility.UrlEncode("\"" + title + "\"");
        return this;
    }

    /// <summary>
    /// Filter by project role.
    /// </summary>
    /// <param name="role">The role to filter by.</param>
    /// <exception cref="InvalidOperationException"> Role can only be set once </exception>
    public NwOpenQueryBuilder WithRole(string role)
    {
        if (_roleQuery != null)
            throw new InvalidOperationException("Role can only be set once");
        _roleQuery = HttpUtility.UrlEncode("\"" + role + "\"");
        return this;
    }

    /// <summary>
    /// Filter by project member last name.
    /// </summary>
    /// <param name="lastName">The last name to filter by.</param>
    /// <exception cref="InvalidOperationException"> Last name can only be set once </exception>
    public NwOpenQueryBuilder WithMemberLastName(string lastName)
    {
        if (_lastNameQuery != null)
            throw new InvalidOperationException("Last name can only be set once");
        _lastNameQuery = HttpUtility.UrlEncode("\"" + lastName + "\"");
        return this;
    }

    /// <summary>
    /// Set the project start date beginning of the range.
    /// </summary>
    /// <param name="from">The start date beginning of the range.</param>
    /// <exception cref="InvalidOperationException"> Start date from can only be set once </exception>
    public NwOpenQueryBuilder WithStartDateFrom(DateTime from)
    {
        if (_startDateFrom.HasValue)
            throw new InvalidOperationException("Start date from can only be set once");
        _startDateFrom = from;
        return this;
    }

    /// <summary>
    /// Set the project start date end of the range.
    /// </summary>
    /// <param name="until">The start date end of the range.</param>
    /// <exception cref="InvalidOperationException"> Start date until can only be set once </exception>
    public NwOpenQueryBuilder WithStartDateUntil(DateTime until)
    {
        if (_startDateUntil.HasValue)
            throw new InvalidOperationException("Start date until can only be set once");
        _startDateUntil = until;
        return this;
    }

    /// <summary>
    /// Set the project end date beginning of the range.
    /// </summary>
    /// <param name="from">The end date beginning of the range.</param>
    /// <exception cref="InvalidOperationException"> End date from can only be set once </exception>
    public NwOpenQueryBuilder WithEndDateFrom(DateTime from)
    {
        if (_endDateFrom.HasValue)
            throw new InvalidOperationException("End date from can only be set once");
        _endDateFrom = from;
        return this;
    }

    /// <summary>
    /// Set the project end date end of the range.
    /// </summary>
    /// <param name="until">The end date end of the range.</param>
    /// <exception cref="InvalidOperationException"> End date until can only be set once </exception>
    public NwOpenQueryBuilder WithEndDateUntil(DateTime until)
    {
        if (_endDateUntil.HasValue)
            throw new InvalidOperationException("End date until can only be set once");
        _endDateUntil = until;
        return this;
    }

    /// <summary>
    /// Set the number of results to return.
    /// </summary>
    /// <param name="numberOfResults">The number of results to return.</param>
    /// <remarks> Must be greater than 0. Can only be set once. </remarks>
    /// <exception cref="InvalidOperationException"> Number of results can only be set once </exception>
    /// <exception cref="ArgumentException"> Number of results must be greater than 0 </exception>
    public NwOpenQueryBuilder WithNumberOfResults(int numberOfResults)
    {
        if (numberOfResults <= 0)
            throw new ArgumentException("Number of results must be greater than 0");
        if (_numberOfResults != null)
            throw new InvalidOperationException("Number of results can only be set once");
        _numberOfResults = numberOfResults;
        return this;
    }

    /// <summary>
    /// Execute the query.
    /// </summary>
    /// <returns> The result of the query. </returns>
    public async Task<NWOpenResult?> ExecuteAsync()
    {
        // Build the query
        List<string> query = BuildQueries();

        // Perform the query
        List<NWOpenResult> results = [];
        foreach (string q in query)
        {
            NWOpenResult? res = await _service.PerformQueryAsync(q);
            if (res is not null) results.Add(res);
        }

        if (results.Count == 0) return null;

        // Combine the results
        NWOpenResult first = results[0];
        if (results.Count == 1) return first;

        NWOpenResult result = results.Skip(1).Aggregate(first, (current, other) => current.Combine(other));
        result.Metadata.Count ??= result.Projects.Count;

        if (_titleExact.HasValue && _titleExact.Value)
            return FilterTitleExact(result);

        return result;
    }

    private NWOpenResult FilterTitleExact(NWOpenResult result)
    {
        int newCount = 0;
        List<Project> projects = [];
        foreach (Project project in result.Projects.Where(project => project.Title == _titleQuery))
        {
            newCount++;
            projects.Add(project);
        }

        result.Metadata.Count = newCount;

        return new()
        {
            Projects = projects,
            Metadata = result.Metadata,
        };
    }

    /// <summary>
    /// Build the query.
    /// </summary>
    /// <returns> The queries. </returns>
    /// <remarks> returns more than 1 query when the number of results is greater than the page size. </remarks>
    /// <exception cref="ArgumentException"> Number of results must be greater than 0 </exception>
    public List<string> BuildQueries()
    {
        int results = _numberOfResults ?? PageSize;
        if (results < PageSize)
            return [BuildQuery(null)];

        int pages = results / PageSize;
        if (_numberOfResults % PageSize > 0) pages++;

        return Enumerable.Range(1, pages).Select(page => BuildQuery(page)).ToList();
    }

    /// <summary>
    /// Build the query for a specific page.
    /// </summary>
    /// <param name="page">The page to build the query for.</param>
    /// <returns> The query </returns>
    private string BuildQuery(int? page)
    {
        // Build the components of the query
        List<string> components = [];

        // Set the page
        if (page.HasValue)
            components.Add("page=" + page);
        else
            components.Add("per_page=" + _numberOfResults);

        if (_organizationQuery != null)
            components.Add("organisation=" + _organizationQuery);

        if (_titleQuery != null)
            components.Add("title=" + _titleQuery);

        if (_roleQuery != null)
            components.Add("role=" + _roleQuery);

        if (_lastNameQuery != null)
            components.Add("last_name=" + _lastNameQuery);

        if (_startDateFrom.HasValue)
            components.Add("rs_start_date=" + GetFormattedDate(_startDateFrom.Value));

        if (_startDateUntil.HasValue)
            components.Add("re_start_date=" + GetFormattedDate(_startDateUntil.Value));

        if (_endDateFrom.HasValue)
            components.Add("rs_end_date=" + GetFormattedDate(_endDateFrom.Value));

        if (_endDateUntil.HasValue)
            components.Add("re_end_date=" + GetFormattedDate(_endDateUntil.Value));

        // Return the query
        return string.Join("&", components);
    }

    /// <summary>
    /// Get a formatted date.
    /// </summary>
    /// <param name="date">The date to format.</param>
    /// <returns> The formatted date. </returns>
    private static string GetFormattedDate(DateTime date) => HttpUtility.UrlEncode(date.ToString("yyyy-MM-dd"));
}
