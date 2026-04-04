using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Offers")]
public class Offers
{
    [Key]
    public int Offer_Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Offer_Name { get; set; }

    [Required]
    public int Offer_Type_Id { get; set; }

    [ForeignKey("Offer_Type_Id")]
    public OfferType OfferType { get; set; }

    [Required]
    public decimal Offer_Value { get; set; }

    [Required]
    public DateTime Start_Time { get; set; }

    [Required]
    public DateTime End_Time { get; set; }

    public string Is_Active { get; set; }
}