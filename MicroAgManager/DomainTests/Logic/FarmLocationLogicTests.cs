using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainTests;

namespace Domain.Logic.Tests
{
    [TestClass()]
    public class FarmLocationLogicTests
    {
        [TestMethod()]
        public async Task OnFarmLocationCreatedTest_ValidId_ReturnsModifiedEntities()
        {

            // Arrange

            var dbContext = new TestMicroAgManagementDbContext().CreateContext();
            var Id = Guid.NewGuid();
            var farmLocation = new Entity.FarmLocation(Id, Id) { Id = 1,Name="Test Farm"};
            dbContext.Farms.Add(farmLocation);
            dbContext.SaveChanges();

            // Act
            var result = await FarmLocationLogic.OnFarmLocationCreated(dbContext, 1, new CancellationToken());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.Count);
            Assert.AreEqual("FarmLocation", result[0].EntityName);
            Assert.AreEqual("Created", result[0].Modification);
        }

        [TestMethod()]
        public async Task OnFarmLocationCreatedTest_InvalidId_ThrowsException()
        {
            // Arrange
            var dbContext = new TestMicroAgManagementDbContext().CreateContext();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => FarmLocationLogic.OnFarmLocationCreated(dbContext, 1, new CancellationToken()));
        }

    }
}