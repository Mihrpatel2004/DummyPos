using System;
using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class ItemCategory
    {
        [Key]
        public int Item_Category_Id { get; set; }

        [Required(ErrorMessage = "Category Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters")]
        [Display(Name = "Category Description")]
        public string Item_Category_Desc { get; set; }

        [Display(Name = "Active Status")]
        public bool Is_Active { get; set; } = true;
    }
}