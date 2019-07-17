using MVCSite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCSite.Controllers
{
    public class OrderController : Controller
    {
        private PhotoContext db = new PhotoContext();
        //
        // GET: /Order/
        public ActionResult Create()
        {
            string cartId;
            if (HttpContext.Request.Cookies.AllKeys.Length > 0 &&
                HttpContext.Request.Cookies.AllKeys.First(c => c.Contains("CartId")) != null)
            {
                cartId = HttpContext.Request.Cookies["CartId"].Value;
            }
            else
            {
                return HttpNotFound();
            }
            var items = db.ShoppingCarts.Where(c => c.CartId.CompareTo(cartId) == 0);
            Order newOrder = new Order();
            List<Item> goods = new List<Item>();
            decimal price = 0;
            foreach (var i in items)
            {
                var p = db.Photos.Find(i.PhotoId);
                if (p == null) continue;
                var tm = new Item() { PhotoId = i.PhotoId, ThePhoto = p };
                price += p.Price;
                goods.Add(tm);
            }

            newOrder.TotalPrice = price;
            newOrder.Items = goods;
            newOrder.Date = DateTime.Now;
            newOrder.LastName = "Введите фамилию";
            newOrder.Name = "Введите имя";
            //db.Orders.Add(newOrder);
            //db.SaveChanges();
            //foreach (var item in goods)
            //{
            //    item.OrderId = newOrder.Id;
            //    db.Items.Add(item);
            //    db.SaveChanges();
            //}
            return View(newOrder);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,LastName,TotalPrice,Date")]Order orderR)
        {
            if (ModelState.IsValid)
            {
                string cartId;
                if (HttpContext.Request.Cookies.AllKeys.Length > 0 &&
                    HttpContext.Request.Cookies.AllKeys.First(c => c.Contains("CartId")) != null)
                {
                    cartId = HttpContext.Request.Cookies["CartId"].Value;
                    var items = db.ShoppingCarts.Where(c => c.CartId.CompareTo(cartId) == 0);
                    List<Item> goods = new List<Item>();
                    foreach (var i in items)
                    {
                        var p = db.Photos.Find(i.PhotoId);
                        if (p == null) continue;
                        var tm = new Item() { PhotoId = i.PhotoId, ThePhoto = p };
                        goods.Add(tm);
                    }
                    orderR.Items = goods;
                    db.Orders.Add(orderR);
                    
                    var carts = db.ShoppingCarts.Where(x => x.CartId == cartId);
                    db.ShoppingCarts.RemoveRange(carts);
                    db.SaveChanges();
                    HttpContext.Request.Cookies.Remove("CartId");
                }

                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var items = db.Items.Where(x => x.OrderId == order.Id);
            db.Items.RemoveRange(items);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}