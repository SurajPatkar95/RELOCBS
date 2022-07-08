using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Common;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.BL.Common
{
    public class EmployeeBL
    {
        private EmployeeDAL _employeeDAL;

        public EmployeeDAL employeeDAL
        {

            get
            {
                if (this._employeeDAL == null)
                    this._employeeDAL = new EmployeeDAL();
                return this._employeeDAL;
            }
        }

        public IEnumerable<Employee> GetEmployeeList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pEmployeeID,int? pBranchId, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<Employee> EmployeeList = employeeDAL.GetEmployeeList(pPageIndex, pPageSize, pOrderBy, pOrder, pEmployeeID, pBranchId, pisActive, SearchKey, LoggedinUserID, out totalCount);

                return EmployeeList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeBL", "GetDetailById", Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Insert(Employee emp,string Actionflag, out string result)
        {
            result = string.Empty;
            try
            {
                return employeeDAL.Insert(emp, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeBL", Actionflag, RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool InsertLeave(EmployeeLeaveDetail employeeLeave,out string result)
        {
            result = string.Empty;
            try
            {
                return employeeDAL.InsertLeave(employeeLeave, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeBL", "InsertLeave", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Employee GetDetailById(int? id)
        {
            Employee empObj = new Employee();
            try
            {
                DataTable empDt = employeeDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (empDt != null && empDt.Rows.Count > 0)
                {

                    empObj = (from rw in empDt.AsEnumerable()
                               select new Employee()
                               {
                                   EmpName = Convert.ToString(rw["EmpName"]),
                                   CardEmpCode = Convert.ToString(rw["CardEmpCode"]),
                                   EmpID = Convert.ToInt32(rw["EmpID"]),
                                   DesignationID = rw["DesignationID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["DesignationID"]),
                                   Address = Convert.ToString(rw["Address"]),
                                   Address2 = Convert.ToString(rw["Address2"]),
                                   Pincode  = Convert.ToString(rw["Pincode"]),
                                   DOB = rw["DOB"] == DBNull.Value? (DateTime?)null : Convert.ToDateTime(rw["DOB"]),
                                   DOJ = rw["DOJ"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["DOJ"]),
                                   DOL = rw["DOL"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["DOL"]),
                                   CityID = rw["CityID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["CityID"]),
                                   CompId = rw["CompID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["CompID"]),
                                   Phone1 = Convert.ToString(rw["TelephoneDirect"]),
                                   Phone2 = Convert.ToString(rw["TelephoneBoard"]),
                                   Mobile = Convert.ToString(rw["Mobile"]),
                                   Email = Convert.ToString(rw["Email"]),
                                   IsActive = Convert.ToBoolean(rw["isActive"]),
                                   BranchID = rw["BranchID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["BranchID"]),
                               }).First();


                    return empObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return empObj;

        }

        public EmployeeLeaveDetail GetEmpLeaveDetail(int id)
        {

            EmployeeLeaveDetail empObj = new EmployeeLeaveDetail();
            try
            {
                DataSet empDs = employeeDAL.GetEmpLeaveDetail(id, UserSession.GetUserSession().LoginID);
                if (empDs != null && empDs.Tables.Count > 1)
                {

                    if (empDs.Tables[0]!=null && empDs.Tables[0].Rows.Count>0)
                    {
                        empObj.employee = (from rw in empDs.Tables[0].AsEnumerable()
                                           select new Employee()
                                           {
                                               EmpName = Convert.ToString(rw["EmpName"]),
                                               CardEmpCode = Convert.ToString(rw["CardEmpCode"]),
                                               EmpID = Convert.ToInt32(rw["EmpID"]),
                                               DesignationID = Convert.ToInt32(rw["DesignationID"]),
                                               Designation=Convert.ToString(rw["DesignationName"]),
                                               Address = Convert.ToString(rw["Address"]),
                                               DOB = rw["DOB"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["DOB"]),
                                               DOJ = rw["DOJ"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["DOJ"]),
                                               DOL = rw["DOL"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["DOL"]),
                                               CityID = Convert.ToInt32(rw["CityID"]),
                                               CompId = Convert.ToInt32(rw["CompID"]),
                                               Mobile = Convert.ToString(rw["Mobile"]),
                                               IsActive = Convert.ToBoolean(rw["Isactive"])
                                           }).First();
                    }

                    if (empDs.Tables[1] != null && empDs.Tables[1].Rows.Count > 0)
                    {
                        empObj.empLeaves = (from rw in empDs.Tables[1].AsEnumerable()
                                            select new EmpLeaves()
                                            {

                                                EmpID = Convert.ToInt32(rw["EmpID"]),
                                                FromDate = Convert.ToDateTime(rw["FromDate"]),
                                                ToDate = Convert.ToDateTime(rw["ToDate"]),
                                                NoOfDays = Convert.ToString(rw["NoOfDays"]),
                                            }).ToList();

                    }




                    return empObj;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeBL", "GetEmpLeaveDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return empObj;


        }

        public bool UpdateWarehouseMapping(EmployeeWarehouseMapping data, out string result)
        {
            result = string.Empty;
            try
            {
                return employeeDAL.UpdateWarehouseMapping(data, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeBL", "UpdateWarehouseMapping", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public EmployeeWarehouseMapping GetWarehouseMapping(int EmpId)
        {
            EmployeeWarehouseMapping mapping = new EmployeeWarehouseMapping();
            try
            {
                DataTable empDt = employeeDAL.GetWarehouseMapping(EmpId, UserSession.GetUserSession().LoginID);
                if (empDt != null && empDt.Rows.Count > 0)
                {

                    mapping = (from rw in empDt.AsEnumerable()
                              select new EmployeeWarehouseMapping()
                              {

                                  EmployeeId = Convert.ToInt32(rw["EmployeeId"]),
                                  WarehouseId= Convert.ToInt32(rw["WarehouseId"]),
                                  FromDate = rw["EffectiveFrom"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EffectiveFrom"]),
                                  EmployeeName= Convert.ToString(rw["EmpName"]),
                              }).First();


                    return mapping;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeBL", "GetWarehouseMapping", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return mapping;
        }

        public IEnumerable<Entities.BranchSurveyorMappingGrid> GetSurveyorList(string SearchStr, string sort, string sortdir, int skip, int pageSize, out int totalCount)
        {
            try
            {
                IQueryable<Entities.BranchSurveyorMappingGrid> grids = employeeDAL.GetSurveyorList(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, SearchStr);

                totalCount = grids.Count();
                if (pageSize > 1)
                {
                    grids = grids.Skip((skip * (pageSize - 1))).Take(skip);
                }
                else
                {
                    grids = grids.Take(skip);
                }

                //AllocationList = AllocationList.OrderBy(sort + " " + sortdir);

                return grids.ToList();

            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "ATRBL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }


        }

        
        public bool UpdateSurveyorBranchMapping(BranchSurveyorMapping data, out string result)
        {
            result = string.Empty;
            try
            {
                return employeeDAL.UpdateSurveyorBranchMapping(data, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeBL", "UpdateWarehouseMapping", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public BranchSurveyorMapping GetSurveyorBranchMapping(int EmpId)
        {
            BranchSurveyorMapping mapping = new BranchSurveyorMapping();
            try
            {
                DataTable empDt = employeeDAL.GetSurveyorBranchMapping(EmpId, UserSession.GetUserSession().LoginID);
                if (empDt != null && empDt.Rows.Count > 0)
                {

                    mapping = (from rw in empDt.AsEnumerable()
                               select new BranchSurveyorMapping()
                               {

                                   EmployeeId = Convert.ToInt32(rw["EmployeeId"]),
                                   BranchId = Convert.ToInt32(rw["BranchId"]),
                                   FromDate = rw["EffectiveFrom"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EffectiveFrom"]),
                                   EmployeeName = Convert.ToString(rw["EmpName"]),
                               }).First();


                    return mapping;
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeBL", "GetWarehouseMapping", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return mapping;
        }
    }
}