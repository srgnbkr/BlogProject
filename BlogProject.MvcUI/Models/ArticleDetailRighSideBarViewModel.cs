using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.ArticleDto;

namespace BlogProject.MvcUI.Models
{
    public class ArticleDetailRighSideBarViewModel
    {
        public string Header { get; set; }
        public ArticleListDto ArticleListDto { get; set; }
        public User User { get; set; }
    }
}
