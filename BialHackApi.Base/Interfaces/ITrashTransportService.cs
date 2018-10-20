using BialHackApi.Base.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BialHackApi.Base.Interfaces
{
    public interface ITrashTransportService
    {
        Task<IEnumerable<TrashTransportDTO>> GetTrashTransports(int limit);
        Task<IEnumerable<TrashTransportDTO>> GetTrashTransportByRefId(string rfId);
        Task<IEnumerable<TrashTransportDTO>> GetTrashTransportByVehicleNumber(string vehicleNumber); 
    }
}
