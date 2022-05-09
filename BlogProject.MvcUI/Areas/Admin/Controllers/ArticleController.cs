using BlogProject.MvcUI.Areas.Admin.Models.ArticleModels;
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
        private readonly ICategoryService _categoryService;

        #endregion

        #region Constructor
        public ArticleController(IArticleService articleService, ICategoryService categoryService)
        {
            _articleService = articleService;
            _categoryService = categoryService;
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
        public async Task<IActionResult> Add()
        {
            var result = await _categoryService.GetAllByNonDeletedAsync();
            if (result.ResultStatus == ResultStatus.Success)
            {
                return View(new ArticleAddViewModel
                {
                    Categories = result.Data.Categories
                });
            }
            return NotFound();
            #endregion
        }
    }
}    
