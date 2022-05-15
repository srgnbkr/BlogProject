using BlogProject.Entities.Concrete;
using System.Collections.Generic;

namespace BlogProject.MvcUI.Models
{
    public class RightSideBarViewModel
    {
        public IList<Category> Categories { get; set; }
        public IList<Article> Articles { get; set; }
    }
}
