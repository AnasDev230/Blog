using Blog_API.Models.Domain;
using Blog_API.Models.DTO;
using Blog_API.Repositories.Implementation;
using Blog_API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository)
        {
            this.blogPostRepository = blogPostRepository;
        }
        [HttpPost("AddPost",Name = "CreateBlogPost")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateBlogPost([FromBody]CreateBlogPostRequestDto request)
        {
            Post post = new Post
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                FeaturedImgUrl = request.FeaturedImgUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                Author=request.Author,
                IsVisible=request.IsVisible,
            };


           post= await blogPostRepository.CreateAsync(post);
            BlogPostDto response = new BlogPostDto
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Content = post.Content,
                FeaturedImgUrl = post.FeaturedImgUrl,
                UrlHandle = post.UrlHandle,
                PublishedDate = post.PublishedDate,
                Author = post.Author,
                IsVisible = post.IsVisible,
            };
            return CreatedAtRoute(response, new {Id=response.Id});
            
        }


        [HttpGet("All", Name = "GetAllPosts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPosts()
        {
            var Posts = await blogPostRepository.GetAllAsync();
            List<BlogPostDto> response = new List<BlogPostDto>();
            foreach (var post in Posts)
            {
                response.Add(new BlogPostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    ShortDescription = post.ShortDescription,
                    Content = post.Content,
                    FeaturedImgUrl = post.FeaturedImgUrl,
                    UrlHandle = post.UrlHandle,
                    PublishedDate = post.PublishedDate,
                    Author = post.Author,
                    IsVisible = post.IsVisible,
                });
            }
            return Ok(response);
        }







        [HttpGet("GetByID/{ID}", Name = "GetPostByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostByID([FromRoute] int ID)
        {
            Post post = await blogPostRepository.GetByID(ID);
            if (post == null)
                return NotFound();
            BlogPostDto blogPostDto = new BlogPostDto
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Content = post.Content,
                FeaturedImgUrl = post.FeaturedImgUrl,
                UrlHandle = post.UrlHandle,
                PublishedDate = post.PublishedDate,
                Author = post.Author,
                IsVisible = post.IsVisible,
            };
            return Ok(blogPostDto);
        }







        [HttpPut("{ID:int}", Name = "EditPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> EditPost([FromRoute] int ID, UpdateBlogPostRequestDto updateBlogPostRequest)
        {
            Post post = new Post
            {
                Id = ID,
                Title = updateBlogPostRequest.Title,
                ShortDescription = updateBlogPostRequest.ShortDescription,
                Content = updateBlogPostRequest.Content,
                FeaturedImgUrl = updateBlogPostRequest.FeaturedImgUrl,
                UrlHandle = updateBlogPostRequest.UrlHandle,
                PublishedDate = updateBlogPostRequest.PublishedDate,
                Author = updateBlogPostRequest.Author,
                IsVisible = updateBlogPostRequest.IsVisible,
            };
            post = await blogPostRepository.UpdateAsync(post);
            if (post == null)
                return NotFound();
            BlogPostDto response = new BlogPostDto
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Content = post.Content,
                FeaturedImgUrl = post.FeaturedImgUrl,
                UrlHandle = post.UrlHandle,
                PublishedDate = post.PublishedDate,
                Author = post.Author,
                IsVisible = post.IsVisible,
            };
            return Ok(response);
        }
        [HttpDelete]
        [Route("{ID:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePost([FromRoute] int ID)
        {
            var post = await blogPostRepository.DeleteAsync(ID);
            if (post is null)
                return NotFound();
            BlogPostDto response = new BlogPostDto
            {
                Id = post.Id,
                Title = post.Title,
                ShortDescription = post.ShortDescription,
                Content = post.Content,
                FeaturedImgUrl = post.FeaturedImgUrl,
                UrlHandle = post.UrlHandle,
                PublishedDate = post.PublishedDate,
                Author = post.Author,
                IsVisible = post.IsVisible,
            };
            return Ok(response);

        }



    }
}
