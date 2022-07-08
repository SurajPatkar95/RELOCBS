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
    public class VendorEvolQuestionBL
    {

        private VendorEvolQuestionDAL _vendorDAL;

        public VendorEvolQuestionDAL vendorDAL
        {

            get
            {
                if (this._vendorDAL == null)
                    this._vendorDAL = new VendorEvolQuestionDAL();
                return this._vendorDAL;
            }
        }

        public bool Insert(JobVendorEvalQuestion model, out string result)
        {
            try
            {
                return vendorDAL.Insert(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorEvolQuestionBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(JobVendorEvalQuestion model, out string result)
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorEvolQuestionBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorEvolQuestionBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public JobVendorEvalQuestion GetDetailById(int? id)
        {
            JobVendorEvalQuestion CrewObj = new JobVendorEvalQuestion();
            try
            {
                DataTable dt = vendorDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (dt != null && dt.Rows.Count > 0)
                {

                    CrewObj = (from rw in dt.AsEnumerable()
                               select new JobVendorEvalQuestion()
                               {
                                   QuestionID = Convert.ToInt32(rw["VendorEvolQstmasterID"]),
                                   QuestionDetail = Convert.ToString(rw["QuestionDet"]),
                                   IsRMCBuss = Convert.ToBoolean(rw["IsForRMC"]),
                                   CompanyID = Convert.ToInt32(rw["CompanyID"]),
                                   RateCompID = Convert.ToInt32(rw["MoveCompID"]),
                                   //RateComp = Convert.ToString(rw["RateComponentName"]),
                                   //Company = Convert.ToString(rw["CompanyName"]),
                                   Weightage = Convert.ToInt32(rw["Weightage"]),
                                   Order = Convert.ToInt32(rw["QuestionOrder"]),
                                   IsActive = Convert.ToBoolean(rw["IsActive"])

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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorEvolQuestionBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CrewObj;

        }

        public IEnumerable<JobVendorEvalQuestion> GetList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<JobVendorEvalQuestion> VendorList = vendorDAL.GetList(pPageIndex, pPageSize, pOrderBy, pOrder, pisActive, SearchKey, LoggedinUserID, out totalCount);

                return VendorList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "VendorBL", "GetList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}