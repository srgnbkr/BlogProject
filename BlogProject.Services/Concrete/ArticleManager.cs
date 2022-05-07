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
    public class ArticleManager : IArticleService
    {
        #region Variables
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public ArticleManager(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        #endregion

        #region Methods
        public async Task<IResult> AddAsync(ArticleAddDto articleAddDto, string createdByName)
        {
            var article = _mapper.Map<Article>(articleAddDto);
            article.CreatedByName = createdByName;
            article.ModifiedByName = createdByName;
            article.UserId = 1;
            await _unitOfWork.Articles.AddAsync(article);
            await _unitOfWork.SaveChangesAsync();
            return new Result(ResultStatus.Success, Messages.Articles.ArticleAdded);

        }

        public async Task<IDataResult<int>> Count()
        {
            var articlesCount = await _unitOfWork.Articles.CountAsync();
            if(articlesCount > -1)
                return new DataResult<int>(ResultStatus.Success, Messages.Articles.ArticleNotFound, articlesCount);
            else
                return new DataResult<int>(ResultStatus.Error, Messages.Articles.ArticleNotFound,-1);

        }

        public async Task<IDataResult<int>> CountByIsDeleted()
        {
            var articlesCount = await _unitOfWork.Articles.CountAsync(c => !c.IsDeleted);
            if (articlesCount > -1)
                return new DataResult<int>(ResultStatus.Success, articlesCount);
            else
                return new DataResult<int>(ResultStatus.Error, Messages.Articles.ArticleNotFound, -1);
        }

        public async Task<IResult> DeleteAsync(int articleId, string modifiedByName)
        {
            var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
                article.IsDeleted = true;
                article.ModifiedByName = modifiedByName;
                article.ModifiedDate = DateTime.Now;
                await _unitOfWork.Articles.UpdateAsync(article);
                await _unitOfWork.SaveChangesAsync();
                return new Result(ResultStatus.Success, Messages.Articles.ArticleDeleted);
            }
            return new Result(ResultStatus.Error, Messages.Articles.ArticleNotFound);
        }

        public async Task<IDataResult<ArticleListDto>> GetAllAsync()
        {
            var articles = await _unitOfWork.Articles
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
            var result = await _unitOfWork.Categories.AnyAsync(x => x.Id == categoryId);
            if (result)
            {
                var articles = await _unitOfWork.Articles
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
            var articles = await _unitOfWork.Articles.GetAllAsync(x => !x.IsDeleted && x.IsActive, a => a.User, a => a.Category);

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
            var articles = await _unitOfWork.Articles.GetAllAsync(x => !x.IsDeleted, a => a.User, a => a.Category);

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

        public async Task<IDataResult<ArticleDto>> GetAsync(int articleId)
        {
            var data = await _unitOfWork.Articles.GetAsync(x => x.Id == articleId, x => x.User, x => x.Category);
            if (data is not null)
                return new DataResult<ArticleDto>
                    (ResultStatus.Success, new ArticleDto {Article=data,ResultStatus = ResultStatus.Success });
            return new DataResult<ArticleDto>(ResultStatus.Error,Messages.Articles.ArticleNotFound, null);
        } 

        public async Task<IResult> HardDeleteAsync(int articleId)
        {
            var result = await _unitOfWork.Articles.AnyAsync(a => a.Id == articleId);
            if (result)
            {
                var article = await _unitOfWork.Articles.GetAsync(a => a.Id == articleId);
                await _unitOfWork.Articles.DeleteAsync(article);
                await _unitOfWork.SaveChangesAsync();
                return new Result(ResultStatus.Success, Messages.Articles.ArticleHardDeleted);
            }
            return new Result(ResultStatus.Error, Messages.Articles.ArticleNotFound);
        }

        public async Task<IResult> UpdateAsync(ArticleUpdateDto articleUpdateDto, string modifiedByName)
        {
            var article = _mapper.Map<Article>(articleUpdateDto);
            article.ModifiedByName = modifiedByName;
            await _unitOfWork.Articles.UpdateAsync(article);
            await _unitOfWork.SaveChangesAsync();
            return new Result(ResultStatus.Success, Messages.Articles.ArticleUpdated);
        }

        #endregion
    }
}
