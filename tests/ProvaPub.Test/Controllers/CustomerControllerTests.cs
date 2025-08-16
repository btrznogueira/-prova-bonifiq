using NSubstitute;
using ProvaPub.Controllers;
using ProvaPub.Interfaces;
using FluentAssertions;

namespace ProvaPub.Test.Controllers
{
    [TestClass]
    public class CustomerControllerTests
    {
        private ICustomerService _mockCustomerService;
        private CustomerController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockCustomerService = Substitute.For<ICustomerService>();
            _controller = new CustomerController(_mockCustomerService);
        }

        [TestMethod]
        public async Task CanPurchase_ReturnsTrue_WhenServiceReturnsTrue()
        {
            // Arrange
            int customerId = 1;
            decimal purchaseValue = 50m;
            _mockCustomerService.CanPurchase(customerId, purchaseValue).Returns(Task.FromResult(true));

            // Act
            var result = await _controller.CanPurchase(customerId, purchaseValue);

            // Assert
            result.Should().BeTrue();
            await _mockCustomerService.Received(1).CanPurchase(customerId, purchaseValue);
        }

        [TestMethod]
        public async Task CanPurchase_ReturnsFalse_WhenServiceReturnsFalse()
        {
            // Arrange
            int customerId = 1;
            decimal purchaseValue = 200m;
            _mockCustomerService.CanPurchase(customerId, purchaseValue).Returns(Task.FromResult(false));

            // Act
            var result = await _controller.CanPurchase(customerId, purchaseValue);

            // Assert
            result.Should().BeFalse();
            await _mockCustomerService.Received(1).CanPurchase(customerId, purchaseValue);
        }

        [TestMethod]
        public async Task CanPurchase_ThrowsException_WhenServiceThrowsException()
        {
            // Arrange
            int customerId = -1;
            decimal purchaseValue = 50m;
            _mockCustomerService.CanPurchase(customerId, purchaseValue)
                .Returns<Task<bool>>(x => throw new ArgumentOutOfRangeException("customerId"));

            // Act
            Func<Task> act = async () => await _controller.CanPurchase(customerId, purchaseValue);

            // Assert
            await act.Should().ThrowAsync<ArgumentOutOfRangeException>();
            await _mockCustomerService.Received(1).CanPurchase(customerId, purchaseValue);
        }
    }
}
