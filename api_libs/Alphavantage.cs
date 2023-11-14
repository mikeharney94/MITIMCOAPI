using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using MITIMCOAPI;
/*
    This class communicates with and handles errors related to the Alpha Vantage api
*/
static class AlphaVantage
{
    private static string apiKey = "VQSIR1QAEKSWZTYX"; // can use "demo" for testing purposes

    public static async Task<TimeSeriesDaily> Time_series_daily(string symbol, string outputsize)
    {
        string QUERY_URL = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol="+symbol+"&apikey="+apiKey+"&outputsize="+outputsize;
        Uri queryUri = new Uri(QUERY_URL);
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(QUERY_URL);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        dynamic result = JsonSerializer.Deserialize<TimeSeriesDaily>(responseBody) ?? throw new ArgumentException("Unable to convert TimeSeriesDaily result to API response object.");

        if (result.Information != null)
        {
            throw new Exception("Information error (likely invalid stock ticker):" + result.Information);
        }
        if (result.Note != null)
        {
            throw new Exception("Note error (likely external API key exceeded daily use):" + result.Note);
        }
        if (result.TimeSeries == null)
        {
            throw new Exception("There is no Alphavantage TimeSeries data for this stock ticker. Please check https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=IBM&outputsize=full&apikey=demo and validate that daily stock data is still included within the 'Time Series (Daily)' index.");
        }
        return result;
    }
}