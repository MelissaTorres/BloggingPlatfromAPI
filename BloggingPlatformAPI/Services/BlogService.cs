using BloggingPlatformAPI.Models;
using BloggingPlatformAPI.DTOs;
using BloggingPlatformAPI.Repository;
using AutoMapper;
using Microsoft.OpenApi.Validations;
using System.Text.Json;
using BloggingPlatformAPI.Helpers;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BloggingPlatformAPI.Services
{
    public class BlogService : ICommonService<BlogDTO, BlogInsertDTO, BlogUpdateDTO>
    {
        public List<string> Errors { get; }
        private IRepository<Blog> _blogRepository;
        private IMapper _mapper;

        public BlogService(IRepository<Blog> blogRepository,
            IMapper mapper) 
        {
            // repository
            _blogRepository = blogRepository;
            // mappers
            _mapper = mapper;
            Errors = new List<string>();
        }

        public async Task<IEnumerable<BlogDTO>> Get()
        {
            // get blog posts
            var blogPosts = await _blogRepository.Get();

            // convert blog posts to blogDTO
            return blogPosts.Select(b => _mapper.Map<BlogDTO>(b));
        }

        public async Task<BlogDTO> GetById(int id)
        {
            // look for blog post
            var blogPost = await _blogRepository.GetById(id);

            // if not null or empty, turn into blogDTO and return it
            if (blogPost != null)
            {
                var blogDTO = _mapper.Map<BlogDTO>(blogPost);
                return blogDTO;
            }

            return null;
        }

        public async Task<BlogDTO> Add(BlogInsertDTO blogInsertDTO)
        {
            Blog blogPost = null;

            // deep clone blogInsertDTO
            var blogInsertCopy = Utils.DeepClone(blogInsertDTO);

            // set CreatedAt, UpdatedAt
            if (blogInsertCopy != null)
            {
                blogInsertCopy.CreatedAt = DateTime.UtcNow;
                blogInsertCopy.UpdatedAt = DateTime.UtcNow;
                blogPost = _mapper.Map<Blog>(blogInsertCopy);
            }
            else
            {
                // convert blogInsertDTO into blogDTO
                blogPost = _mapper.Map<Blog>(blogInsertDTO);
            }

            // insert into db
            await _blogRepository.Add(blogPost);
            // save changes
            await _blogRepository.Save();

            // map blogPost to BlogDTO
            var blogDTO = _mapper.Map<BlogDTO>(blogPost);
           
            return blogDTO;
        }

        public async Task<BlogDTO> Update(int id, BlogUpdateDTO blogUpdateDTO)
        {
            // find blog post
            var blogPost = await _blogRepository.GetById(id);

            // if it's not null
            if (blogPost != null)
            {
                // map updated values to blogPost
                blogPost = _mapper.Map(blogUpdateDTO, blogPost);
                // set blogPost's UpdatedAt
                blogPost.UpdatedAt = DateTime.UtcNow;

                // call update method
                _blogRepository.Update(blogPost);
                // save changes
                await _blogRepository.Save();

                // map blogPost to BlogDTO, return it
                var blogDTO = _mapper.Map<BlogDTO>(blogPost);

                return blogDTO;
            }

            return null;
        }

        public async Task<BlogDTO> Patch(int id, BlogUpdateDTO blogUpdateDTO)
        {
            // find blog
            var blogPost = await _blogRepository.GetById(id);

            // if it's not null
            if (blogPost != null)
            {
                blogPost.Title = blogUpdateDTO.Title != null ? blogUpdateDTO.Title : blogPost.Title;
                blogPost.Content = blogUpdateDTO.Content != null ? blogUpdateDTO.Content : blogPost.Content;
                blogPost.Category = blogUpdateDTO.Category != null ? blogUpdateDTO.Category : blogPost.Category;
                blogPost.Tags = blogUpdateDTO.Tags != null ? blogUpdateDTO.Tags : blogPost.Tags;

                // set updated at
                blogPost.UpdatedAt = DateTime.UtcNow;

                // call update and save methods
                _blogRepository.Update(blogPost);
                await _blogRepository.Save();

                // map blogPost to blogDTO, return it
                var blogDTO = _mapper.Map<BlogDTO>(blogPost);

                return blogDTO;
            }

            return null;
        }
            

        public async Task<BlogDTO> Delete(int id)
        {
            // find blog post
            var blogPost = await _blogRepository.GetById(id);

            // if it's not null
            if (blogPost != null)
            {
                // delete
                _blogRepository.Delete(blogPost);
                // save
                await _blogRepository.Save();

                // map blogPost to BlogDTO, return it
                var blogDTO = _mapper.Map<BlogDTO>(blogPost);

                return blogDTO;
            }

            return null;
        }

        public bool Validate(BlogInsertDTO insertDTO)
        {
            throw new NotImplementedException();
        }

        public bool Validate(BlogUpdateDTO updateDTO)
        {
            throw new NotImplementedException();
        }
    }
}
