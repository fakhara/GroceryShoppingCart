using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GroceryShoppingCart.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set;}
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalMoney { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }

    }
   
}