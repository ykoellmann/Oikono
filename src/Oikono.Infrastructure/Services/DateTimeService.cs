using Oikono.Application.Common.Interfaces.Services;

namespace Oikono.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
}