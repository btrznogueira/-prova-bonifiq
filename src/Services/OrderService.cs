using ProvaPub.Enums;
using ProvaPub.Interfaces;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class OrderService : IOrderService
    {
        TestDbContext _ctx;
        private readonly Dictionary<string, Func<decimal, int, Task>> _paymentStrategies;

        public OrderService(TestDbContext ctx)
        {
            _ctx = ctx;

            _paymentStrategies = new Dictionary<string, Func<decimal, int, Task>>(StringComparer.OrdinalIgnoreCase)
            {
                { PaymentMethod.PIX, PaymentPix },
                { PaymentMethod.CREDIT_CARD, PaymentCreditCard },
                { PaymentMethod.PAYPAL, PaymentPaypal }
            };
        }

        public async Task<Order> PayOrder(string paymentMethod, decimal paymentValue, int customerId)
        {

            if (!_paymentStrategies.TryGetValue(paymentMethod, out var payFunc))
                throw new ArgumentException("Método de pagamento inválido");

            await payFunc(paymentValue, customerId);

            var order = new Order
            {
                Value = paymentValue,
                OrderDate = DateTime.UtcNow
            };

            _ctx.Orders.Add(order);
            await _ctx.SaveChangesAsync();

            return order;
        }

        private Task PaymentPix(decimal amount, int customerId)
        {
            //Faz pagamento...
            return Task.CompletedTask;
        }

        private Task PaymentCreditCard(decimal amount, int customerId)
        {
            //Faz pagamento...
            return Task.CompletedTask;
        }

        private Task PaymentPaypal(decimal amount, int customerId)
        {
            //Faz pagamento...
            return Task.CompletedTask;
        }
    }
}
