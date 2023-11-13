using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.Registration
{
    public partial class RegistrationList : DataComponent<RegistrationModel>
    {
        public TableTemplate<RegistrationModel> _listComponent;
        [Parameter] public IEnumerable<RegistrationModel> Items { get; set; } = new List<RegistrationModel>();

        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<RegistrationModel>? RegistrationSelected { get; set; }
        private RegistrationModel? _editRegistration;
        private RegistrationEditor? _registrationEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("RegistrationList"))
                app.RowDetailsShowing.Add("RegistrationList", new List<object>());
        }
        private async Task<RegistrationModel?> FindRegistration(long id)=> await app.dbContext.Registrations.FindAsync(id);
        
        private async Task EditRegistration(long id)
        {
            _editRegistration = id > 0 ? await FindRegistration(id) : new RegistrationModel { };
            StateHasChanged();
        }
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                RegistrationSelected?.Invoke(Task.Run(async()=> await  FindRegistration(_listComponent.SelectedItems.First().Id)).Result);
        }
        public override async Task FreshenData()
        {
            if (_listComponent is null) return;
            if (Items is null)
                Items = app.dbContext.Registrations.OrderByDescending(f => f.RegistrationDate).ToList() ?? new List<RegistrationModel>();
            
            _listComponent.Update();
        }
        private async Task EditCancelled()
        {
            _editRegistration = null;
            StateHasChanged();
        }
        private async Task RegistrationUpdated(object args)
        {
            var model = args as RegistrationModel;
            if (model?.Id > 0)
            {
                var start = DateTime.Now;
                while (!app.dbContext.Registrations.Any(t => t.Id == model.Id))
                {
                    await Task.Delay(1000);
                    if (DateTime.Now.Subtract(start).TotalSeconds > 10)
                        break;
                }
            }

            _editRegistration = null;
            await Submitted.InvokeAsync(await FindRegistration(model.Id));
        }
    }
}
