using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork_App.DTO
{
    public class FriendshipDto
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("userId1")]
        public ObjectId UserId1 { get; set; }

        [BsonElement("userId2")]
        public ObjectId UserId2 { get; set; }

        [BsonElement("relationshipType")]
        public string? RelationshipType { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}