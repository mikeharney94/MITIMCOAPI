using Microsoft.AspNetCore.Mvc;

namespace MITIMCOAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GetReturnController : ControllerBase
{
    [HttpGet(Name = "GetReturn")]
    public async Task<Dictionary<string, ChangeTypes>> GetReturn(string stockTicker, string? FromDate, string? ToDate)
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
        
        // Utilizing Generics here allows us to use different return types per inherited API, however converting all API results to a specific return type
        // for easier parsing later, is also a viable approach.

        IDailyStockRetriever<TimeSeriesDaily> dailyStockRetriever = new AlphaVantage();
        TimeSeriesDaily res = await dailyStockRetriever.GetDailyStockData(stockTicker, Strategy.GetOutputStrategy(fromDate_date, toDate_date));

        Dictionary<string, ChangeTypes> dailyReturns = new Dictionary<string, ChangeTypes>();
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
            ChangeTypes changes = new ChangeTypes();
            changes.dollars = numClose - numOpen;
            changes.percentage = ((numClose - numOpen) / numOpen) * 100;
            dailyReturns.Add(entry.Key, changes);
        }
        return dailyReturns;
    }
}
