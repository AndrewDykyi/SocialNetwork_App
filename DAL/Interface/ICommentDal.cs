using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialNetwork_App.DTO;

namespace SocialNetwork_App.DAL.Interface
{
    public interface ICommentDal
    {
        Task<IEnumerable<CommentDto>> GetAllAsync();
        Task<CommentDto> GetByIdAsync(ObjectId id);
        Task InsertAsync(CommentDto comment);
        Task UpdateAsync(ObjectId id, CommentDto comment);
        Task DeleteAsync(ObjectId id);
    }
}