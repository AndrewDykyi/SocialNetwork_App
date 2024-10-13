using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SocialNetwork_App.DTO
{
    public class UserDto
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("firstName")]
        public string? FirstName { get; set; }

        [BsonElement("lastName")]
        public string?  LastName { get; set; }

        [BsonElement("interests")]
        public List<string> Interests { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("friends")]
        public List<ObjectId> Friends { get; set; } = new List<ObjectId>();
    }
}
