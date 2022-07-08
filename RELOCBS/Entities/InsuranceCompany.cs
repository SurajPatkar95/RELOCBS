using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class InsuranceCompany
    {

        public int InsCompID { get; set; }

        [Display(Name ="Company Name")]
        public string InsCompName { get; set; }

        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailID { get; set; }
        
        public bool IsActive { get; set; }

        public int CompID { get; set; }
        public String CompanyName { get; set; }
    }
}