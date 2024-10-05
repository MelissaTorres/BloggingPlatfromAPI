using BloggingPlatformAPI.DTOs;
using BloggingPlatformAPI.Helpers;
using BloggingPlatformAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BloggingPlatformAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        IValidator<BlogInsertDTO> _blogInsertValidator;
        IValidator<BlogUpdateDTO> _blogUpdateValidator;
        private ICommonService<BlogDTO, BlogInsertDTO, BlogUpdateDTO> _blogService;

        public BlogsController(
            IValidator<BlogInsertDTO> blogInsertValidator,
            IValidator<BlogUpdateDTO> blogUpdateValidator,
            [FromKeyedServices("blogService")] ICommonService<BlogDTO, BlogInsertDTO, BlogUpdateDTO> blogService)
        {
            _blogInsertValidator = blogInsertValidator;
            _blogUpdateValidator = blogUpdateValidator;
            _blogService = blogService;
        }

        // GET Blogs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BlogDTO>>> Get([FromQuery] BlogQueryParameters queryParameters)
        {
            // get blog posts
            var blogPosts = await _blogService.Get();

            // queryable blog posts
            var queryableBlogPosts = blogPosts.AsQueryable();

            // filters
            queryableBlogPosts = BlogQueryParameters.FilterBlogPosts(queryableBlogPosts, queryParameters);

            // sorting
            if (queryParameters.SortBy.Count() > 0)
                queryableBlogPosts = BlogQueryParameters.ApplySorting(queryableBlogPosts, queryParameters);

            // pagination
            queryableBlogPosts = BlogQueryParameters.Pagination(queryableBlogPosts, queryParameters);

            return Ok(queryableBlogPosts.AsEnumerable()); 
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
