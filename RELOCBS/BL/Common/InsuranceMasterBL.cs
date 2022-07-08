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
    public class InsuranceMasterBL
    {
        private InsuranceMasterDAL  _insuranceMasterDAL;

        public InsuranceMasterDAL insuranceMasterDAL
        {

            get
            {
                if (this._insuranceMasterDAL == null)
                    this._insuranceMasterDAL = new InsuranceMasterDAL();
                return this._insuranceMasterDAL;
            }
        }

        public bool Insert(Insurance_Master model, out string result)
        {
            try
            {
                model.CompID = UserSession.GetUserSession().CompanyID;
                return insuranceMasterDAL.Insert(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceMasterBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(Insurance_Master model, out string result)
        {
            result = string.Empty;
            try
            {
                return insuranceMasterDAL.Update(model, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceMasterBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return insuranceMasterDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceMasterBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Insurance_Master GetDetailById(int? id)
        {
            Insurance_Master model = new Insurance_Master();
            try
            {
                DataTable Dt = insuranceMasterDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (Dt != null && Dt.Rows.Count > 0)
                {

                    model = (from rw in Dt.AsEnumerable()
                                    select new Insurance_Master()
                                    {
                                        Ins_M_Id = Convert.ToInt32(rw["InsPremiumByWriterID"]),
                                        InsComp_ID = Convert.ToInt32(rw["InsCompID"]),
                                        Policy_No = Convert.ToString(rw["PolicyNumber"]),
                                        Policy_Date = Convert.ToDateTime(rw["PolicyFromDate"]),
                                        Prem_Percent_Amt = Convert.ToDecimal(rw["PremiumPercent"]),
                                        Service_Tax = Convert.ToDecimal(rw["Service_Tax"]),
                                        Stamp_Duty = Convert.ToDecimal(rw["StampDuty"]),
                                        Min_Prem = Convert.ToDecimal(rw["Min_Prem"]),
                                        Sum_Ins = Convert.ToDecimal(rw["InsuredAmt"]),
                                        Premium_Amt = Convert.ToDecimal(rw["OpenPremiumAmt"]),
                                        Cheq_Date = Convert.ToDateTime(rw["ChqDate"]),
                                        Cheq_No = Convert.ToString(rw["ChqNo"]),
                                        Bal_Prem = Convert.ToDecimal(rw["Bal_Prem"]),
                                        Bal_SI = Convert.ToDecimal(rw["Bal_SI"]),
                                        IsActive = Convert.ToBoolean(rw["IsActive"]),
                                        CompID = Convert.ToInt32(rw["CompID"]),
                                    }).First();


                    return model;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceMasterBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;

        }

        public IEnumerable<Insurance_Master> GetInsuranceMasterList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pCityID, int? pInsCompID, int? pisActive, string SearchKey, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Insurance_Master> InsuranceMasterList = insuranceMasterDAL.GetInsuranceMasterList(pPageIndex, pPageSize, pOrderBy, pOrder, pCountryID, pCityID, pInsCompID, pisActive, SearchKey, out totalCount);

                return InsuranceMasterList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceMasterBL", "GetWarehouseBrachList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public JobDocument GetDownloadFile(int FileID)
        {
            JobDocument obj = new JobDocument();
            try
            {
                return insuranceMasterDAL.GetDownloadFile(FileID, UserSession.GetUserSession().LoginID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceMasterBL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return obj;
        }
    }
}