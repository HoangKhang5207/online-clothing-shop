using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanQuanAo.Models;
using WebBanQuanAo.Models.EF;

namespace WebBanQuanAo.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart

        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ShoppingCartController()
        {
        }

        public ShoppingCartController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }

        public ActionResult Partial_Item_Cart()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.items.Any())
            {
                return PartialView("_Partial_Item_Cart", cart.items);
            }
            return PartialView("_Partial_Item_Cart");
        }

        public ActionResult ShowCount()
		{
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
			{
                return Json(new { count = cart.items.Count }, JsonRequestBehavior.AllowGet);
            }                
            return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);
		}

        public ActionResult Partial_Item_ThanhToan()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.items.Any())
            {
                return PartialView("_Partial_Item_ThanhToan", cart.items);
            }
            return PartialView("_Partial_Item_ThanhToan");
        }

        public ActionResult Partial_CheckOut()
		{
            var user = UserManager.FindByNameAsync(User.Identity.Name).Result;
            if (user != null)
			{
                ViewBag.User = user;
			}                
            return PartialView("_Partial_CheckOut");
		}

        public ActionResult CheckOutSuccess()
		{
            return View();
		}

        public ActionResult CheckOut()
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null && cart.items.Any())
            {
                ViewBag.CheckCart = cart;
            }
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOut(OrderViewModel item)
		{
            var code = new { success = false, code = -1 };
            if (ModelState.IsValid)
			{
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart != null)
                {
                    Order order = new Order();
                    order.CustomerName = item.CustomerName;
                    order.Phone = item.Phone;
                    order.Address = item.Address;
                    order.Email = item.Email;
                    cart.items.ForEach(c => order.OrderDetails.Add(new OrderDetail
                    {
                        ProductId = c.ProductId,
                        Quantity = c.Quantity,
                        Price = c.Price
                    }));
                    order.TotalAmount = cart.items.Sum(p => (p.Price * p.Quantity));
                    order.TypePayment = item.TypePayment;
                    order.CreatedDate = DateTime.Now;
                    order.ModifierDate = DateTime.Now;
                    order.CreatedBy = item.Phone;
                    Random rd = new Random();
                    order.Code = "DH" + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9) + rd.Next(0, 9);
                   
                    _dbContext.Orders.Add(order);
                    _dbContext.SaveChanges();
                    cart.clearCart();

                    return RedirectToAction("CheckOutSuccess");
                }
            }                
            return Json(code);
		}

        [HttpPost]
        public ActionResult AddToCart(int id, int quantity)
		{
            var code = new { success = false, msg = "", code = -1, count = 0 };
            var db = new ApplicationDbContext();
			
            var checkProduct = db.Products.FirstOrDefault(p => p.Id == id);
            if (checkProduct != null)
            {
                ShoppingCart cart = (ShoppingCart)Session["Cart"];
                if (cart == null)
                {
                    cart = new ShoppingCart();
                }
                ShoppingCartItem item = new ShoppingCartItem()
                {
                    ProductId = checkProduct.Id,
                    ProductName = checkProduct.Title,
                    CategoryName = checkProduct.ProductCategory.Title,
                    Quantity = quantity,
                    Alias = checkProduct.Alias
                };
                if (checkProduct.ProductImages.FirstOrDefault(p => p.IsDefault) != null)
                {
                    item.ProductImg = checkProduct.ProductImages.FirstOrDefault(p => p.IsDefault).Image;

                }
                item.Price = checkProduct.Price;
                if (checkProduct.PriceSale > 0)
                {
                    item.Price = (decimal)checkProduct.PriceSale;
                }
                item.TotalPrice = item.Quantity * item.Price;

                cart.AddToCart(item, quantity);

                Session["Cart"] = cart;

                code = new { success = true, msg = "Thêm sản phẩm vào giỏ hàng thành công!", code = 1, count = cart.items.Count };
            }
            return Json(code);            
		}

        [HttpPost]
        public ActionResult Update(int id, int quantity)
        {
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
            {
                cart.UpdateQuantity(id, quantity);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult Delete(int id)
		{
            var code = new { success = false, msg = "", code = -1, count = 0 };

            ShoppingCart cart = (ShoppingCart)Session["Cart"];

            if (cart != null)
			{
                var checkProduct = cart.items.FirstOrDefault(p => p.ProductId == id);
                if (checkProduct != null)
				{
                    cart.Remove(id);
                    code = new { success = true, msg = "Xóa sản phẩm thành công!", code = 1, count = cart.items.Count };
                }                    
			}                
            return Json(code);
        }

        [HttpPost]
        public ActionResult DeleteAll()
		{
            ShoppingCart cart = (ShoppingCart)Session["Cart"];
            if (cart != null)
			{
                cart.clearCart();
                return Json(new { success = true });
			}
            return Json(new { success = false });
        }
    }
}