using Newtonsoft.Json;

namespace BialHackApi.Base.DTO
{
    public class GoogleMapsGeocodingResponseDTO
    {
        [JsonProperty("results")]
        public GoogleMapsLocationDTO[] Results { get; set; }
    }
}
