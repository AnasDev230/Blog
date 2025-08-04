namespace Blog_API.Models.Domain
{
    public class Post
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string FeaturedImgUrl {  get; set; }
        public string UrlHandle { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Author {  get; set; }
        public bool IsVisible {  get; set; }
        public ICollection<Category> Categories { get; set; }
    }
}
