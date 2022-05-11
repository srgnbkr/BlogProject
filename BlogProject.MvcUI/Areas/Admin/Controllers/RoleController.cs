using Microsoft.AspNetCore.Mvc;

namespace BlogProject.MvcUI.Areas.Admin.Controllers
{
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}