using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace MVCSite.Models
{
    [Table("Cart")]
    public class CartItem
    {
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string CartId { get; set; }
        [HiddenInput(DisplayValue = false)]
        public int PhotoId { get; set; }
        public Photo SelectPhoto { get; set; }
    }
}