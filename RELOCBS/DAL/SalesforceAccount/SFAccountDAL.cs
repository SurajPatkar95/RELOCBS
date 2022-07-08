using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL.SalesforceAccount
{
    public class SFAccountDAL
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


        public IQueryable<Entities.SFAccount> GetGrid(DateTime? FromDate, DateTime? Todate,string searchType,string search)
        {
            int LoggedinUserID = UserSession.GetUserSession().LoginID;
            try
            {

                IQueryable<Entities.SFAccount> data = null;

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Salesforce].[GetSFAccountForGrid]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_Fromdate", SqlDbType.DateTime, 0, ParameterDirection.Input, FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_Todate", SqlDbType.DateTime, 0, ParameterDirection.Input, Todate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_AgentOrCorp", SqlDbType.Char, 1, ParameterDirection.Input, CSubs.PSafeValue(searchType));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentName", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(search));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        DataTable dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

                        if (dt != null)
                        {
                            var result = (from rw in dt.AsEnumerable()
                                          select new Entities.SFAccount()
                                          {
                                              TempAgentID = Convert.ToInt64(rw["TempAgentID"]),
                                              SFAccountID = Convert.ToString(rw["SFAccountID"]),
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
                                              AgentOrCorp = Convert.ToString(rw["AgentOrCorp"]),
                                              AgentOrCorpName = Convert.ToString(rw["AgentOrCorpName"]),
                                          }).ToList();
                            data = result.AsQueryable<Entities.SFAccount>();
                        }
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
                return data;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "SFAccountDAL", "GetGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;


        }

        public DataSet GetDetail(int LoginID, string SFAccountID,int TempAgentID)
        {
            DataSet Dtobj = new DataSet();
            try
            {
                string query = string.Format("[Salesforce].[GetSFAccountForDisplay] @SP_SFAccountID={0},@sp_LoginID={1},@sp_tempAgentID={2}",
                CSubs.QSafeValue(Convert.ToString(SFAccountID))
                ,CSubs.QSafeValue(Convert.ToString(LoginID))
                ,CSubs.QSafeValue(Convert.ToString(TempAgentID))
                );

                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "SFAccountDAL", "GetDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;
        }

        public bool Insert(SFAccount model, int LoginID, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Salesforce].[TransferSFAccountToAgent]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TempAgentID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.TempAgentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SFAccountID", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue( model.SFAccountID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentNameGrpID", SqlDbType.Int, 0, ParameterDirection.Input, model.AgentGroupNameID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentFName", SqlDbType.VarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(model.AgentName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShortName", SqlDbType.VarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(model.AgentshortName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Address1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(model.Address2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityId", SqlDbType.Int, 0, ParameterDirection.Input, model.CityId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PINCode", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.PINCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, model.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContactPerson", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.ContactPerson));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContactPhone", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.ContactPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailID", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.EmailID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Fin_AccountCode", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.Fin_AccountCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VATNo", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.VATNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GSTNO", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(model.GSTNO));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsOnlyIGST", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsOnlyIGST);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSez", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsSez);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentOrCorp", SqlDbType.VarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(model.AgentOrCorp));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShowForRmc", SqlDbType.Bit, 0, ParameterDirection.Input, model.AgentOrCorp.ToUpper() == "A" ? model.ShowForRmc : true);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                //model.TempAgentID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_LoadChartMasterID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "LoadChartDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}