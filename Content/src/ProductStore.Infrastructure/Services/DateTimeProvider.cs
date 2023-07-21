using ProductStore.Application.Interfaces.Services;

namespace ProductStore.Infrastructure.Services;

public class DatetimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
