using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GroceryShoppingCart.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public int CartId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual Product Product { get; set; }
    }
}