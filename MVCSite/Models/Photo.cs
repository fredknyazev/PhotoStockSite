using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MVCSite.Models
{
    [Table("Photos")]
    public class Photo
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; } // ID фотографии

        [Required(ErrorMessage = "Пожалуйста введите название")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; } // название фотографии

        [HiddenInput(DisplayValue = false)]
        [Display(Name = "Фотография")]
        public byte[] PhotoSource { get; set; }// сама фотография
        [Required]
        [Display(Name = "Цена")]
        public decimal Price { get; set; } // цена

        [HiddenInput(DisplayValue = false)]
        public int? CategoryId { get; set; }

        [Display(Name = "Категория")]
        public Category category { get; set; }
    }
}