using Asp.Versioning;
using BloggingPlatformAPI.DTOs;
using BloggingPlatformAPI.Helpers;
using BloggingPlatformAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatformAPI.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        IValidator<BlogInsertDTO> _blogInsertValidator;
        IValidator<BlogUpdateDTO> _blogUpdateValidator;
        IValidator<BlogUpdateDTO> _blogPatchValidator;
        private ICommonService<BlogDTO, BlogInsertDTO, BlogUpdateDTO> _blogService;

        public BlogsController(
            IValidator<BlogInsertDTO> blogInsertValidator,
            IValidator<BlogUpdateDTO> blogUpdateValidator,
            IValidator<BlogUpdateDTO> blogPatchValidator,
            [FromKeyedServices("blogService")] ICommonService<BlogDTO, BlogInsertDTO, BlogUpdateDTO> blogService)
        {
            _blogInsertValidator = blogInsertValidator;
            _blogUpdateValidator = blogUpdateValidator;
            _blogPatchValidator = blogPatchValidator;
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
            if (queryParameters.SortBy != null)
                queryableBlogPosts = QueryParameters.ApplySorting(queryableBlogPosts, queryParameters);

            // pagination
            queryableBlogPosts = QueryParameters.Pagination(queryableBlogPosts, queryParameters);

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

        [HttpPatch("{id}")]
        public async Task<ActionResult<BlogDTO>> Patch(int id, BlogUpdateDTO blogPatchDTO)
        {
            // validate patch
            var validationResult = await _blogPatchValidator.ValidateAsync(blogPatchDTO);

            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var blogDTO = await _blogService.Patch(id, blogPatchDTO);

            return blogDTO == null? NotFound() : Ok(blogDTO);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<BlogDTO>> Delete(int id)
        {
            var blogDTO = await _blogService.Delete(id);

            return blogDTO == null ? NotFound() : NoContent();
        }
    }
}
