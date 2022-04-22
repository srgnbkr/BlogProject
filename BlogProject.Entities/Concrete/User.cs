using System;
using System.Collections.Generic;
using BlogProject.Shared.Entities.Abstract;
using Microsoft.AspNetCore.Identity;

namespace BlogProject.Entities.Concrete
{
    public class User : IdentityUser<int>
    {
        
        
        public string Picture { get; set; }
        
        public ICollection<Article> Articles { get; set; }
    }
}