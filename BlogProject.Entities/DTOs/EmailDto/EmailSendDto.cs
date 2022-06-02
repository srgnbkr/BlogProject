using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Entities.DTOs.EmailDto
{
    public class EmailSendDto
    {
        [DisplayName("İsminiz")]
        [Required(ErrorMessage = "{0} Alanı Zorunludur")]
        [MaxLength(60, ErrorMessage = "{0} Alanı {1} Karakterden Fazla Olamaz")]
        [MinLength(5, ErrorMessage = "{0} Alanı {1} Karakterden Fazla Olamaz")]
        public string Name { get; set; }

        [DisplayName("E-Posta")]    
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "{0} Alanı Zorunludur")]
        [MaxLength(100, ErrorMessage = "{0} Alanı {1} Karakterden Fazla Olamaz")]
        [MinLength(10, ErrorMessage = "{0} Alanı {1} Karakterden Fazla Olamaz")]
        public string Email { get; set; }

        [DisplayName("Konu")]
        [Required(ErrorMessage = "{0} Alanı Zorunludur")]
        [MaxLength(100, ErrorMessage = "{0} Alanı {1} Karakterden Fazla Olamaz")]
        [MinLength(5, ErrorMessage = "{0} Alanı {1} Karakterden Fazla Olamaz")]        
        public string Subject { get; set; }

        [DisplayName("Mesajınız")]
        [Required(ErrorMessage = "{0} Alanı Zorunludur")]
        [MaxLength(1500, ErrorMessage = "{0} Alanı {1} Karakterden Fazla Olamaz")]
        [MinLength(10, ErrorMessage = "{0} Alanı {1} Karakterden Fazla Olamaz")]
        public string Message { get; set; }
        
    }
}
