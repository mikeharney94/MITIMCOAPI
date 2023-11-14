namespace MITIMCOAPI;
using System.Text.Json;
using System.Text.Json.Serialization;

public class TimeSeriesDailyDateData
{
    [JsonPropertyName("1. open")]
    public required string Open { get; set; }

    [JsonPropertyName("2. high")]
    public required string High { get; set; }
    [JsonPropertyName("3. low")]
    public required string Low { get; set; }
    [JsonPropertyName("4. close")]
    public required string Close { get; set; }
    [JsonPropertyName("5. volume")]
    public required string Volume { get; set; }
}