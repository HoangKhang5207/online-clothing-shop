using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo.Models;
using WebBanQuanAo.Models.EF;

namespace WebBanQuanAo.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index()
        {
            var items = _dbContext.Products.ToList();             
            return View(items);
        }

        public ActionResult Detail(string alias, int id)
		{
            var item = _dbContext.Products.Find(id);
            return View(item);
		}

        public ActionResult ProductCategory(string alias, int id)
        {
            var items = _dbContext.Products.ToList();
            if (id > 0)
            {
                items = items.Where(p => p.ProductCategoryID == id).ToList();
            }
            var cate = _dbContext.ProductCategories.Find(id);
            if (cate != null)
			{
                ViewBag.CateName = cate.Title;
			}
            ViewBag.CateId = id;
            return View(items); 
        }

        public ActionResult Partial_ItemsByCateId()
		{
            var items = _dbContext.Products.Where(p => p.IsHome && p.IsActive).Take(10).ToList();
            return PartialView("_Partial_ItemsByCateId", items);
		}

        public ActionResult Partial_ProductSales()
        {
            var items = _dbContext.Products.Where(p => p.IsSale && p.IsActive).Take(10).ToList();
            return PartialView("_Partial_ProductSales", items);
        }
    }
}