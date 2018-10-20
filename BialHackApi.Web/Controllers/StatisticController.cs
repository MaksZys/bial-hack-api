using BialHackApi.Base.DTO;
using BialHackApi.Base.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BialHackApi.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class StatisticController : Controller
    {
        private readonly StatisticService statisticService;

        public StatisticController(StatisticService statisticService)
        {
            this.statisticService = statisticService;
        }

        [HttpGet]
        public async Task<IEnumerable<TimeAnomalyDTO>> CheckForTimeAnomalies() => await statisticService.CheckForTimeAnomalies();


        [HttpGet]
        public async Task<IEnumerable<TimeAnomalyDTO>> CheckForTimeAnomaliesByVehicleNumber(string vehicleNumber) => await statisticService.CheckForTimeAnomaliesByVehicleNumber(vehicleNumber);


        [HttpGet]
        public async Task<IEnumerable<TimeAnomalyDTO>> CheckForTimeAnomaliesByMonth(int month) => await statisticService.CheckForTimeAnomaliesByMonth(month);
    }
}
