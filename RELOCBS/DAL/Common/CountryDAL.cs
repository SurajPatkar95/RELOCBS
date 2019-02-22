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

        public bool Insert(CountryViewModel country)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("Comm.ContinentAddEdit", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContinentID", SqlDbType.Int, 0, ParameterDirection.InputOutput, country.CountryName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContinentName", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(country.Continent));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, country.isActive);
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

        public bool Update(CountryViewModel country)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("Comm.ContinentAddEdit", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContinentID", SqlDbType.Int, 0, ParameterDirection.InputOutput, country.CountryName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContinentName", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(country.Continent));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, country.isActive);
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
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("Comm.ContinentDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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

                throw;
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