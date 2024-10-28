using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetwork_App.DAL.Interface;
using SocialNetwork_App.DTO;

namespace SocialNetwork_App.DAL.Concrete
{
    public class FriendshipDal : IFriendshipDal
    {
        private readonly IMongoCollection<FriendshipDto> _friendships;

        public FriendshipDal(IMongoDatabase database)
        {
            _friendships = database.GetCollection<FriendshipDto>("friendships");
        }

        public async Task CreateFriendshipAsync(FriendshipDto friendship)
        {
            friendship.Id = ObjectId.GenerateNewId();
            await _friendships.InsertOneAsync(friendship);
        }

        public async Task DeleteFriendshipAsync(ObjectId id)
        {
            await _friendships.DeleteOneAsync(f => f.Id == id);
        }

        public async Task<IEnumerable<FriendshipDto>> GetAllFriendshipsAsync()
        {
            return await _friendships.Find(_ => true).ToListAsync();
        }

        public async Task<FriendshipDto> GetFriendshipByIdAsync(ObjectId id)
        {
            return await _friendships.Find(f => f.Id == id).FirstOrDefaultAsync();
        }
    }
}