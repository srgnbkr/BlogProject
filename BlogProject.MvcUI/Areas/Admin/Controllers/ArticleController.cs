using BlogProject.Services.Abstract;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : Controller
    {
        #region Variables
        private readonly IArticleService _articleService;


        #endregion

        #region Constructor
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }
        #endregion


        #region GetAll Methods
        [HttpGet]        
        public async Task<IActionResult> Index()
        {
            var result = await _articleService.GetAllByNonDeletedAsync();
            if (result.ResultStatus == ResultStatus.Success)
                return View(result.Data);
            return NotFound();
            
            
        }
        #endregion

        #region AddMethods
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        #endregion
    }
}
