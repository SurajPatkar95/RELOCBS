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
    public class InsuranceCompanyDAL
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

        public bool Insert(InsuranceCompany model, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Ins].[AddEditInsuranceCompany]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsCompID", SqlDbType.Int, 0, ParameterDirection.InputOutput, -1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsCompName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.InsCompName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_ContactPerson", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.ContactPerson));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_ContactNo", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.ContactNumber));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_EmailID", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.EmailID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, true);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, model.CompID);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceCompanyDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(InsuranceCompany model, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Ins].[AddEditInsuranceCompany]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsCompID", SqlDbType.Int, 0, ParameterDirection.InputOutput, model.InsCompID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsCompName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.InsCompName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_ContactPerson", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.ContactPerson));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_ContactNo", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(model.ContactNumber));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@sp_EmailID", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.EmailID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, model.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, model.CompID);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceCompanyDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            // return true;
        }
        
        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable CityDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Ins].[GETInsuranceCompanyDetail] @SP_InsCompID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CityDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "InsuranceCompanyDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return CityDetailDt;

        }

        public IEnumerable<InsuranceCompany> GetInsCompanyList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pInsCompID, int? pisActive, string SearchKey, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec [Ins].[GETInsuranceCompanyForGrid] @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_InsCompID={4},@SP_isActive={5},@SP_SearchString={6},@SP_LoginID={7},@sp_CompID={8}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(pInsCompID)),
                 CSubs.QSafeValue(Convert.ToString(pisActive)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(UserSession.GetUserSession().LoginID),
                Convert.ToString(UserSession.GetUserSession().CompanyID)
                );

                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                }

                var result = (from rw in dataTable.AsEnumerable()
                              select new InsuranceCompany()
                              {
                                  InsCompName = Convert.ToString(rw["InsCompName"]),
                                  InsCompID = Convert.ToInt32(rw["InsCompID"]),
                                  ContactNumber = Convert.ToString(rw["ContactNumber"]),
                                  ContactPerson = Convert.ToString(rw["ContactPerson"]),
                                  EmailID =Convert.ToString(rw["EmailID"]),
                                  IsActive = Convert.ToBoolean(rw["isActive"]),
                                  CompID = Convert.ToInt32(rw["CompID"]),
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "InsuranceCompanyDAL", "GetInsCompanyList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }

    }
}