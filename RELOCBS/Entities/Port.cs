using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Port
    {
        public int PortID { get; set; }
        public string PortName { get; set; }
        public string PortCode { get; set; }
        public string AirorSea { get; set; }
        public int ModeID { get; set; }
        public int? CityID { get; set; }
        public string CityName { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public bool Isactive { get; set; }
    }
}