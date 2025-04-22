// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NWOpen.Net.Exceptions;
using NWOpen.Net.Models;

namespace NWOpen.Net.Services;

/// <summary>
/// Service to interact with the NWOpen API
/// </summary>
public class NWOpenService : INWOpenService
{
    private readonly HttpClient _httpClient;
    private readonly NWOpenServiceOptions _options;
    private readonly ILogger<NWOpenService> _logger;

    public NWOpenService(
        HttpClient httpClient,
        IOptions<NWOpenServiceOptions> options,
        ILogger<NWOpenService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;

        if (!string.IsNullOrWhiteSpace(_options.BaseUrl))
        {
            _httpClient.BaseAddress = new(_options.BaseUrl);
        }
    }

    public async Task<NWOpenResult?> PerformQueryAsync(string query)
    {
        try
        {
            return await _httpClient
                .GetFromJsonAsync<NWOpenResult>($"?{query}", _options.JsonSerializerOptions);
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "HTTP error while querying NWOpen API with query={Query}", query);
            throw new NWOpenException("Failed to get results from NWOpen", e);
        }
        catch (JsonException e)
        {
            _logger.LogError(e, "JSON error while deserializing NWOpen API response for query={Query}", query);
            throw new NWOpenException("Failed to deserialize results from NWOpen", e);
        }
    }

    public async Task<Project?> GetProjectAsync(string projectId)
    {
        try
        {
            NWOpenResult? result = await _httpClient
                .GetFromJsonAsync<NWOpenResult>($"?project_id={projectId}", _options.JsonSerializerOptions);

            return result?.Projects.FirstOrDefault();
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "HTTP error while fetching project {ProjectId}", projectId);
            throw new NWOpenException("Failed to get project from NWOpen", e);
        }
        catch (JsonException e)
        {
            _logger.LogError(e, "JSON error while deserializing project {ProjectId}", projectId);
            throw new NWOpenException("Failed to get project from NWOpen", e);
        }
    }

    public INWOpenQueryBuilder Query() => new NWOpenQueryBuilder(this);
}
