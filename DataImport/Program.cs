using System;
using System.Collections.Generic;

namespace DataImport
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new MPODataImportService();
            var result = x.ImportData();
            Console.WriteLine("Hello World!");

            Console.ReadKey();
        }
    }
}
