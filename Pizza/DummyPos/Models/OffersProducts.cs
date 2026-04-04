using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Offers_Product")]
public class OfferProducts
{
    [Key]
    public int Offer_Product_Id { get; set; }

    [Required]
    public int Offer_Id { get; set; }

    [ForeignKey("Offer_Id")]
    public Offers Offer { get; set; }

    [Required]
    public int Item_Id { get; set; }

    [ForeignKey("Item_Id")]
    public Items Item { get; set; }
}