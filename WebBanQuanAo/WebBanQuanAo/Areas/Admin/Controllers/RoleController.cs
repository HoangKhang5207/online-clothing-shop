using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class RoleController : Controller
    {
        // GET: Admin/Role
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 5;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<IdentityRole> items = _dbContext.Roles.OrderBy(c => c.Name);
            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.Name.Contains(searchText) || x.Name.Equals(searchText, StringComparison.OrdinalIgnoreCase));
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
        public ActionResult Create(IdentityRole role)
        {
            if (ModelState.IsValid)
			{
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbContext));
                roleManager.Create(role);
                return RedirectToAction("Index");
			}
            return View(role);
        }

        public ActionResult Detail(string id)
		{
            var item = _dbContext.Roles.Where(r => r.Id.Equals(id)).FirstOrDefault();
            return View(item);
		}

        public ActionResult Edit(string id)
        {
            var item = _dbContext.Roles.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_dbContext));
                roleManager.Update(role);
                return RedirectToAction("Index");
            }
            return View(role);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var item = _dbContext.Roles.Find(id);
            if (item != null)
            {
                _dbContext.Roles.Remove(item);
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}