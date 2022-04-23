using BlogProject.Entities.DTOs.UserDto;

namespace BlogProject.MvcUI.Areas.Admin.Models.UsersModels
{
    public class UserAddAjaxViewModel
    {
        public UserAddDto UserAddDto { get; set; }
        public string UserAddPartial { get; set; }
        public UserDto UserDto { get; set; }
    }
}
