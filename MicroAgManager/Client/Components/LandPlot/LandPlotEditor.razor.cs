using BackEnd.BusinessLogic.FarmLocation.LandPlots;
using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Components.Unit;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;


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
        private async void Cancel()
        {
            working = original.Map(working) as LandPlotModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync();
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
            {
                working = Plot;
                SetEditContext(working);
                return;
            }
            var query = app.dbContext.LandPlots.Where(f => f.FarmLocationId == Farm.Id).AsQueryable();
            if (parentPlotId.HasValue && parentPlotId > 0)
                query = query.Where(f => f.ParentPlotId == parentPlotId);
            if (landPlotId.HasValue && landPlotId > 0)
                query = query.Where(f => f.Id == landPlotId);
            working = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LandPlotModel { FarmLocationId= Farm.Id };
            SetEditContext(working);
            
        }
    }
}
