using Domain.Constants;
using Domain.Entity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Logic
{
    public static class RecipientLogic
    {
        public static string RecipientTypeName(DbContext genericContext, string recipientType, long recipientTypeId)
        {
            
            var db = genericContext as IFrontEndDbContext;
            if (db is null) return string.Empty;
            if (recipientType == RecipientTypeConstants.LivestockAnimal)
                return db.LivestockAnimals.Find(recipientTypeId).Name ?? string.Empty;
            if (recipientType == RecipientTypeConstants.LivestockBreed)
                return db.LivestockBreeds.Find(recipientTypeId).Name ?? string.Empty;


            return string.Empty;
        }
        public static string RecipientName(DbContext genericContext, string recipientType, long? recipientId)
        {
            if (!recipientId.HasValue) return string.Empty;
            var db = genericContext as IFrontEndDbContext;
            if (db is null) return string.Empty;
            if (recipientType == RecipientTypeConstants.LivestockAnimal ||
                recipientType == RecipientTypeConstants.LivestockBreed)
                return db.Livestocks.Find(recipientId).Name ?? string.Empty;

            return string.Empty;
        }

        public static string GetRecipientHref(string recipientType, long? recipientId)
        {
            if (recipientType == RecipientTypeConstants.LivestockAnimal || recipientType == RecipientTypeConstants.LivestockBreed)
                return $"/Livestock?LivestockId={recipientId}";
            return string.Empty ;
        }

        public static List<KeyValuePair<long, string>> RecipientTypeIds(DbContext genericContext, string recipientType)
        {
            var db = genericContext as IFrontEndDbContext;
            if (db is null) return new List<KeyValuePair<long, string>>();
            if (recipientType == RecipientTypeConstants.LivestockAnimal)
                return db.LivestockAnimals.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
            if (recipientType == RecipientTypeConstants.LivestockBreed)
                return db.LivestockBreeds.Include(a => a.Animal).OrderBy(a => a.Animal.Name).ThenBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();

            return new List<KeyValuePair<long, string>>();

        }
        public static List<KeyValuePair<long, string>> RecipientIds(DbContext genericContext, string recipientType)
        {
            var db = genericContext as IFrontEndDbContext;
            if (db is null) return new List<KeyValuePair<long, string>>();
            if (recipientType == RecipientTypeConstants.LivestockAnimal)
                return db.Livestocks.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
            if (recipientType == RecipientTypeConstants.LivestockBreed)
                return db.Livestocks.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();

            return new List<KeyValuePair<long, string>>();

        }

        public static void PopulateDynamicRelations(DbContext genericContext, IHasRecipientModel hasRecipientModel)
        {
            var db = genericContext as IFrontEndDbContext;
            if (db is null) return;
            if (hasRecipientModel.RecipientType == nameof(LivestockAnimal))
                hasRecipientModel.RecipientTypeItem = db.LivestockAnimals.Find(hasRecipientModel.RecipientTypeId).Name ?? string.Empty;
            if (hasRecipientModel.RecipientType == nameof(LivestockBreed))
                hasRecipientModel.RecipientTypeItem = db.LivestockBreeds.Find(hasRecipientModel.RecipientTypeId).Name ?? string.Empty;
            if (hasRecipientModel.RecipientType == nameof(LivestockAnimal) || hasRecipientModel.RecipientType == nameof(LivestockBreed))
                hasRecipientModel.RecipientItem = db.Livestocks.Find(hasRecipientModel.RecipientId)?.Name ?? string.Empty;
        }
    }
}
