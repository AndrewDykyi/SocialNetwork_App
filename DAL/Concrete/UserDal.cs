using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetwork_App.DAL.Interface;
using SocialNetwork_App.DTO;


namespace SocialNetwork_App.DAL.Concrete
{
    public class UserDal : IUserDal
    {
        private readonly IMongoCollection<UserDto> _users;

        public UserDal(IMongoDatabase database)
        {
            _users = database.GetCollection<UserDto>("users");
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<UserDto> GetByIdAsync(ObjectId id)
        {
            return await _users.Find(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(UserDto user)
        {
            user.Id = ObjectId.GenerateNewId();
            await _users.InsertOneAsync(user);
        }

        public async Task UpdateAsync(ObjectId id, UserDto user)
        {
            await _users.ReplaceOneAsync(u => u.Id == id, user);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await _users.DeleteOneAsync(user => user.Id == id);
        }
        public async Task AddFriendAsync(ObjectId userId, ObjectId friendId)
        {
            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user != null && !user.Friends.Contains(friendId))
            {
                user.Friends.Add(friendId);
                await _users.ReplaceOneAsync(u => u.Id == userId, user);
            }
        }

        public async Task RemoveFriendAsync(ObjectId userId, ObjectId friendId)
        {
            var user = await _users.Find(u => u.Id == userId).FirstOrDefaultAsync();
            if (user != null && user.Friends.Contains(friendId))
            {
                user.Friends.Remove(friendId);
                await _users.ReplaceOneAsync(u => u.Id == userId, user);
            }
        }

        public async Task CreateUserAsync(UserDto user)
        {
            user.Id = ObjectId.GenerateNewId();
            await _users.InsertOneAsync(user);
        }
        public async Task DeleteUserAsync(ObjectId id)
        {
            await _users.DeleteOneAsync(user => user.Id == id);
        }
    }
}
