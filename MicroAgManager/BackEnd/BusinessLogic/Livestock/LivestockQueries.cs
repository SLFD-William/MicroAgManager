using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Livestock
{
    public class LivestockQueries : BaseQuery
    {
        public LivestockModel NewLivestock { get => (LivestockModel)NewModel; set => NewModel = value; }

        public long? MotherId { get; set; }
        public long? FatherId { get; set; }
        public long? LivestockBreedId { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? Name { get; set; }
        public string? BatchNumber { get; set; }

        public string? Gender { get; set; }
        public string? Variety { get; set; }
        public string? Description { get; set; }
        public string? BirthDefect { get; set; }
        public string? Status { get; set; }
        
        public bool? BeingManaged { get; set; }
        public bool? BornDefective { get; set; }
        public bool? Sterile { get; set; }
        public bool? InMilk { get; set; }
        public bool? BottleFed { get; set; }
        public bool? ForSale { get; set; }
        
        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Livestocks.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (MotherId.HasValue) query = query.Where(_ => _.MotherId == MotherId);
            if (FatherId.HasValue) query = query.Where(_ => _.FatherId == FatherId);
            if (LivestockBreedId.HasValue) query = query.Where(_ => _.Breed.Id == LivestockBreedId);
            if (!string.IsNullOrEmpty(Name)) query = query.Where(_ => _.Name != null && _.Name.Contains(Name));
            if (!string.IsNullOrEmpty(BatchNumber)) query = query.Where(_ => _.BatchNumber != null && _.BatchNumber.Contains(BatchNumber));
            if (Birthdate.HasValue) query = query.Where(_ => _.Birthdate == Birthdate);
            if (!string.IsNullOrEmpty(Gender)) query = query.Where(_ => _.Gender != null && _.Gender.Contains(Gender));
            if (!string.IsNullOrEmpty(Variety)) query = query.Where(_ => _.Variety != null && _.Variety.Contains(Variety));
            if (!string.IsNullOrEmpty(Description)) query = query.Where(_ => _.Description != null && _.Description.Contains(Description));
            if (!string.IsNullOrEmpty(BirthDefect)) query = query.Where(_ => _.BirthDefect != null && _.BirthDefect.Contains(BirthDefect));
            if (!string.IsNullOrEmpty(Status)) query = query.Where(_ => _.Status != null && _.Status.Status.Contains(Status));

            if (BeingManaged.HasValue) query = query.Where(_ => _.BeingManaged == BeingManaged);
            if (BornDefective.HasValue) query = query.Where(_ => _.BornDefective == BornDefective);
            if (Sterile.HasValue) query = query.Where(_ => _.Sterile == Sterile);
            if (InMilk.HasValue) query = query.Where(_ => _.InMilk == InMilk);
            if (BottleFed.HasValue) query = query.Where(_ => _.BottleFed == BottleFed);
            if (ForSale.HasValue) query = query.Where(_ => _.ForSale == ForSale);
            return (IQueryable<T>)query;
        }
    }
}
