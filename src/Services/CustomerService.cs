using Microsoft.EntityFrameworkCore;
using ProvaPub.Commons;
using ProvaPub.Interfaces;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly TestDbContext _ctx;
        private readonly IDateTimeService _dateTimeService;

        public CustomerService(TestDbContext ctx, IDateTimeService dateTimeService)
        {
            _ctx = ctx;
            _dateTimeService = dateTimeService;
        }

        public PagedResult<Customer> ListCustomers(int page)
        {
            return _ctx.Customers
                .OrderBy(c => c.Id)
                .ToPagedResult(page, 10);
        }

        public async Task<bool> CanPurchase(int customerId, decimal purchaseValue)
        {
            if (customerId <= 0) throw new ArgumentOutOfRangeException(nameof(customerId));

            if (purchaseValue <= 0) throw new ArgumentOutOfRangeException(nameof(purchaseValue));

            //Business Rule: Non registered Customers cannot purchase
            var customer = await _ctx.Customers.FindAsync(customerId);
            if (customer == null) throw new InvalidOperationException($"Customer Id {customerId} does not exists");

            //Business Rule: A customer can purchase only a single time per month
            var baseDate = _dateTimeService.UtcNow.AddMonths(-1);
            var ordersInThisMonth = await _ctx.Orders.CountAsync(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
            if (ordersInThisMonth > 0)
                return false;

            //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
            var haveBoughtBefore = await _ctx.Customers.CountAsync(s => s.Id == customerId && s.Orders.Any());
            if (haveBoughtBefore == 0 && purchaseValue > 100)
                return false;

            //Business Rule: A customer can purchases only during business hours and working days
            if (_dateTimeService.UtcNow.Hour < 8 || _dateTimeService.UtcNow.Hour > 18 || _dateTimeService.UtcNow.DayOfWeek == DayOfWeek.Saturday || _dateTimeService.UtcNow.DayOfWeek == DayOfWeek.Sunday)
                return false;


            return true;
        }

    }
}
