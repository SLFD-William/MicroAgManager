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
            ((LandPlotModel)working).AreaUnitId = model.Id;
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
            working = original.Map((LandPlotModel)working);
            SetEditContext((LandPlotModel)working);
            Task.Run(Cancelled.InvokeAsync);
        }
        public async Task OnSubmit()
        {
            var id = (working?.Id <=0)?
                await app.api.ProcessCommand<LandPlotModel, CreateLandPlot>("api/CreateLandPlot", new CreateLandPlot { LandPlot = (LandPlotModel)working }):
                await app.api.ProcessCommand<LandPlotModel, UpdateLandPlot>("api/UpdateLandPlot", new UpdateLandPlot { LandPlot = (LandPlotModel)working });

            if (id <= 0)
                throw new Exception("Unable to save plot");
            ((LandPlotModel)working).Id= id;
            SetEditContext((LandPlotModel)working);
            await Submitted.InvokeAsync(working);
        }
        public override async Task FreshenData()    
        {
            if (Farm is null) return;
            working = new LandPlotModel() {FarmLocationId=Farm.Id };
            if (Plot is not null)
                working = Plot;
            if (Plot is null && landPlotId.HasValue)
                working = await app.dbContext.LandPlots.FindAsync(landPlotId);
            
            SetEditContext((LandPlotModel)working);
            
        }
    }
}
