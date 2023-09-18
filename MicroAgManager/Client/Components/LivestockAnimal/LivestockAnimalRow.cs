using Domain.Models;
using FrontEnd.Services;

namespace FrontEnd.Components.LivestockAnimal
{
    public class LivestockAnimalRow
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string ParentMaleName { get; set; }
        public string ParentFemaleName { get; set; }
        public string Care { get; set; }
        public int BreedsCount { get; set; }
        
        public int StatusesCount { get; set; }
        public int FeedsCount { get; set; }
        public static LivestockAnimalRow Create(ApplicationStateProvider app,LivestockAnimalModel livestock)
        { 
            var model = new LivestockAnimalRow()
            {
                Id = livestock.Id,
                Name = livestock.Name,
                GroupName = livestock.GroupName,
                ParentMaleName = livestock.ParentMaleName,
                ParentFemaleName = livestock.ParentFemaleName,
                Care = livestock.Care,
                BreedsCount = app.dbContext.LivestockBreeds.Count(f => f.LivestockAnimalId == livestock.Id),
                StatusesCount= app.dbContext.LivestockStatuses.Count(f => f.LivestockAnimalId == livestock.Id),
                FeedsCount= app.dbContext.LivestockFeeds.Count(f => f.LivestockAnimalId == livestock.Id)
            };
            return model;
        }
    }
}
