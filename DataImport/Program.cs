using System;

namespace DataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new BlyskDataImportService();
            a.ReadData();
            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
