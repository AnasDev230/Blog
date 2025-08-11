using Blog_API.Data;
using Blog_API.Models.Domain;
using Blog_API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Blog_API.Repositories.Implementation
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogDBContext dBContext;

        public CommentRepository(BlogDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Comment> CreateAsync(Comment comment)
        {
            await dBContext.Comments.AddAsync(comment);
            await dBContext.SaveChangesAsync();
            return comment;
        }

        public async Task DeleteAsync(Guid id)
        {
            var comment = await dBContext.Comments.FindAsync(id);
            if (comment != null)
            {
                dBContext.Comments.Remove(comment);
                await dBContext.SaveChangesAsync();
            }
        }

        public async Task<Comment> GetCommentByIdAsync(Guid id)
        {
            return await dBContext.Comments
            .Include(c => c.Replies)
            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(Guid postId)
        {
            return await dBContext.Comments.Include(c=>c.Replies).Where(c=>c.PostId == postId&&c.ParentCommentId==null).ToListAsync();
        }

        public async Task<Comment> UpdateAsync(Comment comment)
        {
            dBContext.Comments.Update(comment);
            await dBContext.SaveChangesAsync();
            return comment;
        }
    }
}
