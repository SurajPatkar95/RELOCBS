using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Language : Entity
    {
        public int LanguageID { get; set; }
        public string LanguageName { get; set; }
        //public virtual ICollection<Vendor> Vendors { get; set; }

        public Language()
        {
            //this.Vendors = new List<Vendor>();
        }

        
    }
}