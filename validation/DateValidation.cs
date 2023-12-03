using System.Globalization;

public static class DateValidation
{
    // parses a date in the yyyy-MM-dd format and throws an error if there is an issue.
    public static DateTime parseDate(string date)
    {
        DateTime d;
        bool correctFormat = DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d);
        if (!correctFormat) {
            LoggingLib.ThrowException("Passed in date: "+date+"does not follow the yyyy-MM-dd format", "Argument");
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
            LoggingLib.ThrowException(ErrorMessages.fromDatePrecedesAPI, "Argument");
        }
        if (ToDate == null) 
        {
            LoggingLib.ThrowException(ErrorMessages.fromDateRequiredWithToDate, "Argument");
        }

        DateTime toDate_date = parseDate(ToDate);
        if (toDate_date < fromDate_date)
        {
            LoggingLib.ThrowException(ErrorMessages.fromDatePrecedesToDate, "Argument");
        }
        if (toDate_date > today || fromDate_date > today)
        {
            LoggingLib.ThrowException(ErrorMessages.dateInFuture, "Argument");
        }
        return new List<DateTime>{fromDate_date, toDate_date};
    }
}