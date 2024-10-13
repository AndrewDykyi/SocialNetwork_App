using MongoDB.Driver;
using MongoDB.Bson;
using SocialNetwork_App.DTO;
using SocialNetwork_App.DAL.Interface;

namespace SocialNetwork_App.DAL.Concrete
{
    public class CommentDal : ICommentDal
    {
        private readonly IMongoCollection<CommentDto> _comments;

        public CommentDal(IMongoDatabase database)
        {
            _comments = database.GetCollection<CommentDto>("comments");
        }

        public async Task<IEnumerable<CommentDto>> GetAllAsync()
        {
            return await _comments.Find(_ => true).ToListAsync();
        }

        public async Task<CommentDto> GetByIdAsync(ObjectId id)
        {
            return await _comments.Find(comment => comment.Id == id).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(CommentDto comment)
        {
            await _comments.InsertOneAsync(comment);
        }

        public async Task UpdateAsync(ObjectId id, CommentDto comment)
        {
            await _comments.ReplaceOneAsync(c => c.Id == id, comment);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await _comments.DeleteOneAsync(comment => comment.Id == id);
        }
    }
}