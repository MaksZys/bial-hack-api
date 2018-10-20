using System;
using System.Collections.Generic;
using System.Text;

namespace BialHackApi.Base.DTO
{
    public class GroupDTO
    {
        public string Name { get; set; }
        public int Cars { get; set; }
        public List<PointDTO> Points { get; set; }
    }
}
