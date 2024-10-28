using MongoDB.Bson;
using SocialNetwork_App.DTO;

namespace SocialNetwork_App.DAL.Interface
{
    public interface IFriendshipDal
    {
        Task CreateFriendshipAsync(FriendshipDto friendship);
        Task DeleteFriendshipAsync(ObjectId id);
        Task<IEnumerable<FriendshipDto>> GetAllFriendshipsAsync();
        Task<FriendshipDto> GetFriendshipByIdAsync(ObjectId id);
    }
}