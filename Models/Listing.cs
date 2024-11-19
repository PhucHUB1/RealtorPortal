using System;
using System.Collections.Generic;

namespace RealEstate.Models;

public partial class Listing
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int PropertyTypeId { get; set; }

    public decimal Price { get; set; }

    public double Area { get; set; }

    public string? Address { get; set; }

    public int? JuridicalId { get; set; }

    public int? DirectionId { get; set; }

    public int CityId { get; set; }

    public int DistrictId { get; set; }

    public int? WardId { get; set; }

    public int? VipPackageId { get; set; }

    public DateOnly? VipExpiryDate { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual City City { get; set; } = null!;

    public virtual Direction? Direction { get; set; }

    public virtual District District { get; set; } = null!;

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual Juridical? Juridical { get; set; }

    public virtual PropertyType PropertyType { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User? User { get; set; }

    public virtual ListingsVipPackage? VipPackage { get; set; }

    public virtual Ward? Ward { get; set; }
}
