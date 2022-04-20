using BlogProject.Entities.DTOs.CategoryDto;
using BlogProject.MvcUI.Areas.Admin.Models.CategoryModels;
using BlogProject.Services.Abstract;
using BlogProject.Shared.Utilities.Extensions;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await  _categoryService.GetAllByNonDeletedAsync();
            return View(result.Data);
            
            
        }

        [HttpGet]
        public IActionResult Add()
        {
            return PartialView("_CategoryAddPartial");
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryAddDto categoryAddDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _categoryService.AddAsync(categoryAddDto, "Sergen Bakır");
                if (result.ResultStatus==ResultStatus.Success)
                {
                    var categoryAddAjaxModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
                    {
                        CategoryDto = result.Data,
                        CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto)
                    });
                    return Json(categoryAddAjaxModel);
                }
            }
            var categoryAddAjaxErrorModel = JsonSerializer.Serialize(new CategoryAddAjaxViewModel
            {
                CategoryAddPartial = await this.RenderViewToStringAsync("_CategoryAddPartial", categoryAddDto)
            });
            return Json(categoryAddAjaxErrorModel);
            
        }


        public async Task<JsonResult> GetAllCategory()
        {
            var result = await _categoryService.GetAllAsync();
            var categoires = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categoires);
        }

        public async Task<JsonResult> Delete(int categoryId)
        {
            var result = await _categoryService.DeleteAsync(categoryId, "Sergen Bakır");
            var deletedCategory = JsonSerializer.Serialize(result.Data);
            return Json(deletedCategory);
        }

        public async Task<JsonResult> GetAllCategories()
        {
            var result = await _categoryService.GetAllByNonDeletedAsync();
            var categories = JsonSerializer.Serialize(result.Data, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(categories);
        }
    }
}
