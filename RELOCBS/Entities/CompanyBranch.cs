using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class CompanyBranch
    {
        public int CompBranchID { get; set; }

        public int CompId { get; set; }

        public string CompBranchName { get; set; }

        public string CompanyName { get; set; }

        public int CityID { get; set; }

        public string CityName { get; set; }

        public string ContactPerson { get; set; }

        public string ContactNo { get; set; }

        public bool Isactive { get; set; }

        public bool IsRevenueBr { get; set; }
      
    }

    public class CompanyBranchViewModel
    {
        public int CompBranchID { get; set; }

        [Display(Name = "Company Name")]
        [Required(ErrorMessage = "Company Name is required")]
        public int CompId { get; set; }

        [Display(Name = "Branch Name")]
        [Required(ErrorMessage = "Branch Name is required")]
        [MaxLength(100)]
        public string CompBranchName { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required")]
        public int CityID { get; set; }

        [Display(Name = "City")]
        public string CityName { get; set; }

        public string ContactPerson { get; set; }

        public string ContactNo { get; set; }

        public bool Isactive { get; set; }

        public bool IsRevenueBr { get; set; }

    }
}