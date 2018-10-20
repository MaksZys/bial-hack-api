using BialHackApi.Base.DTO;
using BialHackApi.Base.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BialHackApi.Base.Services
{
    public class TrashTransportService : ITrashTransportService
    {
        private readonly IDataConnection dataConnection;

        public TrashTransportService(IDataConnection dataConnection)
        {
            this.dataConnection = dataConnection;
        }

        public async Task<IEnumerable<TrashTransportDTO>> GetTrashTransports(int limit)
        {
            using (var connect = dataConnection.Connect())
            {
                string trashTransportQuery =
                    $@"SELECT TOP(@Limit) [Date], [Description], [RfId0], [VehicleName], [VehicleNumber], [TrashType], [Container], [Note], [MgoType], [Latitude], [Longitude]
                       FROM [dbo].[TrashTransport]";

                var result = await connect.QueryAsync<TrashTransportDTO>(trashTransportQuery, new
                {
                    Limit = 1000
                });

                return result;
            }
        }
        public async Task<IEnumerable<TrashTransportDTO>> GetTrashTransportByRefId(string rfId)
        {
            using (var connect = dataConnection.Connect())
            {
                string trashTransportQuery =
                    $@"SELECT [Id], [Date], [Description], [RfId0], [VehicleName], [VehicleNumber], [TrashType], [Container], [Note], [MgoType], [Latitude], [Longitude]
                       FROM [dbo].[TrashTransport]
                       WHERE [RfId0] = @RfId";

                var result = await connect.QueryAsync<TrashTransportDTO>(trashTransportQuery, new
                {
                    RfId = rfId
                });

                return result;
            }
        }

        public async Task<IEnumerable<TrashTransportDTO>> GetTrashTransportByVehicleNumber(string vehicleNumber)
        {
            using (var connect = dataConnection.Connect())
            {
                string trashTransportQuery =
                    $@"SELECT [Id], [Date], [Description], [RfId0], [VehicleName], [VehicleNumber], [TrashType], [Container], [Note], [MgoType], [Latitude], [Longitude]
                       FROM [dbo].[TrashTransport]
                       WHERE [VehicleNumber] = @VehicleNumber";

                var result = await connect.QueryAsync<TrashTransportDTO>(trashTransportQuery, new
                {
                    VehicleNumber = vehicleNumber
                });

                return result;
            }
        }
    }
}
