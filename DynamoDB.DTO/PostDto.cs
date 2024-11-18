using Amazon.DynamoDBv2.DataModel;

namespace DynamoDB.DTO
{
    [DynamoDBTable("Posts")]
    public class PostDto
    {
        [DynamoDBHashKey] // Partition Key
        public string PostId { get; set; }

        [DynamoDBProperty]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public string Content { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedAt { get; set; }

        [DynamoDBProperty]
        public List<string> Tags { get; set; }

        [DynamoDBProperty]
        public int Likes { get; set; }

        [DynamoDBProperty]
        public int CommentsCount { get; set; }

        public PostDto()
        {
            Tags = new List<string>();
        }
    }
}
