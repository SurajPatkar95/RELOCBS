using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace RELOCBS.Entities
{
    public class ATRPoint
    {
        public Int64? ATRPointId { get; set; }

        [Display(Name = "Issue Heading")]
        [Required(ErrorMessage = "Issue Heading is required")]
        public string IssueHeading { get; set; }

        [Display(Name = "Issue Description")]
        [Required(ErrorMessage = "Issue Description is required")]
        public string IssueDescription { get; set; }

        public string AuditReportSource { get; set; }

        [Display(Name = "MonthOfIssue")]
        [Required(ErrorMessage = "MonthOfIssue is required")]
        public DateTime MonthOfIssue { get; set; }

        [Display(Name = "Risk")]
        [Required(ErrorMessage = "Risk is required")]
        public int RiskId { get; set; }

        public string RiskName { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }


        public string CategoryName { get; set; }


        [Display(Name = "Compliance Status")]
        [Required(ErrorMessage = "Compliance Status is required")]
        public int ComplianceStatusId  { get; set; }

        [Display(Name = "Close Date")]
        public DateTime? CloseDate { get; set; }

        public int? AuditeeStatusId { get; set; }

        [Display(Name = "Compliance Remark")]
        public string Remark { get; set; }

        public string Department { get; set; }
        [Display(Name = "Responsible Department")]
        [Required(ErrorMessage = "Responsible Department is required")]
        public int DepartmentId { get; set; }


        public string FirstPersonRespLogin { get; set; }

        [Display(Name = "1st Responsible")]
        [Required(ErrorMessage = "1st Responsible Person is required")]
        public int FirstPersonRespLoginId { get; set; }

        public string SecondPersonRespLogin { get; set; }

        [Display(Name = "2nd Responsible")]
        [Required(ErrorMessage = "2nd Responsible Person is required")]
        public int SecondPersonRespLoginId { get; set; }


        public bool IsHO { get; set; }
        //public DepartmentResponsible departmentResponsibles = new DepartmentResponsible();
    }



    public class ATRPointReponse
    {
        public Int64? ReponseId { get; set; }
        [Required(ErrorMessage = "Status is required")]
        public int? AuditeeStatusId { get; set; }
        [Required(ErrorMessage ="Committed Date is required")]
        public DateTime? CommittedDate { get; set; }

        [Required(ErrorMessage = "Response is required")]
        public string MgtReponse { get; set; }

        public bool IsCompliance { get; set; } = false;

        public string DocDescription { get; set; }

        public HttpPostedFileBase[] files { get; set; }

        public ATRPoint aTRPoint { get; set; } = new ATRPoint();
        public List<ATRPointHistory> history { get; set; } = new List<ATRPointHistory>();
        public List<ATRPointDoc> docLists { get; set; } = new List<ATRPointDoc>();

        public bool IsHO { get; set; }

        public Int64? FileDeleteId { get; set; }

        public string submit { get; set; }
    }

    public class ATRPointDoc
    {
        public int FileID { get; set; }
        public string DocDescription { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string UploadBy { get; set; }
        public DateTime UploadDate { get; set; }
        public int UploadById { get; set; }
    }

    public class ATRPointHistory
    {
        public string StatusType { get; set; }
        public string Status { get; set; }
        public string SentBy { get; set; }
        public DateTime? CommitedDate { get; set; }
        public string UploadFiles { get; set; }
        public string ResponseType { get; set; }
        public string Response { get; set; }
        public DateTime SentDate { get; set; }

    }

    public class DepartmentResponsible
    {
        public int DepartmentId { get; set; }

        public int FirstPersonRespLoginId { get; set; }

        public int SecondPersonRespLoginId { get; set; }

        public bool IsActive { get; set; }

        public DateTime EffectiveFrom { get; set; } = System.DateTime.Now;
        public DateTime? EffectiveTo { get; set; }
    }

    public class ATRPointGrid
    {
        public Int64? ATRPointId { get; set; }

        public string IssueHeading { get; set; }

        public string IssueDescription { get; set; }

        public string AuditReportSource { get; set; }

        public DateTime MonthOfIssue { get; set; }

        public string IssuedMonth { get; set; }
        
        public string RiskName { get; set; }

        public string CategoryName { get; set; }

        public string ComplianceStatus { get; set; }

        public string AuditeeStatus { get; set; }

        public string ResponsibleDepartment { get; set; }

        public string FirstPersonResponsible { get; set; }

        public string SecondPersonResponsible { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

    }
}