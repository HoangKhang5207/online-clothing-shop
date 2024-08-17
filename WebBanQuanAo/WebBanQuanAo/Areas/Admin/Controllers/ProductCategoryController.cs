using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo.Models;
using WebBanQuanAo.Models.EF;
using PagedList;

namespace WebBanQuanAo.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class ProductCategoryController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Admin/ProductCategory
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<ProductCategory> items = _dbContext.ProductCategories.OrderBy(c => c.Id);
            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Title.Contains(searchText) || x.Alias.Contains(searchText) || x.Title.Equals(searchText, StringComparison.OrdinalIgnoreCase));
            }
            var pageNumber = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageNumber, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items);
        }

        public ActionResult Create()
		{
            return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCategory productCategory)
        {
            if (ModelState.IsValid)
			{
                productCategory.CreatedDate = DateTime.Now;
                productCategory.ModifierDate = DateTime.Now;
                productCategory.Alias = Models.Commons.Filter.FilterChar(productCategory.SeoTitle);
                _dbContext.ProductCategories.Add(productCategory);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }                
            return View();
        }

        public ActionResult Detail(int id)
        {
            var item = _dbContext.ProductCategories.Where(c => c.Id == id).FirstOrDefault();
            return View(item);
        }

        public ActionResult Edit(int id)
        {
            var item = _dbContext.ProductCategories.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCategory model)
        {
            if (ModelState.IsValid)
            {
                _dbContext.ProductCategories.Attach(model);
                model.ModifierDate = DateTime.Now;
                model.Alias = Models.Commons.Filter.FilterChar(model.SeoTitle);
                _dbContext.Entry(model).Property(x => x.Title).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Description).IsModified = true;
                _dbContext.Entry(model).Property(x => x.Alias).IsModified = true;
                _dbContext.Entry(model).Property(x => x.SeoDescription).IsModified = true;
                _dbContext.Entry(model).Property(x => x.SeoKeywords).IsModified = true;
                _dbContext.Entry(model).Property(x => x.SeoTitle).IsModified = true;
                _dbContext.Entry(model).Property(x => x.ModifierDate).IsModified = true;
                _dbContext.Entry(model).Property(x => x.ModifierBy).IsModified = true;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbContext.ProductCategories.Find(id);
            if (item != null)
            {
                _dbContext.ProductCategories.Remove(item);
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}