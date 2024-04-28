using Blog.API.Models.Domain;
using Blog.API.Models.DTO;
using Blog.API.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : Controller
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        //GET: /api/Images
        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await imageRepository.GetAll();

            var res = new List<BlogImageDTO>();

            foreach (var image in images)
            {
                res.Add(new BlogImageDTO
                {
                    Id = image.Id,
                    Title = image.Title,
                    DateCreated = image.DateCreated,
                    FileExtention = image.FileExtention,
                    FileName = image.FileName,
                    Url = image.Url,
                });
            }

            return Ok(res);
        }

        //POST: /api/Images
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid)
            {
                var blogImage = new BlogImage
                {
                    FileExtention = Path.GetExtension(file.FileName.ToLower()),
                    FileName = fileName,
                    Title = title,
                    DateCreated = DateTime.Now
                };

                blogImage = await imageRepository.Upload(file, blogImage);

                var res = new BlogImageDTO
                {
                    Id = blogImage.Id,
                    Title = blogImage.Title,
                    DateCreated = blogImage.DateCreated,
                    FileExtention = blogImage.FileExtention,
                    FileName = blogImage.FileName,
                    Url = blogImage.Url,
                };

                return Ok(blogImage);
            }

            return BadRequest(ModelState);
        }


        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtentions = new string[] { ".jpg", ".png", ".jpeg" };

            if (!allowedExtentions.Contains(Path.GetExtension(file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "Unsupported file format.");
            }

            if (file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size cannot be more than 10MB.");
            }
        }
    }
}

