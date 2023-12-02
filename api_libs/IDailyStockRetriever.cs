interface IDailyStockRetriever<T>
{
    // We could consider using an array of strings for more a more general way to pass in data.
    Task<T> GetDailyStockData(string stockTicker, string outputsize);
}