using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Currency : Entity
    {
        public Currency()
        {
            //this.Vendors = new List<Vendor>();
        }

        public int CurrencyID { get; set; }
        public int CountryID { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyAbbrvation { get; set; }
        public string CurrencyName { get; set; }
        //public virtual Country Country { get; set; }
        //public virtual ICollection<Vendor> Vendors { get; set; }
    }
}