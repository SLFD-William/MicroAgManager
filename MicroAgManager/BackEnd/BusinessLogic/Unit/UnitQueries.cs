using BackEnd.Abstracts;
using Domain.Interfaces;
using Domain.Models;

namespace BackEnd.BusinessLogic.Unit
{
    public class UnitQueries:BaseQuery
    {
        public UnitModel? NewUnit { get => (UnitModel?)NewModel; set => NewModel = value; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Symbol { get; set; }
        public double? ConversionFactorToSIUnit { get; set; }
        

        protected override IQueryable<T> GetQuery<T>(IMicroAgManagementDbContext context)
        {
            var query = PopulateBaseQuery(context.Units.AsQueryable());
            if (query is null) throw new ArgumentNullException(nameof(query));
            if (Name != null) query = query.Where(_ => _.Name == Name);
            if (Category != null) query = query.Where(_ => _.Category == Category);
            if (Symbol != null) query = query.Where(_ => _.Symbol == Symbol);
            if (ConversionFactorToSIUnit != null) query = query.Where(_ => _.ConversionFactorToSIUnit == ConversionFactorToSIUnit);
            return (IQueryable<T>)query;
        }
    }
}
