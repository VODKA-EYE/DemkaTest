using System;
using System.Collections.Generic;

namespace DemkaTest.Models;

public partial class OrderedProduct
{
    public int Orderid { get; set; }

    public string Productarticlenumber { get; set; } = null!;

    public int Productamount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product ProductarticlenumberNavigation { get; set; } = null!;
}
