using DummyPos.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Kitchen_Station")]
public class KitchenStation
{
    [Key]
    public int Station_Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Station_Name { get; set; }

    [Required]
    public int Screen_No { get; set; }

    [ForeignKey("Screen_No")]
    public ItemDisplayScreen ItemDisplayScreen { get; set; }

    [Required]
    public int Branch_Id { get; set; }

    [ForeignKey("Branch_Id")]
    public Branch Branch { get; set; }

    public bool Is_Active { get; set; }

    public DateTime? Created_Date { get; set; }

    [StringLength(100)]
    public string Created_By { get; set; }

    public DateTime? Modified_Date { get; set; }

    [StringLength(100)]
    public string Modified_By { get; set; }
}