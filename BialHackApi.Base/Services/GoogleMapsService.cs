using BialHackApi.Base.DTO;
using BialHackApi.Base.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BialHackApi.Base.Services
{
    public class GoogleMapsService : IMapsService
    {
        private readonly IConfiguration configuration;

        public GoogleMapsService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<IDistanceDuration> CalculateDistanceByCoords(double startLat, double startLng, double targetLat, double targetLng)
        {
            string path = string.Format("https://maps.googleapis.com/maps/api/distancematrix/json?units=imperial&origins={0},{1}&destinations={2},{3}&key={4}", startLat, startLng, targetLat, targetLng, configuration["Google:GoogleDistanceMatrixApiKey"]);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(path);
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoogleMapsDistanceResponseDTO>(responseData);

            return new GoogleMapsDistanceDurationDTO
            {
                Distance = result.Rows[0].Elements[0].Distance.Value,
                Duration = result.Rows[0].Elements[0].Duration.Value,
            };
        }

        public async Task<ILocation> GetLocationByLatLng(decimal latitude, decimal longitude)
        {
            string path = string.Format("https://maps.googleapis.com/maps/api/geocode/json?latlng={0},{1}&key={2}", latitude, longitude, configuration["Google:GoogleGeocodingApiKey"]);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(path);
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoogleMapsGeocodingResponseDTO>(responseData);

            return new LocationDTO
            {
                Address = result.Results[0].Address,
                Latitude = result.Results[0].Geometry.Location.Latitude,
                Longitude = result.Results[0].Geometry.Location.Longitude,
                ExternalId = result.Results[0].PlaceId
            };
        }

        public async Task<ILocation> GetLocationByAddress(string address)
        {
            string path = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", address, configuration["Google:GoogleGeocodingApiKey"]);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(path);
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoogleMapsGeocodingResponseDTO>(responseData);

            return new LocationDTO
            {
                Address = result.Results[0].Address,
                Latitude = result.Results[0].Geometry.Location.Latitude,
                Longitude = result.Results[0].Geometry.Location.Longitude,
                ExternalId = result.Results[0].PlaceId
            };
        }

        public async Task<ILocation> GetLocationByExternalId(string externalId)
        {
            string path = string.Format("https://maps.googleapis.com/maps/api/place/details/json?placeid={0}&key={1}", externalId, configuration["Google:GoogleGeocodingApiKey"]);

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(path);
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoogleMapsLocationResultDTO>(responseData);

            return new LocationDTO
            {
                Address = result.result.Address,
                Latitude = result.result.Geometry.Location.Latitude,
                Longitude = result.result.Geometry.Location.Longitude,
                ExternalId = result.result.PlaceId,
                Title = result.result.Name != null ? result.result.Name : string.Empty
            };
        }


        //public async Task<ILocation> GetLocationByAddress(string address)
        //{
        //    string path = string.Format("https://maps.googleapis.com/maps/api/geocode/json?address={0}&key={1}", address, configuration["Google:GoogleGeocodingApiKey"]);

        //    HttpClient client = new HttpClient();
        //    HttpResponseMessage response = await client.GetAsync(path);
        //    var responseData = await response.Content.ReadAsStringAsync();
        //    var result = JsonConvert.DeserializeObject<GoogleMapsGeocodingResponseDTO>(responseData);


        //    return new GoogleMapsLocationDTO
        //    {
        //        Address = result.Results[0].Address,
        //        Geometry = result.Results[0].Geometry,
        //        PlaceId = result.Results[0].PlaceId,
        //        AddressComponents = result.Results[0].AddressComponents.Take(1).ToArray()
        //    };
        //}

        public async Task<StepsMapsDrowning> CreateStepsByCoords(decimal startLat, decimal startLng, decimal targetLat, decimal targetLng)
        {
            string path = string.Format("https://maps.googleapis.com/maps/api/directions/json?origin={0},{1}&destination={2},{3}&key={4}", startLat, startLng, targetLat, targetLng, configuration["Google:GoogleDistanceMatrixApiKey"]);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(path);
            var responseData = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<GoogleMapsRoutsResponseDTO>(responseData);

            return new StepsMapsDrowning
            {
                Steps = result.routes[0].legs[0].steps.Select(c => c.start_location).ToArray(),
                EncodedPlaces = result.routes[0].overview_polyline.points
            };
        }
    }
}
