using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Crew
    {
        public int CrewID { get; set; }

        [Required(ErrorMessage = "Crew Code is required")]
        [Display(Name = "Crew Code")]
        public string CrewCode { get; set; }

        [Required(ErrorMessage = "Superviser is required")]
        [Display(Name = "Superviser")]
        public int  SuperviserID { get; set; }

        public string Superviser { get; set; }

        [Required(ErrorMessage = "Service Line is required")]
        [Display(Name = "Service Line")]
        public int ServiceLineID { get; set; }

        public string ServiceLine { get; set; }

        public bool IsActive { get; set; }

        public List<CrewMember> members { get; set; } = new List<CrewMember>();

        public int CompID { get; set; }

        public string CompanyName { get; set; }

    }


    public class CrewMember
    {
        public Int64 CWMID { get; set; }

        public int EmpID { get; set; }

        [Display(Name = "Emp Code")]
        public string CardEmpCode { get; set; }

        [Display(Name = "Emp Name")]
        public string EmpName { get; set; }
        
        [Display(Name = "Designation")]
        public int DesignationID { get; set; }

        public string Designation { get; set; }

        public DateTime? EffectiveFrom { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public float? Rate { get; set; }

        public int? NoOfDays { get; set; }

        public float? OTRate { get; set; }

        public int? OTHrs { get; set; }

        public float? OTCost { get; set; }

        public float? Total { get; set; }

        public bool IsActive { get; set; }

        public Int32 PJR_Status { get; set; }

        public string JobNo { get; set; }

        public bool IsSupervisor { get; set; }

        public bool ShowIsSupervisor { get; set; }
    }

    public class CrewModel
    {
        public int CrewID { get; set; }

        public int MoveID { get; set; }

        public string JobNo { get; set; }

        public string CrewCode { get; set; }

        public int SuperviserID { get; set; }
        public string Superviser { get; set; }
        
        public DateTime FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public List<CrewMember> members { get; set; } = new List<CrewMember>();
        
    }



}