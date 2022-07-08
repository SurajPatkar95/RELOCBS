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
    public class InsuranceCompanyBL
    {

        private InsuranceCompanyDAL _insCompDAL;

        public InsuranceCompanyDAL insCompDAL
        {

            get
            {
                if (this._insCompDAL == null)
                    this._insCompDAL = new InsuranceCompanyDAL();
                return this._insCompDAL;
            }
        }

        public bool Insert(InsuranceCompany model, out string result)
        {
            try
            {
                model.CompID = UserSession.GetUserSession().CompanyID;
                return insCompDAL.Insert(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceCompanyBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(InsuranceCompany model, out string result)
        {
            result = string.Empty;
            try
            {
                return insCompDAL.Update(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceCompanyBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public InsuranceCompany GetDetailById(int? id)
        {
            InsuranceCompany Obj = new InsuranceCompany();
            try
            {
                DataTable Dt = insCompDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (Dt != null && Dt.Rows.Count > 0)
                {

                    Obj = (from rw in Dt.AsEnumerable()
                               select new InsuranceCompany()
                               {
                                   InsCompName = Convert.ToString(rw["InsCompName"]),
                                   InsCompID = Convert.ToInt32(rw["InsCompID"]),
                                   ContactNumber = Convert.ToString(rw["ContactNumber"]),
                                   ContactPerson = Convert.ToString(rw["ContactPerson"]),
                                   EmailID = Convert.ToString(rw["EmailID"]),
                                   IsActive = Convert.ToBoolean(rw["isActive"]),
                                   CompID = Convert.ToInt32(rw["CompID"]),
                               }).First();


                    return Obj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceCompanyBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return Obj;

        }

        public IEnumerable<InsuranceCompany> GetInsCompanyList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pInsCompID, int? pisActive, string SearchKey,  out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<InsuranceCompany> List = insCompDAL.GetInsCompanyList(pPageIndex, pPageSize, pOrderBy, pOrder, pInsCompID, pisActive, SearchKey, out totalCount);

                return List;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceCompanyBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}