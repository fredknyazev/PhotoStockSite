using MVCSite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSite.Controllers
{
    public class CartController : Controller
    {
        private PhotoContext db = new PhotoContext();
        //
        // GET: /Cart/
        public ActionResult Index()
        {
            string cartId;
            if (HttpContext.Request.Cookies.AllKeys.Length > 0 &&
                HttpContext.Request.Cookies.AllKeys.Contains("CartId"))
            {
                cartId = HttpContext.Request.Cookies["CartId"].Value;
            }
            else
            {
                return HttpNotFound();
            }
            var shoppingcarts =
                db.ShoppingCarts.Where(t => t.CartId == cartId).Include(c => c.SelectPhoto);
            decimal price = 0;
            foreach (var cart in shoppingcarts)
            {
                price += cart.SelectPhoto.Price;
            }
            ViewBag.Total = price.ToString();

            return View(shoppingcarts.ToList());
        }
        public ActionResult Add(int photoId)
        {
            string cartId;
            if (HttpContext.Request.Cookies.AllKeys.Length > 0 &&
                HttpContext.Request.Cookies.AllKeys.Contains("CartId"))
            {
                cartId = HttpContext.Request.Cookies["CartId"].Value;
            }
            else
            {
                cartId = Guid.NewGuid().ToString();
                HttpCookie c = new HttpCookie("CartId", cartId);
                HttpContext.Response.Cookies.Add(c);
            }

            var b = db.ShoppingCarts.Where(c => c.CartId.CompareTo(cartId) == 0 && c.PhotoId == photoId);
            if (!b.Any())
            {
                var item = new CartItem
                {
                    CartId = cartId,
                    PhotoId = photoId,
                };
                db.ShoppingCarts.Add(item);
            }

            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartitem = db.ShoppingCarts.Find(id);
            if (cartitem == null)
            {
                return HttpNotFound();
            }
            cartitem.SelectPhoto = db.Photos.Find(cartitem.PhotoId);
            return View(cartitem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CartId,PhotoId")] CartItem cartitem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PhotoId = new SelectList(db.Photos, "Id", "Name", cartitem.PhotoId);
            return View(cartitem);
        }
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartItem cartitem = db.ShoppingCarts.Find(id);
            if (cartitem == null)
            {
                return HttpNotFound();
            }
            cartitem.SelectPhoto = db.ShoppingCarts.Find(id).SelectPhoto;
            return View(cartitem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CartItem cartitem = db.ShoppingCarts.Find(id);
            db.ShoppingCarts.Remove(cartitem);
            db.SaveChanges();
            return RedirectToAction("Index");
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