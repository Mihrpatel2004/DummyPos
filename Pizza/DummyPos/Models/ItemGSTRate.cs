using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Item_GST_Rate")]
public class ItemGSTRate
{
    [Key]
    public int IGR_Id { get; set; }

    [Required]
    public int Item_Id { get; set; }

    [ForeignKey("Item_Id")]
    public Items Item { get; set; }

    [Required]
    public int Service_Type_Id { get; set; }

    [ForeignKey("Service_Type_Id")]
    public ServiceType ServiceType { get; set; }

    [Required]
    public decimal GST_Rate { get; set; }
    public bool Is_Active { get; set; }

    public DateTime? Created_Date { get; set; }

    [StringLength(100)]
    public string Created_By { get; set; }

    public DateTime? Modified_Date { get; set; }

    [StringLength(100)]
    public string Modified_By { get; set; }
}