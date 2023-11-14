using System.Globalization;

static class DateValidation
{
    // parses a date in the yyyy-MM-dd format and throws an error if there is an issue.
    public static DateTime parseDate(string date)
    {
        DateTime d;
        bool correctFormat = DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        if (!correctFormat) {
            throw new ArgumentException("Passed in date: "+date+"does not follow the yyyy-MM-dd format");
        }
        return d;
    }

    // Validates that the FromDate and ToDate values are correct for the GetAlpha and GetReturn endpoints.
    public static List<DateTime> fromDate_toDate_validation(string FromDate, string? ToDate)
    {
        DateTime alphavantageStart = parseDate("1999-11-01");
        DateTime fromDate_date = parseDate(FromDate);
        DateTime today = DateTime.Today;
        if (fromDate_date < alphavantageStart)
        {
            throw new ArgumentException("The FromDate is from before accessible records are present.");
        }
        if (ToDate == null) 
        {
            throw new ArgumentException("If FromDate is provided, so must ToDate.");
        }

        DateTime toDate_date = parseDate(ToDate);
        if (toDate_date < fromDate_date)
        {
            throw new ArgumentException("The ToDate cannot be before FromDate.");
        }
        if (toDate_date > today || fromDate_date > today)
        {
            throw new ArgumentException("Neither the 'From Date' nor the 'To Date' can be in the future.");
        }
        return new List<DateTime>{fromDate_date, toDate_date};
    }
}