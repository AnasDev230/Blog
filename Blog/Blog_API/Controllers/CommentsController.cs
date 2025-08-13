using System.Security.Claims;
using Blog_API.Data;
using Blog_API.Models.Domain;
using Blog_API.Models.DTO.Comment;
using Blog_API.Repositories.Implementation;
using Blog_API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository commentRepository;
        private readonly IBlogPostRepository blogPostRepository;

        public CommentsController(ICommentRepository commentRepository,IBlogPostRepository blogPostRepository)
        {
            this.commentRepository = commentRepository;
            this.blogPostRepository = blogPostRepository;
        }
        [HttpPost]
        [Authorize(Roles ="Writer,Reader")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateComment(CreateCommentRequestDto request)
        {
            var postExists=await blogPostRepository.GetByID(request.PostId);
            if (postExists == null)
                return BadRequest("Post not found");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(request.Content))
                return BadRequest("Content cannot be empty");
            var comment = new Comment
            {
                PostId = request.PostId,
                Content = request.Content,
                CreatedAt = DateTime.Now,
                AuthorId = userId,
                ParentCommentId = request.ParentCommentId,

            };
           comment=await commentRepository.CreateAsync(comment);
            var response = new CommentDto
            {
                CommentId = comment.Id,
                PostId = comment.PostId,
                CreatedAt = comment.CreatedAt,
                Content = comment.Content,
                AuthorId = comment.AuthorId,
                ParentCommentId = comment.ParentCommentId,
            };

            return CreatedAtRoute(response, new { Id = response.CommentId });
        }

        [HttpGet("post/{postId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetCommentsByPostID(Guid postId)
        {
            var post = await blogPostRepository.GetByID(postId);
            if (post == null)
                return NotFound("Post not found");

            var comments = await commentRepository.GetCommentsByPostIdAsync(postId);

            var commentsDto = comments.Select(c => new CommentDto
            {
                CommentId = c.Id,
                PostId = c.PostId,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                AuthorId = c.AuthorId,
                ParentCommentId = c.ParentCommentId
            }).ToList();
            return Ok(commentsDto);
        }


        [HttpGet("{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetCommentById(Guid commentId)
        {
            var comment = await commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null)
                return NotFound("Comment not found");

            var commentDto = new CommentDto
            {
                CommentId = comment.Id,
                PostId = comment.PostId,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                AuthorId = comment.AuthorId,
                ParentCommentId = comment.ParentCommentId
            };

            return Ok(commentDto);
        }


        [HttpPut("{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> UpdateComment([FromRoute]Guid commentId, [FromBody] UpdateCommentRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.NewContent))
                return BadRequest("Content cannot be empty");

            var comment = await commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null)
                return NotFound("Comment not found");

            comment.Content = request.NewContent;
            await commentRepository.UpdateAsync(comment);

            return Ok("Comment updated");
        }

        [HttpDelete("{commentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var comment = await commentRepository.GetCommentByIdAsync(commentId);
            if (comment == null)
                return NotFound("Comment not found");

            await commentRepository.DeleteAsync(commentId);

            return Ok("Comment deleted");
        }
    }
}
