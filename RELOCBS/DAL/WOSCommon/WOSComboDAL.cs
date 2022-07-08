using RELOCBS.App_Code;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RELOCBS.DAL.WOSCommon
{
    public class WOSComboDAL
    {
        private CommonSubs _CSubs;
        public CommonSubs CSubs
        {
            get
            {
                if (_CSubs == null)
                    _CSubs = new CommonSubs();
                return _CSubs;
            }
        }

        public IEnumerable<SelectListItem> GetWOSServiceDropdown(string LoginID, string SPTYPE = "ALLACTIVE", int? ServiceMastID = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[WOS].[ForComboServiceMaster] @SP_LoginId={0}", CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetWOSSubServiceDropdown(string LoginID, string SPTYPE = "ALLACTIVE", int? ServiceMastID = null, int? SubServiceMastID = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[WOS].[ForComboSubServiceMaster] @SP_LoginId={0},@SP_ServiceMastID={1}", CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(ServiceMastID))));
        }

        public IEnumerable<SelectListItem> GetWOSHouseDropdown(string LoginID, string SPTYPE = "ALLACTIVE", int? HouseMasterID = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[WOS].[ForComboHouseMaster] @SP_LoginId={0}, @SP_HouseMasterID={1}", CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(HouseMasterID))));
        }

        public IEnumerable<SelectListItem> GetWOSSchoolTypeDropdown(string LoginID, string SPTYPE = "ALLACTIVE", int? SchoolTypeMastID = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[WOS].[ForComboSchoolTypeMaster] @SP_LoginId={0}, @SP_SchoolTypeMastID={1}", CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(SchoolTypeMastID))));
        }

        public IEnumerable<SelectListItem> GetWOSLocationTypeDropdown(string LoginID, string SPTYPE = "ALLACTIVE", int? LocationTypeMastID = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[WOS].[ForComboLocationTypeMaster] @SP_LoginId={0}, @SP_LocationTypeMastID={1}", CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(LocationTypeMastID))));
        }
    }
}