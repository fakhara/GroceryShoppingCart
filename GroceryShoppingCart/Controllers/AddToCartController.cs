using GroceryShoppingCart.Models;
using GroceryShoppingCart.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroceryShoppingCart.Controllers
{
    public class AddToCartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: AddToCart
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }
        public ActionResult Add(int id)
        {
            var addedProduct = db.Products.Single(product => product.Id == id);
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedProduct);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            string productName = db.Carts.FirstOrDefault(item => item.ProductId == id).Product.Name;
            int itemCount = cart.RemoveFromCart(id);
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) + "has been removed your shopping cart",
                CartTotal = cart.GetTotal(),
                ItemCount = itemCount,
                DeletedId = id
            };
            return Json(results);
        }
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}