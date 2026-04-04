using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Restaurant_Table")]
public class RestaurantTable
{
    [Key]
    public int Table_Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Table_Desc { get; set; }

    [Required]
    public int Size { get; set; }

    [StringLength(20)]
    public string Status { get; set; }
}