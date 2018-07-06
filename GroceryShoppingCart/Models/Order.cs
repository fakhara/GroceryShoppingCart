using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroceryShoppingCart.Models
{
    public class Order
    {   [Key]
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public string OrderName { get; set; }
        public string OrderType { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public decimal TotalMoney { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }

    }
}