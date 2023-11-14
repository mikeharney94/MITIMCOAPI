using Microsoft.AspNetCore.Mvc;

namespace MITIMCOAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GetAlphaController : ControllerBase
{
    [HttpGet(Name = "GetAlpha")]
    public async Task<float> GetAlpha(string stockTicker, string? FromDate, string? ToDate)
    {
        stockTicker = stockTicker.ToUpper();
        DateTime fromDate_date = new DateTime(); // default = Year 1
        DateTime toDate_date = new DateTime(); // default = Year 1
        DateTime today = DateTime.Today;
        bool using_dates = false; // tracks if we are using FromDate & ToDate

        if (FromDate != null)
        {
            List<DateTime> validDates = DateValidation.fromDate_toDate_validation(FromDate, ToDate);
            fromDate_date = validDates[0];
            toDate_date = validDates[1];
            using_dates = true;
        }

        string outputStrategy = Strategy.GetOutputStrategy(fromDate_date, toDate_date);
        TimeSeriesDaily tickerValues = await AlphaVantage.Time_series_daily(stockTicker, outputStrategy);
        TimeSeriesDaily benchmarkValues = await AlphaVantage.Time_series_daily("SPY", outputStrategy);

        float tickerEndValue = -1F;
        float tickerStartValue = -1F;
        float benchmarkEndValue = -1F;
        float benchmarkStartValue = -1F;
        bool toDateRecorded = false; // used for weekend cases

        if (!using_dates) {
            DateTime previousDate;
            // If we are using the YTD range, then the first, most recent value will always be the final value used for calculations.
            float.TryParse(tickerValues.TimeSeries[tickerValues.TimeSeries.Keys.First()].Close, out tickerEndValue);
            float.TryParse(benchmarkValues.TimeSeries[benchmarkValues.TimeSeries.Keys.First()].Close, out benchmarkEndValue);

            foreach(KeyValuePair<string, TimeSeriesDailyDateData> entry in tickerValues.TimeSeries)
            {
                DateTime entryDate = DateValidation.parseDate(entry.Key);
                // Since 1/01 is a market holiday, the first day we find of the previous year is used as the initial value for calculations.
                if (entryDate.Year < today.Year) {
                    float.TryParse(tickerValues.TimeSeries[entry.Key].Close, out tickerStartValue);
                    float.TryParse(benchmarkValues.TimeSeries[entry.Key].Close, out benchmarkStartValue);
                    return tickerEndValue - tickerStartValue - benchmarkEndValue - benchmarkStartValue;
                }
                previousDate = entryDate;
            }
        }

        foreach(KeyValuePair<string, TimeSeriesDailyDateData> entry in tickerValues.TimeSeries)
        {
            DateTime entryDate = DateValidation.parseDate(entry.Key);

            // First case: if toDate data is present, Second case: If toDate is on a weekend, the previous day's close is used.
            if (entryDate == toDate_date || (!toDateRecorded && entryDate < toDate_date))
            {
                float.TryParse(entry.Value.Close, out tickerEndValue);
                float.TryParse(benchmarkValues.TimeSeries[entry.Key].Close, out benchmarkEndValue);
                toDateRecorded = true;
                continue;
            }
            if (entryDate == fromDate_date)
            {
                float.TryParse(entry.Value.Open, out tickerStartValue);
                float.TryParse(benchmarkValues.TimeSeries[entry.Key].Open, out benchmarkStartValue);
                return tickerEndValue - tickerStartValue - benchmarkEndValue - benchmarkStartValue;
            }
            // If we got here, then the desired from date fell on a weekend, so the previous day's "close" is used.
            if (entryDate < fromDate_date)
            {
                float.TryParse(entry.Value.Close, out tickerStartValue);
                float.TryParse(benchmarkValues.TimeSeries[entry.Key].Close, out benchmarkStartValue);
                return tickerEndValue - tickerStartValue - benchmarkEndValue - benchmarkStartValue;
            }        
        }
        return -1F;
    }
}
