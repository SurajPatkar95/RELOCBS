using RELOCBS.DAL.WOSCommon;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RELOCBS.BL.WOSCommon
{
    public class WOSComboBL
    {
        private WOSComboDAL _WOSComboDAL;
        public WOSComboDAL WOSComboDAL
        {
            get
            {
                if (_WOSComboDAL == null)
                    _WOSComboDAL = new WOSComboDAL();
                return _WOSComboDAL;
            }
        }

        public IEnumerable<SelectListItem> GetWOSServiceDropdown(string SPTYPE = "ALLACTIVE", int? ServiceMastID = null, string SearchString = null)
        {
            return WOSComboDAL.GetWOSServiceDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, ServiceMastID, SearchString);
        }

        public IEnumerable<SelectListItem> GetWOSSubServiceDropdown(string SPTYPE = "ALLACTIVE", int? ServiceMastID = null, int? SubServiceMastID = null, string SearchString = null)
        {
            return WOSComboDAL.GetWOSSubServiceDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, ServiceMastID, SubServiceMastID, SearchString);
        }

        public IEnumerable<SelectListItem> GetWOSHouseDropdown(string SPTYPE = "ALLACTIVE", int? HouseMasterID = null, string SearchString = null)
        {
            return WOSComboDAL.GetWOSHouseDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, HouseMasterID, SearchString);
        }

        public IEnumerable<SelectListItem> GetWOSSchoolTypeDropdown(string SPTYPE = "ALLACTIVE", int? SchoolTypeMastID = null, string SearchString = null)
        {
            return WOSComboDAL.GetWOSSchoolTypeDropdown(Convert.ToString(UserSession.GetUserSession()?.LoginID ?? 0), SPTYPE, SchoolTypeMastID, SearchString);
        }

        public IEnumerable<SelectListItem> GetWOSLocationTypeDropdown(string SPTYPE = "ALLACTIVE", int? LocationTypeMastID = null, string SearchString = null)
        {
            return WOSComboDAL.GetWOSLocationTypeDropdown(Convert.ToString(UserSession.GetUserSession()?.LoginID ?? 0), SPTYPE, LocationTypeMastID, SearchString);
        }
    }
}