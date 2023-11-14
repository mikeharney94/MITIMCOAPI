public static class Strategy
{
    /*
        Allows us to functionally determine our approaches based on static functions
    */
    public static string GetOutputStrategy(DateTime fromDate, DateTime toDate)
    {
        DateTime today = DateTime.Today;
        if (
            fromDate.Year == 1 && 
            toDate.Year == 1 &&
            ((today - DateValidation.parseDate(today.Year+"-01-01")).Days <= 100)
        )
        {
            return "compact";
        }
        if (fromDate.Year != 1 && (today - fromDate).TotalDays <= 100)
        {
            return "compact";
        }
        return "full";
    }
}