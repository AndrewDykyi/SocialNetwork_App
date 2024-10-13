using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork_App.DTO
{
    public class PostDto
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("title")]
        public string? Title { get; set; }

        [BsonElement("content")]
        public string? Content { get; set; }

        [BsonElement("userId")]
        public ObjectId UserId { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("comments")]
        public List<CommentDto> Comments { get; set; } = new List<CommentDto>();

        [BsonElement("reactions")]
        public List<ReactionDto> Reactions { get; set; } = new List<ReactionDto>();
    }
}
