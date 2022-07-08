using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class ShippingLine
    {
        [Key]
        public int ShipLineID { get; set; }

        [Required (ErrorMessage = "ShippingLine is required")]
        [Display(Name = "ShippingLine")]
        public string ShipLineName { get; set; }

        [Required(ErrorMessage = "Mode is required")]
        [Display(Name = "Mode")]
        public int ModeID { get; set; }

        [Display(Name = "Mode")]
        public string ModeName { get; set; }

        public bool Isactive { get; set; }

        public int CompID { get; set; }
        public string CompanyName { get; set; }

    }
}