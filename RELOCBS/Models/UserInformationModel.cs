using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Models
{
    [Serializable]
    public class UserInformationModel
    {

        public int    LoginID { get; set; }
        public string LoginText { get; set; }
        public string AdLoginText { get; set; }
        public string UserName { get; set; }
        public string MobileNo { get; set; }
        public string EmpID { get; set; }
        public bool IsFirstLogin { get; set; }
        public bool IsChangePassword { get; set; }
        public string TempSessionID { get; set; }
        public int DaysLeft { get; set; }
        public string IsDummy { get; set; }
        public string PasswordExpired { get; set; }
        public string LoginType { get; set; }

        public int CompanyID { get; set; }

        public string CompanyName { get; set; }

        public string BussinessLine { get; set; }

		public int BaseCurrID { get; set; }

		public string LoginIP { get; set; }

        public string SessionID { get; set; }

        //public int LanguageID { get; set; }
        //public Nullable<int> UserTypeID { get; set; }
        //public Nullable<int> TimeZoneID { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
        public Nullable<System.DateTime> ActivatedOn { get; set; }
        public Nullable<System.DateTime> DeactivatedOn { get; set; }

        public string UserFeedbackStatus { get; set; }
    }
}