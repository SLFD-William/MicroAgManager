using Domain.Constants;
using Domain.Interfaces;
using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components;

namespace MicroAgManager.Components.Shared
{
    public class HasRecipientsEditor:BaseEditor
    {
        [Parameter] public bool ShowRecipient { get; set; } = true;
        protected List<KeyValuePair<long, string>> recipientTypeIds(FrontEndDbContext db)
        {
            if (((IHasRecipient)editContext.Model).RecipientType == RecipientTypeConstants.LivestockAnimal) 
                return db.LivestockAnimals.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
            return new List<KeyValuePair<long, string>>();
        }
        protected List<KeyValuePair<long, string>> recipientIds(FrontEndDbContext db)
        {
            if(((IHasRecipient)editContext.Model).RecipientType == RecipientTypeConstants.LivestockAnimal)
                return db.Livestocks.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
            return new List<KeyValuePair<long, string>>();
        }
    }
}
