using BlogProject.Entities.Concrete;
using BlogProject.Shared.Entities.Abstract;
using BlogProject.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Entities.DTOs.ArticleDto
{
    public class ArticleDto : DtoGetBase
    {
        public Article Article { get; set; }
        
    }
}
