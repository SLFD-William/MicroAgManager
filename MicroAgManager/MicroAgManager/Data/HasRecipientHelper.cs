using Domain.Constants;
using FrontEnd.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MicroAgManager.Data
{
    public static class HasRecipientHelper
    {
        public static string RecipientTypeName(FrontEndDbContext db, string recipientType,long recipientTypeId)
        {
            if (recipientType == RecipientTypeConstants.LivestockAnimal)
                return db.LivestockAnimals.Find(recipientTypeId).Name ?? string.Empty;
            if (recipientType == RecipientTypeConstants.LivestockBreed)
                return db.LivestockBreeds.Find(recipientTypeId).Name ?? string.Empty;


            return string.Empty;
        }
        public static string RecipientName(FrontEndDbContext db, string recipientType, long? recipientId)
        {
            if(!recipientId.HasValue) return string.Empty;
            if (recipientType == RecipientTypeConstants.LivestockAnimal ||
                recipientType == RecipientTypeConstants.LivestockBreed)
                return db.Livestocks.Find(recipientId).Name ?? string.Empty;
            
            return string.Empty;
        }
        public static List<KeyValuePair<long, string>> RecipientTypeIds(FrontEndDbContext db, string recipientType)
        {
            if (recipientType == RecipientTypeConstants.LivestockAnimal)
                return db.LivestockAnimals.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
            if (recipientType == RecipientTypeConstants.LivestockBreed)
                return db.LivestockBreeds.Include(a => a.Animal).OrderBy(a => a.Animal.Name).ThenBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();

            return new List<KeyValuePair<long, string>>();

        }

    }
}
