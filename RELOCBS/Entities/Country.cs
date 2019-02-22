using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Country
    {

        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public int ContinentID { get; set; }
        public string ContinentName { get; set; }
        public bool Isactive { get; set; }
        public Nullable<DateTime> Createddate { get; set; }
        public string Createdby { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string Modifiedby { get; set; }
        
    }


    public partial class CountryViewModel
    {
        [Key]
        public int? CountryID { get; set; }

        [RegularExpression(@"^[a-zA-Z().''/\s-]*$", ErrorMessage = "Please enter alphabets only.")]
        [Display(Name = "Country Code")]
        [Required(ErrorMessage = "Please enter country Code.")]
        public string CountryCode { get; set; }

        [RegularExpression(@"^[a-zA-Z().''/\s-]*$", ErrorMessage = "Please enter alphabets only.")]
        [Display(Name = "Country Name")]
        [Required(ErrorMessage = "Please enter country.")]
        public string CountryName { get; set; }

        public int ContinentID { get; set; }

        [Required(ErrorMessage = "Please enter continent.")]
        public string Continent { get; set; }
        
        public bool isActive { get; set; }
    }

}