using MongoDB.Bson;
using MongoDB.Driver;
using SocialNetwork_App.DTO;
using SocialNetwork_App.DAL.Interface;

namespace SocialNetwork_App.DAL.Concrete
{
    public class PostDal : IPostDal
    {
        private readonly IMongoCollection<PostDto> _posts;

        public PostDal(IMongoDatabase database)
        {
            _posts = database.GetCollection<PostDto>("posts");
        }

        public async Task<IEnumerable<PostDto>> GetAllAsync()
        {
            return await _posts.Find(_ => true).ToListAsync();
        }

        public async Task<PostDto> GetByIdAsync(ObjectId id)
        {
            return await _posts.Find(post => post.Id == id).FirstOrDefaultAsync();
        }

        public async Task InsertAsync(PostDto post)
        {
            post.Id = ObjectId.GenerateNewId();
            await _posts.InsertOneAsync(post);
        }

        public async Task UpdateAsync(ObjectId id, PostDto post)
        {
            await _posts.ReplaceOneAsync(p => p.Id == id, post);
        }

        public async Task DeleteAsync(ObjectId id)
        {
            await _posts.DeleteOneAsync(post => post.Id == id);
        }

        public async Task<int> GetCommentCountAsync(ObjectId userId)
        {
            var filter = Builders<PostDto>.Filter.Eq(p => p.UserId, userId);
            var commentCount = await _posts.CountDocumentsAsync(filter);
            return (int)commentCount;
        }

        public async Task<int> GetLikeCountAsync(ObjectId userId)
        {
            var filter = Builders<PostDto>.Filter.And(
                Builders<PostDto>.Filter.Eq(p => p.UserId, userId),
                Builders<PostDto>.Filter.Eq("reactionType", "like")
            );
            var likeCount = await _posts.CountDocumentsAsync(filter);
            return (int)likeCount;
        }

        public async Task AddCommentAsync(ObjectId postId, CommentDto comment)
        {
            var update = Builders<PostDto>.Update.Push(p => p.Comments, comment);
            await _posts.UpdateOneAsync(p => p.Id == postId, update);
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsAsync(ObjectId postId)
        {
            var post = await GetByIdAsync(postId);
            return post?.Comments ?? new List<CommentDto>();
        }

        public async Task AddReactionAsync(ObjectId postId, ReactionDto reaction)
        {
            var update = Builders<PostDto>.Update.Push(p => p.Reactions, reaction);
            await _posts.UpdateOneAsync(p => p.Id == postId, update);
        }

        public async Task<IEnumerable<ReactionDto>> GetReactionsAsync(ObjectId postId)
        {
            var post = await GetByIdAsync(postId);
            return post?.Reactions ?? new List<ReactionDto>(); ;
        }
    }
}
