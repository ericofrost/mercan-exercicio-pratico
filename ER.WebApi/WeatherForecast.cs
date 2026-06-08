namespace ER.WebApi;

/// <summary>
/// Scaffold weather forecast model retained from the default ASP.NET Core Web API template.
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// Forecast date.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Temperature in degrees Celsius.
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// Temperature in degrees Fahrenheit derived from <see cref="TemperatureC"/>.
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// Optional short summary of the forecast conditions.
    /// </summary>
    public string? Summary { get; set; }
}
