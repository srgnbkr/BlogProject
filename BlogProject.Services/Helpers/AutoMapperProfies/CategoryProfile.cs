using AutoMapper;
using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.CategoryDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services.Helpers.AutoMapperProfies
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryAddDto, Category>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<CategoryUpdateDto, Category>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));

        }
    }
}
