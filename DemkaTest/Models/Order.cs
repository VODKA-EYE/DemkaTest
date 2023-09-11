using System;
using System.Collections.Generic;

namespace DemkaTest.Models;

public partial class Order
{
    public int Orderid { get; set; }

    public int Orderstatus { get; set; }

    public DateTime Orderdate { get; set; }

    public DateTime Orderdeliverydate { get; set; }

    public int Orderpickuppoint { get; set; }

    public int? Orderclient { get; set; }

    public int Orderpickupcode { get; set; }

    public virtual User? OrderclientNavigation { get; set; }

    public virtual ICollection<OrderedProduct> OrderedProducts { get; set; } = new List<OrderedProduct>();

    public virtual OrderPickupPoint OrderpickuppointNavigation { get; set; } = null!;

    public virtual OrderStatus OrderstatusNavigation { get; set; } = null!;
}
