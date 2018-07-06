using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroceryShoppingCart.Models
{
    public class Product
    {
        public Product() {}
        public Product(Product row)
        {
            Id = row.Id;
            Name = row.Name;
            Description = row.Description;
            Price = row.Price;
            CatagoryName = row.CatagoryName;
            CatagoryId = row.CatagoryId;
            ImageName = row.ImageName;
        }

        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "A Product Name is required")]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999.99, ErrorMessage = "Price must be between 0.01 and 999.99")]
        public decimal Price { get; set; }
        public string CatagoryName { get; set; }
        [Required]
        public int CatagoryId { get; set; }
        public string ImageName { get; set; }

        public IEnumerable<SelectListItem> Catagories { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
        

    }
}