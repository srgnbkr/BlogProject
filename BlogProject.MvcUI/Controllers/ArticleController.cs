using BlogProject.MvcUI.Models;
using BlogProject.Services.Abstract;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword,int currentPage=1,int pageSize=5, bool isAscending=false)
        {
            var searchResult = await _articleService.SearchAsync(keyword, currentPage, pageSize, isAscending);
            if (searchResult.ResultStatus == ResultStatus.Success)
                return View(new ArticleSearchViewModel
                {
                    ArticleListDto = searchResult.Data,
                    Keyword = keyword,
                });
            return NotFound();
            
        }
        
        [HttpGet]
        public async Task<IActionResult> Detail(int articleId)
        {
            var result = await _articleService.GetAsync(articleId);
            if (result.ResultStatus == ResultStatus.Success)
                return View(result.Data);
           
            return NotFound();


        }
    }
}
