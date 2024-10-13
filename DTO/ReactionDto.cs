using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork_App.DTO
{
    public class ReactionDto
    {
        [BsonElement("userId")]
        public ObjectId UserId { get; set; }

        [BsonElement("reactionType")]
        public string? ReactionType { get; set; }
    }
}