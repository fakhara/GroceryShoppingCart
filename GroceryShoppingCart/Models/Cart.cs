using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GroceryShoppingCart.Models
{
    public class Cart
    {
        [Key]
        public int ID { get; set; }
        public String CartId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = true, ErrorMessage = " ")]
        [Range(0, 100, ErrorMessage = "Quantity must be between 0 and 100")]
        [DisplayName("Quantity")]
        public int Count { get; set; }
        public System.DateTime DateCreated { get; set; }
        public virtual Product Product { get; set; }
    }
}