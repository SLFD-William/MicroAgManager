using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.BreedingRecord
{
    public class BreedingRecordQueries : BaseQuery
    {
        public BreedingRecordModel? NewBreedingRecord { get => (BreedingRecordModel?)NewModel; set => NewModel = value; }
        public long? RecipientId { get; set; }
        public long? MaleId { get; set; }
        public DateTime? ServiceDate { get; set; }
        public DateTime? ResolutionDate { get; set; }
        public DateTime? ExpectingDate { get; set; }

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.BreedingRecords.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (RecipientId.HasValue) query=query.Where(_ => _.RecipientId == RecipientId);
            if (MaleId.HasValue) query = query.Where(_ => _.MaleId == MaleId);
            if (ServiceDate.HasValue) query = query.Where(_ => _.ServiceDate == ServiceDate);
            if (ResolutionDate.HasValue) query = query.Where(_ => _.ResolutionDate == ResolutionDate);
            if (ExpectingDate.HasValue) query = query.Where(_ =>_.Female !=null && _.ServiceDate == ExpectingDate.Value.AddDays(-_.Female.Breed.GestationPeriod));
            return (IQueryable<T>)query;
        }
    }
}
