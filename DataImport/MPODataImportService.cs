using Dapper;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace DataImport
{
    public class MPODataImportService
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

        private int[] sectorArray = new int[] { 1, 3, 4 };

        public IEnumerable<TrashTransportMPO> ImportData()
        {
            int totalCounter = 0;
            List<TrashTransportMPO> trashTransportList = new List<TrashTransportMPO>();
            foreach (var x in monthsDictionary)
            {
                string sWebRootFolder = System.AppDomain.CurrentDomain.BaseDirectory;
                string sFileName = $@"Inwentaryzacja {x.Key}.xlsx";
                FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
                using (ExcelPackage package = new ExcelPackage(file))
                {
                    foreach (var sector in sectorArray)
                    {
                        ExcelWorksheet workSheet = package.Workbook.Worksheets[$"Sektor {sector}"];
                        int totalRows = workSheet.Dimension.Rows;

                        for (int i = 4; i <= totalRows; i++)
                        {
                            var trashType = SelectTrashType(workSheet, i);
                            int containerColumn = SelectContainerColumn(trashType);

                            trashTransportList.Add(new TrashTransportMPO
                            {
                                Date = new DateTime(2017, x.Value, 1),
                                Latitude = Convert.ToDouble(workSheet.Cells[i, 2].Value?.ToString()),
                                Longitude = Convert.ToDouble(workSheet.Cells[i, 3].Value?.ToString()),
                                TrashType = trashType,
                                Container = workSheet.Cells[i, containerColumn].Value?.ToString(),
                                RfId0 = workSheet.Cells[i, containerColumn + 1].Value?.ToString(),
                            });
                        }
                    }
                }
            }

            while(totalCounter < trashTransportList.Count)
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

            return trashTransportList;
        }

    private string SelectTrashType(ExcelWorksheet workSheet, int i)
    {
        if (workSheet.Cells[i, 4].Value != null)
            return "zmieszane";
        else if (workSheet.Cells[i, 6].Value != null)
            return "suche";
        else if (workSheet.Cells[i, 8].Value != null)
            return "szklo";
        else return "none";
    }

    private int SelectContainerColumn(string trashType)
    {
        if (trashType == "zmieszane")
            return 4;
        else if (trashType == "suche")
            return 6;
        else if (trashType == "szklo")
            return 8;
        else return 4;
    }
}
}
