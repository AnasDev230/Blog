using Blog_API.Models.Domain;
using Blog_API.Models.DTO;
using Blog_API.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blog_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]       
        
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, 
            [FromForm] string FileName, [FromForm] string Title)
        {
            ValidateFileUpload(file);
            if (ModelState.IsValid)
            {
                //Image Upload
                var Image = new Image
                {
                    FileExtension = Path.GetExtension(file.FileName).ToLower(),
                    FileName = FileName,
                    Title = Title,
                    DateCreated = DateTime.Now,
                };
                Image = await imageRepository.Upload(file, Image);
                ImageDto response = new ImageDto
                {
                    ID = Image.ID,
                    FileExtension = Image.FileExtension,
                    FileName = Image.FileName,
                    Title = Title,
                    DateCreated = DateTime.Now,
                };
                return Ok(response);
            }
            return BadRequest(ModelState);    
        }
        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpej", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported File Format");
            }
            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File Size Cannot Be More Than 10 Mb");
            }
        }
    }
}
