using Domain.Interfaces;
using Microsoft.AspNetCore.Components;

namespace MicroAgManager.Components.Shared
{
    public abstract class HasRecipientsEditor:BaseEditor
    {
        [Parameter] public bool ShowRecipient { get; set; } = true;
        [Parameter] public bool ShowUpdateCancel { get; set; } = true;

        public abstract Task<IHasRecipient> SubmitEditContext();

    }
}
