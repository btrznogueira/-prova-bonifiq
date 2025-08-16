using ProvaPub.Interfaces;

namespace ProvaPub.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
