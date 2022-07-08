using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class RMC
    {
        public int RMCID { get; set; }
        public String RMCName { get; set; }
        public String ShortRMCName { get; set; }
        public int RateTypeGrpID { get; set; }
        public String RateTypeName { get; set; }
        public String Address1    { get; set; }
        public String  Address2 { get; set; }
        public String  Address3 { get; set; }
        public int? CityID { get; set; }
        public String CityName { get; set; }
        public int? CountryID { get; set; }
        public String CountryName { get; set; }
        public bool isActive { get; set; }
         public int TotalRows { get; set; }
    }

    public class RMCViewModel
    {
        [Key]
        public int RMCID { get; set; }

        [Display(Name = "RMC Name")]
        [Required(ErrorMessage = "Please enter RMC Name.")]
        public String RMCName { get; set; }
        public String ShortRMCName { get; set; }

        [Required(ErrorMessage = "Please enter RateType Name.")]
        public int RateTypeGrpID { get; set; }

        [Display(Name = "RateType Name")]
        public String RateTypeName { get; set; }

        [Display(Name = "Address1")]
        //[Required(ErrorMessage = "Please enter Address1.")]
        public String Address1 { get; set; }
        public String Address2 { get; set; }
        public String Address3 { get; set; }

        [Display(Name = "City")]
        //[Required(ErrorMessage = "Please enter City.")]
        public int? CityID { get; set; }
        public String CityName { get; set; }

        public int? CountryID { get; set; }
        public String CountryName { get; set; }

        public int? ClientID { get; set; }
        public String ClientName { get; set; }

        public bool isActive { get; set; }

    }
}