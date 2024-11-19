using System;
using System.Collections.Generic;

namespace RealEstate.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? UserId { get; set; }

    public int? ListingId { get; set; }

    public int? PackageId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public decimal Amount { get; set; }

    public virtual Listing? Listing { get; set; }

    public virtual User? User { get; set; }
}
