using DynamoDB.DTO;

namespace DynamoDB.DAL.Interfaces
{
    public interface ICommentDal
    {
        Task InsertCommentAsync(CommentDto comment);
        Task<IEnumerable<CommentDto>> GetAllCommentsAsync();
        Task<CommentDto> GetCommentByIdAsync(string commentId);
        Task DeleteCommentAsync(string commentId);
    }
}