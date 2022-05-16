using AutoMapper;
using BlogProject.DataAccess.Abstract;
using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.CommentDto;
using BlogProject.Services.Abstract;
using BlogProject.Services.Constants;
using BlogProject.Shared.Utilities.Results.Abstract;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using BlogProject.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services.Concrete
{
    public class CommentManager : BaseManager,ICommentService
    {

        

        #region Constructor
        public CommentManager(IUnitOfWork unitOfWork, IMapper mapper):base(unitOfWork,mapper)
        {
            
        }
        #endregion

        #region AddMethod
        public async Task<IDataResult<CommentDto>> AddAsync(CommentAddDto commentAddDto)
        {
            var article = await UnitOfWork.Articles.GetAsync(x => x.Id == commentAddDto.ArticleId);
            if (article is null)
                return new DataResult<CommentDto>(ResultStatus.Error, Messages.Articles.ArticleNotFound, null);


            var comment = Mapper.Map<Comment>(commentAddDto);
            var addedComment = await UnitOfWork.Comments.AddAsync(comment);
            article.CommentCount = await UnitOfWork.Comments.CountAsync(x => x.ArticleId == article.Id && !x.IsDeleted);
            await UnitOfWork.Articles.UpdateAsync(article);
            await UnitOfWork.SaveChangesAsync();
            return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.CommentAdded, new CommentDto
            {
                Comment =  addedComment
            });
        }

        
        #endregion

        #region CountMethod
        public async Task<IDataResult<int>> CountAsync()
        {
            var commentsCount = await UnitOfWork.Comments.CountAsync();
            if (commentsCount > -1)
                return new DataResult<int>(ResultStatus.Success, commentsCount);
            else
                return new DataResult<int>(ResultStatus.Error,"Hata", -1);
        }
        #endregion


        #region CountByNonDeletedMethod
        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var commentsCount = await UnitOfWork.Comments.CountAsync(c => !c.IsDeleted);
            if (commentsCount > -1)
                return new DataResult<int>(ResultStatus.Success, commentsCount);
            else
                return new DataResult<int>(ResultStatus.Error,"Hata" ,-1);
        }
        #endregion


        #region DeleteMethod
        public async Task<IDataResult<CommentDto>> DeleteAsync(int commentId, string modifiedByName)
        {
            var comment = await UnitOfWork.Comments.GetAsync(x => x.Id == commentId, x=> x.Article);
            if (comment != null)
            {
                var article = comment.Article;
                comment.IsDeleted = true;
                comment.IsActive = false;
                comment.ModifiedByName = modifiedByName;
                comment.ModifiedDate = DateTime.Now;
                var deletedComment = await UnitOfWork.Comments.UpdateAsync(comment);
                article.CommentCount = await UnitOfWork.Comments.CountAsync(x => x.ArticleId == article.Id && !x.IsDeleted);
                await UnitOfWork.Articles.UpdateAsync(article);
                await UnitOfWork.SaveChangesAsync();
                return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.CommentDeleted, new CommentDto
                {
                    Comment = deletedComment
                });
            }
            return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.CommentNotFound, new CommentDto{ Comment = null });



        }
        #endregion

        #region GetAllMethod
        public async Task<IDataResult<CommentListDto>> GetAllAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(null,c => c.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.CommentNotFound, new CommentListDto
            {
                Comments = null,
            });
        }
        #endregion


        #region GetAllByDeletedMethod
        public async Task<IDataResult<CommentListDto>> GetAllByDeletedAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(c => c.IsDeleted, c => c.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.CommentNotFound, new CommentListDto
            {
                Comments = null,
            });
        }
        #endregion

        #region GetAllByNonDeletedAndActiveMethod
        public async Task<IDataResult<CommentListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(c => !c.IsDeleted && c.IsActive, c => c.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.CommentNotFound, new CommentListDto
            {
                Comments = null,
            });
        }
        #endregion

        #region GetAllByNonDeletedMethod
        public async Task<IDataResult<CommentListDto>> GetAllByNonDeletedAsync()
        {
            var comments = await UnitOfWork.Comments.GetAllAsync(c => !c.IsDeleted,c=> c.Article);
            if (comments.Count > -1)
            {
                return new DataResult<CommentListDto>(ResultStatus.Success, new CommentListDto
                {
                    Comments = comments,
                });
            }
            return new DataResult<CommentListDto>(ResultStatus.Error, Messages.Comment.CommentNotFound, new CommentListDto
            {
                Comments = null,
            });
        }
        #endregion

        #region GetByIdMethod
        public async Task<IDataResult<CommentDto>> GetAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId);
            if (comment != null)
            {
                return new DataResult<CommentDto>(ResultStatus.Success, new CommentDto
                {
                    Comment = comment,
                });
            }
            return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.CommentNotFound, new CommentDto
            {
                Comment = null,
            });
        }
        #endregion

        #region GetCommentUpdateDtoMethod
        public async Task<IDataResult<CommentUpdateDto>> GetCommentUpdateDtoAsync(int commentId)
        {
            var result = await UnitOfWork.Comments.AnyAsync(c => c.Id == commentId);
            if (result)
            {
                var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId);
                var commentUpdateDto = Mapper.Map<CommentUpdateDto>(comment);
                return new DataResult<CommentUpdateDto>(ResultStatus.Success, commentUpdateDto);
            }
            else
            {
                return new DataResult<CommentUpdateDto>(ResultStatus.Error, Messages.Comment.CommentNotFound, null);
            }
        }
        #endregion

        #region HardDeleteMethod
        public async Task<IResult> HardDeleteAsync(int commentId)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId,x => x.Article);
            if (comment != null)
            {
                var article = comment.Article;
                await UnitOfWork.Comments.DeleteAsync(comment);
                article.CommentCount = await UnitOfWork.Comments.CountAsync(x => x.ArticleId == article.Id && !x.IsDeleted);
                await UnitOfWork.Articles.UpdateAsync(article);
                await UnitOfWork.SaveChangesAsync();
                return new Result(ResultStatus.Success, Messages.Comment.CommentHardDeleted);
            }
            return new Result(ResultStatus.Error, Messages.Comment.CommentNotFound);
        }
        #endregion


        #region UpdateMethod
        public async Task<IDataResult<CommentDto>> UpdateAsync(CommentUpdateDto commentUpdateDto, string modifiedByName)
        {
            var oldComment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentUpdateDto.Id);
            var comment = Mapper.Map<CommentUpdateDto, Comment>(commentUpdateDto, oldComment);
            comment.ModifiedByName = modifiedByName;
            var updatedComment = await UnitOfWork.Comments.UpdateAsync(comment);
            updatedComment.Article = await UnitOfWork.Articles.GetAsync(c => c.Id == commentUpdateDto.ArticleId);
            await UnitOfWork.SaveChangesAsync();
            return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.CommentUpdated, new CommentDto
            {
                Comment = updatedComment,
            });
        }
        #endregion

        #region ApproveMethod
        public async Task<IDataResult<CommentDto>> ApproveAsync(int commentId, string modifiedByName)
        {
            var comment = await UnitOfWork.Comments.GetAsync(c => c.Id == commentId, c => c.Article);
            if (comment is not null)
            {
                comment.IsActive = true;
                comment.ModifiedByName = modifiedByName;
                comment.ModifiedDate = DateTime.Now;
                var updatedComment = await UnitOfWork.Comments.UpdateAsync(comment);
                await UnitOfWork.SaveChangesAsync();
                return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.CommentApproved, new CommentDto
                {
                    Comment = updatedComment,
                });
            }
            return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.CommentNotFound, null);
        }


        #endregion

        #region UndoDeleteMethod
        public async Task<IDataResult<CommentDto>> UndoDeleteAsync(int commentId, string modifiedByName)
        {
            var comment = await UnitOfWork.Comments.GetAsync(x => x.Id == commentId, c => c.Article);
            if (comment != null)
            {
                var article = comment.Article;
                comment.IsDeleted = false;
                comment.IsActive = true;
                comment.ModifiedByName = modifiedByName;
                comment.ModifiedDate = DateTime.Now;
                var deletedComment = await UnitOfWork.Comments.UpdateAsync(comment);
                article.CommentCount = await UnitOfWork.Comments.CountAsync(x => x.ArticleId == article.Id && !x.IsDeleted);
                await UnitOfWork.Articles.UpdateAsync(article);
                await UnitOfWork.SaveChangesAsync();
                return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.CommentUndoDeleted, new CommentDto
                {
                    Comment = deletedComment
                });
            }
            return new DataResult<CommentDto>(ResultStatus.Error, Messages.Comment.CommentNotFound, new CommentDto { Comment = null });
        }        
        #endregion


    }
}
