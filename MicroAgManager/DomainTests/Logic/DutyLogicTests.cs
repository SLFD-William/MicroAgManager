using Domain.Constants;
using Domain.Models;

namespace Domain.Logic.Tests
{
    [TestClass()]
    public class DutyLogicTests
    {
        [TestMethod()]
        public async Task GetNextChoreDueDateTest_ScheduledDutyNotChore()
        {
            // Arrange
            var context = new TestFrontEndDbContext().CreateContext();
            var scheduledDuty = new ScheduledDutyModel { Id = 1 , ScheduleSource=ScheduledDutySourceConstants.Event};
            
            // Act
            var result = await DutyLogic.GetNextChoreDueDate(context, scheduledDuty);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public async Task GetNextChoreDueDateTest_ChoreNotFound()
        {
            // Arrange
            var context = new TestFrontEndDbContext().CreateContext();
             var scheduledDuty = new ScheduledDutyModel { Id = 1, ScheduleSourceId=2, ScheduleSource = ScheduledDutySourceConstants.Chore, CompletedOn=DateTime.Now };

            // Act
            var result = await DutyLogic.GetNextChoreDueDate(context, scheduledDuty);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public async Task GetNextChoreDueDateTest_ValidChore_OnceADay()
        {
            // Arrange
            var context = new TestFrontEndDbContext().CreateContext();
  
            var completedOn = DateTime.Now;
            var chore = new ChoreModel {
                RecipientTypeId =1,
                RecipientType="LivestockAnimal",
                Name ="Test Chore",     
                Color  = "transparent",
                DueByTime=TimeSpan.FromHours(9), 
                PerScalar=1,
                PerUnitId=1, 
                EveryScalar=1, 
                EveryUnitId=1  
            };
            context.Chores.Add(chore);
            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel
            {
                Id = 1,
                ScheduleSourceId = chore.Id,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                CompletedOn = completedOn
            };
            // Act
            var result = await DutyLogic.GetNextChoreDueDate(context, scheduledDuty);

            // Assert
            Assert.AreEqual(completedOn.AddDays(1).Date+ chore.DueByTime, result);
         }
        [TestMethod()]
        public async Task GetNextChoreDueDateTest_ValidChore_TwiceADayOneOfTwo()
        {
            // Arrange
            var context = new TestFrontEndDbContext().CreateContext();

            var completedOn = DateTime.Now;
            var chore = new ChoreModel
            {
                RecipientTypeId = 1,
                RecipientType = "LivestockAnimal",
                Name = "Test Chore",
                Color = "transparent",
                DueByTime = TimeSpan.FromHours(9),
                PerScalar = 2,
                PerUnitId = 1,
                EveryScalar = 1,
                EveryUnitId = 1
            };
            context.Chores.Add(chore);
            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel
            {
                Id = 1,
                ScheduleSourceId = chore.Id,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                CompletedOn = completedOn
            };
            // Act
            var result = await DutyLogic.GetNextChoreDueDate(context, scheduledDuty);

            // Assert
            Assert.AreEqual(completedOn.Date + chore.DueByTime + TimeSpan.FromSeconds(86400/2), result);
            // Add more assertions based on the expected result
        }
    }
}