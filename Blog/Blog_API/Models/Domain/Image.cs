namespace Blog_API.Models.Domain
{
    public class Image
    {
        public Guid ID { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
