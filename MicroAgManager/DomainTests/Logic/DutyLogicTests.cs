using Domain.Constants;
using Domain.Entity;
using Domain.Models;
using DomainTests;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
                DutyId = 1,
                RecipientId = 1,
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
                DutyId = 1,
                RecipientId = 1,
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
                DutyId = 1,
                RecipientId = 1,
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
        [TestMethod()]
        public async Task GetNextFreqAndDurationDueDateTest_ScheduledDutyIsChore()
        {
            // Arrange

            var context = new TestFrontEndDbContext().CreateContext();
            var scheduledDuty = new ScheduledDutyModel { Id = 1, ScheduleSource = ScheduledDutySourceConstants.Chore };

            // Act
            var result = await DutyLogic.GetNextFreqAndDurationDueDate(context, scheduledDuty);

            // Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        public async Task GetNextFreqAndDurationDueDateTest_ScheduledDutyIsNotComplete()
        {
            // Arrange
            var context = new TestFrontEndDbContext().CreateContext();
            var scheduledDuty = new ScheduledDutyModel { Id = 1, ScheduleSource = ScheduledDutySourceConstants.Event };

            // Act
            var result = await DutyLogic.GetNextFreqAndDurationDueDate(context, scheduledDuty);

            // Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        public async Task GetNextFreqAndDurationDueDateTest_ScheduledDutyDutyIsNotIHasFrequencyAndDuration()
        {
            // Arrange
            var completedOn = DateTime.Now;
            var context = new TestFrontEndDbContext().CreateContext();
            var measure = new MeasureModel { UnitId = 1, Name = "Test Measure", Method = "Test Method", };
            context.Measures.Add(measure);
            context.SaveChanges();
            var duty = new DutyModel
            {
                Command = DutyCommandConstants.Measurement,
                CommandId = measure.Id,
                Name = "Test Duty",
                RecipientType = "Test Recipient",
                RecipientTypeId = 0,
                Relationship = "Self"
            };
            context.Duties.Add(duty);
            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel { Id = 1, ScheduleSource = ScheduledDutySourceConstants.Event, DutyId=duty.Id,CompletedOn=completedOn };

            // Act
            var result = await DutyLogic.GetNextFreqAndDurationDueDate(context, scheduledDuty);

            // Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        public async Task GetNextFreqAndDurationDueDateTest_ScheduledDutyDuty_HasOnceFrequencyAndDuration()
        {
            // Arrange
            var completedOn = DateTime.Now;
            var context = new TestFrontEndDbContext().CreateContext();

            var treatment =await context.Treatments.FirstAsync();
            var duty = await context.Duties.FirstAsync();
            var eventModel = await context.Events.FirstAsync();
            duty.Command= DutyCommandConstants.Treatment;
            duty.CommandId = treatment.Id;
            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel { Id = 1,ScheduleSourceId= eventModel.Id, ScheduleSource = ScheduledDutySourceConstants.Event, DutyId = duty.Id, CompletedOn = completedOn };

            // Act
            var result = await DutyLogic.GetNextFreqAndDurationDueDate(context, scheduledDuty);

            // Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        public async Task GetNextFreqAndDurationDueDateTest_ScheduledDutyDuty_HasTwiceADayEveryOneDayForOneDayFirst()
        {
            // Arrange
            var completedOn = DateTime.Now;
            var context = new TestFrontEndDbContext().CreateContext();

            var treatment = await context.Treatments.FirstAsync();
            treatment.PerScalar = 2;
            var duty = await context.Duties.FirstAsync();
            var eventModel = await context.Events.FirstAsync();
            duty.Command = DutyCommandConstants.Treatment;
            duty.CommandId = treatment.Id;
            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel { Id = 1, 
                ScheduleSourceId = eventModel.Id, 
                ScheduleSource = ScheduledDutySourceConstants.Event, 
                DutyId = duty.Id, 
                CompletedOn = completedOn 
            };

            // Act
            var result = await DutyLogic.GetNextFreqAndDurationDueDate(context, scheduledDuty);

            // Assert
            Assert.AreEqual(completedOn.Date + TimeSpan.FromSeconds(86400 / 2), result);
        }
        [TestMethod()]
        public async Task GetNextFreqAndDurationDueDateTest_ScheduledDutyDuty_HasTwiceADayEveryOneDayForOneDaySecond()
        {
            // Arrange
            var completedOn = DateTime.Today + TimeSpan.FromHours(10);
            var context = new TestFrontEndDbContext().CreateContext();

            var treatment = await context.Treatments.FirstAsync();
            treatment.PerScalar = 2;
            var duty = await context.Duties.FirstAsync();
            var eventModel = await context.Events.FirstAsync();
            eventModel.StartDate= DateTime.Today;
            duty.Command = DutyCommandConstants.Treatment;
            duty.CommandId = treatment.Id;
            context.TreatmentRecords.Add(new TreatmentRecordModel
            {
                DatePerformed= DateTime.Today + TimeSpan.FromHours(9),
                TreatmentId=treatment.Id,
                RecipientType="LivestockAnimal",
                RecipientTypeId=1
            });

            context.ScheduledDuties.Add(new ScheduledDutyModel
            {
                Id = 1,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                CompletedOn = DateTime.Today + TimeSpan.FromHours(9),
                DutyId = duty.Id,
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            });

            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel
            {
                Id = 2,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                DutyId = duty.Id,
                CompletedOn = completedOn,
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            };

            // Act
            var result = await DutyLogic.GetNextFreqAndDurationDueDate(context, scheduledDuty);

            // Assert
            Assert.IsNull(result);
        }
        [TestMethod()]
        public async Task GetNextFreqAndDurationDueDateTest_ScheduledDutyDuty_HasOnceADayEveryOtherDayForTwoDayFirst()
        {
            // Arrange
            var completedOn = DateTime.Now;
            var context = new TestFrontEndDbContext().CreateContext();

            var treatment = await context.Treatments.FirstAsync();
            treatment.PerScalar = 1;
            treatment.EveryScalar = 2;
            treatment.DurationScalar = 2;
            var duty = await context.Duties.FirstAsync();
            var eventModel = await context.Events.FirstAsync();
            duty.Command = DutyCommandConstants.Treatment;
            duty.CommandId = treatment.Id;
            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel
            {
                Id = 1,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                DutyId = duty.Id,
                CompletedOn = completedOn
            };

            // Act
            var result = await DutyLogic.GetNextFreqAndDurationDueDate(context, scheduledDuty);

            // Assert
            Assert.AreEqual(completedOn.Date + TimeSpan.FromSeconds(86400 *2 ), result);
        }
        [TestMethod()]
        public async Task GetNextFreqAndDurationDueDateTest_ScheduledDutyDuty_HasOnceADayEveryOtherDayForTwoDaySecond()
        {
            // Arrange
            var completedOn = DateTime.Now.AddDays(-2);
            var context = new TestFrontEndDbContext().CreateContext();

            var treatment = await context.Treatments.FirstAsync();
            treatment.PerScalar = 1;
            treatment.EveryScalar = 2;
            treatment.DurationScalar = 2;
            var duty = await context.Duties.FirstAsync();
            var eventModel = await context.Events.FirstAsync();
            eventModel.StartDate=completedOn;
            duty.Command = DutyCommandConstants.Treatment;
            duty.CommandId = treatment.Id;
            context.TreatmentRecords.Add(new TreatmentRecordModel
            {
                DatePerformed = completedOn,
                TreatmentId = treatment.Id,
                RecipientType = "LivestockAnimal",
                RecipientTypeId = 1
            });

            context.ScheduledDuties.Add(new ScheduledDutyModel
            {
                Id = 1,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                CompletedOn = completedOn,
                DutyId = duty.Id,
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            });

            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel
            {
                Id = 2,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                DutyId = duty.Id,
                CompletedOn = completedOn.AddDays(2),
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            };

            // Act
            var result = await DutyLogic.GetNextFreqAndDurationDueDate(context, scheduledDuty);

            // Assert
            Assert.IsNull(result);

        }
        [TestMethod()]
        public async Task GetNextFreqAndDurationDueDateTest_ScheduledDutyDuty_HasTwiceADayEveryOtherDayForTwoDayFirst()
        {
            // Arrange
            var completedOn = DateTime.Now;
            var context = new TestFrontEndDbContext().CreateContext();

            var treatment = await context.Treatments.FirstAsync();
            treatment.PerScalar = 2;
            treatment.EveryScalar = 2;
            treatment.DurationScalar = 2;
            var duty = await context.Duties.FirstAsync();
            var eventModel = await context.Events.FirstAsync();
            duty.Command = DutyCommandConstants.Treatment;
            duty.CommandId = treatment.Id;
            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel
            {
                Id = 1,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                DutyId = duty.Id,
                CompletedOn = completedOn
            };

            // Act
            var result = await DutyLogic.GetNextFreqAndDurationDueDate(context, scheduledDuty);

            // Assert
            Assert.AreEqual(completedOn.Date + TimeSpan.FromSeconds(86400 / 2), result);
        }
        [TestMethod()]
        public async Task GetNextFreqAndDurationDueDateTest_ScheduledDutyDuty_HasTwiceADayEveryOtherDayForTwoDaySecond()
        {
            // Arrange
            var completedOn = DateTime.Today+TimeSpan.FromHours(9);
            var context = new TestFrontEndDbContext().CreateContext();

            var treatment = await context.Treatments.FirstAsync();
            treatment.PerScalar = 2;
            treatment.EveryScalar = 2;
            treatment.DurationScalar = 2;
            var duty = await context.Duties.FirstAsync();
            var eventModel = await context.Events.FirstAsync();
            eventModel.StartDate = completedOn;
            duty.Command = DutyCommandConstants.Treatment;
            duty.CommandId = treatment.Id;
            context.TreatmentRecords.Add(new TreatmentRecordModel
            {
                DatePerformed = completedOn,
                TreatmentId = treatment.Id,
                RecipientType = "LivestockAnimal",
                RecipientTypeId = 1
            });

            context.ScheduledDuties.Add(new ScheduledDutyModel
            {
                Id = 1,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                CompletedOn = completedOn,
                DutyId = duty.Id,
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            });

            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel
            {
                Id = 2,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                DutyId = duty.Id,
                CompletedOn = DateTime.Today + TimeSpan.FromHours(10),
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            };
                        // Act
            var result = await DutyLogic.GetNextFreqAndDurationDueDate(context, scheduledDuty);

            // Assert
            Assert.AreEqual(completedOn.Date + TimeSpan.FromSeconds(86400 * 2), result);
        }
        [TestMethod()]
        public async Task GetNextFreqAndDurationDueDateTest_ScheduledDutyDuty_HasTwiceADayEveryOtherDayForTwoDayThree()
        {
            // Arrange
            var completedOn = DateTime.Today.AddDays(-2) + TimeSpan.FromHours(9);
            var context = new TestFrontEndDbContext().CreateContext();

            var treatment = await context.Treatments.FirstAsync();
            treatment.PerScalar = 2;
            treatment.EveryScalar = 2;
            treatment.DurationScalar = 2;
            var duty = await context.Duties.FirstAsync();
            var eventModel = await context.Events.FirstAsync();
            eventModel.StartDate = completedOn;
            duty.Command = DutyCommandConstants.Treatment;
            duty.CommandId = treatment.Id;
            context.TreatmentRecords.Add(new TreatmentRecordModel
            {
                DatePerformed = completedOn,
                TreatmentId = treatment.Id,
                RecipientType = "LivestockAnimal",
                RecipientTypeId = 1
            });

            context.ScheduledDuties.Add(new ScheduledDutyModel
            {
                Id = 1,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                CompletedOn = completedOn,
                DutyId = duty.Id,
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            });
            context.TreatmentRecords.Add(new TreatmentRecordModel
            {
                DatePerformed = completedOn + TimeSpan.FromHours(8),
                TreatmentId = treatment.Id,
                RecipientType = "LivestockAnimal",
                RecipientTypeId = 1
            });

            context.ScheduledDuties.Add(new ScheduledDutyModel
            {
                Id = 2,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                CompletedOn = completedOn + TimeSpan.FromHours(8),
                DutyId = duty.Id,
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            });

            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel
            {
                Id = 3,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                DutyId = duty.Id,
                CompletedOn = DateTime.Today,
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            };
            // Act
            var result = await DutyLogic.GetNextFreqAndDurationDueDate(context, scheduledDuty);

            // Assert
            Assert.AreEqual(DateTime.Today + TimeSpan.FromSeconds(86400 / 2), result);
        }
        [TestMethod()]
        public async Task GetNextFreqAndDurationDueDateTest_ScheduledDutyDuty_HasTwiceADayEveryOtherDayForTwoDayFour()
        {
            // Arrange
            var completedOn = DateTime.Today.AddDays(-2) + TimeSpan.FromHours(9);
            var context = new TestFrontEndDbContext().CreateContext();

            var treatment = await context.Treatments.FirstAsync();
            treatment.PerScalar = 2;
            treatment.EveryScalar = 2;
            treatment.DurationScalar = 2;
            var duty = await context.Duties.FirstAsync();
            var eventModel = await context.Events.FirstAsync();
            eventModel.StartDate = completedOn;
            duty.Command = DutyCommandConstants.Treatment;
            duty.CommandId = treatment.Id;
            context.TreatmentRecords.Add(new TreatmentRecordModel
            {
                DatePerformed = completedOn,
                TreatmentId = treatment.Id,
                RecipientType = "LivestockAnimal",
                RecipientTypeId = 1
            });

            context.ScheduledDuties.Add(new ScheduledDutyModel
            {
                Id = 1,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                CompletedOn = completedOn,
                DutyId = duty.Id,
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            });
            context.TreatmentRecords.Add(new TreatmentRecordModel
            {
                DatePerformed = completedOn + TimeSpan.FromHours(8),
                TreatmentId = treatment.Id,
                RecipientType = "LivestockAnimal",
                RecipientTypeId = 1
            });

            context.ScheduledDuties.Add(new ScheduledDutyModel
            {
                Id = 2,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                CompletedOn = completedOn + TimeSpan.FromHours(8),
                DutyId = duty.Id,
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            });
            context.TreatmentRecords.Add(new TreatmentRecordModel
            {
                DatePerformed = completedOn.AddDays(2),
                TreatmentId = treatment.Id,
                RecipientType = "LivestockAnimal",
                RecipientTypeId = 1
            });
            context.ScheduledDuties.Add(new ScheduledDutyModel
            {
                Id = 3,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                DutyId = duty.Id,
                CompletedOn = completedOn.AddDays(2),
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            });
            context.SaveChanges();
            var scheduledDuty = new ScheduledDutyModel
            {
                Id = 4,
                ScheduleSourceId = eventModel.Id,
                ScheduleSource = ScheduledDutySourceConstants.Event,
                DutyId = duty.Id,
                CompletedOn = DateTime.Today + TimeSpan.FromHours(10),
                Record = "TreatmentRecord",
                RecipientId = 1,
                Recipient = "Livestock",
            };
            // Act
            var result = await DutyLogic.GetNextFreqAndDurationDueDate(context, scheduledDuty);

            // Assert
            Assert.IsNull(result);
        }
    }
}