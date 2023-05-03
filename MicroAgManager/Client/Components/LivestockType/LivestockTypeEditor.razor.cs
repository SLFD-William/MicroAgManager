using BackEnd.BusinessLogic.Livestock.Types;
using Domain.Models;
using FrontEnd.Data;
using FrontEnd.Persistence;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockType
{
    public partial class LivestockTypeEditor : IAsyncDisposable
    {
        [CascadingParameter] IFrontEndApiServices api { get; set; }
        [CascadingParameter] DataSynchronizer dbSync { get; set; }
        [CascadingParameter] FrontEndDbContext dbContext { get; set; }
        [CascadingParameter] public LivestockTypeModel livestockType { get; set; }
        [Parameter] public EventCallback<LivestockTypeModel> Submitted { get; set; }
        [Parameter] public long? livestockTypeId { get; set; }
        
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
            dbSync.OnUpdate += DbSync_OnUpdate;
            await FreshenData();
        }

        private void DbSync_OnUpdate() => Task.Run(FreshenData);

        private async Task FreshenData() 
        { 
            if(dbContext is null) dbContext = await dbSync.GetPreparedDbContextAsync();
            if(livestockType is not null)
            { 
                editContext = new EditContext(livestockType);
                StateHasChanged();
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

                if (livestockType.Id <= 0)
                    livestockType.Id = await api.ProcessCommand<LivestockTypeModel, CreateLivestockType>("api/CreateLivestockType", new CreateLivestockType { LivestockType=livestockType });
                else
                    livestockType.Id = await api.ProcessCommand<LivestockTypeModel, UpdateLivestockType>("api/UpdateLivestockType", new UpdateLivestockType { LivestockType = livestockType });

                if(livestockType.Id <= 0)
                    throw new Exception("Failed to save livestock type");

                editContext = new EditContext(livestockType);
                await Submitted.InvokeAsync(livestockType);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
        public ValueTask DisposeAsync()
        {
            dbSync.OnUpdate -= DbSync_OnUpdate;
            return ValueTask.CompletedTask;
        }
    }
}
