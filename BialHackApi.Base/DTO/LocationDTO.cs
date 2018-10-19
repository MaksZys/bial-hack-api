using BialHackApi.Base.Interfaces;

namespace BialHackApi.Base.DTO
{
    public class LocationDTO : ILocation
    {
        public long Id { get; set; }
        public string ExternalId { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
