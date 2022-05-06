using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Entities.DTOs.UserDto
{
    public class UserPasswordChangeDto
    {
        [DisplayName("Şu Anki Şifreniz")]
        [Required(ErrorMessage = "{0} Boş Olmamalı.")]
        [MaxLength(30, ErrorMessage = "{0} {1}  Karakterden Fazla Olmamalı.")]
        [MinLength(5, ErrorMessage = "{0} {1}  Karakterden Az Olmamalı.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DisplayName("Yeni Şifreniz")]
        [Required(ErrorMessage = "{0} Boş Olmamalı.")]
        [MaxLength(30, ErrorMessage = "{0} {1}  Karakterden Fazla Olmamalı.")]
        [MinLength(5, ErrorMessage = "{0} {1}  Karakterden Az Olmamalı.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DisplayName("Yeni Şifrenizi Tekrar Giriniz")]
        [Required(ErrorMessage = "{0} Boş Olmamalı.")]
        [MaxLength(30, ErrorMessage = "{0} {1}  Karakterden Fazla Olmamalı.")]
        [MinLength(5, ErrorMessage = "{0} {1}  Karakterden Az Olmamalı.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Şifreler Uyuşmuyor.")]
        public string RepeatPassword { get; set; }
    }
}
