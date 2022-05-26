using AutoMapper;
using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.CommentDto;
using BlogProject.MvcUI.Areas.Admin.Models.CommentModels;
using BlogProject.MvcUI.Helpers.Abstract;
using BlogProject.Services.Abstract;
using BlogProject.Shared.Utilities.Extensions;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogProject.MvcUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin")]
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;
        public CommentController(UserManager<User> userManager, IMapper mapper, IImageHelper imageHelper, ICommentService commentService) : base(userManager, mapper, imageHelper)
        {
            _commentService = commentService;
        }

        #region Index
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var result = await _commentService.GetAllByNonDeletedAsync();
            return View(result.Data);
        }
        #endregion


        #region GetDetail
        [HttpGet]
        public async Task<IActionResult> GetDetail(int commentId)
        {
            var result = await _commentService.GetAsync(commentId);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_CommentDetailPartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        #endregion


        #region GetAllComments

        [HttpGet]
        public async Task<IActionResult> GetAllComments()
        {
            var result = await _commentService.GetAllByNonDeletedAsync();
            var commentResult = JsonSerializer.Serialize(result, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve, });
            return Json(commentResult);
        }


        #endregion


        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int commentId)
        {
            var result = await _commentService.GetCommentUpdateDtoAsync(commentId);
            if (result.ResultStatus == ResultStatus.Success)
            {
                return PartialView("_CommentUpdatePartial", result.Data);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Update(CommentUpdateDto commentUpdateDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _commentService.UpdateAsync(commentUpdateDto, LoggedInUser.UserName);
                if (result.ResultStatus == ResultStatus.Success)
                {
                    var commentUpdateAjaxModel = JsonSerializer.Serialize(new CommentUpdateAjaxViewModel
                    {
                        CommentDto = result.Data,
                        CommentUpdatePartial = await this.RenderViewToStringAsync("_CommentUpdatePartial", commentUpdateDto)
                    }, new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    });
                    return Json(commentUpdateAjaxModel);
                }
            }
            var commentUpdateAjaxErrorModel = JsonSerializer.Serialize(new CommentUpdateAjaxViewModel
            {
                CommentUpdatePartial = await this.RenderViewToStringAsync("_CommentUpdatePartial", commentUpdateDto)
            });
            return Json(commentUpdateAjaxErrorModel);
        }
        #endregion

        #region Approve
        [HttpPost]
        public async Task<IActionResult> Approve(int commentId)
        {
            var result = await _commentService.ApproveAsync(commentId, LoggedInUser.UserName);
            var commentResult = JsonSerializer.Serialize(result, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve });
            return Json(commentResult);
        }
        #endregion

        #region DeletedComments

        [HttpGet]
        public async Task<IActionResult> DeletedComments()
        {
            var result = await _commentService.GetAllByDeletedAsync();
            return View(result.Data);

        }
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public async Task<JsonResult> GetAllDeletedComments()
        {
            var result = await _commentService.GetAllByDeletedAsync();
            var comments = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(comments);
        }
        #endregion

        #region UndoDelete
        
        [HttpPost]
        public async Task<JsonResult> UndoDelete(int commentId)
        {
            var result = await _commentService.UndoDeleteAsync(commentId, LoggedInUser.UserName);
            var undoDeleteCommentResult = JsonSerializer.Serialize(result, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(undoDeleteCommentResult);
        }
        #endregion

        #region HardDelete
        [HttpPost]
        public async Task<JsonResult> HardDelete(int commentId)
        {
            var result = await _commentService.HardDeleteAsync(commentId);
            var hardDeletedCommentResult = JsonSerializer.Serialize(result,new JsonSerializerOptions { ReferenceHandler=ReferenceHandler.Preserve});
            return Json(hardDeletedCommentResult);
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<JsonResult> Delete(int commentId)
        {
            var result = await _commentService.DeleteAsync(commentId, LoggedInUser.UserName);
            var commentResult = JsonSerializer.Serialize(result,new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve});
            return Json(commentResult);
        }
        #endregion
    }
}