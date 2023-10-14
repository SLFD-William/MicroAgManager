using BackEnd.BusinessLogic.FarmLocation.LandPlots;
using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Unit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;


namespace FrontEnd.Components.LandPlot
{
    public partial class LandPlotEditor : DataComponent<LandPlotModel>
    {
        [CascadingParameter] FarmLocationModel Farm { get; set; }
        [CascadingParameter] public LandPlotModel Plot { get; set; }
        [Parameter] public long? landPlotId { get; set; }
        [Parameter] public long? parentPlotId { get; set; }

        protected new LandPlotModel working { get => base.working as LandPlotModel; set { base.working = value; } }
        private ValidatedForm _validatedForm;
        private UnitEditor _unitEditor;
        private bool showUnitModal = false;
        private void ShowUnitEditor()
        {
            showUnitModal = true;
            StateHasChanged();
        }
        private void UnitCreated(object e)
        {
            var model = e as UnitModel;
            showUnitModal = false;
            working.AreaUnitId = model.Id;
            editContext = new EditContext(working);
            StateHasChanged();
        }
        private void UnitCanceled()
        {
            showUnitModal = false;
            StateHasChanged();
        }
        private void Cancel()
        {
            working = original.Map(working) as LandPlotModel;
            SetEditContext(working);
            Task.Run(Cancelled.InvokeAsync);
        }
        public async Task OnSubmit()
        {
            var id = working?.Id ?? 0;
            if (id <= 0)
                id = await app.api.ProcessCommand<LandPlotModel, CreateLandPlot>("api/CreateLandPlot", new CreateLandPlot { LandPlot = working });
            else
                id = await app.api.ProcessCommand<LandPlotModel, UpdateLandPlot>("api/UpdateLandPlot", new UpdateLandPlot { LandPlot = working });

            if (id <= 0)
                throw new Exception("Unable to save plot");
            working.Id= id;
            SetEditContext(working);
            await Submitted.InvokeAsync(working);
        }
        public override async Task FreshenData()    
        {
            if (Farm is null) return;
            if (Plot is not null)
                working = Plot;
            if (Plot is null && landPlotId.HasValue)
                working = await app.dbContext.LandPlots.FindAsync(landPlotId);
            
            SetEditContext(working);
            
        }
    }
}
