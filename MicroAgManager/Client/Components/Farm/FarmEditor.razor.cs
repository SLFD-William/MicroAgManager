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
    public partial class FarmEditor : Editor<FarmLocationModel>
    {
        [Parameter] public long? farmId { get; set; }
        [Parameter] public string? farmName { get; set; }
        [Parameter] public bool showUpdateCancelButtons { get; set; }
        [Inject] protected IGeolocationService GeoLoc { get; set; }
        [Inject] private IFrontEndApiServices api { get; set; }
        FarmLocationModel farm { get; set; }

        bool locationEnabled { get; set; } = true;
        protected override async Task OnInitializedAsync() => await FreshenData();
        protected override void OnAfterRender(bool firstRender)
        {
            if (farm is null) return;
            if (firstRender && !(farm.Longitude.HasValue || farm.Latitude.HasValue)) 
                GeoLoc.GetCurrentPosition(this, nameof(OnCoordinatesPermitted), nameof(OnErrorRequestingCoordinates));
        }
        [JSInvokable]
        public async void OnCoordinatesPermitted(GeolocationPosition position)
        {
            locationEnabled = true;
            var locChanged = farm.Latitude != position.Coords.Latitude || farm.Longitude != position.Coords.Longitude;
            if(!locChanged) return;
            farm.Latitude = position.Coords.Latitude;
            farm.Longitude = position.Coords.Longitude;
            await OnCoordinateChange();
            StateHasChanged();
        }
        private async Task OnCoordinateChange()
        {
            var geoLoc = await api.GetClosestAddress(farm.Latitude.Value, farm.Longitude.Value);
            var address=geoLoc?.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.Address;
            if (address == null) return;

            farm.StreetAddress = address.AddressLine;
            farm.City = address.Locality;
            farm.State = address.AdminDistrict;
            farm.Country = address.CountryRegion;
            farm.Zip = address.PostalCode;
            StateHasChanged();
        }
        private async Task GetGeoLocation()
        {
            var geoLoc = await app.api.GetClosestGeoLocation(farm);
            var point = geoLoc?.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.Point;
            if (point == null) return;
            farm.Latitude = point.Coordinates[0];
            farm.Longitude = point.Coordinates[1];
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
            farm = await query.OrderBy(f=>f.Id).FirstOrDefaultAsync() ?? new FarmLocationModel();
            if (string.IsNullOrEmpty(farm.Name) && !string.IsNullOrEmpty(farmName))
                farm.Name = farmName;
            editContext=new EditContext(farm);
        }
        private async void Cancel()
        {
            editContext = new EditContext(farm);
            await Cancelled.InvokeAsync();
            StateHasChanged();
        }
        public override async Task OnSubmit()
        {
            try
            {
                var id = farm.Id;
                if (!(farm.Latitude.HasValue || farm.Longitude.HasValue))
                    await GetGeoLocation();
                if (id <= 0)
                    farm.Id= await app.api.ProcessCommand<FarmLocationModel, CreateFarmLocation>("api/CreateFarmLocation", new CreateFarmLocation { Farm=farm });
                else
                    farm.Id = await app.api.ProcessCommand<FarmLocationModel, UpdateFarmLocation>("api/UpdateFarmLocation", new UpdateFarmLocation { Farm = farm });

                if (id <= 0)
                    throw new Exception("Unable to save farm location");
                farm.Id = id;
                editContext = new EditContext(farm);
                await Submitted.InvokeAsync(farm);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
