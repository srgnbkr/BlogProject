using BlogProject.DataAccess.Abstract;
using BlogProject.DataAccess.Concrete.EntityFramework.Context;
using BlogProject.DataAccess.Concrete.EntityFramework.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.DataAccess.Concrete.EntityFramework
{
    internal class UnitOfWork : IUnitOfWork
    {

        #region Variables
        private readonly BlogProjectDbContext _context;
        private ArticleRepository _articleRepository;
        private CategoryRepository _categoryRepository;
        private CommentRepository _commentRepository;
        private RoleRepository _roleRepository;
        private UserRepository _userRepository;
        #endregion

        #region Constructor
        public UnitOfWork(BlogProjectDbContext context)
        {
            _context = context;
        }
        #endregion

        public IArticleRepository Articles => _articleRepository ?? new ArticleRepository(_context);

        public ICategoryRepository Categories => _categoryRepository ?? new CategoryRepository(_context);

        public ICommentRepository Comments => _commentRepository ?? new CommentRepository(_context);

        public IRoleRepository Roles => _roleRepository ?? new RoleRepository(_context);

        public IUserRepository Users => _userRepository ?? new UserRepository(_context);


        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        
    }
}
