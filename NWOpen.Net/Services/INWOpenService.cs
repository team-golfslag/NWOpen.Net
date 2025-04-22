// This program has been developed by students from the bachelor Computer Science at Utrecht
// University within the Software Project course.
// 
// © Copyright Utrecht University (Department of Information and Computing Sciences)

using NWOpen.Net.Models;

namespace NWOpen.Net.Services;

public interface INWOpenService
{
    Task<NWOpenResult?> PerformQueryAsync(string query);
    Task<Project?> GetProjectAsync(string projectId);
    NWOpenQueryBuilder Query();
}
