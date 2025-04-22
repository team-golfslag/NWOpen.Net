// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using NWOpen.Net.Models;

namespace NWOpen.Net;

/// <summary>
/// Fluent builder for NWOpen project queries.
/// </summary>
public interface INWOpenQueryBuilder
{
    INWOpenQueryBuilder WithOrganization(string organization);
    INWOpenQueryBuilder WithTitle(string title, bool exact = false);
    INWOpenQueryBuilder WithRole(string role);
    INWOpenQueryBuilder WithMemberLastName(string lastName);
    INWOpenQueryBuilder WithStartDateFrom(DateTime from);
    INWOpenQueryBuilder WithStartDateUntil(DateTime until);
    INWOpenQueryBuilder WithEndDateFrom(DateTime from);
    INWOpenQueryBuilder WithEndDateUntil(DateTime until);

    /// <summary>
    /// How many results to return.
    /// </summary>
    INWOpenQueryBuilder WithNumberOfResults(int numberOfResults);
    
    List<string> BuildQueries();

    /// <summary>
    /// Execute the query.
    /// </summary>
    Task<NWOpenResult?> ExecuteAsync();
}