using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace tsc.RestaurantBookingApp.Core;

public partial class DiningTable
{
    [Key]
    public int Id { get; set; }

    public int RestaurantBranchId { get; set; }

    [StringLength(100)]
    public string? TableName { get; set; }

    public int Capacity { get; set; }

    [ForeignKey("RestaurantBranchId")]
    [InverseProperty("DiningTables")]
    public virtual RestaurantBranch RestaurantBranch { get; set; } = null!;

    [InverseProperty("DiningTable")]
    public virtual ICollection<TimeSlot> TimeSlots { get; set; } = new List<TimeSlot>();
}
