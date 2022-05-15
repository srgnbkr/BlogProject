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



        public async Task<IActionResult> Index()
        {
            var result = await _articleService.GetAllByNonDeletedAndActiveAsync();
            return View(result.Data);
        }

      
    }
}
