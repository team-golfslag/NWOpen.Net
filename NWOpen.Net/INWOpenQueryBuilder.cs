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
    /// <summary>
    /// Filter by project organization.
    /// </summary>
    /// <param name="organization">The organization name to filter by.</param>
    /// <remarks> Can only be called once. </remarks>
    INWOpenQueryBuilder WithOrganization(string organization);
    
    /// <summary>
    /// Filter by project title.
    /// </summary>
    /// <param name="title">The title to filter by.</param>
    /// <param name="exact">Whether the title should be an exact match.</param>
    INWOpenQueryBuilder WithTitle(string title, bool exact = false);
    
    /// <summary>
    /// Filter by project role.
    /// </summary>
    /// <param name="role">The role to filter by.</param>
    INWOpenQueryBuilder WithRole(string role);
    
    /// <summary>
    /// Filter by project member last name.
    /// </summary>
    /// <param name="lastName">The last name to filter by.</param>
    INWOpenQueryBuilder WithMemberLastName(string lastName);
    
    /// <summary>
    /// Set the project start date beginning of the range.
    /// </summary>
    /// <param name="from">The start date beginning of the range.</param>
    INWOpenQueryBuilder WithStartDateFrom(DateTime from);
    
    /// <summary>
    /// Set the project start date end of the range.
    /// </summary>
    /// <param name="until">The start date end of the range.</param>
    INWOpenQueryBuilder WithStartDateUntil(DateTime until);
    
    /// <summary>
    /// Set the project end date beginning of the range.
    /// </summary>
    /// <param name="from">The end date beginning of the range.</param>
    INWOpenQueryBuilder WithEndDateFrom(DateTime from);
    
    /// <summary>
    /// Set the project end date end of the range.
    /// </summary>
    /// <param name="until">The end date end of the range.</param>
    INWOpenQueryBuilder WithEndDateUntil(DateTime until);

    /// <summary>
    /// Set the number of results to return.
    /// </summary>
    /// <param name="numberOfResults">The number of results to return.</param>
    /// <remarks> Must be greater than 0. Can only be set once. </remarks>
    INWOpenQueryBuilder WithNumberOfResults(int numberOfResults);
    
    /// <summary>
    /// Build the query.
    /// </summary>
    /// <returns> The queries. </returns>
    /// <remarks> returns more than 1 query when the number of results is greater than the page size. </remarks>
    List<string> BuildQueries();

    /// <summary>
    /// Execute the query.
    /// </summary>
    /// <returns> The result of the query. </returns>
    Task<NWOpenResult?> ExecuteAsync();
}