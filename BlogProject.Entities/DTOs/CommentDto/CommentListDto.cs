﻿using BlogProject.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Entities.DTOs.CommentDto
{
    public class CommentListDto
    {
        public IList<Comment> Comments { get; set; }
    }
}
