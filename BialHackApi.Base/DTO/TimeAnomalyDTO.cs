using System;
using System.Collections.Generic;
using System.Text;

namespace BialHackApi.Base.DTO
{
    public class TimeAnomalyDTO
    {
        public int SecondsDifference { get; set; }
        public DateTime Date { get; set; }
        public string RfId01 { get; set; }
        public string RfId02  { get; set; }
    }
}
