using RELOCBS.App_Code;
using RELOCBS.DAL.Repository;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.DAL.Common
{
    public class ContinentDAL : Repository<Continent>, IDisposable
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

        public bool Insert(Continent continent)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("Comm.AddEditContinent", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContinentID", SqlDbType.Int, 0, ParameterDirection.InputOutput, continent.ContinentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShortContinentName", SqlDbType.NVarChar, 25, ParameterDirection.Input, CSubs.PSafeValue(continent.ShortContinentName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContinentName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(continent.ContinentName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, continent.Isactive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            
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
                throw ex;
            }
            return true;
        }

        public bool Update(Continent continent)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("Comm.AddEditContinent", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContinentID", SqlDbType.Int, 0, ParameterDirection.InputOutput, continent.ContinentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShortContinentName", SqlDbType.NVarChar, 25, ParameterDirection.Input, CSubs.PSafeValue(continent.ShortContinentName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContinentName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(continent.ContinentName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, continent.Isactive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {

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
                throw ex;
            }

            return true;
        }

        public bool DeleteById(int id)
        {
            string result = string.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("Comm.ContinentDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContinentID", SqlDbType.Int, 0, ParameterDirection.Input, id);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {

                            result = "";
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
                throw ex;
            }


            return true;
        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable ContinentDetailDt = new DataTable();

            try
            {
                string query = string.Format("EXEC [Comm].[GETContinentDetail] @SP_ContinentID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                ContinentDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {

                throw;
            }

            return ContinentDetailDt;

        }

        public IEnumerable<Continent> GetContinentList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pContinentID, int? pisActive, string SearchKey, int LoggedinUserID,out int TotalCount)
        {
            TotalCount = 0;
            string query = string.Format("exec Comm.GETContinentForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_ContinentID={4},@SP_isActive={5},@SP_SearchString={6},@SP_LoginID={7}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(pContinentID)),
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
                          select new Continent()
                          {
                              ContinentName = Convert.ToString(rw["ContinentName"]),
                              ContinentID = Convert.ToInt32(rw["ContinentID"]),
                              ShortContinentName = Convert.ToString(rw["ShortContinentName"]),
                              Isactive = Convert.ToBoolean(rw["isActive"])
                          }).ToList();

            

            return result;
        }

        public IEnumerable<SelectListItem> GetContinentDropdown(string LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Continent] @SP_Type='ALL',@SP_Loginid={0}", CSubs.QSafeValue(LoginID)));

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}