using System;
using System.Collections.Generic;

namespace DemkaTest.Models;

public partial class OrderStatus
{
    public int Statusid { get; set; }

    public string Statusname { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
