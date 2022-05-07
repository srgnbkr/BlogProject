using BlogProject.DataAccess.Abstract;
using BlogProject.Services.Abstract;
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

        private readonly IUnitOfWork _unitOfWork;

        public CommentManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IDataResult<int>> Count()
        {
            var commentsCount = await _unitOfWork.Comments.CountAsync();
            if (commentsCount > -1)
                return new DataResult<int>(ResultStatus.Success, commentsCount);
            else
                return new DataResult<int>(ResultStatus.Error,"Hata", -1);
        }

        public async Task<IDataResult<int>> CountByNonDeleted()
        {
            var commentsCount = await _unitOfWork.Comments.CountAsync(c => !c.IsDeleted);
            if (commentsCount > -1)
                return new DataResult<int>(ResultStatus.Success, commentsCount);
            else
                return new DataResult<int>(ResultStatus.Error,"Hata" ,-1);
        }
    }
}
