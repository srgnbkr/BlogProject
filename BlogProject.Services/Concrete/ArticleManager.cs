using AutoMapper;
using BlogProject.DataAccess.Abstract;
using BlogProject.Entities.ComplexTypes;
using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.ArticleDto;
using BlogProject.Services.Abstract;
using BlogProject.Services.Constants;
using BlogProject.Shared.Utilities.Results.Abstract;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using BlogProject.Shared.Utilities.Results.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlogProject.Services.Concrete
{
    public class ArticleManager : BaseManager,IArticleService
    {


        #region Constructor
        private readonly UserManager<User> _userManager;
        public ArticleManager(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
        }

        #endregion

        #region Methods
        public async Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName,int userId)
        {
            var article = Mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = userId;
            await UnitOfWork.Articles.AddAsync(article);
            await UnitOfWork.SaveChangesAsync();
            return new Result(ResultStatus.Success, Messages.Articles.ArticleAdded);

        }

        public async Task<IDataResult<int>> Count()
        {
            var articlesCount = await UnitOfWork.Articles.CountAsync();
            if(articlesCount > -1)
                return new DataResult<int>(ResultStatus.Success, Messages.Articles.ArticleNotFound, articlesCount);
            else
                return new DataResult<int>(ResultStatus.Error, Messages.Articles.ArticleNotFound,-1);

        }

        public async Task<IDataResult<int>> CountByNonDeleted()
        {
            var articlesCount = await UnitOfWork.Articles.CountAsync(c => !c.IsDeleted);
            if (articlesCount > -1)
                return new DataResult<int>(ResultStatus.Success, articlesCount);
            else
                return new DataResult<int>(ResultStatus.Error, Messages.Articles.ArticleNotFound, -1);
        }

        public async Task<IResult> DeleteAsync(int articleId, string modifiedByName)
        {
            var result = await UnitOfWork.Articles.AnyAsync(a => a.Id == articleId);
            if (result)
            {
                var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId);
                article.IsDeleted = true;
                article.IsActive = false;
                article.ModifiedByName = modifiedByName;
                article.ModifiedDate = DateTime.Now;
                await UnitOfWork.Articles.UpdateAsync(article);
                await UnitOfWork.SaveChangesAsync();
                return new Result(ResultStatus.Success, Messages.Articles.ArticleDeleted);
            }
            return new Result(ResultStatus.Error, Messages.Articles.ArticleNotFound);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllAsync()
        {
            var articles = await UnitOfWork.Articles
                .GetAllAsync(null,x=>x.User,x=>x.Category);
            if (articles.Count>-1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Articles.ArticleNotFound, null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByCategoryAsync(int categoryId)
        {
            var result = await UnitOfWork.Categories.AnyAsync(x => x.Id == categoryId);
            if (result)
            {
                var articles = await UnitOfWork.Articles
                    .GetAllAsync(a => a.CategoryId == categoryId && !a.IsDeleted && a.IsActive, ar => ar.User, ar => ar.Category);
                if (articles.Count > -1)
                {
                    return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                    {
                        Articles = articles,
                        ResultStatus = ResultStatus.Success
                    });
                       
                }
                return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Articles.ArticleNotFound, null);
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Category.CategoryNotFound, null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByDeletedAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(x => x.IsDeleted, a => a.User, a => a.Category);

            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });

            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Articles.ArticleNotFound, null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(x => !x.IsDeleted && x.IsActive, a => a.User, a => a.Category);

            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });

            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Articles.ArticleNotFound, null);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByNonDeletedAsync()
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(x => !x.IsDeleted, a => a.User, a => a.Category);

            if (articles.Count > -1)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Success,new ArticleListDto
                {
                    Articles = articles,
                    ResultStatus = ResultStatus.Success
                });
            
            }
            return new DataResult<ArticleListDto>(ResultStatus.Error, Messages.Articles.ArticleNotFound, null);

        }

        public async Task<IDataResult<ArticleListDto>> GetAllByPagingAsync(int? categoryId, int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            var articles = categoryId == null
                ? await UnitOfWork.Articles.GetAllAsync(x => x.IsActive && !x.IsDeleted, x => x.User, x=>x.Category)
                : await UnitOfWork.Articles.GetAllAsync(x => x.CategoryId == categoryId && x.IsActive && !x.IsDeleted, x => x.User, x => x.Category);

            var sortedArticles = isAscending
                ? articles.OrderBy(x => x.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : articles.OrderByDescending(x => x.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();

            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = sortedArticles,
                CategoryId = categoryId == null ? null : categoryId.Value,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = articles.Count,
                IsAscending = isAscending,
            });
        }

        /// <summary>
        /// Get article view count 
        /// </summary>
        /// <param name="isAscending"></param>
        /// <param name="takeSize"></param>
        /// <returns></returns>
        public async Task<IDataResult<ArticleListDto>> GetAllByViewCountAsync(bool isAscending, int? takeSize)
        {
            var articles = await UnitOfWork.Articles.GetAllAsync(x => !x.IsDeleted && x.IsActive, a => a.User, a => a.Category);
            
            var sortedArticles = isAscending 
                ? articles.OrderBy(x => x.ViewCount) 
                : articles.OrderByDescending(x => x.ViewCount);

            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = takeSize == null ? sortedArticles.ToList() : sortedArticles.Take(takeSize.Value).ToList()
                
            });

        }

        public async Task<IDataResult<ArticleUpdateDto>> GetArticleUpdateDtoAsync(int articleId)
        {
            var result = await UnitOfWork.Articles.AnyAsync(x => x.Id == articleId);
            if(result)
            {
                var article = await UnitOfWork.Articles.GetAsync(x => x.Id == articleId);
                var articleUpdateDto = Mapper.Map<ArticleUpdateDto>(article);
                return new DataResult<ArticleUpdateDto>(ResultStatus.Success, articleUpdateDto);
            }
            else
            {
                return new DataResult<ArticleUpdateDto>(ResultStatus.Error, Messages.Articles.ArticleNotFound, null);
            }
                

        }

        public async Task<IDataResult<ArticleDto>> GetAsync(int articleId)
        {
            var article = await UnitOfWork.Articles.GetAsync(x => x.Id == articleId, x => x.User, x => x.Category);
            
            if (article != null)
            {
                article.Comments = await UnitOfWork.Comments.GetAllAsync(x => x.ArticleId == articleId && !x.IsDeleted && x.IsActive);
                return new DataResult<ArticleDto>
                        (ResultStatus.Success, new ArticleDto { Article = article, ResultStatus = ResultStatus.Success });
            }
                
            
            return new DataResult<ArticleDto>(ResultStatus.Error,Messages.Articles.ArticleNotFound, null);
        } 

        public async Task<IResult> HardDeleteAsync(int articleId)
        {
            var result = await UnitOfWork.Articles.AnyAsync(a => a.Id == articleId);
            if (result)
            {
                var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId);
                await UnitOfWork.Articles.DeleteAsync(article);
                await UnitOfWork.SaveChangesAsync();
                return new Result(ResultStatus.Success, Messages.Articles.ArticleHardDeleted);
            }
            return new Result(ResultStatus.Error, Messages.Articles.ArticleNotFound);
        }

        public async Task<IResult> IncreaseViewCountAsync(int articleId)
        {
            var article = await UnitOfWork.Articles.GetAsync(x => x.Id == articleId);
            if (article is null)
                return new Result(ResultStatus.Error, Messages.Articles.ArticleNotFound);
            article.ViewCount++;
            await UnitOfWork.Articles.UpdateAsync(article);
            await UnitOfWork.SaveChangesAsync();
            return new Result(ResultStatus.Success);
        }

        /// <summary>
        /// Search Article
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="categoryId"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="isAscending"></param>
        /// <returns></returns>
        public async Task<IDataResult<ArticleListDto>> SearchAsync(string keyword,  int currentPage = 1, int pageSize = 5, bool isAscending = false)
        {
            pageSize = pageSize > 20 ? 20 : pageSize;
            if (string.IsNullOrWhiteSpace(keyword))
            {
                var articles =
                    await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted, a => a.Category,
                        a => a.User);
                var sortedArticles = isAscending
                    ? articles.OrderBy(a => a.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                    : articles.OrderByDescending(a => a.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
                {
                    Articles = sortedArticles,
                    CurrentPage = currentPage,
                    PageSize = pageSize,
                    TotalCount = articles.Count,
                    IsAscending = isAscending
                });
            }

            var searchedArticles = await UnitOfWork.Articles.SearchAsync(new List<Expression<Func<Article, bool>>>
            {
                (a) => a.Title.Contains(keyword),
                (a) => a.Category.Name.Contains(keyword),
                (a) => a.SeoDescription.Contains(keyword),
                (a) => a.SeoTags.Contains(keyword)
            },
            a => a.Category, a => a.User);
            var searchedAndSortedArticles = isAscending
                ? searchedArticles.OrderBy(a => a.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList()
                : searchedArticles.OrderByDescending(a => a.Date).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = searchedAndSortedArticles,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalCount = searchedArticles.Count,
                IsAscending = isAscending
            });
        }

        public async Task<IResult> UndoDeleteAsync(int articleId, string modifiedByName)
        {
            var result = await UnitOfWork.Articles.AnyAsync(a => a.Id == articleId);
            if (result)
            {
                var article = await UnitOfWork.Articles.GetAsync(a => a.Id == articleId);
                article.IsDeleted = false;
                article.IsActive = true;
                article.ModifiedByName = modifiedByName;
                article.ModifiedDate = DateTime.Now;
                await UnitOfWork.Articles.UpdateAsync(article);
                await UnitOfWork.SaveChangesAsync();
                return new Result(ResultStatus.Success, Messages.Articles.ArticleDeleted);
            }
            return new Result(ResultStatus.Error, Messages.Articles.ArticleNotFound);
        }

        public async Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var oldArticle = await UnitOfWork.Articles.GetAsync(a => a.Id == articleUpdateDto.Id);
            var article = Mapper.Map<ArticleUpdateDto,Article>(articleUpdateDto,oldArticle);
            article.ModifiedByName = modifiedByName;
            await UnitOfWork.Articles.UpdateAsync(article);
            await UnitOfWork.SaveChangesAsync();
            return new Result(ResultStatus.Success, Messages.Articles.ArticleUpdated);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllByUserIdOnFilter(int userId, FilterBy filterBy, OrderBy orderBy, bool isAscending, int takeSize,
            int categoryId, DateTime startAt, DateTime endAt, int minViewCount, int maxViewCount, int minCommentCount,
            int maxCommentCount)
        {
            var anyUser = await _userManager.Users.AnyAsync(u => u.Id == userId);
            if (!anyUser)
            {
                return new DataResult<ArticleListDto>(ResultStatus.Error, $"{userId} numaralı kullanıcı bulunamadı.",
                    null);
            }

            var userArticles =
                await UnitOfWork.Articles.GetAllAsync(a => a.IsActive && !a.IsDeleted && a.UserId == userId);
            List<Article> sortedArticles = new List<Article>();
            switch (filterBy)
            {
                case FilterBy.Category:
                    switch (orderBy)
                    {
                        case OrderBy.Date:
                            sortedArticles = isAscending
                                ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize)
                                    .OrderBy(a => a.Date).ToList()
                                : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize)
                                    .OrderByDescending(a => a.Date).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sortedArticles = isAscending
                                ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize)
                                    .OrderBy(a => a.ViewCount).ToList()
                                : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize)
                                    .OrderByDescending(a => a.ViewCount).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sortedArticles = isAscending
                                ? userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize)
                                    .OrderBy(a => a.CommentCount).ToList()
                                : userArticles.Where(a => a.CategoryId == categoryId).Take(takeSize)
                                    .OrderByDescending(a => a.CommentCount).ToList();
                            break;
                    }
                    break;
                case FilterBy.Date:
                    switch (orderBy)
                    {
                        case OrderBy.Date:
                            sortedArticles = isAscending
                                ? userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize)
                                    .OrderBy(a => a.Date).ToList()
                                : userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize)
                                    .OrderByDescending(a => a.Date).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sortedArticles = isAscending
                                ? userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize)
                                    .OrderBy(a => a.ViewCount).ToList()
                                : userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize)
                                    .OrderByDescending(a => a.ViewCount).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sortedArticles = isAscending
                                ? userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize)
                                    .OrderBy(a => a.CommentCount).ToList()
                                : userArticles.Where(a => a.Date >= startAt && a.Date <= endAt).Take(takeSize)
                                    .OrderByDescending(a => a.CommentCount).ToList();
                            break;
                    }
                    break;
                case FilterBy.ViewCount:
                    switch (orderBy)
                    {
                        case OrderBy.Date:
                            sortedArticles = isAscending
                                ? userArticles.Where(a => a.ViewCount >= minViewCount && a.ViewCount <= maxViewCount).Take(takeSize)
                                    .OrderBy(a => a.Date).ToList()
                                : userArticles.Where(a => a.ViewCount >= minViewCount && a.ViewCount <= maxViewCount).Take(takeSize)
                                    .OrderByDescending(a => a.Date).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sortedArticles = isAscending
                                ? userArticles.Where(a => a.ViewCount >= minViewCount && a.ViewCount <= maxViewCount).Take(takeSize)
                                    .OrderBy(a => a.ViewCount).ToList()
                                : userArticles.Where(a => a.ViewCount >= minViewCount && a.ViewCount <= maxViewCount).Take(takeSize)
                                    .OrderByDescending(a => a.ViewCount).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sortedArticles = isAscending
                                ? userArticles.Where(a => a.ViewCount >= minViewCount && a.ViewCount <= maxViewCount).Take(takeSize)
                                    .OrderBy(a => a.CommentCount).ToList()
                                : userArticles.Where(a => a.ViewCount >= minViewCount && a.ViewCount <= maxViewCount).Take(takeSize)
                                    .OrderByDescending(a => a.CommentCount).ToList();
                            break;
                    }
                    break;
                case FilterBy.CommentCount:
                    switch (orderBy)
                    {
                        case OrderBy.Date:
                            sortedArticles = isAscending
                                ? userArticles.Where(a =>
                                        a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount)
                                    .Take(takeSize)
                                    .OrderBy(a => a.Date).ToList()
                                : userArticles.Where(a =>
                                        a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount)
                                    .Take(takeSize)
                                    .OrderByDescending(a => a.Date).ToList();
                            break;
                        case OrderBy.ViewCount:
                            sortedArticles = isAscending
                                ? userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount)
                                    .Take(takeSize)
                                    .OrderBy(a => a.ViewCount).ToList()
                                : userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount)
                                    .Take(takeSize)
                                    .OrderByDescending(a => a.ViewCount).ToList();
                            break;
                        case OrderBy.CommentCount:
                            sortedArticles = isAscending
                                ? userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount)
                                    .Take(takeSize)
                                    .OrderBy(a => a.CommentCount).ToList()
                                : userArticles.Where(a => a.CommentCount >= minCommentCount && a.CommentCount <= maxCommentCount)
                                    .Take(takeSize)
                                    .OrderByDescending(a => a.CommentCount).ToList();
                            break;
                    }

                    break;
            }

            return new DataResult<ArticleListDto>(ResultStatus.Success, new ArticleListDto
            {
                Articles = sortedArticles
            });
        }

        #endregion
    }
}
