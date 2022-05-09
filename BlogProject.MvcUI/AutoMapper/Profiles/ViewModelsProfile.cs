using AutoMapper;
using BlogProject.Entities.DTOs.ArticleDto;
using BlogProject.MvcUI.Areas.Admin.Models.ArticleModels;

namespace BlogProject.MvcUI.AutoMapper.Profiles
{
    public class ViewModelsProfile : Profile
    {
        public ViewModelsProfile()
        {
            CreateMap<ArticleAddViewModel, ArticleAddDto>();
            CreateMap<ArticleUpdateDto, ArticleUpdateViewModel>().ReverseMap();
        }
    }
}
