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
    public class CityDAL : Repository<City>, IDisposable
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

        public bool Insert(CityViewModel city,out string result)
        {
            result=string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditCity]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.InputOutput, (city.CityID<=0 ? -1 : city.CityID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(city.CityCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(city.CityName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_CountryID", SqlDbType.Int, 0, ParameterDirection.Input, (city.CountryID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_StateID", SqlDbType.Int, 0, ParameterDirection.Input, (city.StateID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, city.isActive);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(CityViewModel city,out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditCity]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.InputOutput, city.CityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(city.CityCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(city.CityName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_CountryID", SqlDbType.Int, 0, ParameterDirection.Input, (city.CountryID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_StateID", SqlDbType.Int, 0, ParameterDirection.Input, (city.StateID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, city.isActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE,"@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));
                            if (ReturnStatus==0)
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

           // return true;
        }

        public bool DeleteById(int id,out string result)
        {
            result = string.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("Comm.CityDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id,int LoginID)
        {
             DataTable CityDetailDt= new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GETCityDetail] @SP_CityID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CityDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CityDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CityDetailDt;

        }

        public IEnumerable<City> GetCityList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCityID, int? pisActive, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETCityForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_CityID={4},@SP_isActive={5},@SP_SearchString={6},@SP_LoginID={7}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(pCityID)),
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
                              select new City()
                              {
                                  CityName = Convert.ToString(rw["CityName"]),
                                  CityID = Convert.ToInt32(rw["CityID"]),
                                  CountryID = Convert.ToInt32(rw["CountryID"]),
                                  CountryName = Convert.ToString(rw["CountryName"]),
                                  StateName = Convert.ToString(rw["StateName"]),
                                  Isactive = Convert.ToBoolean(rw["isActive"])
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "CityDAL", "GetCityList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }

        public IEnumerable<SelectListItem> GetCityDropdown(string LoginID)
        {
            try
            {
                return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_City] @SP_Type='ALL',@SP_Loginid={0}", CSubs.QSafeValue(LoginID)));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CityDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }


        public IEnumerable<SelectListItem> GetCountryByContinent(string ContinentID, string LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Country] @SP_Type='ALL',@SP_Loginid={0}", CSubs.QSafeValue(LoginID)));
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }
}