using Amazon.DynamoDBv2.DataModel;

namespace DynamoDB.DTO
{
    [DynamoDBTable("Comments")]
    public class CommentDto
    {
        [DynamoDBHashKey]
        public string CommentId { get; set; }

        [DynamoDBProperty]
        public string PostId { get; set; }

        [DynamoDBProperty]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public string Content { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedAt { get; set; }
    }
}