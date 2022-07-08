using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.LoadChart;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using RELOCBS.Entities;
using System.Data;
using RELOCBS.DAL.SalesforceAccount;

namespace RELOCBS.BL.SalesforceAccount
{
    public class SFAccountBL
    {

        private SFAccountDAL _accountDAL;

        public SFAccountDAL accountDAL
        {

            get
            {
                if (this._accountDAL == null)
                    this._accountDAL = new SFAccountDAL();
                return this._accountDAL;
            }
        }


        public IEnumerable<Entities.SFAccount> GetGrid(DateTime? FromDate, DateTime? Todate,string searchType,string search, string sort, string sortdir, int skip, int pageSize, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IQueryable<Entities.SFAccount> List = accountDAL.GetGrid(FromDate, Todate, searchType, search);
                if (List != null)
                {
                    totalCount = List.Count();

                    if (pageSize > 1)
                    {
                        List = List.Skip((skip * (pageSize - 1))).Take(skip);
                    }
                    else
                    {
                        List = List.Take(skip);
                    }

                    if (!string.IsNullOrEmpty(sort))
                    {
                        List = List.OrderBy(sort + " " + sortdir);
                    }
                    return List.ToList();
                }
                else
                {
                    return new List<Entities.SFAccount>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "SFAccountBL", "GetJobReportList", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public SFAccount GetDetail(String SFAccountID,int TempAgentID)
        {
            SFAccount model = new SFAccount();

            try
            {
                DataSet data = accountDAL.GetDetail(UserSession.GetUserSession().LoginID, SFAccountID, TempAgentID);

                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {
                        model = (from rw in data.Tables[0].AsEnumerable()
                                 select new SFAccount()
                                 {
                                     TempAgentID = Convert.ToInt64(rw["TempAgentID"]),
                                     SFAccountID = Convert.ToString(rw["SFAccountID"]),
                                     AgentshortName = Convert.ToString(rw["AgentName"]),
                                     AgentName = Convert.ToString(rw["AgentName"]),
                                     AgentFName = Convert.ToString(rw["AgentFName"]),
                                     AgentLName = Convert.ToString(rw["AgentLName"]),
                                     Address1 = Convert.ToString(rw["Address1"]),
                                     Address2 = Convert.ToString(rw["Address2"]),
                                     CityName = Convert.ToString(rw["CityName"]),
                                     StateName = Convert.ToString(rw["StateName"]),
                                     CountryName = Convert.ToString(rw["CountryName"]),
                                     CompName = Convert.ToString(rw["CompName"]),
                                     EmailID = Convert.ToString(rw["EmailID"]),
                                     PINCode = Convert.ToString(rw["PINCode"]),
                                     Fin_AccountCode = Convert.ToString(rw["Fin_AccountCode"]),
                                     GSTNO = Convert.ToString(rw["GSTNO"]),
                                     VATNo = Convert.ToString(rw["VATNo"]),
                                     CityId = !string.IsNullOrWhiteSpace(Convert.ToString(rw["CityID"])) ?  Convert.ToInt32(rw["CityID"]): (int?)null,
                                     StateId = !string.IsNullOrWhiteSpace(Convert.ToString(rw["StateID"])) ?  Convert.ToInt32(rw["StateID"]): (int?)null,
                                     CountryId = !string.IsNullOrWhiteSpace(Convert.ToString(rw["CountryID"])) ? Convert.ToInt32(rw["CountryID"]): (int?)null,
                                     CompID = !string.IsNullOrWhiteSpace(Convert.ToString(rw["CompID"])) ?  Convert.ToInt32(rw["CompID"]): (int?)null,
                                     AgentOrCorp = Convert.ToString(rw["AgentOrCorp"]),

                                 }).FirstOrDefault();

                    }

                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {
                        model.ContactPerson = Convert.ToString(data.Tables[1].Rows[0]["ContactName"]);
                        model.ContactPhone = Convert.ToString(data.Tables[1].Rows[0]["ContactPhone"]);
                    }
                    
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "SFAccountBL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }

        public bool Insert(SFAccount model, out string result)
        {
            try
            {
                return accountDAL.Insert(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "LoadChartBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
    }
}