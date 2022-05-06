using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Entities.DTOs.UserDto
{
    public class ImageDeletedDto
    {
        public string FullName { get; set; }
        public string Extensions { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
    }
}
