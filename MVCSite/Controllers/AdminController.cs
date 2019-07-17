using MVCSite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVCSite.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        PhotoContext db = new PhotoContext();

        public ActionResult Index(int? categoryId)
        {
            List<Photo> photoList = db.Photos.Include(p => p.category).ToList();
            if (categoryId != null && categoryId != 0)
            {
                var category = db.Categories.Find(categoryId);
                if (category == null)
                {
                    return HttpNotFound();
                }
                photoList = photoList.Where(b => b.CategoryId == category.Id).ToList();
            }
            return View(photoList);
        }
        public ActionResult UsersList()
        {
            return View();
        }
        public ActionResult CreatePhoto()
        {
            SelectList categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            ViewBag.Categories = categories;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePhoto(Photo photo, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid && uploadImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                photo.PhotoSource = imageData;
                db.Photos.Add(photo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(photo);
        }
        public ActionResult EditPhoto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            SelectList categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            ViewBag.Categories = categories;
            return View(photo);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPhoto([Bind(Include = "Id,Name,Price,CategoryId")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(photo);
        }
        public ActionResult DeletePhoto(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }
        [HttpPost, ActionName("DeletePhoto")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Photo photo = db.Photos.Find(id);
            db.Photos.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult OrdersList(DateTime? startDate, DateTime? endDate)
        {
            var result = new List<Order>();
            if (startDate == null || endDate == null)
            {
                result = db.Orders.ToList();
            }
            else
            {
                result = db.Orders.Where(x => (x.Date > startDate && x.Date <= endDate)).ToList();
            }
            return View(result);

        }
	}
}