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
            return await dBContext.Posts.ToListAsync();

        }

        public async Task<Post> GetByID(int ID)
        {
            return await dBContext.Posts.FirstOrDefaultAsync(x => x.Id == ID);
        }


        public async Task<Post?> UpdateAsync(Post post)
        {
            Post existingPost = await dBContext.Posts.FirstOrDefaultAsync(x => x.Id == post.Id);
            if (existingPost != null)
            {
                dBContext.Entry(existingPost).CurrentValues.SetValues(post);
                await dBContext.SaveChangesAsync();
                return post;
            }
            return null;
        }
        public async Task<Post> DeleteAsync(int ID)
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
