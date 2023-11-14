using Microsoft.AspNetCore.Mvc;

namespace MITIMCOAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GetReturnController : ControllerBase
{
    [HttpGet(Name = "GetReturn")]
    public async Task<Dictionary<string, float>> GetReturn(string stockTicker, string? FromDate, string? ToDate)
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

        TimeSeriesDaily res = await AlphaVantage.Time_series_daily(stockTicker, Strategy.GetOutputStrategy(fromDate_date, toDate_date));

        Dictionary<string, float> dailyReturns = new Dictionary<string, float>();
        foreach(KeyValuePair<string, TimeSeriesDailyDateData> entry in res.TimeSeries)
        {
            DateTime entryDate = DateValidation.parseDate(entry.Key);
            if (using_dates) {
                if (entryDate < fromDate_date)
                {
                    break;
                }
                if (entryDate > toDate_date)
                {
                    continue;
                }
            } else if (entryDate.Year != today.Year)
            {
                break;
            }
            
            float.TryParse(entry.Value.Open, out float numOpen);
            float.TryParse(entry.Value.Close, out float numClose);
            dailyReturns.Add(entry.Key, numOpen - numClose);
        }
        return dailyReturns;
    }
}
