using Blog_API.Models.Domain;

namespace Blog_API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<Post> CreateAsync(Post post);
        Task<IEnumerable<Post>> GetAllAsync();
        Task<Post?> GetByID(int id);
        Task<Post?> UpdateAsync(Post post);
        Task<Post?> DeleteAsync(int id);
    }
}
