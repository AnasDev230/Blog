using Blog_API.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Repositories.Interface
{
    public interface ICategoryRepository
    {
        Task<Category> CreateAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByID(Guid id);
        Task<Category?> UpdateAsync(Category category);
        Task<Category?> DeleteAsync(Guid id);
    }
}
