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

        [HttpGet]
        public async Task<IEnumerable<GroupDTO>> CreateGroups()
        {
            var companies = new List<GroupDTO>
            {
                new GroupDTO
                {
                    Cars = 100,
                    Name = "Firma1",
                    Points = new List<PointDTO>()
                },
                new GroupDTO
                {
                    Cars = 100,
                    Name = "Firma2",
                    Points = new List<PointDTO>()
                },
                new GroupDTO
                {
                    Cars = 100,
                    Name = "Firma3",
                    Points = new List<PointDTO>()
                },
                new GroupDTO
                {
                    Cars = 100,
                    Name = "Firma4",
                    Points = new List<PointDTO>()
                },
            };

            var cars = 400;



            var startPoint = new PointDTO
            {
                Lat = 23,
                Lng = 51,
                Visited = true
            };

            var points = GetPoints();

            var pointsByCar = points.Count / cars + 1;

            var counter = 0;
            while (points.Where(p => !p.Visited).FirstOrDefault() != null)
            {
                var minLength = 999999999999.999;

                var nearestPoint = new PointDTO();

                foreach (var item in points.Where(p => !p.Visited))
                {
                    var val = Math.Sqrt(startPoint.Lat * startPoint.Lat + startPoint.Lng * startPoint.Lng + item.Lat * item.Lat + item.Lng * item.Lng - 2 * (startPoint.Lng * item.Lng + startPoint.Lat * item.Lat));

                    if (val < minLength)
                    {
                        minLength = val;
                        nearestPoint = new PointDTO
                        {
                            Lat = item.Lat,
                            Lng = item.Lng,
                            Visited = false
                        };
                    }
                }

                foreach (var item in points.Where(p => p.Lat == nearestPoint.Lat && p.Lng == nearestPoint.Lng).ToList())
                {
                    item.Visited = true;

                    foreach (var com in companies)
                    {
                        if (com.Points.Count <= com.Cars * pointsByCar)
                        {
                            com.Points.Add(item);
                            break;
                        }
                    }

                }

            }

            return companies;
        }

        public static List<PointDTO> GetPoints()
        {
            string sWebRootFolder = System.AppDomain.CurrentDomain.BaseDirectory;
            List<PointDTO> trashTransportList = new List<PointDTO>();
            string sFileName = $"Raport Czerwiec.xlsx";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));

            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets["szablon"];
                int totalRows = workSheet.Dimension.Rows;

                for (int i = 22; i <= totalRows; i++)
                {
                    var latitudeString = workSheet.Cells[i, 9].Value.ToString().Split(',')[0].Replace('.', ',');

                    var latitude = 0.000;

                    var longitude = 0.000;

                    if (latitudeString != "")
                    {
                        latitude = Convert.ToDouble(workSheet.Cells[i, 9].Value.ToString().Split(',')[0].Replace('.', ','));

                        longitude = Convert.ToDouble(workSheet.Cells[i, 9].Value.ToString().Split(',')[1].Replace('.', ','));
                    }

                    trashTransportList.Add(new PointDTO
                    {
                        Lat = latitude,
                        Lng = longitude,
                        Visited = false
                    });
                }

                return trashTransportList.Where(t => t.Lat > 1).ToList();
            }
        }
    }
}
