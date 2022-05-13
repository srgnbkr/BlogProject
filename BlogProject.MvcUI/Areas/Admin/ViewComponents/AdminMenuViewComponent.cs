using BlogProject.Entities.Concrete;
using BlogProject.MvcUI.Areas.Admin.ViewModels.UsersViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Areas.Admin.ViewComponents
{
    public class AdminMenuViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;

        public AdminMenuViewComponent(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async  Task<IViewComponentResult> InvokeAsync()
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);
            var roles = await _userManager.GetRolesAsync(user);
           
            if (user == null)
                return Content("Kullanıcı bulunamadı.");
            
            if (roles == null)
                return Content("Roller bulunamadı.");
            
            return View(new UserWithRolesViewModel
            {
                User = user,
                Roles = roles
            });

        }
    }
}
