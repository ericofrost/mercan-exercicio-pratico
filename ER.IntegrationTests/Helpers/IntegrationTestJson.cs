using System.Text.Json;

namespace ER.IntegrationTests.Helpers;

internal static class IntegrationTestJson
{
    public static JsonSerializerOptions Options { get; } = new()
    {
        PropertyNameCaseInsensitive = true
    };
}
