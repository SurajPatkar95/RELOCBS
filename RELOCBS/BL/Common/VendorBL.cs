using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Common;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.BL.Common
{
    public class VendorBL
    {
        private VendorDAL _vendorDAL;

        public VendorDAL vendorDAL
        {

            get
            {
                if (this._vendorDAL == null)
                    this._vendorDAL = new VendorDAL();
                return this._vendorDAL;
            }
        }

        public bool Insert(Vendor model, out string result)
        {
            try
            {
                model.CompID = UserSession.GetUserSession().CompanyID;
                return vendorDAL.Insert(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CrewBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(Vendor model, out string result)
        {
            result = string.Empty;
            try
            {
                return vendorDAL.Update(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CrewBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return vendorDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CrewBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Vendor GetDetailById(int? id)
        {
            Vendor CrewObj = new Vendor();
            try
            {
                DataTable dt = vendorDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (dt != null && dt.Rows.Count > 0)
                {

                    CrewObj = (from rw in dt.AsEnumerable()
                               select new Vendor()
                               {
                                   Vendor_ID = Convert.ToInt32(rw["Vendor_ID"]),
                                   Vendor_RefCode = Convert.ToString(rw["Vendor_RefCode"]),
                                   Vendor_Name = Convert.ToString(rw["Vendor_Name"]),
                                   Address = Convert.ToString(rw["Address"]),
                                   Oper_MailID = Convert.ToString(rw["Oper_MailID"]),
                                   Finance_MailID = Convert.ToString(rw["Finance_MailID"]),
                                   CONTACT_PERSON = Convert.ToString(rw["CONTACT_PERSON"]),
                                   CONTACT_NUMBER = Convert.ToString(rw["CONTACT_NUMBER"]),
                                   CONTACT_FAX_NUMBER = Convert.ToString(rw["CONTACT_FAX_NUMBER"]),
                                   GST_NO = Convert.ToString(rw["GST_NO"]),
                                   PAN_NO = Convert.ToString(rw["PAN_NO"]),
                                   IsActive = Convert.ToBoolean(rw["IsActive"]),
                                   CompID = Convert.ToInt32(rw["CompID"]),
                                   CompanyName = Convert.ToString(rw["CompanyName"]),

                               }).First();

                    return CrewObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CrewBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CrewObj;

        }

        public IEnumerable<Vendor> GetVendorList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pCityID, int? pisActive, string SearchKey,out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Vendor> VendorList = vendorDAL.GetVendorList(pPageIndex, pPageSize, pOrderBy, pOrder, pCountryID, pCityID, pisActive, SearchKey, out totalCount);

                return VendorList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorBL", "GetVendorList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}