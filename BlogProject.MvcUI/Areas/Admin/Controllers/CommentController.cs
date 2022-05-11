using Microsoft.AspNetCore.Mvc;

namespace BlogProject.MvcUI.Areas.Admin.Controllers
{
    public class Comment : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}