using BialHackApi.Base.DTO;
using BialHackApi.Base.Interfaces;
using BialHackApi.Base.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BialHackApi.Web.Controllers
{

    [Route("api/[controller]/[action]")]
    public class TrashTransportController : Controller
    {
        private readonly ITrashTransportService trashTransportService;
        private readonly IMapsService mapsService;

        public TrashTransportController(ITrashTransportService trashTransportService, IMapsService mapsService)
        {
            this.trashTransportService = trashTransportService;
            this.mapsService = mapsService;
        }

        [HttpGet]
        public async Task<IEnumerable<TrashTransportDTO>> Get(int limit) => await trashTransportService.GetTrashTransports(limit);

        [HttpGet]
        public async Task<StepsMapsDrowning> GetRoute(decimal startLat, decimal startLng, decimal destLat, decimal destLng) => await mapsService.CreateStepsByCoords(startLat, startLng, destLat, destLng);
    }
}
