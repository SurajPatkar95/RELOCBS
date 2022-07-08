using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RELOCBS.Entities
{
    public class User : SysUser
    {


        public User()
        {
            
        }
    }

    public partial class SysUser 
    {
        [Key]
        public int UserID { get; set; }
        public int? LoginID { get; set; }
        public string UserName { get; set; }
        public string LoginText { get; set; }
        public string ADLoginText { get; set; }
        public bool IsADLogin { get; set; }
        public bool IsStandardLogin { get; set; }
        [Display(Name = "Employee")]
        public int EmpId { get; set; }
        public byte[] Password { get; set; }
        public int LanguageID { get; set; }
        public Nullable<int> UserTypeID { get; set; }
        public Nullable<int> TimeZoneID { get; set; }
        public bool isActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<System.DateTime> LoginExpiryDate { get; set; }
        public string MobileNo { get; set; }
        [Display(Name = "Activated Date")]
        public Nullable<System.DateTime> ActivatedOn { get; set; }
        [Display(Name = "Deactivated Date")]
        public Nullable<System.DateTime> DeactivatedOn { get; set; }

        [Display(Name = "Deactivated By")]
        public string DeactivatedBy { get; set; }
        public string LoginType { get; set; }
        [Display(Name = "Requisition Number")]
        public string ReqNo { get; set; }

        public DateTime? LastLogInDateTime { get; set; }
        public DateTime? PasswordExpiryDate { get; set; }
        public int PasswordExpiryDays { get; set; }
        public int AttemptCount { get; set; }

    }

    public class CreateUser
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string LoginID { get; set; }
        public byte[] Password { get; set; }
        public int LanguageID { get; set; }
        public Nullable<int> UserTypeID { get; set; }
        public Nullable<int> TimeZoneID { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public string MobileNo { get; set; }
        [Display(Name = "Activated Date")]
        public Nullable<System.DateTime> ActivatedOn { get; set; }
        [Display(Name = "Deactivated Date")]
        public Nullable<System.DateTime> DeactivatedOn { get; set; }
    }

    public class UserLocation
    {
        [Key]
        public int UserID { get; set; }
        public int[]  CountryList { get; set; }
        public int[] CityList { get; set; }
        public int[] CompList { get; set; }
        public int[] RoleList { get; set; }
        public int[] RMCList { get; set; }
        public string[] BussList { get; set; }
        public int[] MappedWarehouseList { get; set; }
        public string Type { get; set; }
        public int[] MappedReportList { get; set; }
        public int[] MappedServicelineList { get; set; }
        public int[] MappedrvbranchList { get; set; }
        public int[] MappedclickrestrictList { get; set; }

    }

    public class CountryList
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }

    }

    public class CityList
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }

    public class CompList
    {
        public int CompId { get; set; }
        public string CompName { get; set; }
    }

    public class RoleList
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class RMCList
    {
        public int RMCId { get; set; }
        public string RMCName { get; set; }
    }

    public class BussList
    {
        //public int RMCId { get; set; }
        public string BussName { get; set; }
    }


}