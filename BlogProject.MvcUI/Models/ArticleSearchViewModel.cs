using BlogProject.Entities.DTOs.ArticleDto;

namespace BlogProject.MvcUI.Models
{
    public class ArticleSearchViewModel
    {
        public ArticleListDto ArticleListDto { get; set; }
        public string Keyword { get; set; }
    }
}
