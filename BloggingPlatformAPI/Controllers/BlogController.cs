using BloggingPlatformAPI.DTOs;
using BloggingPlatformAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatformAPI.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<IEnumerable<BlogDTO>> Get() => await _blogService.Get();

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
            var blogDTO = _blogService.Delete(id);

            return blogDTO == null ? NotFound() : NoContent();
        }
    }
}
