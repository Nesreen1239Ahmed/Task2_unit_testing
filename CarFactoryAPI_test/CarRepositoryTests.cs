using CarAPI.Entities;
using CarFactoryAPI.Entities;
using CarFactoryAPI.Repositories_DAL;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace CarFactoryAPI_test
{
    public class CarRepositoryTests
    {
        private Mock<FactoryContext> factoryContextMock;
        private CarRepository carRepository;

        public CarRepositoryTests()
        {
            // Create Mock of dependencies
            factoryContextMock = new Mock<FactoryContext>();

            // Use fake object as dependency
            carRepository = new CarRepository(factoryContextMock.Object);
        }

        [Fact]
        public void GetCarById_WhenCarExists_ReturnsCar()
        {
            // Arrange
            int carId = 10;
            Car expectedCar = new Car() { Id = carId };
            factoryContextMock.Setup(fcm => fcm.Cars.Find(carId)).Returns(expectedCar);

            // Act 
            Car car = carRepository.GetCarById(carId);

            // Assert
            Assert.NotNull(car);
            Assert.Equal(expectedCar, car);
        }

        [Fact]
        public void GetCarById_WhenCarDoesNotExist_ReturnsNull()
        {
            // Arrange
            int carId = 20; // Assuming car with ID 20 does not exist
            factoryContextMock.Setup(fcm => fcm.Cars.Find(carId)).Returns<Car>(null);

            // Act 
            Car car = carRepository.GetCarById(carId);

            // Assert
            Assert.Null(car);
        }
    }
}
