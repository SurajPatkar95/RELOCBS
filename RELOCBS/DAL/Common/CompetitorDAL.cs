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
    public class CompetitorDAL
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


        public bool Insert(CompetitorViewModel comp, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Comm].[AddEditCompetitor]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompitID", SqlDbType.Int, 0, ParameterDirection.InputOutput, company.CompBranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompitName", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(comp.CompitName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_ContactPerson", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(comp.ContactPerson));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_ContactNo", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(comp.ContactNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, comp.CompId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, comp.Isactive);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompetitorDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(CompetitorViewModel comp, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditCompetitor]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompitID", SqlDbType.Int, 0, ParameterDirection.InputOutput, company.CompBranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompitName", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(comp.CompitName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_ContactPerson", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(comp.ContactPerson));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_ContactNo", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(comp.ContactNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, comp.CompId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, comp.Isactive);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompetitorDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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

                        conn.AddCommand("Comm.CompetitorDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompetitorID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompetitorDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable CompanyDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GETCompetitorDetail] @SP_CompitID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CompanyDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CompetitorDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CompanyDetailDt;

        }

        public IEnumerable<Competitor> GetCompetitorList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? CountryID, int? CityID, int? CompID, int? pisActive, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETCompetitorForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_CountryID={4},@SP_CityID={5},@SP_CompanyID={6},@SP_isActive={7},@SP_SearchString={8},@SP_LoginID={9}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(CountryID)),
                CSubs.QSafeValue(Convert.ToString(CityID)),
                CSubs.QSafeValue(Convert.ToString(CompID)),
                 CSubs.QSafeValue(Convert.ToString(pisActive)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(LoggedinUserID)
                );

                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                }

                var result = (from rw in dataTable.AsEnumerable()
                              select new Competitor()
                              {
                                  CompitID = Convert.ToInt32(rw["CompitID"]),
                                  CompitName = Convert.ToString(rw["CompitName"]),
                                  CompanyName = Convert.ToString(rw["CompanyName"]),
                                  ContactNo = Convert.ToString(rw["ContactNo"]),
                                  ContactPerson = Convert.ToString(rw["ContactPerson"]),
                                  CompId = Convert.ToInt32(rw["CompId"]),
                                  //CityID = Convert.ToInt32(rw["CityID"]),
                                  //CityName = Convert.ToString(rw["CityName"]),
                                  Isactive = Convert.ToBoolean(rw["IsActive"]),
                                  
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "CompetitorDAL", "GetCompetitorList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }

    }
}