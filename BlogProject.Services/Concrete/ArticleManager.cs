using AutoMapper;
using BlogProject.DataAccess.Abstract;
using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs.ArticleDto;
using BlogProject.Services.Abstract;
using BlogProject.Services.Constants;
using BlogProject.Shared.Utilities.Results.Abstract;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using BlogProject.Shared.Utilities.Results.Concrete;
using System;
using System.Threading.Tasks;

namespace BlogProject.Services.Concrete
{
    public class ArticleManager : BaseManager,IArticleService
    {
       

        #region Constructor
        public ArticleManager(IUnitOfWork unitOfWork, IMapper mapper):base(unitOfWork,mapper)
        {
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
            var data = await UnitOfWork.Articles.GetAsync(x => x.Id == articleId, x => x.User, x => x.Category);
            if (data is not null)
                return new DataResult<ArticleDto>
                    (ResultStatus.Success, new ArticleDto {Article=data,ResultStatus = ResultStatus.Success });
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

        public async Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var oldArticle = await UnitOfWork.Articles.GetAsync(a => a.Id == articleUpdateDto.Id);
            var article = Mapper.Map<ArticleUpdateDto,Article>(articleUpdateDto,oldArticle);
            article.ModifiedByName = modifiedByName;
            await UnitOfWork.Articles.UpdateAsync(article);
            await UnitOfWork.SaveChangesAsync();
            return new Result(ResultStatus.Success, Messages.Articles.ArticleUpdated);
        }

        #endregion
    }
}
