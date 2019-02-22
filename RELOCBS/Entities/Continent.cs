using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Continent
    {
        public int ContinentID { get; set; }
        public string ContinentName { get; set; }
        public string ShortContinentName { get; set; }
        public bool Isactive { get; set; }
        public Nullable<DateTime> Createddate { get; set; }
        public string Createdby { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string Modifiedby { get; set; }

    }
}