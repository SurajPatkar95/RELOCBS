using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Competitor
    {
        public int CompitID { get; set; }

        public string CompitName { get; set; }

        public int CompId { get; set; }

        public string CompanyName { get; set; }
        
        public string ContactPerson { get; set; }

        public string ContactNo { get; set; }

        public bool Isactive { get; set; }
        
    }

    public class CompetitorViewModel
    {
        public int CompitID { get; set; }

        [Display(Name = "Competitor Name")]
        [Required(ErrorMessage = "Company Name is required")]
        public string CompitName { get; set; }

        [Display(Name = "Company")]
        [Required(ErrorMessage = "Company Name is required")]
        public int CompId { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        public string ContactPerson { get; set; }

        public string ContactNo { get; set; }

        public bool Isactive { get; set; }
        
    }
    
}