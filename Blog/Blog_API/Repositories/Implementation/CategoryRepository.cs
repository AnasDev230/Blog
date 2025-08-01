using Blog_API.Data;
using Blog_API.Models.Domain;
using Blog_API.Models.DTO;
using Blog_API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Repositories.Implementation
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BlogDBContext dBContext;

        public CategoryRepository(BlogDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Category> CreateAsync(Category category)
        {
            await dBContext.Categories.AddAsync(category);
            await dBContext.SaveChangesAsync();
            return category;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await dBContext.Categories.ToListAsync();
            
        }

        public async Task<Category> GetByID(Guid ID)
        {
            return await dBContext.Categories.FirstOrDefaultAsync(x => x.Id == ID);
        }
        

        public async Task<Category?> UpdateAsync(Category category)
        {
           Category existingCategory= await dBContext.Categories.FirstOrDefaultAsync(x=>x.Id == category.Id);
            if (existingCategory != null)
            {
                 dBContext.Entry(existingCategory).CurrentValues.SetValues(category);
                await dBContext.SaveChangesAsync();
                return category;
            }
            return null;
        }
        public async Task<Category> DeleteAsync(Guid ID)
        {
            Category category= await dBContext.Categories.FirstOrDefaultAsync(x=>x.Id == ID);
            if (category is null) {
                return null;
            }
            dBContext.Categories.Remove(category);
            await dBContext.SaveChangesAsync();
            return category;
        }
    }
}
