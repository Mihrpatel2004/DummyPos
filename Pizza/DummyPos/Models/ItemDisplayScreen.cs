using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Item_Display_Screen")]
public class ItemDisplayScreen
{
    [Key]
    public int Screen_No { get; set; }

    public int? Item_Id { get; set; }

    [ForeignKey("Item_Id")]
    public Items Item { get; set; }
}