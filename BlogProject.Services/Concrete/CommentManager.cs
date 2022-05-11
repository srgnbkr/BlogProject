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
    public class CommentManager : ICommentService
    {

        #region Variables
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public CommentManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region AddMethod
        public async Task<IDataResult<CommentDto>> AddAsync(CommentAddDto commentAddDto)
        {
            var comment = _mapper.Map<Comment>(commentAddDto);
            var addedComment = await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.CommentAdded, new CommentDto
            {
                Comment =  addedComment
            });
        }
        #endregion

        #region CountMethod
        public async Task<IDataResult<int>> CountAsync()
        {
            var commentsCount = await _unitOfWork.Comments.CountAsync();
            if (commentsCount > -1)
                return new DataResult<int>(ResultStatus.Success, commentsCount);
            else
                return new DataResult<int>(ResultStatus.Error,"Hata", -1);
        }
        #endregion


        #region CountByNonDeletedMethod
        public async Task<IDataResult<int>> CountByNonDeletedAsync()
        {
            var commentsCount = await _unitOfWork.Comments.CountAsync(c => !c.IsDeleted);
            if (commentsCount > -1)
                return new DataResult<int>(ResultStatus.Success, commentsCount);
            else
                return new DataResult<int>(ResultStatus.Error,"Hata" ,-1);
        }
        #endregion


        #region DeleteMethod
        public async Task<IDataResult<CommentDto>> DeleteAsync(int commentId, string modifiedByName)
        {
            var comment = await _unitOfWork.Comments.GetAsync(x => x.Id == commentId);
            if (comment != null)
            {
                comment.IsDeleted = true;
                comment.ModifiedByName = modifiedByName;
                comment.ModifiedDate = DateTime.Now;
                var deletedComment = await _unitOfWork.Comments.UpdateAsync(comment);
                await _unitOfWork.SaveChangesAsync();
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
            var comments = await _unitOfWork.Comments.GetAllAsync();
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
            var comments = await _unitOfWork.Comments.GetAllAsync(c => c.IsDeleted);
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
            var comments = await _unitOfWork.Comments.GetAllAsync(c => !c.IsDeleted && c.IsActive);
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
            var comments = await _unitOfWork.Comments.GetAllAsync(c => !c.IsDeleted);
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
            var comment = await _unitOfWork.Comments.GetAsync(c => c.Id == commentId);
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
            var result = await _unitOfWork.Comments.AnyAsync(c => c.Id == commentId);
            if (result)
            {
                var comment = await _unitOfWork.Comments.GetAsync(c => c.Id == commentId);
                var commentUpdateDto = _mapper.Map<CommentUpdateDto>(comment);
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
            var comment = await _unitOfWork.Comments.GetAsync(c => c.Id == commentId);
            if (comment != null)
            {
                await _unitOfWork.Comments.DeleteAsync(comment);
                await _unitOfWork.SaveChangesAsync();
                return new Result(ResultStatus.Success, Messages.Comment.CommentHardDeleted);
            }
            return new Result(ResultStatus.Error, Messages.Comment.CommentNotFound);
        }
        #endregion


        #region UpdateMethod
        public async Task<IDataResult<CommentDto>> UpdateAsync(CommentUpdateDto commentUpdateDto, string modifiedByName)
        {
            var oldComment = await _unitOfWork.Comments.GetAsync(c => c.Id == commentUpdateDto.Id);
            var comment = _mapper.Map<CommentUpdateDto, Comment>(commentUpdateDto, oldComment);
            comment.ModifiedByName = modifiedByName;
            var updatedComment = await _unitOfWork.Comments.UpdateAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            return new DataResult<CommentDto>(ResultStatus.Success, Messages.Comment.CommentUpdated, new CommentDto
            {
                Comment = updatedComment,
            });
        }
        #endregion
    }
}
