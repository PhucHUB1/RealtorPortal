using System;
using System.Collections.Generic;

namespace RealEstate.Models;

public partial class Image
{
    public int Id { get; set; }

    public int ListingId { get; set; }

    public string Url { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Listing Listing { get; set; } = null!;
}
