using BlogProject.Entities.DTOs.CategoryDto;

namespace BlogProject.MvcUI.Areas.Admin.Models.CategoryModels
{
    public class CategoryAddAjaxViewModel
    {
        public CategoryAddDto CategoryAddDto { get; set; }
        public string CategoryAddPartial { get; set; }
        public CategoryDto CategoryDto { get; set; }
    }
}
