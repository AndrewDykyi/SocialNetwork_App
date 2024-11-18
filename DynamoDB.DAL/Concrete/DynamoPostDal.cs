using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using DynamoDB.DAL.Interfaces;
using DynamoDB.DTO;


namespace DynamoDB.DAL.Concrete
{
    public class DynamoPostDal : IPostDal
    {
        private readonly DynamoDBContext _context;

        public DynamoPostDal(IAmazonDynamoDB dynamoDbClient)
        {
            _context = new DynamoDBContext(dynamoDbClient);
        }

        public async Task InsertPostAsync(PostDto post)
        {
            await _context.SaveAsync(post);
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            return await _context.ScanAsync<PostDto>(new List<ScanCondition>()).GetRemainingAsync();
        }

        public async Task<PostDto> GetPostByIdAsync(string postId)
        {
            return await _context.LoadAsync<PostDto>(postId);
        }

        public async Task DeletePostAsync(string postId)
        {
            await _context.DeleteAsync<PostDto>(postId);
        }
    }
}