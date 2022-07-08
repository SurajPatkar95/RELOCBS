using MvcValidationExtensions.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class Employee
    {
        [Key]
        public int EmpID { get; set; }
        [Required()]
        [Display(Name = "Emp Name")]
        [MaxLength(length: 100, ErrorMessage = "Name can have a max of {1} characters")]
        public string EmpName { get; set; }

        [Display(Name = "Emp Code")]
        [Required()]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Only Alphabets and Numbers allowed.")]
        [MaxLength(length: 15, ErrorMessage = "EmpCode can have a max of {1} characters")]
        public string CardEmpCode { get; set; }

        [Display(Name = "Designation")]
        public int DesignationID { get; set; }
        [Display(Name = "Designation")]
        public string Designation { get; set; }

        [MaxLength(length: 250, ErrorMessage = "Address1 can have a max of {1} characters")]
        public string Address { get; set; }
        [MaxLength(length: 250, ErrorMessage = "Address2 can have a max of {1} characters")]
        public string Address2 { get; set; }
        [MaxLength(length: 20, ErrorMessage = "{0} can have a max of {1} characters")]
        public string Pincode { get; set; }
        
        [Display(Name = "City")]
        public string CityName { get; set; }
        public int? CityID { get; set; }

        [MaxLength(length: 20, ErrorMessage = "{0} can have a max of {1} characters")]
        public string Phone1 { get; set; }

        [MaxLength(length: 20, ErrorMessage = "{0} can have a max of {1} characters")]
        public string Phone2 { get; set; }

        [MaxLength(length: 20,ErrorMessage = "{0} can have a max of {1} characters")]
        public string Mobile { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        [MaxLength(length: 300, ErrorMessage = "Name can have a max of {1} characters")]
        public string Email { get; set; }
        
        public DateTime? DOB { get; set; }
        public DateTime? DOJ { get; set; }
        //[RequiredIf("IsActive", ErrorMessage = "DOL is mandatory for inactive employee.")]
        public DateTime? DOL { get; set; }
        
        [Display(Name = "Company")]
        public int Company { get; set; }
        public int CompId { get; set; }

        public bool IsContract { get; set; }
        public bool IsActive { get; set; }

        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Branch")]
        public string Branch { get; set; }
        public int BranchID { get; set; }

        [Display(Name = "Int Day Rate")]
        public float InternationDayRate { get; set; }
        [Display(Name = "Domestic Day Rate")]
        public float DomesticDayRate { get; set; }
        [Display(Name = "OT Rate")]
        public float OTRate { get; set; }

        public bool ShowWarehoueMap { get; set; }

    }

    public class EmployeeWarehouseMapping
    {
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "FromDate")]
        [Required(ErrorMessage ="From Date required")]
        public DateTime? FromDate { get; set; }

        [Display(Name ="Warehouse")]
        [Required(ErrorMessage = "Warehouse required")]
        public int WarehouseId { get; set; }
    }

    public class BranchSurveyorMapping
    {
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "FromDate")]
        [Required(ErrorMessage = "From Date required")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "Branch")]
        [Required(ErrorMessage = "Branch required")]
        public int BranchId { get; set; }
    }

    public class BranchSurveyorMappingGrid
    {
        public int EmployeeId { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "FromDate")]
        public DateTime? FromDate { get; set; }

        [Display(Name = "BranchId")]
        public int BranchId { get; set; }

        [Display(Name = "Branch")]
        public string BranchName { get; set; }
    }

}