using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Company
    {

        [Key]
        public int CompID { get; set; }
        public string CompanyName { get; set; }
        public string ShortCompanyName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public int ZIPNO { get; set; }
        public int CityID { get; set; }
        public string CityName { get; set; }
        public bool IsActive { get; set; }
        public Nullable<DateTime> Createddate { get; set; }
        public string Createdby { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string Modifiedby { get; set; }

    }

    public class CompanyViewModel
    {
        [Key]
        public int CompID { get; set; }

        [Display(Name = "Company Name")]
        [StringLength(100)]
        [Required(ErrorMessage = "Please enter Company Name.")]
        public string CompanyName { get; set; }

        [Display(Name = "Short Name")]
        [StringLength(50)]
        [Required(ErrorMessage = "Please enter Short Name.")]
        public string ShortCompanyName { get; set; }

        [Required(ErrorMessage = "Please enter Address1.")]
        [StringLength(100)]
        public string Address1 { get; set; }

        [StringLength(100)]
        public string Address2 { get; set; }

        [StringLength(100)]
        public string Address3 { get; set; }

        [Required(ErrorMessage = "Please enter ZipCode.")]
        [RegularExpression(@"^\d{6}(-\d{4})?$", ErrorMessage = "Invalid Zip")]
        [Display(Name = "ZipCode")]
        public int ZIPNO { get; set; }

        [Required(ErrorMessage = "Please select City")]
        public int CityID { get; set; }

        [Display(Name = "City")]
        public string CityName { get; set; }

        public bool IsActive { get; set; }
        
    }
}