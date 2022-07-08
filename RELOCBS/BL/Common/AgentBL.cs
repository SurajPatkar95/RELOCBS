using RELOCBS.Common.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System.Data;
using RELOCBS.DAL.Common;

namespace RELOCBS.BL.Common
{
    public class AgentBL
    {

        private AgentDAL _agentDAL;

        public AgentDAL agentDAL
        {

            get
            {
                if (this._agentDAL == null)
                    this._agentDAL = new AgentDAL();
                return this._agentDAL;
            }
        }

        public bool Insert(AgentViewModel agent, out string result)
        {
            try
            {
                return agentDAL.Insert(agent, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Update(AgentViewModel agent, out string result)
        {
            result = string.Empty;
            try
            {
                return agentDAL.Update(agent, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentBL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;
            try
            {
                return agentDAL.DeleteById(id, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentBL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public AgentViewModel GetDetailById(int? id)
        {
            AgentViewModel CompanyObj = new AgentViewModel();
            try
            {
                DataTable CompanyDt = agentDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (CompanyDt != null && CompanyDt.Rows.Count > 0)
                {

                    CompanyObj = (from rw in CompanyDt.AsEnumerable()
                                  select new AgentViewModel()
                                  {
                                      AgentID = Convert.ToInt32(rw["AgentID"]),
                                      AgentName = Convert.ToString(rw["AgentName"]),
                                      ShortAgentName = Convert.ToString(rw["ShortAgentName"]),
                                      AgentOrCorp = Convert.ToString(rw["AgentOrCorp"]),
                                      CompID = Convert.ToInt32(rw["CompID"]),
                                      CompanyName = Convert.ToString(rw["CompanyName"]),
                                      Address1 = Convert.ToString(rw["Address1"]),
                                      Address2 = Convert.ToString(rw["Address2"]),
                                      PinCode = Convert.ToString(rw["PINCode"]),
									  EmailID = Convert.ToString(rw["EmailID"]),
									  ContactPerson = Convert.ToString(rw["ContactPerson"]),
                                      ContactPhone = Convert.ToString(rw["ContactPhone"]),
                                      CityID = Convert.ToInt32(rw["CityID"]),
                                      CityName = Convert.ToString(rw["CityName"]),
                                      //BusinessLineID = Convert.ToInt32(rw["BussinessLineID"]),
                                      //BusinessLineName = Convert.ToString(rw["BussLineName"]),
                                      AgentGroupID = rw["AgentNameGrpID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["AgentNameGrpID"]),
                                      GST = Convert.ToString(rw["GSTNO"]),
                                      Isactive = Convert.ToBoolean(rw["IsActive"]),
                                      IsUseForRMC = !string.IsNullOrWhiteSpace(Convert.ToString(rw["UseforRMC"])) ? Convert.ToBoolean(rw["UseforRMC"]) : (bool?)null,
                                      issez = !string.IsNullOrWhiteSpace(Convert.ToString(rw["issez"])) ? Convert.ToBoolean(rw["issez"]) : (bool?)null,
                                      isonlyIGST = !string.IsNullOrWhiteSpace(Convert.ToString(rw["isonlyIGST"])) ? Convert.ToBoolean(rw["isonlyIGST"]) : (bool?)null,
                                      VATNO = Convert.ToString(rw["VATno"]),
                                      Fin_AccountCode = Convert.ToString(rw["Fin_AccountCode"]),
                                      CreatedDate =Convert.ToDateTime(rw["Createddate"]),
									  DynamicBankID = Convert.ToInt16(rw["DynamicBankID"]),
                                      VendorCode = Convert.ToString(rw["VendorCode"]),
                                  }).First();


                    return CompanyObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return CompanyObj;

        }

        public IEnumerable<Agent> GetAgentList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pCityID, string CorA, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Agent> AgentList = agentDAL.GetAgentList(pPageIndex, pPageSize, pOrderBy, pOrder, pCountryID, pCityID, CorA, SearchKey, LoggedinUserID, out totalCount);

                return AgentList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}