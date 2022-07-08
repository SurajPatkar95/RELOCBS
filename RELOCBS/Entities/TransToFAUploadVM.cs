using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class TransToFAUploadVM
    {
        [Display(Name = "Application")]
        [Required(ErrorMessage = "Application selection required")]
        public int AppID { get; set; }

        [Display(Name = "Upload File")]
        [Required(ErrorMessage = "Please Upload File")]
        //[RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.xls|.xlsx|.xml)$", ErrorMessage = "Only excel files allowed.")]
        public HttpPostedFileBase file { get; set; }

        public DataTable dtTable { get; set; } = new DataTable();
    }
}