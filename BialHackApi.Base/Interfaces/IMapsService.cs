using BialHackApi.Base.Services;
using System.Threading.Tasks;

namespace BialHackApi.Base.Interfaces
{
    public interface IMapsService
    {
        Task<IDistanceDuration> CalculateDistanceByCoords(double startLat, double startLng, double targetLat, double targetLng);
        Task<ILocation> GetLocationByLatLng(decimal latitude, decimal longitude);
        Task<ILocation> GetLocationByAddress(string address);
        Task<ILocation> GetLocationByExternalId(string externalId);
        Task<StepsMapsDrowning> CreateStepsByCoords(decimal startLat, decimal startLng, decimal targetLat, decimal targetLng);
    }
}
