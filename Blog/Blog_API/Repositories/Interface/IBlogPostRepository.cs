using Blog_API.Models.Domain;

namespace Blog_API.Repositories.Interface
{
    public interface IBlogPostRepository
    {
        Task<Post> CreateAsync(Post post);
        Task<IEnumerable<Post>> GetAllAsync(string? filterOn=null,string? filterQuery=null);
        Task<Post?> GetByID(Guid id);
        Task<Post?> UpdateAsync(Post post);
        Task<Post?> DeleteAsync(Guid id);
    }
}
