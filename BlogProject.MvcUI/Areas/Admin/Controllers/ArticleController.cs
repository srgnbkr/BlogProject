using AutoMapper;
using BlogProject.Entities.ComplexTypes;
using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.ArticleDto;
using BlogProject.MvcUI.Areas.Admin.Models.ArticleModels;
using BlogProject.MvcUI.Helpers.Abstract;
using BlogProject.Services.Abstract;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ArticleController : BaseController
    {
        #region Variables
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;

        #endregion

        #region Constructor
        public ArticleController(IArticleService articleService, UserManager<User> userManager, ICategoryService categoryService, IMapper mapper, IImageHelper imageHelper) : base(userManager, mapper, imageHelper)
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

        #region AddMethod

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

        }

        [HttpPost]
        public async Task<IActionResult> Add(ArticleAddViewModel articleAddViewModel)
        {
            if (ModelState.IsValid)
            {
                var articleAddDto = Mapper.Map<ArticleAddDto>(articleAddViewModel);
                var imageResult = await ImageHelper.Upload(articleAddViewModel.Title, articleAddViewModel.ThumbnailFile, PictureType.Post);

                articleAddDto.Thumbnail = imageResult.Data.FullName;
                var result = await _articleService.AddAsync(articleAddDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    TempData.Add("SuccessMessage", result.Message);
                    return RedirectToAction("Index", "Article");
                }
                else
                {
                    ModelState.AddModelError("", result.Message);
                    return View(articleAddViewModel);
                }
            }
            return View(articleAddViewModel);


        }
        #endregion


        #region UpdateMethod

        
        [HttpGet]
        public async Task<IActionResult> Update(int articleId)
        {
            var articleResult = await _articleService.GetArticleUpdateDtoAsync(articleId);
            var categoryResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            if (articleResult.ResultStatus == ResultStatus.Success &&  categoryResult.ResultStatus == ResultStatus.Success)
            {
                var articleUpdateViewModel = Mapper.Map<ArticleUpdateViewModel>(articleResult.Data);
                articleUpdateViewModel.Categories = categoryResult.Data.Categories;
                return View(articleUpdateViewModel);
            }
            else
            {
                return NotFound();
            }


        }
        #endregion        
    }
}
