using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.ArticleDto;
using System.Collections.Generic;

namespace BlogProject.MvcUI.Areas.Admin.Models.DashboardModels
{
    public class DashboadrViewModel
    {
        public int CategoriesCount { get; set; }
        public int ArticlesCount { get; set; }
        public int CommentsCount { get; set; }
        public int UsersCount { get; set; }
        public ArticleListDto Articles { get; set; }
    }
}
