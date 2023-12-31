using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using MITIMCOAPI;
/*
    This class communicates with and handles errors related to the Alpha Vantage api
*/
public class AlphaVantage : IDailyStockRetriever<TimeSeriesDaily>
{
    public async Task<TimeSeriesDaily> GetDailyStockData(string symbol, string outputsize)
    {
        string apiKey = Environment.GetEnvironmentVariable("alphavantage_key", EnvironmentVariableTarget.Machine);
        string QUERY_URL = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol="+symbol+"&apikey="+apiKey+"&outputsize="+outputsize;
        Uri queryUri = new Uri(QUERY_URL);
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(QUERY_URL);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        TimeSeriesDaily result = JsonSerializer.Deserialize<TimeSeriesDaily>(responseBody) ?? throw new ArgumentException("Unable to convert TimeSeriesDaily result to API response object.");

        if (result.Information != null)
        {
            LoggingLib.ThrowException("Information error (likely invalid stock ticker):" + result.Information, "Argument");
        }
        if (result.Note != null)
        {
            LoggingLib.ThrowException("Note error (likely external API key exceeded daily use):" + result.Note, null);
        }
        if (result.ErrorMessage != null)
        {
            LoggingLib.ThrowException("Error (likely that api key is invalid or not recorded in registry):" + result.ErrorMessage, null);
        }
        if (result.TimeSeries == null)
        {
            LoggingLib.ThrowException("There is no Alphavantage TimeSeries data for this stock ticker. Please check https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=IBM&outputsize=full&apikey=demo and validate that daily stock data is still included within the 'Time Series (Daily)' index.", null);
        }
        return result;
    }
}