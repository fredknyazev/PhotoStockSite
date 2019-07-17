using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MVCSite.Models
{
    [Table("Categories") ]
    public class Category
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; } // ID категории
        [Required]
        [Display(Name = "Категория")]
        public string Name { get; set; } // название категории
        public IEnumerable<Photo> Photos { get; set; }
    }
}