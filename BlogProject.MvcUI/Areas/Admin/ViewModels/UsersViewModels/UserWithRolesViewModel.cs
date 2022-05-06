using BlogProject.Entities.Concrete;
using System.Collections.Generic;

namespace BlogProject.MvcUI.Areas.Admin.ViewModels.UsersViewModels
{
    public class UserWithRolesViewModel
    {
        public  User User { get; set; }
        public IList<string> Roles { get; set; }
    }

}
