using BlogProject.Entities.Concrete;
using BlogProject.Shared.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Entities.DTOs.UserDto
{
    public class UserDto :DtoGetBase
    {
        public User User { get; set; }
    }
}
