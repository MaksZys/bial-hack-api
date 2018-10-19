using System;
using System.Collections.Generic;

namespace DataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            //var blyskService = new BlyskDataImportService();
            //blyskService.ReadData();

            //var x = new MPODataImportService();
            //var result = x.ImportData();

            var y = new KomaDataImportService();
            y.ReadData();

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
