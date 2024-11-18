using DynamoDB.BLL.Interfaces;
using DynamoDB.DAL.Interfaces;
using DynamoDB.DTO;

namespace DynamoDB.BLL.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentDal _commentDal;

        public CommentService(ICommentDal commentDal)
        {
            _commentDal = commentDal;
        }

        public async Task AddCommentAsync(CommentDto comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment), "Comment cannot be null");
            }

            await _commentDal.InsertCommentAsync(comment);
        }

        public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync()
        {
            return await _commentDal.GetAllCommentsAsync();
        }

        public async Task<CommentDto> GetCommentByIdAsync(string commentId)
        {
            if (string.IsNullOrEmpty(commentId))
            {
                throw new ArgumentException("Comment ID cannot be null or empty", nameof(commentId));
            }

            return await _commentDal.GetCommentByIdAsync(commentId);
        }

        public async Task DeleteCommentAsync(string commentId)
        {
            if (string.IsNullOrEmpty(commentId))
            {
                throw new ArgumentException("Comment ID cannot be null or empty", nameof(commentId));
            }

            await _commentDal.DeleteCommentAsync(commentId);
        }
    }
}
