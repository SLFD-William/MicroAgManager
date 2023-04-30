using BackEnd.BusinessLogic.Livestock.Types;
using Domain.Models;
using FrontEnd.Persistence;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockType
{
    public partial class LivestockTypeEditor
    {
        [CascadingParameter] IFrontEndApiServices api { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [Parameter] public EventCallback<LivestockTypeModel> Submitted { get; set; }
        [Parameter] public long? livestockTypeId { get; set; }
        [Parameter] public LivestockTypeModel livestockType { get; set; }
        private async Task CheckNameExists(ChangeEventArgs args)
        {
            var name = args.Value?.ToString();
            if (string.IsNullOrWhiteSpace(name)) return;
            livestockType.Name = name;
            if (!dbContext.LivestockTypes.Any(l => l.Name == livestockType.Name && l.Id!=livestockType.Id)) return;
            var check= await dbContext.LivestockTypes.FirstOrDefaultAsync(l => l.Name == livestockType.Name);
            if (check is not null) livestockType = check;
            await Submitted.InvokeAsync(livestockType);
            StateHasChanged();
        }
        public EditContext editContext { get; private set; }
        protected async override Task OnInitializedAsync()
        {
            if (dbContext is null) return;
            if(livestockType is not null)
            { 
                editContext = new EditContext(livestockType);
                return;
            }
            var query = dbContext.LivestockTypes.AsQueryable();
            if (livestockTypeId.HasValue && livestockTypeId > 0)
                query = query.Where(f => f.Id == livestockTypeId);
            livestockType = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LivestockTypeModel();
            editContext = new EditContext(livestockType);
        }
        public async Task OnSubmit()
        {
            try
            {
                var state = livestockType.Id <= 0 ? EntityState.Added : EntityState.Modified;

                if (state == EntityState.Added)
                    livestockType.Id = await api.ProcessCommand<LivestockTypeModel, CreateLivestockType>("api/CreateLivestockType", new CreateLivestockType { LivestockType=livestockType });
                else
                    livestockType.Id = await api.ProcessCommand<LandPlotModel, UpdateLivestockType>("api/UpdateLivestockType", new UpdateLivestockType { LivestockType = livestockType });

                dbContext.Attach(livestockType);
                dbContext.Entry(livestockType).State = state;
                await dbContext.SaveChangesAsync();
                editContext = new EditContext(livestockType);
                await Submitted.InvokeAsync(livestockType);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
