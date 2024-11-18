using DynamoDB.DTO;

namespace DynamoDB.BLL.Interfaces
{
    public interface IPostService
    {
        Task AddPostAsync(PostDto post);
        Task<IEnumerable<PostDto>> GetAllPostsAsync();
    }
}
