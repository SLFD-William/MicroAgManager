﻿using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Shared.Sortable;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.Registrar
{
    public partial class RegistrarList : DataComponent<RegistrarModel>
    {
        protected TableTemplate<RegistrarModel> _listComponent;
        [Parameter] public IEnumerable<RegistrarModel>? Items { get; set; }
        [Parameter] public bool Multiselect { get; set; } = false;
        [Parameter] public Action<RegistrarModel>? RegistrarSelected { get; set; }

        private RegistrarModel? _editRegistrar;
        private RegistrarEditor? _registrarEditor;
        protected override void OnInitialized()
        {
            if (!app.RowDetailsShowing.ContainsKey("RegistrarList"))
                app.RowDetailsShowing.Add("RegistrarList", new List<object>());
        }
        public override async Task FreshenData()
        {

            if (Items is null)
                Items = (await app.dbContext.Registrars.OrderBy(r=>r.Name).ToListAsync()).AsEnumerable();

            _listComponent?.Update();
        }
        private async Task<RegistrarModel?> FindRegistrar(long Id) => await app.dbContext.Registrars.FindAsync(Id);
        private void TableItemSelected()
        {
            if (_listComponent.SelectedItems.Count() > 0)
                RegistrarSelected?.Invoke(Task.Run(async () => await FindRegistrar(_listComponent.SelectedItems.First().Id)).Result);
        }
        private async Task EditRegistrar(long id)
        {
            _editRegistrar = id > 0 ? await FindRegistrar(id) : new RegistrarModel();
            StateHasChanged();
        }
        private async Task EditCancelled()
        {
            _editRegistrar = null;
            StateHasChanged();
        }
        private async Task RegistrarUpdated(object args)
        {
            var model = args as RegistrarModel;
            if (model?.Id > 0)
            {
                var start = DateTime.Now;
                while (!app.dbContext.Registrars.Any(t => t.Id == model.Id))
                {
                    await Task.Delay(1000);
                    if (DateTime.Now.Subtract(start).TotalSeconds > 10)
                        break;
                }
            }

            _editRegistrar = null;
            await Submitted.InvokeAsync(await FindRegistrar(model.Id));
        }
    }
}
