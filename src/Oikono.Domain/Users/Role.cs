﻿using System.ComponentModel.DataAnnotations.Schema;
using Oikono.Domain.Models;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Domain.Users;

public class Role : Entity<RoleId>
{
    public Role(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    [NotMapped]
    [Obsolete("Because of generic AggregateRoot")]
    public override UserId CreatedBy { get; set; } = null!;

    [NotMapped]
    [Obsolete("Because of generic AggregateRoot")]
    public override UserId UpdatedBy { get; set; } = null!;


    [NotMapped]
    [Obsolete("Because of generic AggregateRoot")]
    public override User CreatedByUser { get; set; } = null!;

    [NotMapped]
    [Obsolete("Because of generic AggregateRoot")]
    public override User UpdatedByUser { get; set; } = null!;

    public virtual IEnumerable<UserRole> UserRoles { get; set; } = null!;
}