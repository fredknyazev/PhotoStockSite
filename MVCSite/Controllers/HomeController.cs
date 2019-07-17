using MVCSite.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCSite.Controllers
{
    public class HomeController : Controller
    {
        private PhotoContext db = new PhotoContext();
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
            var categoriesList = db.Categories.ToList();
            categoriesList.Insert(0, new Category() { Id = 0, Name = "все" });
            SelectList categories = new SelectList(categoriesList, "Id", "Name");
            ViewBag.Genres = categories;
            return View(photoList);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Никакого серьёзного проекта.";

            return View();
        }
        public FileContentResult GetImage(int Id)
        {
            byte[] photoSrc = db.Photos.Find(Id).PhotoSource;
            ImageConverter ic = new ImageConverter();
            Image img = (Image)ic.ConvertFrom(photoSrc);
            Bitmap bitmapImg = new Bitmap(img);
            var y = bitmapImg.Height/10;
            var x = bitmapImg.Width/10;
            Bitmap smallerImg = new Bitmap(bitmapImg, x, y);
            var stream = new MemoryStream();
            smallerImg.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] SmallerPhotoSrc = stream.ToArray();
            var base64 = Convert.ToBase64String(SmallerPhotoSrc);
            var imgSrc = String.Format("data:image/jpg;base64,{0}", base64);
            return new FileContentResult(SmallerPhotoSrc, "image/jpg");
        }
	}
    }