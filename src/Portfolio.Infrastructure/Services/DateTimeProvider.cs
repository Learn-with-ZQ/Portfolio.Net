using Portfolio.Application.Common.Interfaces;

namespace Portfolio.Infrastructure.Services;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
