using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCSite.Models
{
    public class RoleViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; } // ID 

        [Required(ErrorMessage = "Пожалуйста введите название")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        [Display(Name = "Название")]
        public string Name { get; set; } // название 

        [Display(Name = "Список пользователей")]
        public List<ApplicationUser> UsersInRole { get; set; }
    }
    public class RoleEditModel
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<ApplicationUser> Members { get; set; }
        public IEnumerable<ApplicationUser> NonMembers { get; set; }
    }
    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}