using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class EmployeeLeaveRegister
    {
        public Dictionary<int, string> EmployeeList { get; set; } = new Dictionary<int, string>();

        public EmployeeLeaveDetail employeeLeave { get; set; } = new EmployeeLeaveDetail();

    }

    public class EmployeeLeaveDetail
    {
        public Employee employee { get; set; }

        public List<EmpLeaves> empLeaves { get; set; } = new List<EmpLeaves>();

        public int TotalLeaves { get; set; }
    } 

    public class EmpLeaves
    {
        public int RgisterID { get; set; }

        public int EmpID { get; set; }

        public string EmpName { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string  NoOfDays { get; set; }
        
    }

      
}