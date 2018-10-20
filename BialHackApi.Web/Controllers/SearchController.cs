using BialHackApi.Base.DTO;
using BialHackApi.Base.Interfaces;
using BialHackApi.Base.Services;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BialHackApi.Web.Controllers
{

    [Route("api/[controller]/[action]")]
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        [HttpGet]
        public async Task<IEnumerable<TrashTransportDTO>> Search(string query) => await searchService.Search(query);

        [HttpGet]
        public async Task<IEnumerable<string>> Suggest(string query) => await searchService.Suggest(query);
    }
}
