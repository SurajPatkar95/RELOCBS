using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class State
    {
        [Key]
        public int StateID { get; set; }
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public int CountryID { get; set; }
        public string CountryName { get; set; }
        public bool Isactive { get; set; }
        public Nullable<DateTime> Createddate { get; set; }
        public string Createdby { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string Modifiedby { get; set; }

    }

    public partial class StateViewModel
    {
        [Key]
        public int StateID { get; set; }

        [Display(Name = "State Code")]
        public string StateCode { get; set; }

        [Required(ErrorMessage = "Please enter State name.")]
        [Display(Name = "State Name")]
        public string StateName { get; set; }
        
        [Required(ErrorMessage = "Please select country.")]
        public int CountryID { get; set; }

        [Display(Name = "Country Name")]
        public string CountryName { get; set; }

        public bool isActive { get; set; }

    }
}