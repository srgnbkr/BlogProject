using AutoMapper;
using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.ArticleDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services.Helpers.AutoMapperProfies
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<ArticleAddDto, Article>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<ArticleUpdateDto, Article>()
                .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.Now));
            CreateMap<Article, ArticleUpdateDto>();
        }
    }
}
