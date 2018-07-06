using GroceryShoppingCart.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace GroceryShoppingCart.Controllers
{
    public class ProductsController : Controller
    {
         //Get: Products/AddProduct
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult AddProduct()
        {
            //Init model
            Product model = new Product();

            //Add select list of categories to model
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model.Catagories = new SelectList(db.Catagories.ToList(), "Id", "Name");
            }

            //Return view with model
            return View(model);
        }

        //Post: Products/AddProduct
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddProduct(Product model, HttpPostedFileBase file)
        {
            //Check model state
            if (!ModelState.IsValid)
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    model.Catagories = new SelectList(db.Catagories.ToList(), "Id", "Name");
                    return View(model);
                }
            }

            //Make sure product name is unique
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Products.Any(x => x.Name == model.Name))
                {
                    model.Catagories = new SelectList(db.Catagories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "That product name is taken!");
                    return View(model);
                }
            }
            //Declear product id
            int id;
            //Init and save productDTO
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Product product = new Product();

                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CatagoryId = model.CatagoryId;

                Catagory CAT = db.Catagories.FirstOrDefault(x => x.Id == model.CatagoryId);
                product.CatagoryName = CAT.Name;
                db.Products.Add(product);
                db.SaveChanges();

                //Get the id
                id = product.Id;
            }
            //Set TempData message
            TempData["SM"] = "You have added a product!";

            #region Upload Image
            //create necessary dircetories
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);
            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);
            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);
            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);
            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            //Check if a file was uploaded
            if (file != null && file.ContentLength > 0)
            {
                //Get file extension
                string ext = file.ContentType.ToLower();
                //Verify extension
                if (ext != "image/jpg" &&
                   ext != "image/jpeg" &&
                   ext != "image/pjpeg" &&
                   ext != "image/x-png" &&
                   ext != "image/png")
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        model.Catagories = new SelectList(db.Catagories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                        return View(model);
                    }
                }
                //Init image name
                string imageName = file.FileName;
                //Save image name to product
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Product product = db.Products.Find(id);
                    product.ImageName = imageName;
                    db.SaveChanges();
                }
                // Set original and thumd image path
                var path = string.Format("{0}\\{1}", pathString2, imageName);
                var path2 = string.Format("{0}\\{1}", pathString3, imageName);

                // Save original
                file.SaveAs(path);
                // Create and save thumb
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }
            #endregion

            //Redirect

            return RedirectToAction("AddProduct");
        }
        // GET: Products
        //Get:Products/Products
        public ActionResult Products(int? page, int? catId)
        {
            //Declear a list of ProductVM
            List<Product> listOfProduct;

            //Set page number
            var pageNumber = page ?? 1;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //Init the list
                listOfProduct = db.Products.ToArray().Where(x => catId == null || catId == 0 || x.CatagoryId == catId)
                                    .Select(x => new Product(x)).ToList();

                //Populate catagories select list
                ViewBag.Catagories = new SelectList(db.Catagories.ToList(), "Id", "Name");

                // Set selected catagory
                ViewBag.SelectedCat = catId.ToString();
            }
            //Set Pagination
            var OnePageOfProducts = listOfProduct.ToPagedList(pageNumber, 3);
            ViewBag.OnePageOfProducts = OnePageOfProducts;

            //Return view with list 

            return View(listOfProduct);
        }
        
        //Get:Products/EditProduct/id
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult EditProduct(int id)
        {
            //Declear productVM
            Product model;
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                //Get the product
                Product product = db.Products.Find(id);
                //Make sure product exists
                if (product == null)
                {
                    return Content("That product does not exist.");
                }

                //Init model
                model = new Product(product);

                //Make a select list
                model.Catagories = new SelectList(db.Catagories.ToList(), "Id", "Name");

                //Get all gallery images
                model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                                               .Select(fn => Path.GetFileName(fn));
            }

            //return View

            return View(model);
        }
        //Post:Products/EditProduct/id
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditProduct(Product model, HttpPostedFileBase file)
        {
            // Get product id
            int id = model.Id;

            // Populate categories select list and gallery images
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                model.Catagories = new SelectList(db.Catagories.ToList(), "Id", "Name");
            }
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                            .Select(fn => Path.GetFileName(fn));

            //Check model state 
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //Make sure product name is unique
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                if (db.Products.Where(x => x.Id != id).Any(x => x.Name == model.Name))
                {
                    ModelState.AddModelError("", "That product name is taken!");
                    return View(model);
                }
            }
            //Update product
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Product product = db.Products.Find(id);
                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CatagoryId = model.CatagoryId;
                product.ImageName = model.ImageName;

                Catagory catDTO = db.Catagories.FirstOrDefault(x => x.Id == model.CatagoryId);
                product.CatagoryName = catDTO.Name;
                db.SaveChanges();
            }

            //Set TempData 
            TempData["SM"] = "You have edited the Product!";

            #region Image Upload
            //Check for file upload
            if (file != null && file.ContentLength > 0)
            {
                //Get extension
                string ext = file.ContentType.ToLower();
                // verify extension
                if (ext != "image/jpg" &&
                   ext != "image/jpeg" &&
                   ext != "image/pjpeg" &&
                   ext != "image/x-png" &&
                   ext != "image/png")
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        ModelState.AddModelError("", "The image was not uploaded - wrong image extension.");
                        return View(model);
                    }
                }

                //Set upload dircetories path
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");

                //Delete files from directories
                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);

                foreach (FileInfo file2 in di1.GetFiles())
                    file2.Delete();
                foreach (FileInfo file3 in di2.GetFiles())
                    file3.Delete();

                //Save image name
                string imageName = file.FileName;
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    Product product = db.Products.Find(id);
                    product.ImageName = imageName;
                    db.SaveChanges();
                }
                //Save original and thumb images
                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);

                file.SaveAs(path);
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);
            }
            #endregion

            //Redirect
            return RedirectToAction("EditProduct");
        }
        //Get:Products/DeleteProduct/id
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteProduct(int id)
        {
            //Delete product from DB
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                Product product = db.Products.Find(id);
                db.Products.Remove(product);
                db.SaveChanges();
            }

            //Delete product folder
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string pathString = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            if (Directory.Exists(pathString))
                Directory.Delete(pathString, true);

            //Redirect
            return RedirectToAction("Products");
        }
    }
}