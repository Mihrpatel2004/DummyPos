using System;
using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class Toppings
    {
        [Key]
        public int Topping_Id { get; set; }

        [Required(ErrorMessage = "Topping Name is required")]
        [Display(Name = "Topping Name")]
        public string Topping_Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 10000.00, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Display(Name = "Active Status")]
        public bool Is_Active { get; set; } = true;
    }
}
