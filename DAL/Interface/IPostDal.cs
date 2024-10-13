using MongoDB.Bson;
using SocialNetwork_App.DTO;

namespace SocialNetwork_App.DAL.Interface
{
    public interface IPostDal
    {
        Task<IEnumerable<PostDto>> GetAllAsync();
        Task<PostDto> GetByIdAsync(ObjectId id);
        Task InsertAsync(PostDto post);
        Task UpdateAsync(ObjectId id, PostDto post);
        Task DeleteAsync(ObjectId id);
        Task<int> GetCommentCountAsync(ObjectId userId);
        Task<int> GetLikeCountAsync(ObjectId userId);
        Task AddCommentAsync(ObjectId postId, CommentDto comment);
        Task<IEnumerable<CommentDto>> GetCommentsAsync(ObjectId postId);
        Task AddReactionAsync(ObjectId postId, ReactionDto reaction);
        Task<IEnumerable<ReactionDto>> GetReactionsAsync(ObjectId postId);
    }
}
