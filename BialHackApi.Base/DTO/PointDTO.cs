using System;
using System.Collections.Generic;
using System.Text;

namespace BialHackApi.Base.DTO
{
    public class PointDTO
    {
        public double Lng { get; set; }
        public double Lat { get; set; }
        public bool Visited { get; set; }
    }
}
