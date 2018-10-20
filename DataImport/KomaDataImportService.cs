using Dapper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace DataImport
{
    public class KomaDataImportService
    {
        private Dictionary<string, int> monthsDictionary = new Dictionary<string, int>
        {
            { "Marzec", 3 },
            { "Kwiecień", 4 },
            { "Maj", 5 },
            { "Czerwiec", 6 },
            { "Lipiec", 7 },
            { "Sierpień", 8 },
            { "Październik", 10 },
            { "Listopad", 11},
            { "Grudzień", 12 },
        };

        public void ReadData()
        {
            string sWebRootFolder = System.AppDomain.CurrentDomain.BaseDirectory;
            List<TrashTransportKoma> trashTransportList = new List<TrashTransportKoma>();

            var worksheets = new string[] { "SEKTOR II", "SEKTOR V"};

            foreach (var worksheet in worksheets)
            {
                foreach (KeyValuePair<string, int> month in monthsDictionary)
                {

                    string sFileName = $"Inwentaryzacja {month.Key}.xlsx";
                    FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                    using (ExcelPackage package = new ExcelPackage(file))
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[worksheet];
                        int totalRows = workSheet.Dimension.Rows;

                        for (int i = 4; i <= totalRows; i++)
                        {

                            var latitude = Convert.ToDouble(workSheet.Cells[i, 2].Value?.ToString().Replace('.',','));

                            var longitude = Convert.ToDouble(workSheet.Cells[i, 1].Value?.ToString().Replace('.', ','));

                            DateTime date = new DateTime(2017, month.Value, 1);

                            trashTransportList.Add(new TrashTransportKoma
                            {
                                Date = date,
                                Latitude = latitude,
                                Longitude = longitude,
                                TrashType = workSheet.Cells[i, 6].Value?.ToString()
                            });

                            if (workSheet.Cells[i, 9].Value != null)
                            {
                                var val = workSheet.Cells[i, 9].Value.ToString();

                                var counter = trashTransportList.Count-1;

                                while(trashTransportList[counter].RfId0 == null && counter > 0)
                                {
                                    trashTransportList[counter].RfId0 = val;
                                    counter--;
                                }
                            }
                        }
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
