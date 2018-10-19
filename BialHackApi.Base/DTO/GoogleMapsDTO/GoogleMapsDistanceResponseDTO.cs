using Newtonsoft.Json;

namespace BialHackApi.Base.DTO
{
    public class GoogleMapsDistanceResponseDTO
    {
        [JsonProperty("origin_addresses")]
        public string[] StartLocation { get; set; }

        [JsonProperty("destination_addresses")]
        public string[] TargetLocation { get; set; }

        [JsonProperty("rows")]
        public RowsDTO[] Rows { get; set; }
    }
    public class RowDTO
    {
        [JsonProperty("distance")]
        public TextValueDTO Distance { get; set; }
        [JsonProperty("duration")]
        public TextValueDTO Duration { get; set; }
    }

    public class TextValueDTO
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("value")]
        public int Value { get; set; }
    }

    public class RowsDTO
    {
        [JsonProperty("elements")]
        public RowDTO[] Elements { get; set; }
    }
}
