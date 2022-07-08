using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace RELOCBS.Entities
{
    public class Complaints
    {
        public Int64? ComplaintId { get; set; }
        [Display(Name ="Enq No.")]
        public Int32? EnqID { get; set; }
        [Display(Name = "Shipment No.")]
        public Int64? EnqDetail_ID { get; set; }
        [Display(Name = "Job No.")]
        public Int64? MoveID { get; set; }
        [Required]
        [Display(Name = "Classification")]
        public Int32 ClassificationId { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Logger Name")]
        public string Logger_Name { get; set; }
        [Required]
        [Display(Name = "Logger Email")]
        [MaxLength(250)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string Logger_Email { get; set; }
        [Display(Name = "Logger Phone")]
        [MaxLength(20)]
        [RegularExpression(@"^[\s0-9+-]*$", ErrorMessage = "Please enter correct phone")]
        public string Logger_Phone { get; set; }
        [Display(Name = "Logger Mobile")]
        [RegularExpression(@"^[\s0-9+-]*$", ErrorMessage = "Please enter correct mobile")]
        [MaxLength(20)]
        public string Logger_Mobile { get; set; }
        [Display(Name = "Status")]
        public int StatusId { get; set; } = 1;
        [Required]
        [Display(Name = "Source")]
        public int? SourceId { get; set; }
        public string LastCreatedBy { get; set; }
        public DateTime LastCreatedDate { get; set; }

        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string Shipper { get; set; }
        public string Mode { get; set; }
        public string EnqNo { get; set; }
        public string EnqDetail_No { get; set; }
        public string JoBNo { get; set; }
        public string Classification { get; set; }
        public string Source { get; set; } 
         
    }


    public class ComplaintGrid
    {
        public Int64? ComplaintId { get; set; }
        public string EnqNo { get; set; }
        public string EnqDetail_ID { get; set; }
        public string JoBNo { get; set; }
        public string Classification { get; set; }
        public string Description { get; set; }
        public string Logger_Name { get; set; }
        public string Logger_Email { get; set; }
        public string Logger_Phone { get; set; }
        public string Logger_Mobile { get; set; }
        public string Status { get; set; }
        public string Source { get; set; }
        public string LastCreatedBy { get; set; }
        public DateTime LastCreatedDate { get; set; }
        public string Shipper { get; set; }
        public string Serviceline { get; set; }
        public string BusinessLine { get; set; }
        public string AccountName { get; set; }
        public string AgentName { get; set; }
        public string Mode { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
    }

    public class EnqJobDto
    {
        public string FromCity { get; set; }
        public string ToCity { get; set; }
        public string Mode { get; set; }
        public string Shipper { get; set; }
    }
}