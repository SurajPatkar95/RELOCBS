using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Entities
{
    public class CommonReport
    {
        public IEnumerable<SelectListItem> ReportNameList { get; set; } = new List<SelectListItem>();

        public List<ReportColumn>  ReportColumns { get; set; } = new List<ReportColumn>();

        [Required(ErrorMessage = "Report Name is required")]
        public int ReportID { get; set; }
        public string ReportName { get; set; }

		[Required(ErrorMessage = "Business Line is required")]
		public string[] BussLineID { get; set; }

		[Required (ErrorMessage = "Revenue Branch is required")]
        public int[] RevenueBranchId { get; set; }

        [Required(ErrorMessage = "ServiceLine is required")]
        public int[] ServiceLineId { get; set; }

        [Required(ErrorMessage = "Date Column is required")]
        public int? SelectedDateType { get; set; }
        public string SelectedDateTypeName { get; set; }

        [Required(ErrorMessage = "From Date is required")]
        public DateTime? FromDate { get; set; }

        [Required(ErrorMessage = "To Date is required")]
        public DateTime? Todate { get; set; }
        
    }

    public class ReportColumn
    {
        public string ColumnID { get; set; }
        public string ColumnName { get; set; }
        public bool Selected { get; set; }
    }

}