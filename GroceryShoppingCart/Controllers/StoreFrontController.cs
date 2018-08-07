using GroceryShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GroceryShoppingCart.Controllers
{
    public class StoreFrontController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: StoreFront
        public ActionResult Index(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catagory catagory = db.Catagories.Find(id);
            if(catagory == null)
            {
                return HttpNotFound();
            }
            return View(catagory);
        }
        

    }
}