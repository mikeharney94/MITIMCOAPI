interface IDailyStockRetriever<T>
{
    // We could consider using an array of strings for more a more general way to pass in data.
    // An argument could be made for not using generics here, in case we wish to enforce a more uniform return type.
    Task<T> GetDailyStockData(string stockTicker, string outputsize);
}