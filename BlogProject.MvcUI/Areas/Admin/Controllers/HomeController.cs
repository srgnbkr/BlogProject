using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogProject.MvcUI.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        
        [Area("Admin")]
        [Authorize(Roles = "Admin,Editor")]
        public IActionResult Index()
        {
            return View();
        }

      
    }
}
