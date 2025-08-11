using System.ComponentModel.DataAnnotations;

namespace Blog_API.Models.DTO
{
    public class UpdateBlogPostRequestDto
    {
        [Required]
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FeaturedImgUrl { get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public bool IsVisible { get; set; }
        public List<Guid> Categories { get; set; }=new List<Guid>();    
    }
}
