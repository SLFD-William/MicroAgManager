﻿using DomainTests;
using Microsoft.EntityFrameworkCore;

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
            var result = await FarmLocationLogic.OnFarmLocationCreated(dbContext as DbContext, 1, new CancellationToken());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(8, result.Count);
            Assert.AreEqual("FarmLocationModel", result[0].ModelType);
        }

        [TestMethod()]
        public async Task OnFarmLocationCreatedTest_InvalidId_ThrowsException()
        {
            // Arrange
            var dbContext = new TestMicroAgManagementDbContext().CreateContext();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<Exception>(() => FarmLocationLogic.OnFarmLocationCreated(dbContext as DbContext, 1, new CancellationToken()));
        }

    }
}