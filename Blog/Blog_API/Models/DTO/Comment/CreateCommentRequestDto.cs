using System.ComponentModel.DataAnnotations;

namespace Blog_API.Models.DTO.Comment
{
    public class CreateCommentRequestDto
    {
        [Required]
        public Guid PostId { get; set; }
        [Required]
        public string Content { get; set; }
        public Guid? ParentCommentId { get; set; }
    }
}
