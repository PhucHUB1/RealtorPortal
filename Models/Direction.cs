﻿using System;
using System.Collections.Generic;

namespace RealEstate.Models;

public partial class Direction
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
}
