namespace Blog_API.Models.Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string UrlHandle {  get; set; }
        public ICollection<Post> BlogPosts { get; set; }
    }
}
