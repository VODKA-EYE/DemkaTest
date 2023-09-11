using System;
using System.Collections.Generic;

namespace DemkaTest.Models;

public partial class ProductCategory
{
    public int Productcategoryid { get; set; }

    public string Productcategoryname { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
