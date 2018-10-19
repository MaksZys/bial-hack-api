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

        public TrashTransportController(ITrashTransportService trashTransportService)
        {
            this.trashTransportService = trashTransportService;
        }

        [HttpGet]
        public async Task<IEnumerable<TrashTransportDTO>> Get(int limit) => await trashTransportService.GetTrashTransports(limit);
    }
}
