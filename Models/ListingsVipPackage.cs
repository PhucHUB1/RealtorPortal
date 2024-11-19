using System;
using System.Collections.Generic;

namespace RealEstate.Models;

public partial class ListingsVipPackage
{
    public int VipPackageId { get; set; }

    public string VipPackageName { get; set; } = null!;

    public int DurationDays { get; set; }

    public decimal Price { get; set; }

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
