using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVCSite.Models
{
    public class PhotoContext:DbContext
    {
        public PhotoContext()
        {
            Database.SetInitializer<PhotoContext>(null);
        }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<CartItem> ShoppingCarts { get; set; }
    }
}