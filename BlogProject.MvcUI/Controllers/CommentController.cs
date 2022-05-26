using BlogProject.Entities.DTOs.CommentDto;
using BlogProject.MvcUI.Models;
using BlogProject.Services.Abstract;
using BlogProject.Shared.Utilities.Extensions;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }


        #region AddMethod
        [HttpPost]
        public async Task<JsonResult> Add(CommentAddDto commentAddDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentService.AddAsync(commentAddDto);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var commentAddAjaxViewModel = JsonSerializer.Serialize(new CommentAddAjaxViewModel
                    {
                        CommentDto = result.Data,
                        CommentAddPartial = await this.RenderViewToStringAsync("_CommentAddPartial", commentAddDto)
                    },
                    new JsonSerializerOptions
                    { 
                        ReferenceHandler = ReferenceHandler.Preserve 
                    });
                    return Json(commentAddAjaxViewModel);
                }
                ModelState.AddModelError("", result.Message);
            }
            
            
            var commentAddAjaxErrorModel = JsonSerializer.Serialize(new CommentAddAjaxViewModel
            {
                CommentAddDto = commentAddDto,
                CommentAddPartial = await this.RenderViewToStringAsync("_CommentAddPartial", commentAddDto)
            });
            return Json(commentAddAjaxErrorModel);

        }
        #endregion
    }
}
