using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services.Constants
{
    public static class Messages
    {
        public static class Category
        {
            public static string CategoryNotFound => "Cateogory not found";
            public static string CategoryAdded => "Category added successfully";
            public static string CategoryUpdated => "Category updated successfully";
            public static string CategoryDeleted => "Category deleted successfully";
            public static string CategoryHardDeleted => "Category deleted from database successfully";
        }

        public static class Articles
        {
            public static string ArticleNotFound => "Article not found";
            public static string ArticleAdded => "Article added successfully";
            public static string ArticleUpdated => "Article updated successfully";
            public static string ArticleDeleted => "Article deleted successfully";
            public static string ArticleHardDeleted => "Article deleted from database successfully";
        }

        public static class User
        {
            public static string CreateUser => "User created successfully";
            public static string DeletedUser => "User deleted successfully";
            public static string UpdatedUser => "User updated successfully";
            public static string UserNotFound => "User not found";
            public static string  PasswordWrong => "Email or password is wrong";

        }
    }
}
