using System;

namespace DataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var blyskService = new BlyskDataImportService();
            blyskService.ReadData();

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
