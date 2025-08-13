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
        public async Task<IEnumerable<Post>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true)
        {
            var posts = dBContext.Posts.Include(x => x.Categories).AsQueryable();
            if(string.IsNullOrWhiteSpace(filterOn)==false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    posts=posts.Where(x=>x.Title.Contains(filterQuery));
                }
            }
            if (string.IsNullOrWhiteSpace(sortBy)==false){
                if (sortBy.Equals("Title", StringComparison.OrdinalIgnoreCase))
                {
                    posts = isAscending ? posts.OrderBy(x=>x.Title):posts.OrderByDescending(x => x.Title);
                }
            }
            
            return await posts.ToListAsync(); 
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
