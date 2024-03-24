using Domain.Abstracts;
using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Logic
{
    public class CreateScheduledDuty : ICreateScheduledDuty, IBaseCommand
    {
        public Guid CreatedBy { get; set; }
        public ScheduledDutyModel ScheduledDuty { get; set ; }
        public Guid ModifiedBy { get; set; }
        public Guid TenantId { get; set; }
    }
    public static class DutyLogic
    {
        public static List<IDuty> DutySelections(DbContext genericContext, IDuty duty, Dictionary<string, long>? recipientTypes)
        {
            var context = genericContext as IFrontEndDbContext;
            if (context is null) return null;
            var query = context.Duties.Where(d => d.Command == duty.Command);
            if (recipientTypes?.Any() == true)
            {
                query.AsEnumerable().Where(d => recipientTypes.Any(rt => rt.Key == d.RecipientType && rt.Value == d.RecipientTypeId));
            }
            return query.Select(c=>c as IDuty).ToList();
        }
        public static string GetRecordTypeFromCommand(IDuty duty)
        { 
            switch (duty.Command)
            {
                case nameof(DutyCommandConstants.Measurement):
                    return nameof(Measurement);
                case nameof(DutyCommandConstants.Treatment):
                    return nameof(TreatmentRecord);
                default:
                    return string.Empty;
            }
        }
        public static string GetCommandIcon(IDuty? duty)
        {
            if (!(duty is IDuty)) return null;
            if (duty.Command == DutyCommandConstants.Treatment)
                return "fa-kit-medical";
            if (duty.Command == DutyCommandConstants.Measurement)
                return "fa-scale-unbalanced";
            if (duty.Command == DutyCommandConstants.Registration)
                return "fa-newspaper";
            if (duty.Command == DutyCommandConstants.Birth)
                return "fa-cake-candles";
            if (duty.Command == DutyCommandConstants.Breed)
                return "fa-venus-mars";
            if (duty.Command == DutyCommandConstants.Service)
                return "fa-mars-and-venus-burst";
            return null;
        }
        public async static Task<BaseModel?> GetCommandItem(DbContext genericContext, IDuty? duty)
        {
            if (!(duty is IDuty)) return null;
            var context = genericContext as IFrontEndDbContext;
            if (context is null) return null;
            if (duty.Command == DutyCommandConstants.Treatment)
                return await context.Treatments.FindAsync(duty.CommandId);
            if (duty.Command == DutyCommandConstants.Measurement)
                return await context.Measures.FindAsync(duty.CommandId);
            if (duty.Command == DutyCommandConstants.Registration)
                return await context.Registrars.FindAsync(duty.CommandId);
            return null;
        }
       
        public static IQueryable<BaseModel>? GetCommandRecordPerformedAfter(DbContext genericContext, IDuty? duty,DateTime startDate)
        {
            if (!(duty is IDuty)) return null;
            var context = genericContext as IFrontEndDbContext;
            if (context is null) return null; if (duty.Command == DutyCommandConstants.Treatment)
                return context.TreatmentRecords.Where(t=>t.DatePerformed>=startDate).AsQueryable();
            if (duty.Command == DutyCommandConstants.Measurement)
                return context.Measurements.Where(t => t.DatePerformed >= startDate).AsQueryable();
            if (duty.Command == DutyCommandConstants.Registration)
                return context.Registrations.Where(t => t.RegistrationDate >= startDate).AsQueryable();
            return null;
        }
        
    }
}
