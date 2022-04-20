using AutoMapper;
using BlogProject.DataAccess.Abstract;
using BlogProject.Entities.Concrete;
using BlogProject.Entities.DTOs;
using BlogProject.Entities.DTOs.CategoryDto;
using BlogProject.Services.Abstract;
using BlogProject.Services.Constants;
using BlogProject.Shared.Utilities.Results.Abstract;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using BlogProject.Shared.Utilities.Results.Concrete;
using System;
using System.Threading.Tasks;

namespace BlogProject.Services.Concrete
{
    public class CategoryManager : ICategoryService  
    {
        #region Variables
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #endregion

        #region QueryMethods
        public async Task<IDataResult<CategoryDto>> GetByIdAsync(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId, c => c.Articles);
            if (category != null)
            {
                return new DataResult<CategoryDto>(ResultStatus.Success, new CategoryDto
                {
                    Category = category,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryDto>(ResultStatus.Error, Messages.Category.CategoryNotFound, new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.CategoryNotFound
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(null, c => c.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.CategoryNotFound, new CategoryListDto
            {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.CategoryNotFound
            });
                
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted, c=>c.Articles);
            if(categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.CategoryNotFound, new CategoryListDto
            {
                Categories = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.CategoryNotFound
            });
        }

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var categories = await _unitOfWork.Categories.GetAllAsync(c => !c.IsDeleted && c.IsActive, c => c.Articles);
            if (categories.Count > -1)
            {
                return new DataResult<CategoryListDto>(ResultStatus.Success, new CategoryListDto
                {
                    Categories = categories,
                    ResultStatus = ResultStatus.Success
                });
            }
            return new DataResult<CategoryListDto>(ResultStatus.Error, Messages.Category.CategoryNotFound, null);
        }


        #endregion

        #region CommandMethods
        public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName)
        {
            var category = _mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;
            var addedCategory =await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return new DataResult<CategoryDto>(ResultStatus.Success,Messages.Category.CategoryAdded ,new CategoryDto
            {
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.CategoryAdded,
                Category = addedCategory


            });


        }

        public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var category = _mapper.Map<Category>(categoryUpdateDto);
            category.ModifiedByName = modifiedByName;
            var updatedCategory = await _unitOfWork.Categories.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.CategoryUpdated, new CategoryDto
            {
                Category = updatedCategory,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.CategoryUpdated
            });
        }

        public async Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                category.ModifiedDate = DateTime.Now;
                category.ModifiedByName = modifiedByName;
                var deletedCategory = await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveChangesAsync();

                return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.CategoryDeleted, new CategoryDto
                {
                    Category = deletedCategory,
                    ResultStatus = ResultStatus.Success,
                    Message = Messages.Category.CategoryDeleted
                });

            }
            return new DataResult<CategoryDto>(ResultStatus.Error, Messages.Category.CategoryNotFound, new CategoryDto
            {
                Category = null,
                ResultStatus = ResultStatus.Error,
                Message = Messages.Category.CategoryNotFound
            });
        }

        public async Task<IResult> HardDeleteAsync(int categoryId)
        {
            var category = await _unitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                await _unitOfWork.Categories.DeleteAsync(category);
                await _unitOfWork.SaveChangesAsync();
                return new Result(ResultStatus.Success, Messages.Category.CategoryHardDeleted);
            }
            return new Result(ResultStatus.Error, Messages.Category.CategoryNotFound);
        }
        #endregion







    }
}
