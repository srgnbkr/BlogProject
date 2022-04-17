using Microsoft.AspNetCore.Mvc;

namespace BlogProject.MvcUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

      
    }
}
