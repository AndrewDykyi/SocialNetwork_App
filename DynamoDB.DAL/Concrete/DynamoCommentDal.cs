using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using DynamoDB.DAL.Interfaces;
using DynamoDB.DTO;

namespace DynamoDB.DAL.Concrete
{
    public class DynamoCommentDal : ICommentDal
    {
        private readonly DynamoDBContext _context;

        public DynamoCommentDal(IAmazonDynamoDB dynamoDbClient)
        {
            _context = new DynamoDBContext(dynamoDbClient);
        }

        public async Task InsertCommentAsync(CommentDto comment)
        {
            await _context.SaveAsync(comment);
        }

        public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync()
        {
            return await _context.ScanAsync<CommentDto>(new List<ScanCondition>()).GetRemainingAsync();
        }

        public async Task<CommentDto> GetCommentByIdAsync(string commentId)
        {
            return await _context.LoadAsync<CommentDto>(commentId);
        }

        public async Task DeleteCommentAsync(string commentId)
        {
            await _context.DeleteAsync<CommentDto>(commentId);
        }
    }
}