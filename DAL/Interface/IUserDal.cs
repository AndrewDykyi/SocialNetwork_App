using MongoDB.Bson;
using SocialNetwork_App.DTO;

namespace SocialNetwork_App.DAL.Interface
{
    public interface IUserDal
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(ObjectId id);
        Task InsertAsync(UserDto user);
        Task UpdateAsync(ObjectId id, UserDto user);
        Task DeleteAsync(ObjectId id);
        Task AddFriendAsync(ObjectId userId, ObjectId friendId);
        Task RemoveFriendAsync(ObjectId userId, ObjectId friendId);
    }
}
