using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo.Models;
using WebBanQuanAo.Models.EF;

namespace WebBanQuanAo.Areas.Admin.Controllers
{
    public class ProductImageController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Admin/ProductImage
        public ActionResult Index(int? id)
        {
            ViewBag.ProductId = id;
            var items = _dbContext.ProductImages.Where(p => p.ProductId == id).ToList();
            return View(items);
        }

        [HttpPost]
        public ActionResult AddImage(int productId, string url)
		{
            _dbContext.ProductImages.Add(new ProductImage()
            {
                ProductId = productId,
                Image = url,
                IsDefault = false
            });
            _dbContext.SaveChanges();
            return Json(new { success = true });
        } 

        [HttpPost]
        public ActionResult UpdateImage(int id, int productId)
		{
            foreach (var item in _dbContext.ProductImages)
			{
                if (item.IsDefault && item.ProductId == productId)
				{
                    item.IsDefault = false;
                }
			}

            var model = _dbContext.ProductImages.Find(id);
            _dbContext.ProductImages.Attach(model);
            _dbContext.Entry(model).State = System.Data.Entity.EntityState.Modified;
            model.IsDefault = true;
            _dbContext.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult Delete(int id)
		{
            var item = _dbContext.ProductImages.Find(id);
            _dbContext.ProductImages.Remove(item);
            _dbContext.SaveChanges();
            return Json(new { success = true });
		}
    }
}