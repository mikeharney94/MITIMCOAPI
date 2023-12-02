public static class ErrorMessages 
{
    public const string fromDatePrecedesAPI = "The FromDate is from before accessible records are present.";
    public const string fromDateRequiredWithToDate = "If FromDate is provided, so must ToDate.";
    public const string fromDatePrecedesToDate = "The ToDate cannot be before FromDate.";
    public const string dateInFuture = "Neither the 'From Date' nor the 'To Date' can be in the future.";
}