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
    public class EmployeeWagesBL
    {
        private EmployeeWagesDAL _employeeDAL;

        public EmployeeWagesDAL employeeDAL
        {

            get
            {
                if (this._employeeDAL == null)
                    this._employeeDAL = new EmployeeWagesDAL();
                return this._employeeDAL;
            }
        }

        public IEnumerable<EmployeeWages> GetEmployeeList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pEmployeeID, int? pisActive, string SearchKey, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<EmployeeWages> EmployeeList = employeeDAL.GetEmployeeList(pPageIndex, pPageSize, pOrderBy, pOrder, pEmployeeID, pisActive, SearchKey, out totalCount);

                return EmployeeList;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeWagesBL", "GetDetailById", Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool Insert(EmployeeWages emp, string Actionflag, out string result)
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeWagesBL", Actionflag, RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
        
        public EmployeeWages GetDetailById(int? id)
        {
            EmployeeWages empObj = new EmployeeWages();
            try
            {
                DataTable empDt = employeeDAL.GetDetailById(id, UserSession.GetUserSession().LoginID);
                if (empDt != null && empDt.Rows.Count > 0)
                {

                    empObj = (from rw in empDt.AsEnumerable()
                              select new EmployeeWages()
                              {
                                  EmpID = Convert.ToInt32(rw["EmpID"]),
                                  EmpName = Convert.ToString(rw["EmpName"]),
                                  CardEmpCode = Convert.ToString(rw["CardEmpCode"]),
                                  DesignationID = Convert.ToInt32(rw["DesignationID"]),
                                  Designation = Convert.ToString(rw["DesignationName"]),
                                  Mobile = Convert.ToString(rw["Mobile"]),
                                  Email = Convert.ToString(rw["Email"]),
                                  SalaryAmt = Convert.ToDecimal(rw["SalaryAmt"]),
                                  OtherAmt = Convert.ToDecimal(rw["OtherAmt"])
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
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "EmployeeWagesBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return empObj;

        }
        
    }
}