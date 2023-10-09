using BackEnd.BusinessLogic.FarmLocation;
using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace FrontEnd.Components.Farm
{
    public partial class FarmEditor:DataComponent<FarmLocationModel> 
    {
        [Parameter] public long? farmId { get; set; }
        [Parameter] public string? farmName { get; set; }
        
        [Inject] protected IGeolocationService GeoLoc { get; set; }
        [Inject] private IFrontEndApiServices api { get; set; }
        private ValidatedForm _validatedForm;
        protected new FarmLocationModel working { get => base.working as FarmLocationModel; set { base.working = value; } }
        bool locationEnabled { get; set; } = true;
       
        protected override void OnAfterRender(bool firstRender)
        {
            if (working is null) return;
            if (firstRender && !(working.Longitude.HasValue || working.Latitude.HasValue)) 
                GeoLoc.GetCurrentPosition(this, nameof(OnCoordinatesPermitted), nameof(OnErrorRequestingCoordinates));
        }
        [JSInvokable]
        public async void OnCoordinatesPermitted(GeolocationPosition position)  
        {
            locationEnabled = true;
            var locChanged = working.Latitude != position.Coords.Latitude || working.Longitude != position.Coords.Longitude;
            if(!locChanged) return;
            working.Latitude = position.Coords.Latitude;
            working.Longitude = position.Coords.Longitude;
            await OnCoordinateChange();
            StateHasChanged();
        }
        private async Task OnCoordinateChange()     
        {
            var geoLoc = await api.GetClosestAddress(working.Latitude.Value, working.Longitude.Value);
            var address=geoLoc?.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.Address;
            if (address == null) return;

            working.StreetAddress = address.AddressLine;
            working.City = address.Locality;
            working.State = address.AdminDistrict;
            working.Country = address.CountryRegion;
            working.Zip = address.PostalCode;
            StateHasChanged();
        }
        private async Task GetGeoLocation()
        {
            var geoLoc = await app.api.GetClosestGeoLocation(working    );
            var point = geoLoc?.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.Point;
            if (point == null) return;
            working.Latitude = point.Coordinates[0];
            working.Longitude = point.Coordinates[1];
            StateHasChanged();
        }
        [JSInvokable]
        public void OnErrorRequestingCoordinates(GeolocationPositionError error)
        {
            // TODO: consume/handle error.
            locationEnabled = false;
            StateHasChanged();
        }
        public override async Task FreshenData()
        {
            if(app?.dbContext == null) return;
            var query= app.dbContext.Farms.AsQueryable();
            if (farmId.HasValue && farmId>0)
                query = query.Where(f => f.Id == farmId);
            working  = await query.OrderBy(f=>f.Id).FirstOrDefaultAsync() ?? new FarmLocationModel();
            if (string.IsNullOrEmpty(working.Name) && !string.IsNullOrEmpty(farmName))
                working.Name = farmName;
            SetEditContext(working);
        }
        private async Task Cancel()
        {
            working =original.Map(working) as FarmLocationModel;
            SetEditContext(working);
            await Cancelled.InvokeAsync(working);
        }
        public async Task OnSubmit()
        {
            try
            {
                if (!(working.Latitude.HasValue || working.Longitude.HasValue))
                    await GetGeoLocation();

                var id=(working.Id <= 0)?
                    await app.api.ProcessCommand<FarmLocationModel, CreateFarmLocation>("api/CreateFarmLocation", new CreateFarmLocation { Farm=working }):
                    await app.api.ProcessCommand<FarmLocationModel, UpdateFarmLocation>("api/UpdateFarmLocation", new UpdateFarmLocation { Farm = working });

                if (id <= 0)
                    throw new Exception("Unable to save farm location");
                working.Id = id;
                SetEditContext(working);
                await Submitted.InvokeAsync(working);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
