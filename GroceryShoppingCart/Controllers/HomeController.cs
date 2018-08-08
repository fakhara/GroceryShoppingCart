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
        public ActionResult Index()
        {

             return View();
        }

        public ActionResult About()
        {
           return View();
        }

        public ActionResult Contact()
        {
             return View();
        }
        public ActionResult Map()
        {
            return View();
        }
    }
}