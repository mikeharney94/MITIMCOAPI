namespace MITIMCOAPI;
using System.Text.Json;
using System.Text.Json.Serialization;

public class TimeSeriesDaily
{
    [JsonPropertyName("Meta Data")]
    public object? MetaData { get; set; }

    [JsonPropertyName("Time Series (Daily)")]
    public Dictionary<string, TimeSeriesDailyDateData>? TimeSeries { get; set; }
    public string? Information { get; set; }
    public string? Note { get; set;}

    [JsonPropertyName("Error Message")]
    public string? ErrorMessage { get; set;}
}
