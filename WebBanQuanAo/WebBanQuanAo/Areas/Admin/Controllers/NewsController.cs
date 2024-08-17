using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo.Models;
using WebBanQuanAo.Models.EF;

namespace WebBanQuanAo.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Admin/News
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 10;
            if (page == null)
			{
                page = 1;
			}
            IEnumerable<News> news = _dbContext.News.OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(searchText))
            {
                news = news.Where(x => x.Title.Contains(searchText) || x.Alias.Contains(searchText));
			}                
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            news = news.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(news);
        }

        public ActionResult Create()
		{
            return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(News news)
		{
            if (ModelState.IsValid)
			{
                news.CreatedDate = DateTime.Now;
                news.ModifierDate = DateTime.Now;
                news.CategoryId = 3;
                news.Alias = Models.Commons.Filter.FilterChar(news.Title);
                _dbContext.News.Add(news);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
			}
            return View(news);
		}

        public ActionResult Edit(int id)
        {
            News news = _dbContext.News.Find(id);
            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(News news)
        {
            if (ModelState.IsValid)
            {
                news.ModifierDate = DateTime.Now;
                news.Alias = Models.Commons.Filter.FilterChar(news.Title);
                _dbContext.News.Attach(news);
                _dbContext.Entry(news).State = System.Data.Entity.EntityState.Modified; 
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        [HttpPost]
        public ActionResult Delete(int id)
		{
            var item = _dbContext.News.Find(id);
            if (item != null)
			{
                _dbContext.News.Remove(item);
                _dbContext.SaveChanges();
                return Json(new { success = true });
			}                
            return Json(new { success = false });
		}

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = _dbContext.News.Find(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                _dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
                return Json(new { success = true, isActive = item.IsActive });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult DeleteAll(string ids)
		{
            if (!string.IsNullOrEmpty(ids))
			{
                var items = ids.Split(',');
                if (items != null && items.Any())
				{
                    foreach (var item in items)
					{
                        var obj = _dbContext.News.Find(int.Parse(item));
                        _dbContext.News.Remove(obj);
                        _dbContext.SaveChanges();
					}                        
				}
                return Json(new { success = true });
			}                
            return Json(new { success = false });
		}
    }
}