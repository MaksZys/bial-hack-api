using Dapper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace DataImport
{
    public class BlyskDataImportService
    {
        private Dictionary<string, int> monthsDictionary = new Dictionary<string, int>
        {
            { "Styczeń", 1 },
            { "Luty", 2 },
            { "Marzec", 3 },
            { "Kwiecień", 4 },
            { "Maj", 5 },
            { "Czerwiec", 6 },
            { "Lipiec", 7 },
            { "Sierpień", 8 },
            { "Wrzesień", 9 },
            { "Październik", 10 },
            { "Listopad", 11},
            { "Grudzień", 12 },
        };

        public void ReadData()
        {
            string sWebRootFolder = System.AppDomain.CurrentDomain.BaseDirectory;
            List<TrashTransportBlysk> trashTransportList = new List<TrashTransportBlysk>();
            foreach (KeyValuePair<string, int> month in monthsDictionary)
            {

                string sFileName = $"Raport {month.Key}.xlsx";
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
                        var c = workSheet.Cells[i, 1].Value.ToString();
                        DateTime date = DateTime.FromOADate(Convert.ToDouble(c));

                        if (latitudeString != "")
                        {
                            latitude = Convert.ToDouble(workSheet.Cells[i, 9].Value.ToString().Split(',')[0].Replace('.', ','));

                            longitude = Convert.ToDouble(workSheet.Cells[i, 9].Value.ToString().Split(',')[1].Replace('.', ','));
                        }

                        trashTransportList.Add(new TrashTransportBlysk
                        {
                            Date = date,
                            Container = workSheet.Cells[i, 16].Value?.ToString(),
                            Description = workSheet.Cells[i, 2].Value?.ToString(),
                            MgoType = workSheet.Cells[i, 14].Value?.ToString(),
                            RfId0 = workSheet.Cells[i, 3].Value?.ToString(),
                            Note = workSheet.Cells[i, 17].Value?.ToString(),
                            VehicleName = workSheet.Cells[i, 6]?.Value.ToString(),
                            VehicleNumber = workSheet.Cells[i, 7]?.Value.ToString(),
                            Latitude = latitude,
                            Longitude = longitude,
                            TrashType = workSheet.Cells[i, 15].Value?.ToString()
                        });
                    }
                }
            }

            var totalCounter = 0;

            while (totalCounter < trashTransportList.Count)
            {
                using (var connect = new SqlConnection("Server=tcp:bial-hack-server.database.windows.net,1433;Initial Catalog=bial-hack-db;Persist Security Info=False;User ID=bialhack;Password=Hackathon1@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
                {
                    while (true)
                    {
                        string insertTrashTransportQuery =
                             $@"INSERT INTO [TrashTransport]
                        OUTPUT INSERTED.[Id]
                        VALUES (@Date, @Description, @RfId0, @VehicleName, @VehicleNumber, @TrashType, @Container, @Note, @MgoType, @Latitude, @Longitude)";

                        var trashTransportId = connect
                            .QueryFirst<int>(insertTrashTransportQuery, new
                            {
                                trashTransportList[totalCounter].Date,
                                trashTransportList[totalCounter].Description,
                                trashTransportList[totalCounter].RfId0,
                                trashTransportList[totalCounter].VehicleName,
                                trashTransportList[totalCounter].VehicleNumber,
                                trashTransportList[totalCounter].TrashType,
                                trashTransportList[totalCounter].Container,
                                trashTransportList[totalCounter].Note,
                                trashTransportList[totalCounter].MgoType,
                                trashTransportList[totalCounter].Latitude,
                                trashTransportList[totalCounter].Longitude,
                            });

                        totalCounter++;
                        if (totalCounter % 100 == 0) break;
                    }
                }
            }
        }
    }
}
