using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class EmployeeWages
    {
        public int EmpID { get; set; }
        
        [Display(Name = "Emp Name")]
        public string EmpName { get; set; }

        [Display(Name = "Emp Code")]
        public string CardEmpCode { get; set; }

        [Display(Name = "Designation")]
        public int DesignationID { get; set; }

        [Display(Name = "Designation")]
        public string Designation { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }
        
        public decimal? SalaryAmt { get; set; }
        
        public decimal? OtherAmt { get; set; }

    }
}