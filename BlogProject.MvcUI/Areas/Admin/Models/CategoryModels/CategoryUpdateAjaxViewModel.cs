using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogProject.Entities.DTOs.CategoryDto;

namespace BlogProject.MvcUI.Areas.Admin.Models.CategoryModels
{
    public class CategoryUpdateAjaxViewModel
    {
        public CategoryUpdateDto CategoryUpdateDto { get; set; }
        public string  CategoryUpdatePartial { get; set; }
        public CategoryDto CategoryDto { get; set; }


    }
}