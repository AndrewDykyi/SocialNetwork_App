using MongoDB.Bson;
using SocialNetwork_App.DAL.Interface;
using SocialNetwork_App.DTO;

public class UserService
{
    private readonly IUserDal _mongoUserDal;
    private readonly INeo4JUserDal _neo4JUserDal;
    private readonly IFriendshipDal _friendshipDal;

    public UserService(IUserDal mongoUserDal, INeo4JUserDal neo4JUserDal, IFriendshipDal friendshipDal)
    {
        _mongoUserDal = mongoUserDal;
        _neo4JUserDal = neo4JUserDal;
        _friendshipDal = friendshipDal;
    }

    public async Task AddUserAsync(UserDto user)
    {
        await _mongoUserDal.CreateUserAsync(user);

        string firstName = user.FirstName ?? "Unnamed";

        await _neo4JUserDal.CreateUserAsync(user.Id.ToString(), firstName);
    }

    public async Task DeleteUserAsync(string userId)
    {
        if (ObjectId.TryParse(userId, out ObjectId objectId))
        {
            await _mongoUserDal.DeleteUserAsync(objectId);
            await _neo4JUserDal.DeleteUserAsync(userId);
        }
        else
        {
            throw new ArgumentException("Invalid userId format.", nameof(userId));
        }
    }

    public async Task AddFriendshipAsync(ObjectId userId1, ObjectId userId2)
    {
        var friendship = new FriendshipDto
        {
            UserId1 = userId1,
            UserId2 = userId2,
            RelationshipType = "FRIEND",
            CreatedAt = DateTime.UtcNow
        };

        await _friendshipDal.CreateFriendshipAsync(friendship);
        await _neo4JUserDal.CreateFriendAsync(userId1.ToString(), userId2.ToString());
    }

    public async Task RemoveFriendshipAsync(ObjectId friendshipId)
    {
        var friendship = await _friendshipDal.GetFriendshipByIdAsync(friendshipId);
        if (friendship != null)
        {
            await _friendshipDal.DeleteFriendshipAsync(friendshipId);
            await _neo4JUserDal.DeleteFriendAsync(friendship.UserId1.ToString(), friendship.UserId2.ToString());
        }
    }

    public async Task FollowUserAsync(ObjectId followerId, ObjectId followeeId)
    {
        await _neo4JUserDal.CreateFollowerAsync(followerId.ToString(), followeeId.ToString());
    }

    public async Task UnfollowUserAsync(ObjectId followerId, ObjectId followeeId)
    {
        await _neo4JUserDal.DeleteFollowerAsync(followerId.ToString(), followeeId.ToString());
    }

    public async Task SubscribeUserAsync(ObjectId subscriberId, ObjectId subscribedId)
    {
        await _neo4JUserDal.CreateSubscriberAsync(subscriberId.ToString(), subscribedId.ToString());
    }

    public async Task UnsubscribeUserAsync(ObjectId subscriberId, ObjectId subscribedId)
    {
        await _neo4JUserDal.DeleteSubscriberAsync(subscriberId.ToString(), subscribedId.ToString());
    }
}
