using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo.Models;
using WebBanQuanAo.Models.EF;

namespace WebBanQuanAo.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MenuTop()
		{
            var items = _dbContext.Categories.OrderBy(m => m.Position).ToList();
            return PartialView("_MenuTop", items);
		}

        public ActionResult MenuProductCategory()
		{
            var items = _dbContext.ProductCategories.ToList();
            return PartialView("_MenuProductCategory", items);
		}

        public ActionResult MenuLeft(int? id)
        {
            if (id != null)
			{
                ViewBag.CateId = id;
			}
            var items = _dbContext.ProductCategories.ToList();
            return PartialView("_MenuLeft", items);
        }

        public ActionResult MenuArrivals()
        {
            var items = _dbContext.ProductCategories.ToList();
            return PartialView("_MenuArrivals", items);
        }
    }
}