using BackEnd.BusinessLogic.FarmLocation;
using Domain.Models;
using FrontEnd.Components.Shared;
using FrontEnd.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace FrontEnd.Components.Farm
{
    public partial class FarmEditor:DataComponent<FarmLocationModel>
    {
        [CascadingParameter] FarmLocationModel Farm { get; set; }
        [Parameter] public long? farmId { get; set; }
        [Parameter] public string? farmName { get; set; }
        
        [Inject] protected IGeolocationService GeoLoc { get; set; }
        [Inject] private IFrontEndApiServices api { get; set; }
        private ValidatedForm _validatedForm;
        bool locationEnabled { get; set; } = true;
       
        protected override void OnAfterRender(bool firstRender)
        {
            if (((FarmLocationModel)working) is null) return;
            if (firstRender && !(((FarmLocationModel)working).Longitude.HasValue || ((FarmLocationModel)working).Latitude.HasValue)) 
                GeoLoc.GetCurrentPosition(this, nameof(OnCoordinatesPermitted), nameof(OnErrorRequestingCoordinates));
        }
        [JSInvokable]
        public void OnCoordinatesPermitted(GeolocationPosition position)  
        {
            locationEnabled = true;
            var locChanged = ((FarmLocationModel)working).Latitude != position.Coords.Latitude || ((FarmLocationModel)working).Longitude != position.Coords.Longitude;
            if(!locChanged) return;
            ((FarmLocationModel)working).Latitude = position.Coords.Latitude;
            ((FarmLocationModel)working).Longitude = position.Coords.Longitude;

            Task.Run(OnCoordinateChange);
            StateHasChanged();
        }
        private async Task OnCoordinateChange()     
        {
            var geoLoc = await api.GetClosestAddress(((FarmLocationModel)working).Latitude.Value, ((FarmLocationModel)working).Longitude.Value);
            var address=geoLoc?.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.Address;
            if (address == null) return;

            ((FarmLocationModel)working).StreetAddress = address.AddressLine;
            ((FarmLocationModel)working).City = address.Locality;
            ((FarmLocationModel)working).State = address.AdminDistrict;
            ((FarmLocationModel)working).Country = address.CountryRegion;
            ((FarmLocationModel)working).Zip = address.PostalCode;
            StateHasChanged();
        }
        private async Task GetGeoLocation()
        {
            var geoLoc = await app.api.GetClosestGeoLocation((FarmLocationModel)working    );
            var point = geoLoc?.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.Point;
            if (point == null) return;
            ((FarmLocationModel)working).Latitude = point.Coordinates[0];
            ((FarmLocationModel)working).Longitude = point.Coordinates[1];
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
            working = new FarmLocationModel();

            if(Farm is not null)
                working = Farm;
            if (Farm is null && farmId.HasValue)
                working = await app.dbContext.Farms.FindAsync(farmId);

            SetEditContext((FarmLocationModel)working);
        }
        private async Task Cancel()
        {
            working =original.Map((FarmLocationModel)working);
            SetEditContext((FarmLocationModel)working);
            await Cancelled.InvokeAsync((FarmLocationModel)working);
        }
        public async Task OnSubmit()
        {
            try
            {
                if (!(((FarmLocationModel)working).Latitude.HasValue || ((FarmLocationModel)working).Longitude.HasValue))
                    await GetGeoLocation();

                var id=(((FarmLocationModel)working).Id <= 0)?
                    await app.api.ProcessCommand<FarmLocationModel, CreateFarmLocation>("api/CreateFarmLocation", new CreateFarmLocation { Farm=(FarmLocationModel)working }):
                    await app.api.ProcessCommand<FarmLocationModel, UpdateFarmLocation>("api/UpdateFarmLocation", new UpdateFarmLocation { Farm = (FarmLocationModel)working });

                if (id <= 0)
                    throw new Exception("Unable to save farm location");
                ((FarmLocationModel)working).Id = id;
                SetEditContext((FarmLocationModel)working);
                await Submitted.InvokeAsync((FarmLocationModel)working);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
