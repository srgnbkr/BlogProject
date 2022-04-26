using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Entities.DTOs.UserDto
{
    public class UserUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [DisplayName("Kullanıcı Adı")]
        [Required(ErrorMessage = "{0} Boş Olmamalı.")]
        [MaxLength(50, ErrorMessage = "{0} {1}  Karakterden Fazla Olmamalı.")]
        [MinLength(3, ErrorMessage = "{0} {1}  Karakterden Az Olmamalı.")]
        public string UserName { get; set; }

        [DisplayName("E-Posta Adresi")]
        [Required(ErrorMessage = "{0} Boş Olmamalı.")]
        [MaxLength(100, ErrorMessage = "{0} {1}  Karakterden Fazla Olmamalı.")]
        [MinLength(10, ErrorMessage = "{0} {1}  Karakterden Az Olmamalı.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        


        [DisplayName("Telefon Numarası")]
        [Required(ErrorMessage = "{0} Boş Olmamalı.")]
        [MaxLength(13, ErrorMessage = "{0} {1}  Karakterden Fazla Olmamalı.")]
        [MinLength(13, ErrorMessage = "{0} {1}  Karakterden Az Olmamalı.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }



        [DisplayName("Fotoğraf Ekle")]
        [DataType(DataType.Upload)]
        public IFormFile PictureFile { get; set; }

        [DisplayName("Fotoğraf")]
        public string Picture { get; set; } 



    }
}
