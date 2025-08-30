using System.ComponentModel.DataAnnotations.Schema;
using Oikono.Domain.Models;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Domain.Users;

public class Permission : Entity<PermissionId>
{
    private readonly List<UserPermission> _userPermissions = new();

    public Permission(string feature, string name)
    {
        Feature = feature;
        Name = name;
    }

    public string Feature { get; set; } = null!;
    public string Name { get; private set; } = default!;


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

    public IReadOnlyList<UserPermission> UserPermissions => _userPermissions.AsReadOnly();
}