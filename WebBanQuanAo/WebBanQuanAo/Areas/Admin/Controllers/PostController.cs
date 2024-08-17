using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo.Models;
using WebBanQuanAo.Models.EF;

namespace WebBanQuanAo.Areas.Admin.Controllers
{
    public class PostController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Admin/Posts
        public ActionResult Index()
        {
            List<Post> posts = _dbContext.Post.ToList();
            return View(posts);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post)
        {
            if (ModelState.IsValid)
            {
                post.CreatedDate = DateTime.Now;
                post.ModifierDate = DateTime.Now;
                post.CategoryId = 3;
                post.Alias = Models.Commons.Filter.FilterChar(post.Title);
                _dbContext.Post.Add(post);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        public ActionResult Edit(int id)
        {
            Post post = _dbContext.Post.Find(id);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                post.ModifierDate = DateTime.Now;
                post.Alias = Models.Commons.Filter.FilterChar(post.Title);
                _dbContext.Post.Attach(post);
                _dbContext.Entry(post).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbContext.Post.Find(id);
            if (item != null)
            {
                _dbContext.Post.Remove(item);
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = _dbContext.Post.Find(id);
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
                        var obj = _dbContext.Post.Find(int.Parse(item));
                        _dbContext.Post.Remove(obj);
                        _dbContext.SaveChanges();
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}