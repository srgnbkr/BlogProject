using Microsoft.AspNetCore.Mvc;

namespace BlogProject.MvcUI.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
