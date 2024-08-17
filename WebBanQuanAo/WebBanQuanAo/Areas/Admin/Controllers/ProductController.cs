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
    [Authorize(Roles = "Admin, Employee")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        // GET: Admin/Product
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Product> items = _dbContext.Products.OrderByDescending(c => c.Id);
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
            ViewBag.ProductCategory = new SelectList(_dbContext.ProductCategories.ToList(), "Id", "Title");
            return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, List<string> Images, List<int> rDefault)
		{
            if (ModelState.IsValid)
			{
                if (Images != null && Images.Count > 0)
				{
                    for (int i = 0; i < Images.Count; i++)
					{
                        if (i + 1 == rDefault[0])
						{
                            product.Image = Images[i];
                            product.ProductImages.Add(new ProductImage() 
                            { 
                                ProductId = product.Id, 
                                Image = Images[i], 
                                IsDefault = true 
                            });
						}  
                        else
						{
                            product.ProductImages.Add(new ProductImage() 
                            { 
                                ProductId = product.Id, 
                                Image = Images[i], 
                                IsDefault = false 
                            });
                        }                            
					}                        
				}                    
                product.CreatedDate = DateTime.Now;
                product.ModifierDate = DateTime.Now;

                if (string.IsNullOrEmpty(product.SeoTitle))
                    product.SeoTitle = product.Title;

                if (string.IsNullOrEmpty(product.Alias))
                    product.Alias = Models.Commons.Filter.FilterChar(product.Title);

                _dbContext.Products.Add(product);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
			}

            ViewBag.ProductCategory = new SelectList(_dbContext.ProductCategories.ToList(), "Id", "Title");
            return View(product);
		}

        public ActionResult Detail(int id)
		{
            ViewBag.ProductCategory = new SelectList(_dbContext.ProductCategories.ToList(), "Id", "Title");
            var item = _dbContext.Products.Find(id);
            return View(item);
        }

        public ActionResult Edit(int id)
		{
            ViewBag.ProductCategory = new SelectList(_dbContext.ProductCategories.ToList(), "Id", "Title");
            var item = _dbContext.Products.Find(id);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                product.ModifierDate = DateTime.Now;
                product.Alias = Models.Commons.Filter.FilterChar(product.Title);
                _dbContext.Products.Attach(product);
                _dbContext.Entry(product).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var item = _dbContext.Products.Find(id);
            if (item != null)
            {
                _dbContext.Products.Remove(item);
                _dbContext.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsActive(int id)
        {
            var item = _dbContext.Products.Find(id);
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
        public ActionResult IsHome(int id)
        {
            var item = _dbContext.Products.Find(id);
            if (item != null)
            {
                item.IsHome = !item.IsHome;
                _dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
                return Json(new { success = true, isActive = item.IsHome });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsSale(int id)
        {
            var item = _dbContext.Products.Find(id);
            if (item != null)
            {
                item.IsSale = !item.IsSale;
                _dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
                return Json(new { success = true, isActive = item.IsSale });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult IsFeature(int id)
        {
            var item = _dbContext.Products.Find(id);
            if (item != null)
            {
                item.IsFeature = !item.IsFeature;
                _dbContext.Entry(item).State = System.Data.Entity.EntityState.Modified;
                _dbContext.SaveChanges();
                return Json(new { success = true, isActive = item.IsFeature });
            }
            return Json(new { success = false });
        }
    }
}