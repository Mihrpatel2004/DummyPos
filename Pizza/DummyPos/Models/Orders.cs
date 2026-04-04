using DummyPos.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Orders")]
public class Orders
{
    [Key]
    public int Order_Id { get; set; }

    public int? Customer_Id { get; set; }

    [ForeignKey("Customer_Id")]
    public Customers Customer { get; set; }

    [Required]
    public int Staff_Id { get; set; }

    [ForeignKey("Staff_Id")]
    public Staffs Staff { get; set; }

    public DateTime? Order_Date { get; set; }

    public DateTime? Expected_Ready_Time { get; set; }

    [Required]
    public int Status_Id { get; set; }

    [ForeignKey("Status_Id")]
    public OrderStatus OrderStatus { get; set; }

    [Required]
    public int Source_Id { get; set; }

    [ForeignKey("Source_Id")]
    public OrderSource OrderSource { get; set; }

    [Required]
    public int Branch_Id { get; set; }

    [ForeignKey("Branch_Id")]
    public Branch Branch { get; set; }

    [Required]
    public int Service_Type_Id { get; set; }

    [ForeignKey("Service_Type_Id")]
    public ServiceType ServiceType { get; set; }

    public int? Table_Id { get; set; }

    [ForeignKey("Table_Id")]
    public RestaurantTable RestaurantTable { get; set; }

    // Navigation Properties

    public ICollection<OrderDetail> OrderDetails { get; set; }

    public ICollection<Payment> Payments { get; set; }

    public ICollection<FeedBack> Feedbacks { get; set; }
}