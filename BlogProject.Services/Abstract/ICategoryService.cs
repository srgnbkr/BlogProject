using BlogProject.Entities.Concrete;
using BlogProject.Shared.Utilities.Results.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services.Abstract
{
    public interface ICategoryService
    {
        Task<DataResult<Category>> GetAsync(int categoryId);
        Task<DataResult<IList<Category>>> GetListAsync();
    }
}
