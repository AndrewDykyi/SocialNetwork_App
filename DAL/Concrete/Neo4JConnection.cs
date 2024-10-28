using Neo4j.Driver;

public class Neo4JConnection : IDisposable
{
    private readonly IDriver _driver;

    public Neo4JConnection(string uri, string username, string password)
    {
        _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, password));
    }

    public async Task ExecuteAsync(string query)
    {
        await using var session = _driver.AsyncSession();
        await session.RunAsync(query);
    }

    public async Task<List<string>> ExecuteQueryAsync(string query)
    {
        var results = new List<string>();

        await using var session = _driver.AsyncSession();
        var resultCursor = await session.RunAsync(query);

        while (await resultCursor.FetchAsync())
        {
            var record = resultCursor.Current;
            results.Add(record.Values.Values.FirstOrDefault()?.ToString());
        }

        return results;
    }

    public void Dispose()
    {
        _driver?.Dispose();
    }
}