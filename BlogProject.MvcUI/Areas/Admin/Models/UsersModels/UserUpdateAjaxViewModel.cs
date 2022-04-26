using BlogProject.Entities.DTOs.UserDto;

namespace BlogProject.MvcUI.Areas.Admin.Models.UsersModels
{
    public class UserUpdateAjaxViewModel
    {
        public UserUpdateDto UserUpdateDto { get; set; }
        public string UserUpdatePartial { get; set; }
        public UserDto UserDto { get; set; }
    }
}
