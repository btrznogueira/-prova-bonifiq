using ProvaPub.Models;
using ProvaPub.Repository; 
namespace ProvaPub.Test.Helper
{
    internal static class CustomerAddTest
    {
        public static async Task<Customer> AddCustomerAsync(TestDbContext context, int id = 1, string name = "Test Customer")
        {
            var customer = new Customer { Id = id, Name = name };
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
            return customer;
        }
    }
}
