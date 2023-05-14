using BackEnd.BusinessLogic.Livestock.Types;
using Domain.Entity;
using Domain.Models;
using FrontEnd.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

namespace FrontEnd.Components.LivestockType
{
    public partial class LivestockTypeEditor : Editor<LivestockTypeModel>
    {
        [CascadingParameter] public LivestockTypeModel livestockType { get; set; }
        [Parameter] public long? livestockTypeId { get; set; }
        public string Name { get => livestockType.Name;
             set
                { if(!CheckNameExists(value).Result) livestockType.Name = value; } }
        private async Task<bool> CheckNameExists(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return false;
            if (!dbContext.LivestockTypes.Any(l => l.Name == livestockType.Name && l.Id != livestockType.Id)) return false;
            var check= await dbContext.LivestockTypes.FirstOrDefaultAsync(l => l.Name == name);
            if (check is null) return false;
            livestockType = check;
            await Submitted.InvokeAsync(livestockType);
            editContext.MarkAsUnmodified();
            StateHasChanged();
            return true;
        }
        public override async Task FreshenData() 
        {
            if (_submitting) return;
            if (dbContext is null) dbContext = await dbSync.GetPreparedDbContextAsync();
            
            var query = dbContext.LivestockTypes.AsQueryable();
            if (livestockTypeId.HasValue && livestockTypeId > 0)
                query = query.Where(f => f.Id == livestockTypeId);
            livestockType = new LivestockTypeModel();
            if (!createOnly) 
                livestockType = await query.OrderBy(f => f.Id).FirstOrDefaultAsync() ?? new LivestockTypeModel();

            editContext = new EditContext(livestockType);
        }
        public override async Task OnSubmit()
        {
            try
            {
                _submitting = true;
                var id = livestockType.Id;
                if (id <= 0)
                    id = await api.ProcessCommand<LivestockTypeModel, CreateLivestockType>("api/CreateLivestockType", new CreateLivestockType { LivestockType = livestockType });
                else
                    id = await api.ProcessCommand<LivestockTypeModel, UpdateLivestockType>("api/UpdateLivestockType", new UpdateLivestockType { LivestockType = livestockType });

                if (id <= 0)
                    throw new Exception("Failed to save livestock type");

                livestockType.Id = id;
                editContext = new EditContext(livestockType);
                await Submitted.InvokeAsync(livestockType);
                _submitting = false;
                if (createOnly)
                {
                    livestockType = new LivestockTypeModel();
                    editContext = new EditContext(livestockType);
                    editContext.MarkAsUnmodified();
                    await Submitted.InvokeAsync(livestockType);
                }
                StateHasChanged();
            }
            catch (Exception ex)
            { }
            finally { _submitting = false; }
                            
        }
    }
}
