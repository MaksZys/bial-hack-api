using Newtonsoft.Json;

namespace BialHackApi.Base.DTO
{
    public class GoogleMapsLocationResultDTO
    {
        [JsonProperty("result")]
        public GoogleMapsLocationDTO result { get; set; }
    }

    public class GoogleMapsLocationDTO
    {
        [JsonProperty("address_components")]
        public AddressComponent[] AddressComponents { get; set; }

        [JsonProperty("formatted_address")]
        public string Address { get; set; }

        [JsonProperty("geometry")]
        public ExternalGeometry Geometry { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("place_id")]
        public string PlaceId { get; set; }
    }

    public class AddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }
    }

    public class ExternalLocation
    {
        [JsonProperty("lat")]
        public decimal Latitude { get; set; }
        [JsonProperty("lng")]
        public decimal Longitude { get; set; }
    }

    public class ExternalGeometry
    {
        [JsonProperty("location")]
        public ExternalLocation Location { get; set; }
    }
}
