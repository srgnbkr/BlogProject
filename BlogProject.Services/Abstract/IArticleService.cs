using BlogProject.Entities.ComplexTypes;
using BlogProject.Entities.DTOs.ArticleDto;
using BlogProject.Shared.Utilities.Results.Abstract;
using System;
using System.Threading.Tasks;

namespace BlogProject.Services.Abstract
{
    public interface IArticleService
    {

        #region QueryMethods
        Task<IDataResult<ArticleDto>> GetAsync(int articleId);
        Task<IDataResult<ArticleUpdateDto>> GetArticleUpdateDtoAsync(int articleId);
        Task<IDataResult<ArticleListDto>> GetAllAsync();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync();
        Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId);
        Task<IDataResult<ArticleListDto>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false);
        
        Task<IDataResult<ArticleListDto>> SearchAsync(string keyword,  int currentPage = 1, int pageSize = 5, bool isAscending = false);

        Task<IResult> IncreaseViewCountAsync(int articleId);

        Task<IDataResult<int>> Count();
        Task<IDataResult<int>> CountByNonDeleted();
        Task<IDataResult<ArticleListDto>> GetAllByDeletedAsync();
        Task<IDataResult<ArticleListDto>> GetAllByViewCountAsync(bool isAscending, int? takeSize);
        Task<IDataResult<ArticleListDto>> GetAllByUserIdOnFilter(int userId, FilterBy filterBy, OrderBy orderBy,
            bool isAscending, int takeSize, int categoryId, DateTime startAt, DateTime endAt, int minViewCount,
            int maxViewCount, int minCommentCount, int maxCommentCount);

        #endregion

        #region CommandMethods
        Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName,int userId);
        Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName);
        Task<IResult> DeleteAsync(int articleId, string modifiedByName);
        Task<IResult> UndoDeleteAsync(int articleId, string modifiedByName);
        Task<IResult> HardDeleteAsync(int articleId);
        #endregion


    }
}
