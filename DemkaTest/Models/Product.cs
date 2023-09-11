using System;
using System.Collections.Generic;

namespace DemkaTest.Models;

public partial class Product
{
    public string Productarticlenumber { get; set; } = null!;

    public string Productname { get; set; } = null!;

    public string Productdescription { get; set; } = null!;

    public int Productcategory { get; set; }

    public byte[] Productphoto { get; set; } = null!;

    public int Productmanufacturer { get; set; }

    public int Productdeliveler { get; set; }

    public decimal Productcost { get; set; }

    public short? Productdiscountamount { get; set; }

    public int Productquantityinstock { get; set; }

    public virtual ICollection<OrderedProduct> OrderedProducts { get; set; } = new List<OrderedProduct>();

    public virtual ProductCategory ProductcategoryNavigation { get; set; } = null!;

    public virtual Company ProductdelivelerNavigation { get; set; } = null!;

    public virtual Company ProductmanufacturerNavigation { get; set; } = null!;
}
