using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork_App.DTO
{
    public class CommentDto
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("userId")]
        public ObjectId UserId { get; set; }

        [BsonElement("content")]
        public string? Content { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}