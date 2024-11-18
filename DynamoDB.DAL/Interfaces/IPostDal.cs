using DynamoDB.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDB.DAL.Interfaces
{
    public interface IPostDal
    {
        Task InsertPostAsync(PostDto post);
        Task<IEnumerable<PostDto>> GetAllPostsAsync();
        Task<PostDto> GetPostByIdAsync(string postId);
        Task DeletePostAsync(string postId);
    }
}
