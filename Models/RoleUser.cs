using System;
using System.Collections.Generic;

namespace RealEstate.Models;

public partial class RoleUser
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? RoleId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Role? Role { get; set; }

    public virtual User? User { get; set; }
}
