﻿using BlogProject.Entities.DTOs.ArticleDto;
using BlogProject.Shared.Utilities.Results.Abstract;
using System.Threading.Tasks;

namespace BlogProject.Services.Abstract
{
    public interface IArticleService
    {

        #region QueryMethods
        Task<IDataResult<ArticleDto>> GetAsync(int articleId);
        Task<IDataResult<ArticleListDto>> GetAllAsync();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync();
        Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync();
        Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId);
        Task<IDataResult<int>> Count();
        Task<IDataResult<int>> CountByIsDeleted();
        #endregion

        #region CommandMethods
        Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName);
        Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName);
        Task<IResult> DeleteAsync(int articleId, string modifiedByName);
        Task<IResult> HardDeleteAsync(int articleId);
        #endregion


    }
}
