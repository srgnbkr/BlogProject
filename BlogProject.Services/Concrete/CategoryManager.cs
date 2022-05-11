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
    public class CategoryManager : BaseManager,ICategoryService
    {
        

        #region Constructor
        public CategoryManager(IUnitOfWork unitOfWork, IMapper mapper):base(unitOfWork,mapper)
        {
            
        }
        #endregion

        #region QueryMethods
        public async Task<IDataResult<CategoryDto>> GetByIdAsync(int categoryId)
        {
            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
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
            var categories = await UnitOfWork.Categories.GetAllAsync(null);
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
            var categories = await UnitOfWork.Categories.GetAllAsync(c => !c.IsDeleted);
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

        public async Task<IDataResult<CategoryListDto>> GetAllByNonDeletedAndActiveAsync()
        {
            var categories = await UnitOfWork.Categories.GetAllAsync(c => !c.IsDeleted && c.IsActive);
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

        public async Task<IDataResult<CategoryUpdateDto>> GetCategoryUpdateDto(int categoryId)
        {
            var result = await UnitOfWork.Categories.AnyAsync(c => c.Id == categoryId);
            if (result)
            {
                var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
                var categoryUpdateDto = Mapper.Map<CategoryUpdateDto>(category);
                return new DataResult<CategoryUpdateDto>(ResultStatus.Success, categoryUpdateDto);
            }
            else
            {
                return new DataResult<CategoryUpdateDto>(ResultStatus.Error, Messages.Category.CategoryNotFound, null);
            }
        }


        public async Task<IDataResult<int>> Count()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync();
            if (categoriesCount > -1)
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            else
                return new DataResult<int>(ResultStatus.Error, Messages.Category.CategoryNotFound, -1);
        }

        #endregion

        #region CommandMethods
        public async Task<IDataResult<CategoryDto>> AddAsync(CategoryAddDto categoryAddDto, string createdByName)
        {
            var category = Mapper.Map<Category>(categoryAddDto);
            category.CreatedByName = createdByName;
            category.ModifiedByName = createdByName;
            var addedCategory = await UnitOfWork.Categories.AddAsync(category);
            await UnitOfWork.SaveChangesAsync();

            return new DataResult<CategoryDto>(ResultStatus.Success, Messages.Category.CategoryAdded, new CategoryDto
            {
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.CategoryAdded,
                Category = addedCategory


            });


        }

        public async Task<IDataResult<CategoryDto>> UpdateAsync(CategoryUpdateDto categoryUpdateDto, string modifiedByName)
        {
            var oldCategory = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryUpdateDto.Id);
            var category = Mapper.Map<CategoryUpdateDto, Category>(categoryUpdateDto, oldCategory);
            category.ModifiedByName = modifiedByName;
            var updatedCategory = await UnitOfWork.Categories.UpdateAsync(category);
            await UnitOfWork.SaveChangesAsync();
            return new DataResult<CategoryDto>(ResultStatus.Success,Messages.Category.CategoryUpdated, new CategoryDto
            {
                Category = updatedCategory,
                ResultStatus = ResultStatus.Success,
                Message = Messages.Category.CategoryUpdated
            });
        }

        public async Task<IDataResult<CategoryDto>> DeleteAsync(int categoryId, string modifiedByName)
        {
            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                category.IsDeleted = true;
                category.ModifiedDate = DateTime.Now;
                category.ModifiedByName = modifiedByName;
                var deletedCategory = await UnitOfWork.Categories.UpdateAsync(category);
                await UnitOfWork.SaveChangesAsync();

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
            var category = await UnitOfWork.Categories.GetAsync(c => c.Id == categoryId);
            if (category != null)
            {
                await UnitOfWork.Categories.DeleteAsync(category);
                await UnitOfWork.SaveChangesAsync();
                return new Result(ResultStatus.Success, Messages.Category.CategoryHardDeleted);
            }
            return new Result(ResultStatus.Error, Messages.Category.CategoryNotFound);
        }

        public async Task<IDataResult<int>> CountByNonDeleted()
        {
            var categoriesCount = await UnitOfWork.Categories.CountAsync(c => !c.IsDeleted);
            if (categoriesCount > -1)
                return new DataResult<int>(ResultStatus.Success, categoriesCount);
            else
                return new DataResult<int>(ResultStatus.Error, Messages.Category.CategoryNotFound, -1);
        }




        #endregion







    }
}
