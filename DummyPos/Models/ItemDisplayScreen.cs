using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class ItemDisplayScreen
    {
        [Key]
        public int Screen_No { get; set; }

        public int Station_Id { get; set; }

        [Required(ErrorMessage = "Please select an Item to display")]
        [Display(Name = "Display Item")]
        public int Item_Id { get; set; }

        [Display(Name = "Active Status")]
        public bool Is_Active { get; set; } = true;

        // For displaying in lists
        public string? Item_Name { get; set; }
    }
}