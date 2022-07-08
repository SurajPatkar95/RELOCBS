using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using RELOCBS.Entities;
using RELOCBS.Utility;

namespace RELOCBS.DAL.Common
{
    public class EmployeeWagesDAL
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

        public IEnumerable<EmployeeWages> GetEmployeeList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pEmpID, int? pisActive, string SearchKey, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec [Warehouse].[GetEmployeeWagesGrid] @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_EmpID={4},@SP_isActive={5},@SP_SearchString={6},@SP_LoginID={7},@SP_CompID={8}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(pEmpID)),
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
                              select new EmployeeWages()
                              {
                                  EmpID = Convert.ToInt32(rw["EmpID"]),
                                  EmpName = Convert.ToString(rw["EmpName"]),
                                  CardEmpCode = Convert.ToString(rw["CardEmpCode"]),
                                  DesignationID = Convert.ToInt32(rw["DesignationID"]),
                                  Designation = Convert.ToString(rw["DesignationName"]),
                                  Mobile = Convert.ToString(rw["Mobile"]),
                                  Email = Convert.ToString(rw["Email"]),
                                  SalaryAmt =Convert.ToDecimal(rw["SalaryAmt"]),
                                  OtherAmt = Convert.ToDecimal(rw["OtherAmt"])
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeWagesDAL", "GetEmployeeList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }

        public bool Insert(EmployeeWages emp, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Warehouse].[AddEditEmployeeWages]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmpID", SqlDbType.Int, 0, ParameterDirection.Input, (emp.EmpID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SalaryAmt", SqlDbType.Float, 0, ParameterDirection.Input, (emp.SalaryAmt));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OtherAmt", SqlDbType.Float, 0, ParameterDirection.Input, (emp.OtherAmt));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Mobile", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(emp.Mobile));
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeWagesDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }
        
        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable EmployeeDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Warehouse].[GETEmployeeWagesDetail] @SP_EmpID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                EmployeeDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "EmployeeWagesDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return EmployeeDetailDt;

        }
        
    }
}