using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogProject.Entities.DTOs.CategoryDto
{
    public class CategoryAddDto
    {
        [DisplayName("Kategori Adı")]
        [Required(ErrorMessage = "{0} Boş Olmamalı.")]
        [MaxLength(70, ErrorMessage = "{0} {1}  Karakterden Fazla Olmamalı.")]
        [MinLength(3, ErrorMessage = "{0} {1}  Karakterden Az Olmamalı.")]
        public string Name { get; set; }


        [DisplayName("Kategori Açıklaması")]
        [MaxLength(500, ErrorMessage = "{0} {1}  Karakterden Fazla Olmamalı.")]
        [MinLength(3, ErrorMessage = "{0} {1}  Karakterden Az Olmamalı.")]
        public string Description { get; set; }

        [DisplayName("Kategori Özel Not Alanı")]
        [MaxLength(500, ErrorMessage = "{0} {1}  Karakterden Fazla Olmamalı.")]
        [MinLength(3, ErrorMessage = "{0} {1}  Karakterden Az Olmamalı.")]
        public string Note { get; set; }

        [DisplayName("Aktif Mi?")]
        [Required(ErrorMessage = "{0} Boş Olmamalı.")]
        public bool IsActive { get; set; }
    }
}
