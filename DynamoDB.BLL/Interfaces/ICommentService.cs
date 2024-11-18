using DynamoDB.DTO;

namespace DynamoDB.BLL.Interfaces
{
    public interface ICommentService
    {
        Task AddCommentAsync(CommentDto comment);
        Task<IEnumerable<CommentDto>> GetAllCommentsAsync();
        Task<CommentDto> GetCommentByIdAsync(string commentId);
        Task DeleteCommentAsync(string commentId);
    }
}
