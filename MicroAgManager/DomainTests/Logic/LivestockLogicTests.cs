using DomainTests;
using Microsoft.EntityFrameworkCore;

namespace Domain.Logic.Tests
{
    [TestClass()]
    public class LivestockLogicTests
    {
        [TestMethod()]
        public async Task VerifyNoOpenBreedingRecordTest_ValidId_NoExceptionThrown()
        {
            // Arrange
            var context = new TestMicroAgManagementDbContext().CreateContext();
            var femaleId = new List<long> { 1 };
            var tenantId = Guid.NewGuid();
            var cancellationToken = new CancellationToken();

            // Act
            await LivestockLogic.VerifyNoOpenBreedingRecord(context as DbContext, femaleId, tenantId, cancellationToken);

            // Assert
            // No exception is thrown
        }

        [TestMethod()]
        public async Task OnBreedingRecordResolvedTest_ValidId_ReturnsModifiedEntities()
        {
            // Arrange
            var context = new TestMicroAgManagementDbContext().CreateContext();
            var breedingRecordId = 1;

            // Act
            var result = await LivestockLogic.OnBreedingRecordResolved(context as DbContext, breedingRecordId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod()]
        public async Task OnLivestockBredTest_ValidId_ReturnsModifiedEntities()
        {
            // Arrange
            var context = new TestMicroAgManagementDbContext().CreateContext();
            var breedingRecordId = 1;
            var source = "TestSource";
            var sourceId = 1L;
            var cancellationToken = new CancellationToken();

            // Act
            var result = await LivestockLogic.OnLivestockBred(context as DbContext, breedingRecordId, source, sourceId, cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

        [TestMethod()]
        public async Task OnLivestockBornTest_ValidId_ReturnsModifiedEntities()
        {
            // Arrange
            var context = new TestMicroAgManagementDbContext().CreateContext();
            var livestockId = 1L;
            var cancellationToken = new CancellationToken();

            // Act
            var result = await LivestockLogic.OnLivestockBorn(context as DbContext, livestockId, cancellationToken);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Any());
        }

    }
}