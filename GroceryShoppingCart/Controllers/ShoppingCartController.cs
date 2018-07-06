using GroceryShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroceryShoppingCart.Controllers
{
    public class ShoppingCartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add(int id)
        {
            Cart cart = Session["Cart"] as Cart;
            if(cart == null || Session["Cart"] == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            //get infor product
            var itemproduct = db.Products.Where(p => p.Id == id).First();
            //cart.(itemproduct);
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ShowCart()
        {
            if (Session["Cart"] == null)
                return RedirectToAction("ShowProduct", "ShoppingCart");
            Cart cart = Session["Cart"] as Cart;
            return View(cart);
        }
        public ActionResult MyOrder()
        {
            return View((Cart)Session["Cart"]);
        }

    }
}