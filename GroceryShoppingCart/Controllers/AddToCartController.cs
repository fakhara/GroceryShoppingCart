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
            // Set up our ViewModel 
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }
        // 
        // GET: /StoreFront/AddToCart/id
        public ActionResult Add(int id)
        {
            // Retrieve the product from the database
            var addedProduct = db.Products.Single(product => product.Id == id);
            // Add it to the shopping cart 
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedProduct);
            // Go back to the main store page for more shopping 
            return RedirectToAction("Index");
        }
        // AJAX: /AddToCart/UpdateCartCount/id
        [HttpPost]
        public ActionResult UpdateCartCount(int id, int cartCount)
        {
            ShoppingCartRemoveViewModel results = null;
            try
            {
                // Get the cart 
                var cart = ShoppingCart.GetCart(this.HttpContext);

                // Get the name of the product to display confirmation 
                string productName = db.Carts.Single(item => item.ID == id).Product.Name;

                // Update the cart count 
                int itemCount = cart.UpdateCartCount(id, cartCount);

                //Prepare messages
                string msg = "The quantity of " + Server.HtmlEncode(productName) +
                      " has been refreshed in your shopping cart.";
                if (itemCount == 0) msg = Server.HtmlEncode(productName) +
                      " has been removed from your shopping cart.";
                //
                // Display the confirmation message 
                results = new ShoppingCartRemoveViewModel
                {
                    Message = msg,
                    CartTotal = cart.GetTotal(),
                    CartCount = cart.GetCount(),
                    ItemCount = itemCount,
                    DeletedId = id
                };
            }
            catch
            {
                results = new ShoppingCartRemoveViewModel
                {
                    Message = "Error occurred or invalid input...",
                    CartTotal = -1,
                    CartCount = -1,
                    ItemCount = -1,
                    DeletedId = id
                };
            }
            return Json(results);
        }
        // 
        // AJAX: /AddToCart/RemoveFromCart/id 
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Get the name of the product to display confirmation 
            string productName = db.Carts.FirstOrDefault(item => item.ProductId == id).Product.Name;
            // Remove from cart 
            int itemCount = cart.RemoveFromCart(id);
            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) + "has been removed your shopping cart",
                CartTotal = cart.GetTotal(),
                ItemCount = itemCount,
                DeletedId = id
            };
            return Json(results);
        }
        // 
        // GET: /AddToCart/CartSummary 
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}