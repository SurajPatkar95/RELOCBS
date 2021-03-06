using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
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
    public class CountryDAL : Repository<Country>, IDisposable
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

        public bool Insert(CountryViewModel country,out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditCountry]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryID", SqlDbType.Int, 0, ParameterDirection.InputOutput, country.CountryID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(country.CountryCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(country.CountryName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContinentID", SqlDbType.Int, 0, ParameterDirection.Input, country.Continent);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, country.isActive);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CountryDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return true;
        }

        public bool Update(CountryViewModel country, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditCountry]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryID", SqlDbType.Int, 0, ParameterDirection.InputOutput, country.CountryID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(country.CountryCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(country.CountryName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContinentID", SqlDbType.Int, 0, ParameterDirection.Input, country.ContinentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, country.isActive);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CountryDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return true;
        }

        public bool DeleteById(int id)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("Comm.CountryDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CountryDAL", "DeletedById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


            return true;
        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable CountryDetailDt = new DataTable();

            try
            {
                string query = string.Format("EXEC [Comm].[GETCountryDetail] @SP_CountryID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CountryDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CountryDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CountryDetailDt;

        }

        public IEnumerable<Country> GetCountryList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pisActive, string SearchKey, int LoggedinUserID,out int TotalCount)
        {
            TotalCount = 0;

            string query = string.Format("exec Comm.GETCountryForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_CountryID={4},@SP_isActive={5},@SP_SearchString={6},@SP_LoginID={7}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(pCountryID)),
                 CSubs.QSafeValue(Convert.ToString(pisActive)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(LoggedinUserID)
                );


            DataTable dataTable = CSubs.GetDataTable(query);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
            }

            var result = (from rw in CSubs.GetDataTable(query).AsEnumerable()
                          select new Country()
                          {
                              CountryCode = Convert.ToString(rw["CountryCode"]),
                              CountryID = Convert.ToInt32(rw["CountryID"]),
                              CountryName = Convert.ToString(rw["CountryName"]),
                              ContinentID = Convert.ToInt32(rw["continentID"]),
                              ContinentName = Convert.ToString(rw["continentName"]),
                              Isactive = Convert.ToBoolean(rw["isActive"])
                          }).ToList();


            return result;
        }

        public IEnumerable<SelectListItem> GetCountryDropdown(string LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Country] @SP_Type='ALL',@SP_Loginid={0}", CSubs.QSafeValue(LoginID)));

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}