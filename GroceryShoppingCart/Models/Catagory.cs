using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GroceryShoppingCart.Models
{
    public class Catagory
    {
        public Catagory() {}
        public Catagory(Catagory row)
        {
            Id = row.Id;
            Name = row.Name;
            Sorting = row.Sorting;

        }
        [Key]
        [DisplayName("Catagorie ID")]
        public int Id { get; set; }
        [Required]
        [DisplayName("Catagorie")]
        [MaxLength(50)]
        public string Name { get; set; }
        public int Sorting { get; set; }

        public virtual ICollection<Product> Products{ get; set; }
    }
}