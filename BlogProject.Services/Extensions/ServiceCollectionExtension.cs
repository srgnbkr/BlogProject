using BlogProject.DataAccess.Abstract;
using BlogProject.DataAccess.Concrete.EntityFramework;
using BlogProject.DataAccess.Concrete.EntityFramework.Context;
using BlogProject.Entities.Concrete;
using BlogProject.Services.Abstract;
using BlogProject.Services.Concrete;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection LoadMyService(this IServiceCollection serviceCollection,string connectionString)
        {
            serviceCollection.AddDbContext<BlogProjectDbContext>(options => options.UseSqlServer(connectionString));


            serviceCollection.AddIdentity<User, Role>(options =>
            {
                #region Password Settings

                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                #endregion

                #region Username and Email Settings
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+$";
                options.User.RequireUniqueEmail = true;
                #endregion

            }).AddEntityFrameworkStores<BlogProjectDbContext>();
            
            serviceCollection.AddScoped<IUnitOfWork,UnitOfWork>();

            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IArticleService, ArticleManager>();
            return serviceCollection;
        }
    }
}
