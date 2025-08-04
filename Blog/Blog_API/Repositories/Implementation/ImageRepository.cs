using Blog_API.Data;
using Blog_API.Models.Domain;
using Blog_API.Repositories.Interface;

namespace Blog_API.Repositories.Implementation
{
    public class ImageRepository : IImageRepository
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly BlogDBContext blogDBContext;

        public ImageRepository(IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor,BlogDBContext blogDBContext)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.httpContextAccessor = httpContextAccessor;
            this.blogDBContext = blogDBContext;
        }

        public async Task<Image> Upload(IFormFile file, Image image)
        {
            var localPath = Path.Combine(webHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");
            using var stream = new FileStream(localPath, FileMode.Create);
            await file.CopyToAsync(stream);

            var httpRequest = httpContextAccessor.HttpContext.Request;
            var urlPath = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{image.FileName}{image.FileExtension}";
            image.Url = urlPath;
            await blogDBContext.Images.AddAsync(image);
            await blogDBContext.SaveChangesAsync();
            return image;
        }

    }
}
