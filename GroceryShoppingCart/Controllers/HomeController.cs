using GroceryShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroceryShoppingCart.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index(string catagory)
        {
<<<<<<< HEAD
             return View();
        }
=======
            var catagoryModel = db.Catagories.Include("Products").Single(c => c.Name == catagory);
            return View(catagoryModel);
        }
        
        public ActionResult Details(int id)
        {
            var Item = new Product { Name = "Product" + id };
            return View(Item);
        }

>>>>>>> bc0854a3c9fb5ead10f74bf7c09d9bc7b1eaa3fc
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}