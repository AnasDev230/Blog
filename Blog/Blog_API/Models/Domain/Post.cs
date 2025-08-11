using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Blog_API.Models.Domain
{
    public class Post
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FeaturedImgUrl {  get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        [Required]
        public string AuthorId { get; set; }
        public IdentityUser Author { get; set; }
        public bool IsVisible {  get; set; }
        public ICollection<Category> Categories { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
