using CarAPI.Entities;
using CarFactoryAPI.Entities;
using CarFactoryAPI.Repositories_DAL;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace CarFactoryAPI_test
{
    public class OwnerRepositoryTests
    {
        private Mock<FactoryContext> factoryContextMock;
        private OwnerRepository ownerRepository;

        public OwnerRepositoryTests()
        {
            // Create Mock of dependencies
            factoryContextMock = new Mock<FactoryContext>();

            // Use fake object as dependency
            ownerRepository = new OwnerRepository(factoryContextMock.Object);
        }

        [Fact]
        public void GetOwnerById_WhenOwnerExists_ReturnsOwner()
        {
            // Arrange
            int ownerId = 10;
            Owner expectedOwner = new Owner() { Id = ownerId };
            List<Owner> owners = new List<Owner>() { expectedOwner };
            factoryContextMock.Setup(fcm => fcm.Owners).ReturnsDbSet(owners);

            // Act 
            Owner owner = ownerRepository.GetOwnerById(ownerId);

            // Assert
            Assert.NotNull(owner);
            Assert.Equal(expectedOwner, owner);
        }

        [Fact]
        public void GetOwnerById_WhenOwnerDoesNotExist_ReturnsNull()
        {
            // Arrange
            int ownerId = 20; // Assuming owner with ID 20 does not exist
            factoryContextMock.Setup(fcm => fcm.Owners.Find(ownerId)).Returns<Owner>(null);

            // Act 
            Owner owner = ownerRepository.GetOwnerById(ownerId);

            // Assert
            Assert.Null(owner);
        }
    }
}
