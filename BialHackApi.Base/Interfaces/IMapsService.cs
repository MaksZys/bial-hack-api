using System.Threading.Tasks;

namespace BialHackApi.Base.Interfaces
{
    public interface IMapsService
    {
        Task<IDistanceDuration> CalculateDistanceByCoords(decimal startLat, decimal startLng, decimal targetLat, decimal targetLng);
        Task<ILocation> GetLocationByLatLng(decimal latitude, decimal longitude);
        Task<ILocation> GetLocationByAddress(string address);
        Task<ILocation> GetLocationByExternalId(string externalId);
    }
}
