using Domain.Constants;
using Domain.Models;
using DomainTests;

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
        public async Task GetNextChoreDueDateTest_ChoreNotEnabled()
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
                PerScalar = 1,
                PerUnitId = 1,
                EveryScalar = 1,
                EveryUnitId = 1,
                Enabled=false
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
        [TestMethod()]
        public async Task GetNextChoreDueDateTest_ValidChore_TwiceADayTwoOfTwo()
        {
            // Arrange
            var context = new TestFrontEndDbContext().CreateContext();
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
                CompletedOn = DateTime.Today + chore.DueByTime,
                DutyId=1,
                Record="Test Record",
                RecipientId=1,
                Recipient="Test Recipient",
            };
            context.ScheduledDuties.Add(scheduledDuty);
            context.SaveChanges();
            scheduledDuty = new ScheduledDutyModel
            {
                Id = 2,
                ScheduleSourceId = chore.Id,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                CompletedOn = DateTime.Today + chore.DueByTime + TimeSpan.FromSeconds(86400 / 2),
                DutyId = 1
            };
            // Act
            var result = await DutyLogic.GetNextChoreDueDate(context, scheduledDuty);

            // Assert
            Assert.AreEqual(DateTime.Today.AddDays(1) + chore.DueByTime, result);
            // Add more assertions based on the expected result
        }
        [TestMethod()]
        public async Task GetNextChoreDueDateTest_ValidChore_OnceADayEveryTwoDays()
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
                PerScalar = 1,
                PerUnitId = 1,
                EveryScalar = 2,
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
            Assert.AreEqual(completedOn.AddDays(2).Date + chore.DueByTime, result);
        }
        [TestMethod()]
        public async Task GetNextChoreDueDateTest_ValidChore_TwiceADayEveryTwoDays_OneOfFour()
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
                EveryScalar = 2,
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
            Assert.AreEqual(completedOn.Date + chore.DueByTime + TimeSpan.FromSeconds(86400 / 2), result);
        }
        [TestMethod()]
        public async Task GetNextChoreDueDateTest_ValidChore_TwiceADayEveryTwoDays_TwoOfFour()
        {
            // Arrange
            var context = new TestFrontEndDbContext().CreateContext();

            var completedOn = DateTime.Today;
            var chore = new ChoreModel
            {
                RecipientTypeId = 1,
                RecipientType = "LivestockAnimal",
                Name = "Test Chore",
                Color = "transparent",
                DueByTime = TimeSpan.FromHours(9),
                PerScalar = 2,
                PerUnitId = 1,
                EveryScalar = 2,
                EveryUnitId = 1
            };
            context.Chores.Add(chore);
            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel
            {
                Id = 1,
                ScheduleSourceId = chore.Id,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                CompletedOn = completedOn + chore.DueByTime,
                DutyId = 1,
                Record = "Test Record",
                RecipientId = 1,
                Recipient = "Test Recipient",
            };
            context.ScheduledDuties.Add(scheduledDuty);
            context.SaveChanges();
            scheduledDuty = new ScheduledDutyModel
            {
                Id = 2,
                ScheduleSourceId = chore.Id,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                CompletedOn = completedOn + chore.DueByTime + TimeSpan.FromSeconds(86400 / 2),
                DutyId = 1
            };            // Act
            var result = await DutyLogic.GetNextChoreDueDate(context, scheduledDuty);

            // Assert
            Assert.AreEqual(completedOn.AddDays(2).Date + chore.DueByTime, result);
        }

        [TestMethod()]
        public async Task GetNextChoreDueDateTest_ValidChore_TwiceADayEveryTwoDays_ThreeOfFour()
        {
            // Arrange
            var context = new TestFrontEndDbContext().CreateContext();

            var completedOn = DateTime.Today.AddDays(-2);
            var chore = new ChoreModel
            {
                RecipientTypeId = 1,
                RecipientType = "LivestockAnimal",
                Name = "Test Chore",
                Color = "transparent",
                DueByTime = TimeSpan.FromHours(9),
                PerScalar = 2,
                PerUnitId = 1,
                EveryScalar = 2,
                EveryUnitId = 1
            };
            context.Chores.Add(chore);
            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel
            {
                Id = 1,
                CompletedOn = completedOn + chore.DueByTime,
                ScheduleSourceId = chore.Id,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                DutyId = 1,
                Record = "Test Record",
                RecipientId = 1,
                Recipient = "Test Recipient",
            };
            context.ScheduledDuties.Add(scheduledDuty);
            scheduledDuty = new ScheduledDutyModel
            {
                Id = 2,
                CompletedOn = completedOn + chore.DueByTime + TimeSpan.FromSeconds(86400 / 2),
                ScheduleSourceId = chore.Id,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                DutyId = 1,
                Record = "Test Record",
                RecipientId = 1,
                Recipient = "Test Recipient",
            };
            context.ScheduledDuties.Add(scheduledDuty);
            context.SaveChanges();
            scheduledDuty = new ScheduledDutyModel
            {
                Id = 3,
                ScheduleSourceId = chore.Id,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                CompletedOn = completedOn.AddDays(2) + chore.DueByTime
            };
            // Act
            var result = await DutyLogic.GetNextChoreDueDate(context, scheduledDuty);

            // Assert
            Assert.AreEqual(completedOn.AddDays(2).Date + chore.DueByTime + TimeSpan.FromSeconds(86400 / 2), result);
        }
        [TestMethod()]
        public async Task GetNextChoreDueDateTest_ValidChore_TwiceADayEveryTwoDays_FourOfFour()
        {
            // Arrange
            var context = new TestFrontEndDbContext().CreateContext();

            var completedOn = DateTime.Today.AddDays(-2);
            var chore = new ChoreModel
            {
                RecipientTypeId = 1,
                RecipientType = "LivestockAnimal",
                Name = "Test Chore",
                Color = "transparent",
                DueByTime = TimeSpan.FromHours(9),
                PerScalar = 2,
                PerUnitId = 1,
                EveryScalar = 2,
                EveryUnitId = 1
            };
            context.Chores.Add(chore);
            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel
            {
                Id = 1,
                CompletedOn = completedOn + chore.DueByTime,
                ScheduleSourceId = chore.Id,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                DutyId = 1,
                Record = "Test Record",
                RecipientId = 1,
                Recipient = "Test Recipient",
            };
            context.ScheduledDuties.Add(scheduledDuty);
            scheduledDuty = new ScheduledDutyModel
            {
                Id = 2,
                CompletedOn = completedOn + chore.DueByTime + TimeSpan.FromSeconds(86400 / 2),
                ScheduleSourceId = chore.Id,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                DutyId = 1,
                Record = "Test Record",
                RecipientId = 1,
                Recipient = "Test Recipient",
            };
            context.ScheduledDuties.Add(scheduledDuty);
            scheduledDuty = new ScheduledDutyModel
            {
                Id = 3,
                CompletedOn = completedOn.AddDays(2) + chore.DueByTime,
                ScheduleSourceId = chore.Id,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                DutyId = 1,
                Record = "Test Record",
                RecipientId = 1,
                Recipient = "Test Recipient",
            };
            context.ScheduledDuties.Add(scheduledDuty);
            context.SaveChanges();
            scheduledDuty = new ScheduledDutyModel
            {
                Id = 4,
                ScheduleSourceId = chore.Id,
                ScheduleSource = ScheduledDutySourceConstants.Chore,
                CompletedOn = completedOn.AddDays(2) + chore.DueByTime + TimeSpan.FromSeconds(86400 / 2),
                DutyId = 1
            };
            // Act
            var result = await DutyLogic.GetNextChoreDueDate(context, scheduledDuty);

            // Assert
            Assert.AreEqual(completedOn.AddDays(4).Date + chore.DueByTime , result);
        }
        [TestMethod()]
        public async Task OnScheduledDutyCompletedTest_CommandDoesNotRequireReschedule()
        {
            // Arrange
            var context = new TestMicroAgManagementDbContext().CreateContext();
            var command = new BackEnd.BusinessLogic.ScheduledDuty.CreateScheduledDuty() { Reschedule = false, ScheduledDuty = new ScheduledDutyModel() };
            var duty = new ScheduledDutyModel();

            // Act
            var result = await DutyLogic.OnScheduledDutyCompleted(context, command, duty);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod()]
        public async Task OnScheduledDutyCompletedTest_DutyNotFound()
        {
            // Arrange
            var context = new TestMicroAgManagementDbContext().CreateContext();
            var command = new BackEnd.BusinessLogic.ScheduledDuty.CreateScheduledDuty() { Reschedule = true, RescheduleDueOn = DateTime.Now, ScheduledDuty = new ScheduledDutyModel() };
            var duty = new ScheduledDutyModel { Id = 2, CompletedOn = DateTime.Now };

            // Act
            var result = await DutyLogic.OnScheduledDutyCompleted(context, command, duty);

            // Assert
            Assert.IsNull(result);

        }

        [TestMethod()]
        public async Task OnScheduledDutyCompletedTest_ValidDuty()
        {
            // Arrange
            var context = new TestMicroAgManagementDbContext().CreateContext();
            var command = new BackEnd.BusinessLogic.ScheduledDuty.CreateScheduledDuty() { Reschedule = true, RescheduleDueOn = DateTime.Now.AddMinutes(180), ScheduledDuty = new ScheduledDutyModel() };
            var completedOn = DateTime.Now;
            var duty = new ScheduledDutyModel { Id = 1, CompletedOn = completedOn };

            // Act
            var result = await DutyLogic.OnScheduledDutyCompleted(context, command, duty);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(command.RescheduleDueOn.Value, result.ScheduledDuty.DueOn);
        }

    }
}