using BlogProject.DataAccess.Abstract;
using BlogProject.DataAccess.Concrete.EntityFramework;
using BlogProject.DataAccess.Concrete.EntityFramework.Context;
using BlogProject.Services.Abstract;
using BlogProject.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services.Extesions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection LoadMyService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<BlogProjectDbContext>();
            serviceCollection.AddScoped<IUnitOfWork,UnitOfWork>();

            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IArticleService, ArticleManager>();
            return serviceCollection;
        }
    }
}
