using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class KitchenStation
    {
        [Key]
        public int Station_Id { get; set; }

        [Required(ErrorMessage = "Station Name is required")]
        [Display(Name = "Station Name")]
        public string? Station_Name { get; set; }

        public int Branch_Id { get; set; }

        [Display(Name = "Active Status")]
        public bool Is_Active { get; set; } = true;

        // For displaying in lists
        public string? Branch_Name { get; set; }
    }
}