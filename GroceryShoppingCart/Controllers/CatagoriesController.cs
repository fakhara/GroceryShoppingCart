using GroceryShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GroceryShoppingCart.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CatagoriesController : Controller
    {
       
        // GET: Catagories
        public ActionResult Index()
        {
            //Declear a list of models
            List<Catagory> catagoryList;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //Init the List
                catagoryList = db.Catagories.ToArray().OrderBy(x => x.Sorting).Select(x => new Catagory(x)).ToList();
            }
            //Return view with list
            return View(catagoryList);
        }
        //Post:Catagories/AddNewCatagory
        [HttpPost]
        public string AddNewCatagory(string catName)
        {
            //Declear id
            string id;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //Check that the category name is unique
                if (db.Catagories.Any(x => x.Name == catName))
                    return "titletaken";

                //Init Catagory
                Catagory CAT = new Catagory();
                //Add to Catagory
                CAT.Name = catName;
                CAT.Sorting = 100;
                //Save DTO
                db.Catagories.Add(CAT);
                db.SaveChanges();
                //Get the id
                id = CAT.Id.ToString();
            }
            //Return id
            return id;
        }
        //Post:Catagories/ReorderCatagories
        [HttpPost]
        public void ReorderCatagories(int[] id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //Set initial count
                int count = 1;
                //Declear Catagory
                Catagory CAT;
                //Set sorting for each catagory
                foreach (var catId in id)
                {
                    CAT = db.Catagories.Find(catId);
                    CAT.Sorting = count;

                    db.SaveChanges();
                    count++;
                }
            }
        }
        //Get: Catagories/DeleteCatagory/id
        public ActionResult DeleteCatagory(int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //Get the category
                Catagory CAT = db.Catagories.Find(id);
                //Remove the category
                db.Catagories.Remove(CAT);
                //Save
                db.SaveChanges();
            }
            //Redirect
            return RedirectToAction("Index");
        }
        //Post: Catagories/RenameCatagory
        [HttpPost]
        public string RenameCatagory(string newCatName, int id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //Check category name is unique
                if (db.Catagories.Any(x => x.Name == newCatName))
                    return "titletaken";
                // Get DTO
                Catagory CAT = db.Catagories.Find(id);
                //Edit DTO
                CAT.Name = newCatName;
                
                // Save
                db.SaveChanges();
            }
            //return
            return "ok";

        }

    }
}