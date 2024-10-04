using BloggingPlatformAPI.DTOs;
using BloggingPlatformAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatformAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        IValidator<BlogInsertDTO> _blogInsertValidator;
        IValidator<BlogUpdateDTO> _blogUpdateValidator;
        private ICommonService<BlogDTO, BlogInsertDTO, BlogUpdateDTO> _blogService;
        public BlogController(
            IValidator<BlogInsertDTO> blogInsertValidator,
            IValidator<BlogUpdateDTO> blogUpdateValidator,
            [FromKeyedServices("blogService")] ICommonService<BlogDTO, BlogInsertDTO, BlogUpdateDTO> blogService)
        {
            _blogInsertValidator = blogInsertValidator;
            _blogUpdateValidator = blogUpdateValidator;
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDTO>>> Get(int page = 1, int limit = 10)
        {
            // validate page and limit (page size)
            if (page <= 0 || limit <= 0)
                return BadRequest("Page and Limit must be greater than zero.");

            // get blog posts
            var blogPosts = await _blogService.Get();

            // pagination using skip and take
            var paginatedBlogPosts = blogPosts
                .Skip((page - 1) * limit) // skip items from previous pages
                .Take(limit)
                .ToList();


            return Ok(paginatedBlogPosts); 
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BlogDTO>> GetById(int id)
        {
            var blogDTO = await _blogService.GetById(id);
            return blogDTO == null ? NotFound() : Ok(blogDTO);
        }

        [HttpPost]
        public async Task<ActionResult<BlogDTO>> Add(BlogInsertDTO blogInsertDTO)
        {
            // validate blogInsertDTO
            var validationResult = await _blogInsertValidator.ValidateAsync(blogInsertDTO);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var blogDTO = await _blogService.Add(blogInsertDTO);

            return blogDTO == null ? BadRequest() : Ok(blogDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BlogDTO>> Update(int id, BlogUpdateDTO blogUpdateDTO)
        {
            // validate blogUpdateDTO
            var validationResult = await _blogUpdateValidator.ValidateAsync(blogUpdateDTO);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var blogDTO = await _blogService.Update(id, blogUpdateDTO);

            return blogDTO == null ? NotFound() : Ok(blogDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BlogDTO>> Delete(int id)
        {
            var blogDTO = await _blogService.Delete(id);

            return blogDTO == null ? NotFound() : NoContent();
        }
    }
}
