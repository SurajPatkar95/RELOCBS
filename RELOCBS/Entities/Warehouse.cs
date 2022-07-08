using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Warehouse
    {
        public int WH_ID { get; set; }

        [Display(Name= "Warehoue Code")]
        public string Warehoue_Code { get; set; }

        [Display(Name = "Warehoue Name")]
        public string Warehoue_Name { get; set; }

        [Display(Name = "Address")]
        public string WH_Address { get; set; }
        public int City_ID { get; set; }

        [Display(Name = "City")]
        public string CityName { get; set; }
        public int BranchID { get; set; }

        [Display(Name = "Branch")]
        public string BranchName { get; set; }
        public string Incharge { get; set; }

        [Display(Name = "Contact No.")]
        public string Contact_No { get; set; }

        [Display(Name = "No. of Crews")]
        public int? No_Of_Crews { get; set; }

        [Display(Name = "Package crew capacity/Day")]
        public int? PACKAGE_CREW_CAP_PER_DAY { get; set; }
        [Display(Name = "Delivery crew capacity/Day")]
        public int? DELIVERY_CREW_CAP_PER_DAY { get; set; }

        [Display(Name = "Location")]
        public string WARE_LOC { get; set; }

        [Display(Name = "FAX")]
        public string WH_FAX { get; set; }

        [Display(Name = "Email")]
        [MaxLength(100)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string WH_EMAIL { get; set; }

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime MofidiedDate { get; set; }

        public int CompID { get; set; }
        public string CompanyName { get; set; }

        [Display(Name = "Area")]
        public string Area { get; set; }

    }
}