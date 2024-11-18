using DynamoDB.BLL.Interfaces;
using DynamoDB.DAL.Interfaces;
using DynamoDB.DTO;

namespace DynamoDB.BLL.Services
{
    public class PostService : IPostService
    {
        private readonly IPostDal _postDal;

        public PostService(IPostDal postDal)
        {
            _postDal = postDal;
        }

        public async Task AddPostAsync(PostDto post)
        {
            await _postDal.InsertPostAsync(post);
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync()
        {
            return await _postDal.GetAllPostsAsync();
        }
    }
}
