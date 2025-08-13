using Blog_API.Models.Domain;

namespace Blog_API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<Image>Upload(IFormFile file, Image image);
        Task<IEnumerable<Image>> GetAll();
        Task<Image?> DeleteImage(Guid id);
    }
}
