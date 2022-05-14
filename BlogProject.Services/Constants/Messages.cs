using BlogProject.Entities.DTOs.CommentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Services.Constants
{
    public static class Messages
    {
        public static class Role
        {
            public static string RoleAssign => "Rol Atama İşlemi Başarılı";
        }

        public static class Category
        {
            public static string CategoryNotFound =>"Kategori bulunamadı";
            public static string CategoryAdded => "Kategori başarıyla eklendi";
            public static string CategoryUpdated => "Kategori başarıyla güncellendi";
            public static string CategoryDeleted => "Kategori başarıyla silindi";
            public static string CategoryHardDeleted => "Kategori başarıyla veritabanından silindi";

            public static string CategoryUndoDeleted => "Kategori arşivden geri getirildi";
        }

        public static class Articles
        {
            public static string ArticleNotFound => "Makale bulunamadı";
            public static string ArticleAdded => "Makale başarıyla eklendi";
            public static string ArticleUpdated => "Makale başarıyla güncellendi";
            public static string ArticleDeleted => "Makale başarıyla silindi";
            public static string ArticleHardDeleted => "Makale başarıyla veritabanından silindi";
        }

        public static class Comment
        {
            public static string CommentNotFound => "Yorum bulunamadı";
            public static string CommentAdded => "Yorum Eklendi";
            public static string CommentUpdated => "Yorum Güncellendi";
            public static string CommentDeleted => "Yorum Silindi";
            public static string CommentUndoDeleted => "Yorum arşivden geri getirildi";
            public static string CommentHardDeleted => "Yorum veritabanından silindi";

            public static string CommentApproved => "Yorum onaylandı";
        }


        public static class User
        {
            public static string CreateUser => "Kullanıcı başarıyla oluşturuldu";
            public static string DeletedUser => "Kullanıcı başarıyla silindi";
            public static string UpdatedUser => "Kullanıcı başarıyla güncellendi";
            public static string UserNotFound => "Kullanıcı bulunamadı";
            public static string  PasswordWrong => "EPosta Ya da Şifre Hatalı";

        }
    }
}
