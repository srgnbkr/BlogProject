using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.EmailDto;
using BlogProject.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Controllers
{
    public class HomeController : Controller
    {
        #region Variables
        private readonly IArticleService _articleService;
        private readonly AboutAsPageInfo _aboutAsPageInfo;
        #endregion

        #region Constructor
        public HomeController(IArticleService articleService,IOptions<AboutAsPageInfo> aboutAsPageInfo)
        {
            _articleService = articleService;
            _aboutAsPageInfo = aboutAsPageInfo.Value;
        }
        #endregion


        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId,int currentPage = 1,int pageSize=5,bool isAscending=false)
        {

            var result = await (categoryId == null
                ? _articleService.GetAllByPagingAsync(null, currentPage, pageSize,isAscending)
                : _articleService.GetAllByPagingAsync(categoryId.Value, currentPage, pageSize,isAscending));

            return View(result.Data);

        }

        [HttpGet]
        public  IActionResult About()
        {
            
            return View(_aboutAsPageInfo);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            throw new NullReferenceException();
            return View();
        }

        [HttpPost]
        public IActionResult Contact(EmailSendDto emailSendDto)
        {
            return View();
        }






    }
}
