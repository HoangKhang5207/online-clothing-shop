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
    public class OrderController : Controller
    {
        // GET: Admin/Order
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        public ActionResult Index(string searchText, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Order> items = _dbContext.Orders.OrderByDescending(c => c.CreatedDate);
            if (!string.IsNullOrEmpty(searchText))
            {
                items = items.Where(x => x.CustomerName.Contains(searchText) || x.Code.Contains(searchText) || x.CustomerName.Equals(searchText, StringComparison.OrdinalIgnoreCase));
            }
            var pageNumber = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageNumber, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            return View(items);
        }

        public ActionResult ViewOrder(int id)
		{
            var item = _dbContext.Orders.Find(id);
            return View(item);
		}

        public ActionResult Partial_SanPham(int id)
		{
            var items = _dbContext.OrderDetails.Where(o => o.OrderId == id).ToList();
            return PartialView("_Partial_SanPham", items);
		}

        [HttpPost]
        public ActionResult UpdateStatus(int id, int status)
		{
            var item = _dbContext.Orders.Find(id);
            if (item != null)
			{
                _dbContext.Orders.Attach(item);
                item.TypePayment = status;
                _dbContext.Entry(item).Property(o => o.TypePayment).IsModified = true;
                _dbContext.SaveChanges();
                return Json(new { message = "Success", success = true });
			}
            return Json(new { message = "Unsuccess", success = false });
        }
    }
}