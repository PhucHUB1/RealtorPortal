using System;
using System.Collections.Generic;

namespace RealEstate.Models;

public partial class Ward
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? DistrictId { get; set; }

    public virtual District? District { get; set; }

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
