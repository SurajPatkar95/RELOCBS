using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.Common
{
    public class EmployeeDAL
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

        public IEnumerable<Employee> GetEmployeeList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pEmpID,int? pBranchId, int? pisActive, string SearchKey, int LoggedinUserID, out int TotalCount)
        {
            TotalCount = 0;
            try
            {
                string query = string.Format("exec Comm.GETEmployeeForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_EmpID={4},@SP_isActive={5},@SP_SearchString={6},@SP_LoginID={7},@SP_BranchID={8}",
                 CSubs.QSafeValue(Convert.ToString(pPageIndex)),
                CSubs.QSafeValue(Convert.ToString(pPageSize)),
                CSubs.QSafeValue(pOrderBy),
                CSubs.QSafeValue(Convert.ToString(pOrder)),
                CSubs.QSafeValue(Convert.ToString(pEmpID)),
                 CSubs.QSafeValue(Convert.ToString(pisActive)),
                CSubs.QSafeValue(SearchKey),
                Convert.ToString(LoggedinUserID),
                CSubs.QSafeValue(Convert.ToString(pBranchId))
                );

                DataTable dataTable = CSubs.GetDataTable(query);
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    TotalCount = Convert.ToInt32(Convert.ToString(dataTable.Rows[0]["TotalRows"]));
                }

                var result = (from rw in dataTable.AsEnumerable()
                              select new Employee()
                              {
                                  EmpID = Convert.ToInt32(rw["EmpID"]),
                                  EmpName = Convert.ToString(rw["EmpName"]),
                                  CardEmpCode = Convert.ToString(rw["CardEmpCode"]),
                                  DesignationID = Convert.ToInt32(rw["DesignationID"]),
                                  Designation = Convert.ToString(rw["DesignationName"]),
                                  IsActive = Convert.ToBoolean(rw["isActive"]),
                                  ShowWarehoueMap = Convert.ToBoolean(rw["ShowWarehouseMap"]),
                              }).ToList();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "EmployeeDAL", "GetEmployeeList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }

        public bool Insert(Employee emp, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Comm].[AddEditEmployee]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmpID", SqlDbType.Int, 0, ParameterDirection.InputOutput, (emp.EmpID <= 0 ? -1 : emp.EmpID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmpCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(emp.CardEmpCode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmpName", SqlDbType.NVarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(emp.EmpName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DesignationID", SqlDbType.Int, 0, ParameterDirection.Input, (emp.DesignationID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(emp.Address));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(emp.Address2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Pincode", SqlDbType.NVarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(emp.Pincode));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.Int, 0, ParameterDirection.Input, (emp.CityID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, (emp.CompId));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone1", SqlDbType.NVarChar, 25, ParameterDirection.Input, CSubs.PSafeValue(emp.Phone1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone2", SqlDbType.NVarChar, 25, ParameterDirection.Input, CSubs.PSafeValue(emp.Phone2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Mobile", SqlDbType.NVarChar, 25, ParameterDirection.Input, CSubs.PSafeValue(emp.Mobile));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DOB", SqlDbType.DateTime, 0, ParameterDirection.Input, (DateTime?)emp.DOB);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DOJ", SqlDbType.DateTime, 0, ParameterDirection.Input, (DateTime?)emp.DOJ);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DOL", SqlDbType.DateTime, 0, ParameterDirection.Input, (DateTime?)emp.DOL);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Email", SqlDbType.NVarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(emp.Email));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsContract", SqlDbType.Bit, 0, ParameterDirection.Input, emp.IsContract);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ModifiedBy", SqlDbType.Int, 0, ParameterDirection.Input, emp.ModifiedBy);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ModifiedDate", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime.Now);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsActive", SqlDbType.Bit, 0, ParameterDirection.Input, emp.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BranchID", SqlDbType.Int, 0, ParameterDirection.Input, emp.BranchID);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool InsertLeave(EmployeeLeaveDetail employeeLeave, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        string EmpLeaveXml= Convert.ToString(new XElement("Leaves", from emp in employeeLeave.empLeaves
                                                                  select new XElement("Leave",
                                                         new XElement("EmpID", emp.EmpID),
                                                         new XElement("FromDate", emp.FromDate.ToString("dd-MMM-yyyy")),
                                                         new XElement("ToDate", emp.ToDate.ToString("dd-MMM-yyyy"))
                                                     )));

                        
                        conn.AddCommand("[Comm].[AddEditEmployeeLeave]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmpID", SqlDbType.Int, 0, ParameterDirection.Input, employeeLeave.employee.EmpID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LeaveXml", SqlDbType.Xml, 0, ParameterDirection.Input, EmpLeaveXml);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDetailById(int? id, int LoginID)
        {
            DataTable EmployeeDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GETEmployeeDetail] @SP_EmpID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                EmployeeDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "EmployeeDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return EmployeeDetailDt;

        }

        public DataSet GetEmpLeaveDetail(int id,int LoginID)
        {
            DataSet EmpDs = new DataSet();

            try
            {
                string query = string.Format("exec [Comm].[GETEmployeeLeaveDetail] @SP_EmpID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(id)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                EmpDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "EmployeeDAL", "GetEmpLeaveDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return EmpDs;
        }

        public bool UpdateWarehouseMapping(EmployeeWarehouseMapping data, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Comm].[AddEditEmployeeWarehouseMapping]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmpID", SqlDbType.Int, 0, ParameterDirection.Input, data.EmployeeId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (DateTime?)data.FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WarehouseID", SqlDbType.Int, 0, ParameterDirection.Input, data.WarehouseId);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeDAL", "UpdateWarehouseMapping", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetWarehouseMapping(int EmpId,int LoginID)
        {
            DataTable EmployeeDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GetEmployeeWarehouseMapping] @SP_EmpID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(EmpId)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                EmployeeDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "EmployeeDAL", "GetWarehouseMapping", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return EmployeeDetailDt;
        }

        public IQueryable<BranchSurveyorMappingGrid> GetSurveyorList(int LoggedinUserID,int CompID, string SearchKey)
        {
            
            try
            {
                string query = string.Format("exec [Comm].[GetSurveyorBranchMappingGrid] @SP_SearchString={0},@SP_LoginID={1},@SP_CompId={2}",
                CSubs.QSafeValue(SearchKey),LoggedinUserID,CompID);

                DataTable dataTable = CSubs.GetDataTable(query);

                var result = (from rw in dataTable.AsEnumerable()
                              select new BranchSurveyorMappingGrid()
                              {
                                  EmployeeId = Convert.ToInt32(rw["EmpID"]),
                                  EmployeeName = Convert.ToString(rw["EmpName"]),
                                  BranchName = Convert.ToString(rw["BranchName"]),
                                  BranchId = rw["BranchId"] ==DBNull.Value? 0 : Convert.ToInt32(rw["BranchId"]),
                                  FromDate = rw["EffectiveFrom"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EffectiveFrom"]),
                              }).AsQueryable();


                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "EmployeeDAL", "GetSurveyorList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }

        public bool UpdateSurveyorBranchMapping(BranchSurveyorMapping data, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Comm].[AddEditSurveyorBranchMapping]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmpID", SqlDbType.Int, 0, ParameterDirection.Input, data.EmployeeId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (DateTime?)data.FromDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BranchID", SqlDbType.Int, 0, ParameterDirection.Input, data.BranchId);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeDAL", "UpdateSurveyorBranchMapping", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetSurveyorBranchMapping(int EmpId, int LoginID)
        {
            DataTable EmployeeDetailDt = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GetSurveyorBranchMappingDetail]  @SP_EmpID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(EmpId)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                EmployeeDetailDt = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "EmployeeDAL", "GetSurveyorBranchMapping", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return EmployeeDetailDt;
        }
        
    }
}