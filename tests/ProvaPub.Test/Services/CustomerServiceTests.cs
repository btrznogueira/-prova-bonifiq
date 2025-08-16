using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using ProvaPub.Interfaces;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;
using ProvaPub.Test.Helper;

namespace ProvaPub.Test.Services
{
    [TestClass]
    public class CustomerServiceTests
    {
        private TestDbContext _mockContext;
        private IDateTimeService _mockDateTimeService;
        private CustomerService _customerService;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _mockContext = new TestDbContext(options);
            _mockDateTimeService = Substitute.For<IDateTimeService>();
            _customerService = new CustomerService(_mockContext, _mockDateTimeService);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockContext?.Dispose();
        }

        [TestMethod]
        public async Task ListCustomers_ReturnsPagedResultOrderedById()
        {
            // Arrange
            _mockContext.Customers.AddRange(
                new Customer { Id = 3, Name = "Cliente C" },
                new Customer { Id = 1, Name = "Cliente A" },
                new Customer { Id = 2, Name = "Cliente B" }
            );
            await _mockContext.SaveChangesAsync();

            // Act
            var result = _customerService.ListCustomers(1);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(3); 
            result.Items.Select(c => c.Id).Should().ContainInOrder(1, 2, 3); 
        }


        [TestMethod]
        public async Task CanPurchase_WithInvalidCustomerId_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var invalidCustomerId = 0;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
                () => _customerService.CanPurchase(invalidCustomerId, 50m));
        }

        [TestMethod]
        public async Task CanPurchase_FirstPurchaseAboveLimit_ReturnsFalse()
        {
            // Arrange
            await CustomerAddTest.AddCustomerAsync(_mockContext);
            decimal purchaseValue = 150m; 
            _mockDateTimeService.UtcNow.Returns(new DateTime(2024, 1, 15, 10, 0, 0)); 

            // Act
            var result = await _customerService.CanPurchase(1, purchaseValue);

            // Assert
            result.Should().BeFalse();
        }


        [TestMethod]
        public async Task CanPurchase_WithInvalidPurchaseValue_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            await CustomerAddTest.AddCustomerAsync(_mockContext);
            var invalidPurchaseValue = 0m;

            // Act & Assert
            await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(
                () => _customerService.CanPurchase(1, invalidPurchaseValue));
        }


        [TestMethod]
        public async Task CanPurchase_WithValidCustomerAndBusinessHours_ReturnsTrue()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockContext.Customers.Add(customer);
            await _mockContext.SaveChangesAsync();

            _mockDateTimeService.UtcNow.Returns(new DateTime(2024, 1, 15, 10, 0, 0)); 

            // Act
            var result = await _customerService.CanPurchase(1, 100.0m);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public async Task CanPurchase_WithNonExistentCustomer_ThrowsInvalidOperationException()
        {
            // Arrange
            var nonExistentCustomerId = 999;

            // Act & Assert
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                () => _customerService.CanPurchase(nonExistentCustomerId, 100.0m));

            exception.Message.Should().Contain("does not exists");
        }

        [TestMethod]
        public async Task CanPurchase_WithCustomerHavingOrderInCurrentMonth_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockContext.Customers.Add(customer);

            // Adicionar pedido no mês atual
            var order = new Order
            {
                Id = 1,
                CustomerId = 1,
                OrderDate = DateTime.UtcNow.AddDays(-5), 
                Value = 50.0m
            };
            _mockContext.Orders.Add(order);
            await _mockContext.SaveChangesAsync();

            _mockDateTimeService.UtcNow.Returns(new DateTime(2024, 1, 15, 10, 0, 0));

            // Act
            var result = await _customerService.CanPurchase(1, 100.0m);

            // Assert
            result.Should().BeFalse();
        }


        [TestMethod]
        public async Task CanPurchase_WithPurchaseValueExceedingFirstTimeLimit_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockContext.Customers.Add(customer);
            await _mockContext.SaveChangesAsync();

            _mockDateTimeService.UtcNow.Returns(new DateTime(2024, 1, 15, 10, 0, 0));

            var result = await _customerService.CanPurchase(1, 150.0m);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public async Task CanPurchase_WithPurchaseValueWithinFirstTimeLimit_ReturnsTrue()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockContext.Customers.Add(customer);
            await _mockContext.SaveChangesAsync();

            _mockDateTimeService.UtcNow.Returns(new DateTime(2024, 1, 15, 10, 0, 0));

            var result = await _customerService.CanPurchase(1, 50.0m);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public async Task CanPurchase_BeforeBusinessHours_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockContext.Customers.Add(customer);
            await _mockContext.SaveChangesAsync();

            _mockDateTimeService.UtcNow.Returns(new DateTime(2024, 1, 15, 7, 0, 0));

            // Act
            var result = await _customerService.CanPurchase(1, 50.0m);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public async Task CanPurchase_AfterBusinessHours_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockContext.Customers.Add(customer);
            await _mockContext.SaveChangesAsync();

            _mockDateTimeService.UtcNow.Returns(new DateTime(2024, 1, 15, 19, 0, 0));

            // Act
            var result = await _customerService.CanPurchase(1, 50.0m);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public async Task CanPurchase_OnSaturday_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockContext.Customers.Add(customer);
            await _mockContext.SaveChangesAsync();

            _mockDateTimeService.UtcNow.Returns(new DateTime(2024, 1, 13, 10, 0, 0)); 

            // Act
            var result = await _customerService.CanPurchase(1, 50.0m);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public async Task CanPurchase_OnSunday_ReturnsFalse()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockContext.Customers.Add(customer);
            await _mockContext.SaveChangesAsync();

            _mockDateTimeService.UtcNow.Returns(new DateTime(2024, 1, 14, 10, 0, 0)); 

            // Act
            var result = await _customerService.CanPurchase(1, 50.0m);

            // Assert
            result.Should().BeFalse();
        }

        [TestMethod]
        public async Task CanPurchase_OnWeekdayDuringBusinessHours_ReturnsTrue()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockContext.Customers.Add(customer);
            await _mockContext.SaveChangesAsync();

            _mockDateTimeService.UtcNow.Returns(new DateTime(2024, 1, 16, 14, 30, 0));

            // Act
            var result = await _customerService.CanPurchase(1, 50.0m);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public async Task CanPurchase_AtBusinessHoursBoundary_ReturnsTrue()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockContext.Customers.Add(customer);
            await _mockContext.SaveChangesAsync();

            _mockDateTimeService.UtcNow.Returns(new DateTime(2024, 1, 15, 8, 0, 0));

            // Act
            var result = await _customerService.CanPurchase(1, 50.0m);

            // Assert
            result.Should().BeTrue();
        }

        [TestMethod]
        public async Task CanPurchase_AtBusinessHoursEndBoundary_ReturnsTrue()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Test Customer" };
            _mockContext.Customers.Add(customer);
            await _mockContext.SaveChangesAsync();

            _mockDateTimeService.UtcNow.Returns(new DateTime(2024, 1, 15, 18, 0, 0));

            // Act
            var result = await _customerService.CanPurchase(1, 50.0m);

            // Assert
            result.Should().BeTrue();
        }

    }
}

