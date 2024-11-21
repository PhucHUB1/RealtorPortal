using System;
using System.Collections.Generic;

namespace RealEstate.Models;

public partial class Role
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<RoleUser> RoleUsers { get; set; } = new List<RoleUser>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
