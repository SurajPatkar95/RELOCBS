using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class City
    {
        [Key]
        public int CityID { get; set; }
        public string CityName { get; set; }
        public string CityCode { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public bool Isactive { get; set; }
        public Nullable<DateTime> Createddate { get; set; }
        public string Createdby { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string Modifiedby { get; set; }
    }

    public partial class CityViewModel
    {
        [Key]
        public int CityID { get; set; }

        [Required(ErrorMessage = "Please enter city Code.")]
        [Display(Name = "City Code")]
        public string CityCode { get; set; }

        [Required(ErrorMessage = "Please enter city name.")]
        [Display(Name = "City Name")]
        public string CityName { get; set; }

        
        [Required(ErrorMessage = "Please select country.")]
        public int CountryID { get; set; }

        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        public bool isActive { get; set; }
        
    }
}