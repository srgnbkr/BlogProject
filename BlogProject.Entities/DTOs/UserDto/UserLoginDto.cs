using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Entities.DTOs.UserDto
{
    public class UserLoginDto
    {
        [DisplayName("E-Posta Adresi")]
        [Required(ErrorMessage = "{0} Boş Olmamalı.")]
        [MaxLength(100, ErrorMessage = "{0} {1}  Karakterden Fazla Olmamalı.")]
        [MinLength(10, ErrorMessage = "{0} {1}  Karakterden Az Olmamalı.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Şifre")]
        [Required(ErrorMessage = "{0} Boş Olmamalı.")]
        [MaxLength(30, ErrorMessage = "{0} {1}  Karakterden Fazla Olmamalı.")]
        [MinLength(5, ErrorMessage = "{0} {1}  Karakterden Az Olmamalı.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
