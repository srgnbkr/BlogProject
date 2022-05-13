using BlogProject.Entities.DTOs.RoleDto;
using BlogProject.Entities.DTOs.UserDto;

namespace BlogProject.MvcUI.Areas.Admin.Models.UsersModels
{
    public class UserRoleAssignAjaxViewModel
    {
        public UserRoleAssignDto UserRoleAssignDto { get; set; }
        public string RoleAssignPartial { get; set; }
        public UserDto UserDto { get; set; }
    }
}
