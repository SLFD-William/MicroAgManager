using Domain.Abstracts;
using Domain.Constants;
using Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Shared
{
    public abstract class HasRecipientComponent<Model> : DataComponent<Model>, IHasRecipient where Model : BaseModel, new()
    {
        
        [Parameter] public long RecipientTypeId {get;set;}
        [Parameter] public long RecipientId { get; set; }
        [Parameter] public string RecipientType { get; set; }
        protected List<KeyValuePair<long, string>> recipientTypeIds()
        {
            switch (((IHasRecipient)working).RecipientType)
            {
                case nameof(RecipientTypeConstants.LivestockAnimal):
                    return app.dbContext.LivestockAnimals.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                default:
                    return new List<KeyValuePair<long, string>>();
            }
        }
        protected List<KeyValuePair<long, string>> recipientIds()
        {
            switch (((IHasRecipient)working).RecipientType)
            {
                case nameof(RecipientTypeConstants.LivestockAnimal):
                    return app.dbContext.Livestocks.OrderBy(a => a.Name).Select(x => new KeyValuePair<long, string>(x.Id, x.Name)).ToList();
                default:
                    return new List<KeyValuePair<long, string>>();
            }
        }
    }
}
