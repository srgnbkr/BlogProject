using Microsoft.AspNetCore.Mvc;

namespace BlogProject.MvcUI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }

      
    }
}
