using System;
using System.Collections.Generic;

namespace DemkaTest.Models;

public partial class OrderPickupPoint
{
    public int Orderpickuppointid { get; set; }

    public string Orderpickuppointname { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
