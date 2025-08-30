using System.ComponentModel.DataAnnotations.Schema;
using Oikono.Domain.Idempotencies.ValueObjects;
using Oikono.Domain.Models;
using Oikono.Domain.Users;
using Oikono.Domain.Users.ValueObjects;

namespace Oikono.Domain.Idempotencies;

public class Idempotency : Entity<IdempotencyId>
{
    public Idempotency(IdempotencyId id, string requestName)
    {
        Id = id;
        RequestName = requestName;
    }

    public string RequestName { get; set; }


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
}