using Blog_API.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Data
{
    public class BlogDBContext: IdentityDbContext<IdentityUser>
    {
        public BlogDBContext(DbContextOptions<BlogDBContext> dbContextOptions):base(dbContextOptions)
        {
            
        }
       
        public DbSet<Post> Posts { get; set; }
       
        public DbSet<Category> Categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var readerRoleID = "5764320f-325f-45a0-a272-070d466cb321";
            var writerRoleID = "4f5026b1-fde6-4e2e-91a7-d9e213ff45f1";
            var Roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = readerRoleID,
                    ConcurrencyStamp=readerRoleID,
                    Name="Raeder",
                    NormalizedName="Reader".ToUpper()
                },
                 new IdentityRole
                {
                    Id = writerRoleID,
                    ConcurrencyStamp=writerRoleID,
                    Name="Writer",
                    NormalizedName="Writer".ToUpper()
                },
            };
            modelBuilder.Entity<IdentityRole>().HasData(Roles);
        }

    }
}
