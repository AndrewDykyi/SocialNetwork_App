public class Neo4JUserDal : INeo4JUserDal
{
    private readonly Neo4JConnection _connection;

    public Neo4JUserDal(Neo4JConnection connection)
    {
        _connection = connection;
    }

    public async Task CreateUserAsync(string userId, string name)
    {
        var query = $"CREATE (n:User {{id: '{userId}', name: '{name}'}})";
        await _connection.ExecuteQueryAsync(query);
    }

    public async Task DeleteUserAsync(string userId)
    {
        var query = $"MATCH (n:User {{id: '{userId}'}}) DETACH DELETE n";
        await _connection.ExecuteQueryAsync(query);
    }

    public async Task CreateRelationshipAsync(string userId1, string userId2, string relationshipType)
    {
        var query = $"MATCH (a:User {{id: '{userId1}'}}), (b:User {{id: '{userId2}'}}) " +
                    $"CREATE (a)-[:{relationshipType}]->(b)";
        await _connection.ExecuteQueryAsync(query);
    }

    public async Task DeleteRelationshipAsync(string userId1, string userId2, string relationshipType)
    {
        var query = $"MATCH (a:User {{id: '{userId1}'}})-[r:{relationshipType}]->(b:User {{id: '{userId2}'}}) DELETE r";
        await _connection.ExecuteQueryAsync(query);
    }

    public async Task<bool> AreUsersConnectedAsync(string userId1, string userId2)
    {
        var query = $"MATCH (a:User {{id: '{userId1}'}})-[r]->(b:User {{id: '{userId2}'}}) RETURN count(r) > 0";
        var result = await _connection.ExecuteQueryAsync(query);

        return result.FirstOrDefault() == "true";
    }

    public async Task<int> GetDistanceBetweenUsersAsync(string userId1, string userId2)
    {
        var query = $@"
        MATCH (u1:User {{id: '{userId1}'}})-[:FRIEND|FOLLOW|SUBSCRIBE*]-(u2:User {{id: '{userId2}'}})
        RETURN length(shortestPath((u1)-[:FRIEND|FOLLOW|SUBSCRIBE*]-(u2))) AS distance";

        var results = await _connection.ExecuteQueryAsync(query);

        if (results.Count > 0 && int.TryParse(results[0], out int distance))
        {
            return distance;
        }

        return -1;
    }

    public async Task CreateFriendAsync(string userId1, string userId2)
    {
        string query = $"MATCH (u1:User {{id: '{userId1}'}}), (u2:User {{id: '{userId2}'}}) " +
                       "MERGE (u1)-[:FRIEND]->(u2)";
        await _connection.ExecuteQueryAsync(query);
    }

    public async Task DeleteFriendAsync(string userId1, string userId2)
    {
        string query = $"MATCH (u1:User {{id: '{userId1}'}})-[r:FRIEND]->(u2:User {{id: '{userId2}'}}) " +
                       "DELETE r";
        await _connection.ExecuteQueryAsync(query);
    }

    public async Task CreateFollowerAsync(string userId1, string userId2)
    {
        string query = $"MATCH (u1:User {{id: '{userId1}'}}), (u2:User {{id: '{userId2}'}}) " +
                       "MERGE (u1)-[:FOLLOWER]->(u2)";
        await _connection.ExecuteQueryAsync(query);
    }

    public async Task DeleteFollowerAsync(string userId1, string userId2)
    {
        string query = $"MATCH (u1:User {{id: '{userId1}'}})-[r:FOLLOWER]->(u2:User {{id: '{userId2}'}}) " +
                       "DELETE r";
        await _connection.ExecuteQueryAsync(query);
    }

    public async Task CreateSubscriberAsync(string userId1, string userId2)
    {
        string query = $"MATCH (u1:User {{id: '{userId1}'}}), (u2:User {{id: '{userId2}'}}) " +
                       "MERGE (u1)-[:SUBSCRIBER]->(u2)";
        await _connection.ExecuteQueryAsync(query);
    }

    public async Task DeleteSubscriberAsync(string userId1, string userId2)
    {
        string query = $"MATCH (u1:User {{id: '{userId1}'}})-[r:SUBSCRIBER]->(u2:User {{id: '{userId2}'}}) " +
                       "DELETE r";
        await _connection.ExecuteQueryAsync(query);
    }
    public async Task UpdateUserNameAsync(string userId, string newUserName)
    {
        var query = $"MATCH (u:User {{id: '{userId}'}}) SET u.name = '{newUserName}'";
        await _connection.ExecuteAsync(query);
    }
}