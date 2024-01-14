using Domain.Constants;
using Domain.Interfaces;
using FrontEnd.Persistence;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace MicroAgManager.Components.Shared
{
    public abstract class HasRecipientsEditor:BaseEditor
    {
        [Parameter] public bool ShowRecipient { get; set; } = true;
        [Parameter] public bool ShowUpdateCancel { get; set; } = true;

        public abstract Task<IHasRecipient> SubmitEditContext();
        protected List<KeyValuePair<long, string>> recipientTypeIds(FrontEndDbContext db)
        {
            if (((IHasRecipient)editContext.Model).RecipientType == RecipientTypeConstants.LivestockAnimal) 
                return db.LivestockAnimals.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
            if (((IHasRecipient)editContext.Model).RecipientType == RecipientTypeConstants.LivestockBreed)
                return db.LivestockBreeds.Include(a=>a.Animal).OrderBy(a => a.Animal.Name).ThenBy(a=>a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
            return new List<KeyValuePair<long, string>>();
        }
        protected List<KeyValuePair<long, string>> recipientIds(FrontEndDbContext db)
        {
            if(((IHasRecipient)editContext.Model).RecipientType == RecipientTypeConstants.LivestockAnimal)
                return db.Livestocks.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
            if (((IHasRecipient)editContext.Model).RecipientType == RecipientTypeConstants.LivestockBreed)
                return db.Livestocks.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();

            return new List<KeyValuePair<long, string>>();
        }
    }
}
