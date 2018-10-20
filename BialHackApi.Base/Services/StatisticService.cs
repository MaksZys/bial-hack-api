using BialHackApi.Base.DTO;
using BialHackApi.Base.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BialHackApi.Base.Services
{
    public class StatisticService
    {
        private readonly IDataConnection dataConnection;
        private readonly IMapsService mapsService;

        public StatisticService(IDataConnection dataConnection, IMapsService mapsService)
        {
            this.dataConnection = dataConnection;
            this.mapsService = mapsService;
        }

        public async Task<IEnumerable<TimeAnomalyDTO>> CheckForTimeAnomalies()
        {
            List<TimeAnomalyDTO> anomalies = new List<TimeAnomalyDTO>();

            using (var connect = dataConnection.Connect())
            {
                string trashTransportQuery =
                    $@"SELECT TOP(@Limit) [Id], [Date], [Description], [RfId0], [VehicleName], [VehicleNumber], [TrashType], [Container], [Note], [MgoType], [Latitude], [Longitude]
                       FROM [dbo].[TrashTransport]";

                var trashTransports = await connect.QueryAsync<TrashTransportDTO>(trashTransportQuery, new
                {
                    Limit = 100
                });

                var trashTransportList = trashTransports.ToArray();

                for (int i = 0; i < trashTransportList.Length -1 ; i++)
                {
                    var firstTransport = trashTransportList[i];
                    var secondTransport = trashTransportList[i + 1];
                    TimeSpan varTime = (DateTime)secondTransport.Date - (DateTime)firstTransport.Date;
                    int firstDiff = (int)varTime.TotalSeconds;

                    if (firstDiff <= 0) continue;

                    var distanceDuration = await mapsService.CalculateDistanceByCoords(firstTransport.Latitude, firstTransport.Longitude, secondTransport.Latitude, secondTransport.Longitude);
                    var secondDiffMaps = distanceDuration.Duration;

                    if (firstDiff <= 0 || secondDiffMaps <= 0) continue;
                    var timeAnomaly = Math.Max(firstDiff, secondDiffMaps) - Math.Min(firstDiff, secondDiffMaps);

                    anomalies.Add(new TimeAnomalyDTO
                    {
                        Date = firstTransport.Date,
                        SecondsDifference = timeAnomaly,
                        RfId01 = firstTransport.RfId0,
                        RfId02 = secondTransport.RfId0
                    });
                }

                return anomalies;
            }
        }

        public async Task<IEnumerable<TimeAnomalyDTO>> CheckForTimeAnomaliesByVehicleNumber(string vehicleNumber)
        {
            List<TimeAnomalyDTO> anomalies = new List<TimeAnomalyDTO>();

            using (var connect = dataConnection.Connect())
            {
                string trashTransportQuery =
                    $@"SELECT [Id], [Date], [Description], [RfId0], [VehicleName], [VehicleNumber], [TrashType], [Container], [Note], [MgoType], [Latitude], [Longitude]
                       FROM [dbo].[TrashTransport]
                       WHERE [VehicleNumber] = @VehicleNumber";

                var trashTransports = await connect.QueryAsync<TrashTransportDTO>(trashTransportQuery, new
                {
                    VehicleNumber = vehicleNumber
                });

                var trashTransportList = trashTransports.ToArray();

                for (int i = 0; i < trashTransportList.Length - 1; i++)
                {
                    var firstTransport = trashTransportList[i];
                    var secondTransport = trashTransportList[i + 1];
                    TimeSpan varTime = (DateTime)secondTransport.Date - (DateTime)firstTransport.Date;
                    int firstDiff = (int)varTime.TotalSeconds;

                    if (firstDiff <= 0) continue;

                    var distanceDuration = await mapsService.CalculateDistanceByCoords(firstTransport.Latitude, firstTransport.Longitude, secondTransport.Latitude, secondTransport.Longitude);
                    var secondDiffMaps = distanceDuration.Duration;

                    if (firstDiff <= 0 || secondDiffMaps <= 0) continue;
                    var timeAnomaly = Math.Max(firstDiff, secondDiffMaps) - Math.Min(firstDiff, secondDiffMaps);

                    anomalies.Add(new TimeAnomalyDTO
                    {
                        Date = firstTransport.Date,
                        SecondsDifference = timeAnomaly,
                        RfId01 = firstTransport.RfId0,
                        RfId02 = secondTransport.RfId0
                    });
                }

                return anomalies;
            }
        }


        public async Task<IEnumerable<TimeAnomalyDTO>> CheckForTimeAnomaliesByDate(int year, int month, int day)
        {
            List<TimeAnomalyDTO> anomalies = new List<TimeAnomalyDTO>();

            using (var connect = dataConnection.Connect())
            {
                string trashTransportQuery =
                    $@"SELECT [Id], [Date], [Description], [RfId0], [VehicleName], [VehicleNumber], [TrashType], [Container], [Note], [MgoType], [Latitude], [Longitude]
                       FROM [dbo].[TrashTransport]
                       WHERE YEAR([Date]) = @Year AND MONTH([Date]) = @Month AND DAY([Date]) = @Day";

                var trashTransports = await connect.QueryAsync<TrashTransportDTO>(trashTransportQuery, new
                {
                    Year = year,
                    Month = month,
                    Day = day
                });

                var trashTransportList = trashTransports.ToArray();

                for (int i = 0; i < trashTransportList.Length - 1; i++)
                {
                    var firstTransport = trashTransportList[i];
                    var secondTransport = trashTransportList[i + 1];
                    TimeSpan varTime = (DateTime)secondTransport.Date - (DateTime)firstTransport.Date;
                    int firstDiff = (int)varTime.TotalSeconds;

                    if (firstDiff <= 0) continue;

                    var distanceDuration = await mapsService.CalculateDistanceByCoords(firstTransport.Latitude, firstTransport.Longitude, secondTransport.Latitude, secondTransport.Longitude);
                    var secondDiffMaps = distanceDuration.Duration;

                    if (firstDiff <= 0 || secondDiffMaps <= 0) continue;
                    var timeAnomaly = Math.Max(firstDiff, secondDiffMaps) - Math.Min(firstDiff, secondDiffMaps);

                    anomalies.Add(new TimeAnomalyDTO
                    {
                        Date = firstTransport.Date,
                        SecondsDifference = timeAnomaly,
                        RfId01 = firstTransport.RfId0,
                        RfId02 = secondTransport.RfId0
                    });
                }

                return anomalies;
            }
        }
    }
}
