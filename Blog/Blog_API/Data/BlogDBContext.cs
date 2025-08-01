using Blog_API.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Data
{
    public class BlogDBContext:DbContext
    {
        public BlogDBContext(DbContextOptions<BlogDBContext> dbContextOptions):base(dbContextOptions)
        {
            
        }
       
        public DbSet<Post> Posts { get; set; }
       
        public DbSet<Category> Categories { get; set; }
        
    }
}
