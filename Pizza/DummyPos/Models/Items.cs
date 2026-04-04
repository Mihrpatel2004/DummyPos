using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Items")]
public class Items
{
    [Key]
    public int Item_Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Item_Name { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public int Item_Category_Id { get; set; }

    [ForeignKey("Item_Category_Id")]
    public ItemCategory ItemCategory { get; set; }

    public int? Prepare_Time_Minutes { get; set; }

    public bool Is_Active { get; set; }

    public DateTime? Created_Date { get; set; }

    [StringLength(100)]
    public string Created_By { get; set; }

    public DateTime? Modified_Date { get; set; }

    [StringLength(100)]
    public string Modified_By { get; set; }
}