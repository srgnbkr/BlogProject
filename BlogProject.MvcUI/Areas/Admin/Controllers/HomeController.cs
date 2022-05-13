using BlogProject.Entities.Concrete;
using BlogProject.MvcUI.Areas.Admin.Models.DashboardModels;
using BlogProject.Services.Abstract;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {

        #region Variables
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        #endregion


        #region Constructor
        public HomeController(UserManager<User> userManager, ICategoryService categoryService, IArticleService articleService, ICommentService commentService)
        {
            _userManager = userManager;
            _articleService = articleService;
            _categoryService = categoryService;
            _commentService = commentService;
        }
        #endregion


        
        [Area("Admin")]
        [Authorize(Roles = "SuperAdmin,Editor")]
        public async Task<IActionResult> Index() 
        {
            var categoriesCountResult = await _categoryService.CountByNonDeleted();
            var articlesCountResult = await _articleService.CountByNonDeleted();
            var commentsCountResult = await _commentService.CountByNonDeletedAsync();
            var usersCountResult = await _userManager.Users.CountAsync();
            var articlesResult = await _articleService.GetAllAsync();

            if (categoriesCountResult.ResultStatus == ResultStatus.Success 
                && articlesCountResult.ResultStatus == ResultStatus.Success 
                && commentsCountResult.ResultStatus == ResultStatus.Success 
                && usersCountResult>-1 && articlesResult.ResultStatus == ResultStatus.Success)
            {
                return View(new DashboadrViewModel
                {
                    CategoriesCount = categoriesCountResult.Data,
                    ArticlesCount = articlesCountResult.Data,
                    CommentsCount = commentsCountResult.Data,
                    UsersCount = usersCountResult,
                    Articles = articlesResult.Data
                });
            }
            return NotFound();

                
        }

      
    }
}
