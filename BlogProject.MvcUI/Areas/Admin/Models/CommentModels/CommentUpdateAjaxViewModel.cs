using BlogProject.Entities.DTOs.CommentDto;

namespace BlogProject.MvcUI.Areas.Admin.Models.CommentModels
{
    public class CommentUpdateAjaxViewModel
    {
        public CommentUpdateDto CommentUpdateDto { get; set; }
        public string CommentUpdatePartial { get; set; }
        public CommentDto CommentDto { get; set; }
    }
}
