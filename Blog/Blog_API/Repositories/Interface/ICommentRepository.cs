using Blog_API.Models.Domain;

namespace Blog_API.Repositories.Interface
{
    public interface ICommentRepository
    {
        Task<Comment> CreateAsync(Comment comment);
        Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(Guid postId);
        Task<Comment> GetCommentByIdAsync(Guid id);
        Task<Comment>UpdateAsync(Comment comment);
        Task DeleteAsync(Guid id);
    }
}
