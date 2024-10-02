using AutoMapper;
using BloggingPlatformAPI.DTOs;
using BloggingPlatformAPI.Models;

namespace BloggingPlatformAPI.Automappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<BlogInsertDTO, Blog>();
            CreateMap<Blog, BlogDTO>();
            CreateMap<BlogUpdateDTO, Blog>();
        }
    }
}
