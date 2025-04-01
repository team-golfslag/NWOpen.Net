// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NWOpen.Net.Models;

namespace NWOpen.Net.Services;

/// <summary>
/// Service to interact with the NWOpen API
/// </summary>
public sealed class NWOpenService(
    HttpClient httpClient,
    ILogger<NWOpenService> logger,
    NWOpenServiceOptions? options = null) : IDisposable
{
    private readonly NWOpenServiceOptions _options = options ?? new NWOpenServiceOptions();

    public void Dispose() => httpClient?.Dispose();

    /// <summary>
    /// Perform a query to the NWOpen API
    /// </summary>
    /// <returns> The result of the query </returns>
    internal async Task<NWOpenResult?> PerformQuery(string query)
    {
        try
        {
            return await httpClient.GetFromJsonAsync<NWOpenResult>(
                $"{_options.BaseUrl}?{query}", _options.JsonSerializerOptions);
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Failed to get results from NWOpen");
            return null;
        }
        catch (JsonException e)
        {
            logger.LogError(e, "Failed to deserialize results from NWOpen");
            return null;
        }
    }

    /// <summary>
    /// Get a project from the NWOpen API
    /// </summary>
    /// <param name="id"> The ID of the project </param>
    /// <returns> The project or <c>null</c> if it does not exist </returns>
    public async Task<Project?> GetProject(string id)
    {
        try
        {
            NWOpenResult? result =
                await httpClient.GetFromJsonAsync<NWOpenResult>($"{_options.BaseUrl}?project_id={id}",
                    _options.JsonSerializerOptions);
            return result?.Projects.FirstOrDefault();
        }
        catch (HttpRequestException e)
        {
            logger.LogError(e, "Failed to get project from NWOpen");
            return null;
        }
        catch (JsonException e)
        {
            logger.LogError(e, "Failed to deserialize project from NWOpen");
            return null;
        }
    }

    /// <summary>
    /// Query projects from the NWOpen API
    /// </summary>
    /// <returns> A query builder </returns>
    public NwOpenQueryBuilder Query() => new(this);
}
