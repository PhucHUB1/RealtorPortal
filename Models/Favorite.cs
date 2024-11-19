using System;
using System.Collections.Generic;

namespace RealEstate.Models;

public partial class Favorite
{
    public int FavoriteId { get; set; }

    public int? UserId { get; set; }

    public int? ListingId { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual Listing? Listing { get; set; }

    public virtual User? User { get; set; }
}
