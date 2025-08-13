using System.Security.Claims;
using Azure.Core;
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
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository blogPostRepository;

        public readonly ICategoryRepository CategoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository,ICategoryRepository categoryRepository)
        {
            this.blogPostRepository = blogPostRepository;
            CategoryRepository = categoryRepository;
        }
        [HttpPost("AddPost",Name = "CreateBlogPost")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateBlogPost([FromBody]CreateBlogPostRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Post post = new Post
            {
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                FeaturedImgUrl = request.FeaturedImgUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                AuthorId = userId,
                IsVisible = request.IsVisible,
                Categories = new List<Category>()
            };
            foreach(var categoryGuid in request.Categories)
            {
                var existingCategory=await CategoryRepository.GetByID(categoryGuid);
                if (existingCategory is not null)
                    post.Categories.Add(existingCategory);
            }

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
                AuthorId = post.AuthorId,
                IsVisible = post.IsVisible,
                Categories = post.Categories.Select(x => new CategoryDto
                {
                    Id=x.Id,
                    Name=x.Name,
                    UrlHandle=x.UrlHandle,
                }).ToList()
            };
            return CreatedAtRoute(response, new {Id=response.Id});
            
        }

        //blogposts/api/All?filterOn=Title&filterQuery=Sport
        [HttpGet("All", Name = "GetAllPosts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[Authorize(Roles ="Raeder,Writer")]
        public async Task<IActionResult> GetAllPosts([FromQuery] string? filterOn, [FromQuery] string? filterQuery)
        {
            var Posts = await blogPostRepository.GetAllAsync(filterOn,filterQuery);
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
                    AuthorId = post.AuthorId,
                    IsVisible = post.IsVisible,
                    Categories = post.Categories.Select(x => new CategoryDto
                    {
                        Id = x.Id,
                        Name = x.Name,
                        UrlHandle = x.UrlHandle,
                    }).ToList()
                });
            }
            return Ok(response);
        }



        [HttpGet("GetByID/{ID}", Name = "GetPostByID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Raeder,Writer")]
        public async Task<IActionResult> GetPostByID([FromRoute] Guid ID)
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
                AuthorId = post.AuthorId,
                IsVisible = post.IsVisible,
                Categories = post.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };
            return Ok(blogPostDto);
        }


        [HttpPut("{ID:guid}", Name = "EditPost")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditPost([FromRoute] Guid ID, UpdateBlogPostRequestDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Post post = new Post
            {
                Id = ID,
                Title = request.Title,
                ShortDescription = request.ShortDescription,
                Content = request.Content,
                FeaturedImgUrl = request.FeaturedImgUrl,
                UrlHandle = request.UrlHandle,
                PublishedDate = request.PublishedDate,
                AuthorId = userId,
                IsVisible = request.IsVisible,
                Categories = new List<Category>()
            };

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await CategoryRepository.GetByID(categoryGuid);
                if (existingCategory is not null)
                    post.Categories.Add(existingCategory);
            }



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
                AuthorId = post.AuthorId,
                IsVisible = post.IsVisible,
                Categories = post.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlHandle = x.UrlHandle,
                }).ToList()
            };
            return Ok(response);
        }
        [HttpDelete]
        [Route("{ID:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeletePost([FromRoute] Guid ID)
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
                AuthorId = post.AuthorId,
                IsVisible = post.IsVisible,
            };
            return Ok(response);

        }

    }
}
