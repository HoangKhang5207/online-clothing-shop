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
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Admin/Category
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Category> categories = _dbContext.Categories.OrderBy(c => c.Id);
            if (!string.IsNullOrEmpty(searchText))
            {
                categories = categories.Where(x => x.Title.Contains(searchText) || x.Alias.Contains(searchText) || x.Title.Equals(searchText, StringComparison.OrdinalIgnoreCase));
            }
            var pageNumber = page.HasValue ? Convert.ToInt32(page) : 1;
            categories = categories.ToPagedList(pageNumber, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(categories);
        }

        public ActionResult Create()
		{
            return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Category category)
		{
            if (ModelState.IsValid)
			{
                category.CreatedDate = DateTime.Now;
                category.ModifierDate = DateTime.Now;
                category.Alias = Models.Commons.Filter.FilterChar(category.Title);
                _dbContext.Categories.Add(category);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
		}

        public ActionResult Detail(int id)
		{
            var item = _dbContext.Categories.Where(c => c.Id == id).FirstOrDefault();
            return View(item);
		}

        public ActionResult Edit(int id)
		{
            Category category = _dbContext.Categories.Find(id);
            return View(category);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Attach(category);
                category.ModifierDate = DateTime.Now;
                category.Alias = Models.Commons.Filter.FilterChar(category.Title);
                _dbContext.Entry(category).Property(x => x.Title).IsModified = true;
                _dbContext.Entry(category).Property(x => x.Description).IsModified = true;
                _dbContext.Entry(category).Property(x => x.Alias).IsModified = true;
                _dbContext.Entry(category).Property(x => x.SeoDescription).IsModified = true;
                _dbContext.Entry(category).Property(x => x.SeoKeywords).IsModified = true;
                _dbContext.Entry(category).Property(x => x.SeoTitle).IsModified = true;
                _dbContext.Entry(category).Property(x => x.Position).IsModified = true;
                _dbContext.Entry(category).Property(x => x.ModifierDate).IsModified = true;
                _dbContext.Entry(category).Property(x => x.ModifierBy).IsModified = true;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Delete(int id)
		{
            var item = _dbContext.Categories.Find(id);
            if (item != null)
			{
                _dbContext.Categories.Remove(item);
                _dbContext.SaveChanges();
                return Json(new { success = true });
			}
            return Json(new { success = false });
		}
    }
}