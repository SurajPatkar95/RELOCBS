using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Material
    {
        public int MaterialID { get; set; }

        [Display(Name ="Material Code")]
        [Required (ErrorMessage = "Material Code is required")]
        public string MaterialCode { get; set; }
        [Display(Name = "Material Name")]
        [Required(ErrorMessage = "Material Name is required")]
        public string MaterialName { get; set; }
        [Display(Name = "Material Description")]
        [Required(ErrorMessage = "Material Description is required")]
        public string MaterialDescription { get; set; }
        [Required(ErrorMessage = "Measurement is required")]
        public string Measurement { get; set; }

        [Display(Name = "Min. Qty")]
        public int? MinQty { get; set; }
        [Display(Name = "Re-Order Qty")]
        public int? ReOrderQty { get; set; }

        public double? Rate { get; set; }
        public bool IsActive { get; set; }
    }
}