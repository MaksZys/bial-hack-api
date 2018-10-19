using BialHackApi.Base.Interfaces;

namespace BialHackApi.Base.DTO
{
    public class GoogleMapsDistanceDurationDTO : IDistanceDuration
    {
        public int Distance { get; set; }
        public int Duration { get; set; }
    }
}
