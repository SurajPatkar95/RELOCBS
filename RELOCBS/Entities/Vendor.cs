using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Vendor
    {

        public int Vendor_ID { get; set; }

        [Display(Name ="Name")]
        [Required(ErrorMessage = "Name is required")]
        public string Vendor_Name { get; set; }

        [Display(Name = "Ref.Code")]
        [Required(ErrorMessage = "Ref.Code is required")]
        public string Vendor_RefCode { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [Display(Name = "Operation Mail")]
        [MaxLength(200)]
        public string Oper_MailID { get; set; }

        [Display(Name = "Finance Mail")]
        [MaxLength(200)]
        public string Finance_MailID { get; set; }

        [Display(Name = "Contact Person")]
        [MaxLength(150)]
        public string CONTACT_PERSON { get; set; }

        [Display(Name = "Contact No.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string CONTACT_NUMBER { get; set; }

        [Display(Name = "Fax No.")]
        public string CONTACT_FAX_NUMBER { get; set; }

        [Display(Name = "PAN No.")]
        [MaxLength(50)]
        public string PAN_NO { get; set; }

        [Display(Name = "GST No.")]
        [MaxLength(50)]
        public string GST_NO { get; set; }

        public bool IsActive { get; set; }

        public int CompID { get; set; }
        public string CompanyName { get; set; }
    }
}