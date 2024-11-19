using System;
using System.Collections.Generic;

namespace RealEstate.Models;

public partial class District
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CityId { get; set; }

    public virtual City? City { get; set; }

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();

    public virtual ICollection<Ward> Wards { get; set; } = new List<Ward>();
}
