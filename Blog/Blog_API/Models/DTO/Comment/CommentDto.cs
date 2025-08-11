namespace Blog_API.Models.DTO.Comment
{
    public class CommentDto
    {
        public Guid CommentId { get; set; }
        public Guid PostId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string AuthorId { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
