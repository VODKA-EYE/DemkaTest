using System;
using System.Collections.Generic;

namespace DemkaTest.Models;

public partial class Company
{
    public int Companyid { get; set; }

    public string Companyname { get; set; } = null!;

    public virtual ICollection<Product> ProductProductdelivelerNavigations { get; set; } = new List<Product>();

    public virtual ICollection<Product> ProductProductmanufacturerNavigations { get; set; } = new List<Product>();
}
