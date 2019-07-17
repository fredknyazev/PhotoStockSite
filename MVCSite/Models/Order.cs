using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MVCSite.Models
{
    [Table("Orders")]
    public class Order
    {
        [HiddenInput(DisplayValue = false)]// ID покупки
        public int Id { get; set; }

        [Required]
        [Range(1, 50000, ErrorMessage = "Недопустимая сумма")]
        [Display(Name = "Общая стоимость")]
        public decimal TotalPrice { get; set; }

        public DateTime Date { get; set; } // дата покупки
        public IEnumerable<Item> Items { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите фамилию")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 50 символов")]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Пожалуйста введите свое имя")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Длина строки должна быть от 2 до 50 символов")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

    }
    public class Item
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int? OrderId { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int PhotoId { get; set; }

        public Photo ThePhoto { get; set; }
    }
}