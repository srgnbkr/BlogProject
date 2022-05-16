using BlogProject.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Controllers
{
    public class HomeController : Controller
    {
        #region Variables
        private readonly IArticleService _articleService;
        #endregion

        #region Constructor
        public HomeController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        #endregion

        

        public async Task<IActionResult> Index(int? categoryId,int currentPage = 1,int pageSize=5,bool isAscending=false)
        {

            var result = await (categoryId == null
                ? _articleService.GetAllByPagingAsync(null, currentPage, pageSize,isAscending)
                : _articleService.GetAllByPagingAsync(categoryId.Value, currentPage, pageSize,isAscending));

            return View(result.Data);

        }


    }
}
