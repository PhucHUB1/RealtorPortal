using System;
using System.Collections.Generic;

namespace RealEstate.Models;

public partial class City
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<District> Districts { get; set; } = new List<District>();

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
