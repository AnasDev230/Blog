using Blog_API.Data;
using Blog_API.Models.Domain;
using Blog_API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
namespace Blog_API.Repositories.Implementation
{
    
    public class BlogPostRepository:IBlogPostRepository
    {
        private readonly BlogDBContext dBContext;

        public BlogPostRepository(BlogDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Post> CreateAsync(Post post)
        {
            await dBContext.Posts.AddAsync(post);
            await dBContext.SaveChangesAsync();
            return post;
        }
        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await dBContext.Posts.Include(x=>x.Categories).ToListAsync();

        }

        public async Task<Post> GetByID(Guid ID)
        {
            return await dBContext.Posts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == ID);
        }


        public async Task<Post?> UpdateAsync(Post post)
        {
            Post existingPost = await dBContext.Posts.Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == post.Id);
            if (existingPost == null)
            {
                return null; 
            }
            dBContext.Entry(existingPost).CurrentValues.SetValues(post);
            existingPost.Categories=post.Categories;
            await dBContext.SaveChangesAsync();
            return post;
           
        }
        public async Task<Post> DeleteAsync(Guid ID)
        {
            Post post = await dBContext.Posts.FirstOrDefaultAsync(x => x.Id == ID);
            if (post is null)
            {
                return null;
            }
            dBContext.Posts.Remove(post);
            await dBContext.SaveChangesAsync();
            return post;
        }
    }
}
