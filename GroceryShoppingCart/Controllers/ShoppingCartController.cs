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
        string ShoppingCartId { get; set; }

        // GET: ShoppingCart
        
        public ActionResult Add(Product product)
        {
           
            product = db.Products.Find(product.Id);
            

            if (Session["cart"] == null)
            {
                List<Product> li = new List<Product>();
                li.Add(product);
                Session["cart"] = li;
                ViewBag.cart = li.Count();

                Session["count"] = 1;
            }
            else
            {
                List<Product> li = (List<Product>)Session["cart"];
                li.Add(product);
                Session["cart"] = li;
                ViewBag.cart = li.Count();
                Session["count"] = Convert.ToInt32(Session["count"]) + 1;
            }
            return RedirectToAction("Index", "Products");
        }
        public ActionResult MyCart()
        {

            return View((List<Product>)Session["cart"]);

        }

        public ActionResult Remove(Product product)
        {
            List<Product> li = (List<Product>)Session["cart"];
            li.RemoveAll(x => x.Id == product.Id);
            Session["cart"] = li;
            Session["count"] = Convert.ToInt32(Session["count"]) - 1;
            return RedirectToAction("Myorder", "ShoppingCart");
        }
        public List<Cart> GetCartitems()
        {
            return db.Carts.Where(cart => cart.CartId  == ShoppingCartId).ToList();
        }
        public Order CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            order.OrderDetails = new List<OrderDetail>();

            var cartItems = GetCartitems();
            foreach(var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    Price = item.Product.Price,
                    Quantity = item.Count
                };
                orderTotal += (item.Count * item.Product.Price);
                order.OrderDetails.Add(orderDetail);
                db.OrderDetails.Add(orderDetail);
            }
            //set the order's total to the orderTotal count
            order.TotalMoney = orderTotal;
            db.SaveChanges();
            return order;
        }
        
    }
}