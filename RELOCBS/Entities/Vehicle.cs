using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Vehicle
    {
        public int VehicleID { get; set; }

        [Required]
        public string VehicleNo { get; set; }

        [Required]
        public int VendorID { get; set; }

        [Display(Name ="Vendor")]
        public string VendorName { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public double Capacity { get; set; }

        [Required]
        public double Cost { get; set; }

        [Required]
        [Display(Name ="Own Status")]
        public string VehicleType { get; set; }

        [Required]
        public int BranchID { get; set; }

        [Display(Name = "Branch")]
        public string BranchName { get; set; }

        [Required]
        public bool IsActive { get; set; }

        public DateTime? InActiveDate { get; set; }

        public int CompID { get; set; }
        public String CompanyName { get; set; }

        public int? DimensionId { get; set; }
        public String DimensionName { get; set; }
    }
}