using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL.Common
{
    public class AgentGroupDAL
    {
        private CommonSubs _CSubs;

        public CommonSubs CSubs
        {

            get
            {
                if (this._CSubs == null)
                    this._CSubs = new CommonSubs();
                return this._CSubs;
            }
        }


        public bool Insert(AgentGroup agent, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditAgentGroup]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.InputOutput, company.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(agent.AgentGroupName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentShortName", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(agent.ShortAgentGroupName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GSTNO", SqlDbType.VarChar, 25, ParameterDirection.Input, CSubs.PSafeValue(agent.GST));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, agent.CityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentOrCorp", SqlDbType.VarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(agent.AgentOrCorp));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(agent.Address1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(agent.Address2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PINCode", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(agent.PinCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContactPerson", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(agent.ContactPerson));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContactPhone", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(agent.ContactPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, agent.Isactive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);


                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {

                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                            throw new Exception(conn.ErrorMessage);

                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentGroupDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(AgentGroup agent, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Comm].[AddEditAgentGroup]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentID", SqlDbType.Int, 0, ParameterDirection.InputOutput, agent.AgentGroupID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(agent.AgentGroupName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentShortName", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(agent.ShortAgentGroupName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GSTNO", SqlDbType.VarChar, 25, ParameterDirection.Input, CSubs.PSafeValue(agent.GST));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, agent.CityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentOrCorp", SqlDbType.VarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(agent.AgentOrCorp));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(agent.Address1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(agent.Address2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PINCode", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(agent.PinCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContactPerson", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(agent.ContactPerson));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContactPhone", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(agent.ContactPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, agent.Isactive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));
                            if (ReturnStatus == 0)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                        else
                            throw new ArgumentException(conn.ErrorMessage);

                    }
                    else
                        throw new ArgumentException(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentGroupDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            // return true;
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("Comm.AgentDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentID", SqlDbType.Int, 0, ParameterDirection.Input, id);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {

                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

                            if (ReturnStatus == 0)
                            {

                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                            throw new Exception(conn.ErrorMessage);

                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "AgentGroupDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable CompanyDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GETAgentGroupDetail] @SP_AgentGroupID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CompanyDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "AgentGroupDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CompanyDetailDt;

        }

        public IEnumerable<AgentGroup> GetAgentList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? CountryID, int? CityID, string CorA, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec [Comm].[GETAgentGroupForGrid] @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_CountryID={4},@SP_CityID={5},@SP_CorA={6},@SP_SearchString={7},@SP_LoginID={8},@SP_CompanyID={9}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(CountryID)),
                CSubs.QSafeValue(Convert.ToString(CityID)),
                 CSubs.QSafeValue(Convert.ToString(CorA)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(LoggedinUserID),
                CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID))
                );

                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                }

                var result = (from rw in dataTable.AsEnumerable()
                              select new AgentGroup()
                              {
                                  AgentGroupID = Convert.ToInt32(rw["AgentNameGrpID"]),
                                  AgentGroupName = Convert.ToString(rw["AgentNameGrp"]),
                                  ShortAgentGroupName = Convert.ToString(rw["ShortAgentName"]),
                                  AgentOrCorp = Convert.ToString(rw["AgentOrCorp"]),
                                  CompID = Convert.ToInt32(rw["CompID"]),
                                  CompanyName = Convert.ToString(rw["CompanyName"]),
                                  Address1 = Convert.ToString(rw["Address1"]),
                                  Address2 = Convert.ToString(rw["Address2"]),
                                  PinCode = Convert.ToString(rw["PinCode"]),
                                  ContactPerson = Convert.ToString(rw["ContactPerson"]),
                                  ContactPhone = Convert.ToString(rw["ContactPhone"]),
                                  CityID = Convert.ToInt32(rw["CityID"]),
                                  CityName = Convert.ToString(rw["CityName"]),
                                  //BusinessLineID= Convert.ToInt32(rw["BussinessLineID"]),
                                  //BusinessLineName = Convert.ToString(rw["BussLineName"]),
                                  //GST = Convert.ToString(rw["GSTNO"]),
                                  Isactive = Convert.ToBoolean(rw["IsActive"])
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "AgentGroupDAL", "GetAgentList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }


    }
}