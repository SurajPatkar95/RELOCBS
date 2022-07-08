using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class WHAssessmentViewModel
    {
        public Int64? TransId { get; set; }
        [Required(ErrorMessage = "Audit date required")]
        public DateTime? AuditDate { get; set; }
        [Required(ErrorMessage = "Facility required")]
        public int WarehouseId { get; set; }
        public string Area { get; set; }
        public int? NoOfPeople { get; set; }
        public int? NoOfLiftVan { get; set; }
        public float? TotalVolCFT { get; set; }
        public int? NoOfLiftVanStored { get; set; }
        [Required(ErrorMessage = "Participants required")]
        public int[] Participants { get; set; }
        public string Remark { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string HFVQuestions { get; set; }

        public float ScorePercent { get; set; } = 0;

        public List<AssessmentQuestions> questions { get; set; } = new List<AssessmentQuestions>();
        public List<AssessmentQuestions> otherQuestions { get; set; } = new List<AssessmentQuestions>();
    }

    public class AssessmentQuestions
    {
        public Int64? TransId { get; set; }
        public Int64? TransDetailId { get; set; }
        public int QuestionId { get; set; }
        public string Parameter { get; set; }
        public string Desired { get; set; }
        public int QuestionOrder { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryOrder { get; set; }
        public int ResponsibilityId { get; set; }
        public string ResponsibilityName { get; set; }
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        public int Score { get; set; }
        public float? ScoreGiven { get; set; }
        public float? ScoreObtained { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public string Comments { get; set; }

    }

    public class WHAssessmentCategory
    {
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int CategoryOrder { get; set; }
        public bool IsActive { get; set; }
    }

    public class WHAssessmentQuestions
    {
        public int? QuestionId { get; set; } = -1;
        [Required]
        public string Parameter { get; set; }
        [Required]
        public string Desired { get; set; }
        [Required]
        public Int16 QuestionOrder { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        [Required]
        [Display(Name = "Responsibility")]
        public int ResponsibilityId { get; set; }
        public string ResponsibilityName { get; set; }
        [Required]
        [Display(Name = "Priority")]
        public int PriorityId { get; set; }
        public string PriorityName { get; set; }
        [Required]
        public int Score { get; set; }
        [Display(Name = "Valid From")]
        [Required(ErrorMessage ="Valid From required")]
        public DateTime? EffectiveFrom { get; set; }

        [Display (Name ="Valid To")]
        public DateTime? EffectiveTo { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class WHAssessmentQuestionGrid
    {
        public int QuestionId { get; set; }
        public string Parameter { get; set; }
        public string Desired { get; set; }
        public Int16 QuestionOrder { get; set; }
        public string CategoryName { get; set; }
        public string ResponsibilityName { get; set; }
        public string PriorityName { get; set; }
        public int Score { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public bool IsActive { get; set; }
    }

    public class WHAssessmentGrid
    {
        public Int64? TransId { get; set; }
        public DateTime AuditDate { get; set; }
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string Area { get; set; }
        public int? NoOfPeople { get; set; }
        public int? NoOfLiftVan { get; set; }
        public float? TotalVolCFT { get; set; }
        public int? NoOfLiftVanStored { get; set; }
        public string Remark { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
    }

    public class WHAssessmentReport
    {
       
       
        public DateTime? AuditDate { get; set; }
        public string Warehouse { get; set; }
        public string Area { get; set; }
        public int? NoOfPeople { get; set; }
        public int? NoOfLiftVan { get; set; }
        public float? TotalVolCFT { get; set; }
        public int? NoOfLiftVanStored { get; set; }
        
        public string[] Participants { get; set; }
        public string Remark { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        
        public float ScorePercent { get; set; } = 0;

        public List<AssessmentQuestions> questions { get; set; } = new List<AssessmentQuestions>();
    }
}