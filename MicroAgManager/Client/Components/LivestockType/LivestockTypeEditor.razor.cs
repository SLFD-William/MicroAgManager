using Domain.Models;
using FrontEnd.Persistence;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;

namespace FrontEnd.Components.LivestockType
{
    public partial class LivestockTypeEditor
    {
        [CascadingParameter] IFrontEndApiServices api { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [Parameter] public EventCallback<LandPlotModel> Submitted { get; set; }
        [Parameter] public long? livestockTypeId { get; set; }
        public async Task OnSubmit()
        {
            try
            {
                //var state = plot.Id <= 0 ? EntityState.Added : EntityState.Modified;

                //if (state == EntityState.Added)
                //    plot.Id = await api.ProcessCommand<LandPlotModel, CreateLandPlot>("api/CreateLandPlot", new CreateLandPlot { LandPlot = plot });
                //else
                //    plot.Id = await api.ProcessCommand<LandPlotModel, UpdateLandPlot>("api/UpdateLandPlot", new UpdateLandPlot { LandPlot = plot });

                //dbContext.Attach(plot);
                //dbContext.Entry(plot).State = state;
                //await dbContext.SaveChangesAsync();
                //editContext = new EditContext(plot);
                //await Submitted.InvokeAsync(plot);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
