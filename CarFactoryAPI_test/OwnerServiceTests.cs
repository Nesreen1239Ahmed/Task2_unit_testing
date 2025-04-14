using CarAPI.Entities;
using CarAPI.Models;
using CarAPI.Payment;
using CarAPI.Repositories_DAL;
using CarAPI.Services_BLL;
using CarFactoryAPI.Entities;
using CarFactoryAPI.Repositories_DAL;
using Moq;
using Xunit;

namespace CarFactoryAPI_test
{
    public class OwnerServiceTests
    {
        private readonly Mock<ICarsRepository> carRepoMock;
        private readonly Mock<IOwnersRepository> ownersRepoMock;
        private readonly Mock<ICashService> cashServiceMock;
        private readonly OwnersService ownersService;

        public OwnerServiceTests()
        {
            // Create Mock of the dependencies
            carRepoMock = new();
            ownersRepoMock = new();
            cashServiceMock = new();

            // Use fake object as dependency
            ownersService = new OwnersService(carRepoMock.Object, ownersRepoMock.Object, cashServiceMock.Object);
        }

        [Fact]
        public void BuyCar_CarDoesNotExist_ReturnsError()
        {
            // Arrange
            carRepoMock.Setup(cm => cm.GetCarById(10)).Returns<Car>(null);

            BuyCarInput buyCarInput = new BuyCarInput() { CarId = 10, OwnerId = 100, Amount = 5000 };

            // Act 
            string result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("Car not found", result);
        }

        [Fact]
        public void BuyCar_OwnerDoesNotExist_ReturnsError()
        {
            // Arrange
            Car car = new Car() { Id = 5 };

            carRepoMock.Setup(cm => cm.GetCarById(5)).Returns(car);
            ownersRepoMock.Setup(om => om.GetOwnerById(100)).Returns<Owner>(null);

            BuyCarInput buyCarInput = new BuyCarInput() { CarId = 5, OwnerId = 100, Amount = 5000 };

            // Act 
            string result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("Owner not found", result);
        }

        [Fact]
        public void BuyCar_CarAlreadyOwned_ReturnsError()
        {
            // Arrange
            Car car = new Car() { Id = 10, Owner = new Owner() { Id = 100 } };

            carRepoMock.Setup(cm => cm.GetCarById(10)).Returns(car);

            BuyCarInput buyCarInput = new BuyCarInput() { CarId = 10, OwnerId = 200, Amount = 5000 };

            // Act 
            string result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("Already owned by another owner", result);
        }

        [Fact]
        public void BuyCar_PaymentFails_ReturnsError()
        {
            // Arrange
            Car car = new Car() { Id = 5, Price = 5000 };
            Owner owner = new Owner() { Id = 100, Balance = 10000 };

            carRepoMock.Setup(cm => cm.GetCarById(5)).Returns(car);
            ownersRepoMock.Setup(om => om.GetOwnerById(100)).Returns(owner);
            cashServiceMock.Setup(cs => cs.Pay(5000)).Returns("Failed");

            BuyCarInput buyCarInput = new BuyCarInput() { CarId = 5, OwnerId = 100, Amount = 5000 };

            // Act 
            string result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("Payment failed", result);
        }

        [Fact]
        public void BuyCar_AssignmentFails_ReturnsError()
        {
            // Arrange
            Car car = new Car() { Id = 5, Price = 5000 };
            Owner owner = new Owner() { Id = 100, Balance = 10000 };

            carRepoMock.Setup(cm => cm.GetCarById(5)).Returns(car);
            ownersRepoMock.Setup(om => om.GetOwnerById(100)).Returns(owner);
            cashServiceMock.Setup(cs => cs.Pay(5000)).Returns("Success");
            carRepoMock.Setup(cm => cm.AssignToOwner(5, 100)).Returns(false);

            BuyCarInput buyCarInput = new BuyCarInput() { CarId = 5, OwnerId = 100, Amount = 5000 };

            // Act 
            string result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("Assignment failed", result);
        }

        [Fact]
        public void BuyCar_SuccessfulPurchase_ReturnsSuccessMessage()
        {
            // Arrange
            Car car = new Car() { Id = 5, Price = 5000 };
            Owner owner = new Owner() { Id = 100, Balance = 10000 };

            carRepoMock.Setup(cm => cm.GetCarById(5)).Returns(car);
            ownersRepoMock.Setup(om => om.GetOwnerById(100)).Returns(owner);
            cashServiceMock.Setup(cs => cs.Pay(5000)).Returns("Success");
            carRepoMock.Setup(cm => cm.AssignToOwner(5, 100)).Returns(true);

            BuyCarInput buyCarInput = new BuyCarInput() { CarId = 5, OwnerId = 100, Amount = 5000 };

            // Act 
            string result = ownersService.BuyCar(buyCarInput);

            // Assert
            Assert.Contains("Successful purchase", result);
            Assert.Contains($"Car of Id: {buyCarInput.CarId} is bought by Owner Id: {owner.Id}", result);
            Assert.Contains("Payment successful", result);
        }

     

    }
}
