using System;

namespace DataImport
{
    public class TrashTransportBlysk
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string RfId0 { get; set; }
        public string VehicleName { get; set; }
        public string VehicleNumber { get; set; }
        public string TrashType { get; set; }
        public string Container { get; set; }
        public string Note { get; set; }
        public string MgoType { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
