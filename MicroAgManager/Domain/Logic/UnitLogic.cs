using Domain.Entity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Logic
{
    public static class UnitLogic
    {
        public static List<IUnit> UnitSelections(DbContext genericContext, Dictionary<string, string>? unitTypes)
        {
            var context = genericContext as IFrontEndDbContext;
            if (context is null) return null;
            var query = context.Units.AsEnumerable();
            if(unitTypes?.Any() ?? false)
                query = query.Where(u => unitTypes.ContainsKey(u.Category)).OrderBy(u => u.Category).ThenBy(u => u.Symbol);
            
            return query.Select(c => c as IUnit).ToList();
        }
        public static string UnitName(DbContext genericContext, long? unitId)
        {
            var context = genericContext as IFrontEndDbContext;
            if (context is null) return string.Empty;
            return context.Units.Find(unitId)?.Name ?? string.Empty;
         }
    }
}
