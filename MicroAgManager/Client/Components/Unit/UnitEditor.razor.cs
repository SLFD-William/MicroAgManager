using BackEnd.BusinessLogic.Unit;
using Domain.Constants;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace FrontEnd.Components.Unit
{
    public partial class UnitEditor : DataComponent<UnitModel>
    {
        [CascadingParameter] public UnitModel Unit { get; set; }
        [Parameter] public long? unitId { get; set; }
        [Parameter] public string? category { get; set; }
        private ValidatedForm _validatedForm;
        protected new UnitModel working { get => base.working as UnitModel; set { base.working = value; } }
        public override async Task FreshenData()
        {
            working = new UnitModel() { Category = category ?? UnitCategoryConstants.Mass.Key, Name="Unit",Symbol="Symbol", ConversionFactorToSIUnit=0 };

            if (Unit is not null)
                working = Unit;

            if (Unit is null && unitId > 0)
                working = await app.dbContext.Units.FindAsync(unitId);

            editContext = new EditContext(working);
        }
        public async Task OnSubmit()
        {
            long id= (working.Id <= 0)?
                working.Id = await app.api.ProcessCommand<UnitModel, CreateUnit>("api/CreateUnit", new CreateUnit { Unit = working }):
                working.Id = await app.api.ProcessCommand<UnitModel, UpdateUnit>("api/UpdateUnit", new UpdateUnit { Unit = working });

            if (id <= 0)
                throw new Exception("Failed to save Unit");
            working.Id = id;
            original = working.Clone() as UnitModel;
            editContext = new EditContext(working);
            await Submitted.InvokeAsync(working);
            StateHasChanged();
            
        }
        private async Task Cancel()
        {
            editContext = new EditContext(working);
            await Cancelled.InvokeAsync(working);
            StateHasChanged();
        }
    }
}
