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
        public override async Task FreshenData()
        {
            working = new UnitModel() { Category = category ?? UnitCategoryConstants.Mass.Key, Name="Unit",Symbol="Symbol", ConversionFactorToSIUnit=0 };

            if (Unit is not null)
                working = Unit;

            if (Unit is null && unitId > 0)
                working = await app.dbContext.Units.FindAsync(unitId);

            editContext = new EditContext((UnitModel)working);
        }
        public async Task OnSubmit()
        {
            long id= (((UnitModel)working).Id <= 0)?
                ((UnitModel)working).Id = await app.api.ProcessCommand<UnitModel, CreateUnit>("api/CreateUnit", new CreateUnit { Unit = (UnitModel)working }):
                ((UnitModel)working).Id = await app.api.ProcessCommand<UnitModel, UpdateUnit>("api/UpdateUnit", new UpdateUnit { Unit = (UnitModel)working });

            if (id <= 0)
                throw new Exception("Failed to save Unit");
            ((UnitModel)working).Id = id;
            original = (UnitModel)working.Clone();
            editContext = new EditContext((UnitModel)working);
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
