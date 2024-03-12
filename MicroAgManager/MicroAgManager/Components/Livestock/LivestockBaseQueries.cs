using Domain.Constants;
using Domain.Models;
using MicroAgManager.Services;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace MicroAgManager.Components.Livestock
{
    internal static class LivestockBaseQueries
    {
        private static readonly List<string> _animalTypes = new List<string>() { "LivestockAnimal", "LivestockBreed" };
        internal static IQueryable<ScheduledDutyModel> baseScheduledDutyQuery(long livestockId, ApplicationState appState)
        {
            var query = appState.DbContext.ScheduledDuties.Include(p => p.Duty).Where(d => !d.CompletedOn.HasValue
                && d.RecipientId == livestockId
                && _animalTypes.Contains(d.Duty.RecipientType)).ToList();
            foreach (var sd in query)
            {
                sd.PopulateDynamicRelations(appState.DbContext);
                sd.Duty.PopulateDynamicRelations(appState.DbContext);
            }
            return query.AsQueryable();
        }
        internal static IQueryable<MeasurementModel> baseMeasurementQuery(long livestockId, ApplicationState appState) => appState.DbContext.Measurements.Include(p => p.Measure).Where(d => d.RecipientId == livestockId
            && _animalTypes.Contains(d.RecipientType)).AsQueryable();
        internal static IQueryable<TreatmentRecordModel> baseTreatmentRecordQuery(long livestockId, ApplicationState appState) => appState.DbContext.TreatmentRecords.Include(p => p.Treatment).Where(d => d.RecipientId == livestockId
            && _animalTypes.Contains(d.RecipientType)).AsQueryable();
        internal static IQueryable<RegistrationModel> baseRegistrationQuery(long livestockId, ApplicationState appState) => appState.DbContext.Registrations.Include(p => p.Registrar).Where(d => d.RecipientId == livestockId
            && _animalTypes.Contains(d.RecipientType)).AsQueryable();
        internal static IQueryable<BreedingRecordModel> baseBreedingRecordQuery(long livestockId, ApplicationState appState)
        {
            var livestock = appState.DbContext.Livestocks.Find(livestockId);

            var query = appState.DbContext.BreedingRecords.AsQueryable();
            query = (livestock.Gender == GenderConstants.Male) ? query.Where(b => b.MaleId == livestock.Id) : query.Where(b => b.FemaleId == livestock.Id);

            return query.AsQueryable();
        }

    }
}
