using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.EmailDto;
using BlogProject.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NToastNotify;
using System;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Controllers
{
    public class HomeController : Controller
    {
        #region Variables
        private readonly IArticleService _articleService;
        private readonly AboutAsPageInfo _aboutAsPageInfo;
        private readonly IMailService _mailService;
        private readonly IToastNotification _toastNotification;
        #endregion

        #region Constructor
        public HomeController(
            IArticleService articleService,
            IOptions<AboutAsPageInfo> aboutAsPageInfo,
            IMailService mailService,
            IToastNotification toastNotification
            )

        {
            _articleService = articleService;
            _aboutAsPageInfo = aboutAsPageInfo.Value;
            _mailService = mailService;
            _toastNotification = toastNotification;
        }
        #endregion


        [HttpGet]
        public async Task<IActionResult> Index(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {

            var result = await (categoryId == null
                ? _articleService.GetAllByPagingAsync(null, currentPage, pageSize, isAscending)
                : _articleService.GetAllByPagingAsync(categoryId.Value, currentPage, pageSize, isAscending));

            return View(result.Data);

        }

        [HttpGet]
        public IActionResult About()
        {

            return View(_aboutAsPageInfo);
        }

        [HttpGet]
        public IActionResult Contact()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Contact(EmailSendDto emailSendDto)
        {
            if (ModelState.IsValid)
            {
                var result = _mailService.SendContactMail(emailSendDto);
                _toastNotification.AddSuccessToastMessage(result.Message, new ToastrOptions
                {
                    Title = "Başarılı İşlem"
                });
                return View();
            }
            return View(emailSendDto);






        }
    }
}
