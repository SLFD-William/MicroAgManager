using Domain.Entity;
using DomainTests;

namespace Domain.Logic.Tests
{
    [TestClass()]
    public class LivestockAnimalLogicTests
    {
        [TestMethod()]
        public async Task OnLivestockAnimalCreatedTest_ValidId_ReturnsModifiedEntities()
        {
            // Arrange
            var context = new TestMicroAgManagementDbContext().CreateContext();
            var id = 2;
            var cancellationToken = new CancellationToken();
            var livestockAnimal = new LivestockAnimal(Guid.NewGuid(), Guid.NewGuid())
            {
                Id = id,
                Name = "Test Animal",
                Care = "Individual",
                GroupName = "Test Group",
                ParentFemaleName = "Mom",
                ParentMaleName = "Dad",

            };
            context.LivestockAnimals.Add(livestockAnimal);
            await context.SaveChangesAsync(cancellationToken);

            // Act
            var result = await LivestockAnimalLogic.OnLivestockAnimalCreated(context, id, cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count()==8);
        }

        [TestMethod()]
        public async Task OnLivestockAnimalCreatedTest_InvalidId_ThrowsException()
        {
            // Arrange
            var context = new TestMicroAgManagementDbContext().CreateContext();
            var id = 2;
            var cancellationToken = new CancellationToken();

            // Act and Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => LivestockAnimalLogic.OnLivestockAnimalCreated(context, id, cancellationToken));
        }
    }
}