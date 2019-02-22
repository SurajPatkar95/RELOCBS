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
    public class CompanyDAL
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


        public bool Insert(CompanyViewModel company, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditCompany]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.InputOutput, company.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(company.CompanyName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShortCompanyName", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(company.ShortCompanyName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, company.CityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 100, ParameterDirection.Input, company.Address1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 100, ParameterDirection.Input, company.Address2);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address3", SqlDbType.VarChar, 100, ParameterDirection.Input, company.Address3);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ZIPNO", SqlDbType.VarChar, 10, ParameterDirection.Input, company.ZIPNO);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, company.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);


                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_Message"));

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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompanyDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(CompanyViewModel company, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditCompany]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.InputOutput, company.CompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(company.CompanyName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShortCompanyName", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(company.ShortCompanyName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, company.CityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 100, ParameterDirection.Input, company.Address1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 100, ParameterDirection.Input, company.Address2);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address3", SqlDbType.VarChar, 100, ParameterDirection.Input, company.Address3);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ZIPNO", SqlDbType.VarChar, 10, ParameterDirection.Input, company.ZIPNO);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, company.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_Message"));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompanyDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
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

                        conn.AddCommand("Comm.CompanyDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, id);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompanyDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable CompanyDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GETCompanyDetail] @SP_CompanyID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CompanyDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CompanyDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CompanyDetailDt;

        }

        public IEnumerable<Company> GetCompanyList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? RateTypeGrpID, int? CountryID, int? CityID, int? pisActive, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETCompanyForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_OriginCityID={4},@SP_DesitnationCityID={5},@SP_isActive={6},@SP_SearchString={7},@SP_LoginID={8}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                 CSubs.QSafeValue(Convert.ToString(RateTypeGrpID)),
                CSubs.QSafeValue(Convert.ToString(CountryID)),
                CSubs.QSafeValue(Convert.ToString(CityID)),
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
                              select new Company()
                              {
                                  CompanyName = Convert.ToString(rw["CompanyName"]),
                                  ShortCompanyName = Convert.ToString(rw["ShortCompanyName"]),
                                  CompID = Convert.ToInt32(rw["CompID"]),
                                  CityID = Convert.ToInt32(rw["CityID"]),
                                  Address1 = Convert.ToString(rw["Address1"]),
                                  Address2 = Convert.ToString(rw["Address2"]),
                                  Address3 = Convert.ToString(rw["Address2"]),
                                  ZIPNO = Convert.ToInt32(rw["ZIPNO"]),
                                  IsActive = Convert.ToBoolean(rw["IsActive"])
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "CompanyDAL", "GetCompanyList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }



    }
}