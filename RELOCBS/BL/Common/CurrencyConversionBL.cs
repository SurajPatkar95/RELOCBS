using Glimpse.AspNet.Tab;
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
    public class CurrencyConversionBL
    {
        private CurrencyConversionDAL _conversionDAL;

        public CurrencyConversionDAL conversionDAL
        {

            get
            {
                if (this._conversionDAL == null)
                    this._conversionDAL = new CurrencyConversionDAL();
                return this._conversionDAL;
            }
        }

        public bool Insert(HttpPostedFileBase FileUpload,string path1, out string result)
        {
            try
            {
                DataTable exceldt = new DataTable();
                string query = null;
                string connString = "";
                string extension = System.IO.Path.GetExtension(FileUpload.FileName).ToLower();
                if (System.IO.File.Exists(path1))
                { System.IO.File.Delete(path1); }
                FileUpload.SaveAs(path1);
                if (extension == ".csv")
                {
                    exceldt = ExcelToDatatable.ConvertCSVtoDataTable(path1);
                    
                }
                //Connection String to Excel Workbook  
                else if (extension.Trim() == ".xls")
                {
                    connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path1 + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    exceldt = ExcelToDatatable.ConvertXSLXtoDataTable(path1, connString);   
                }
                else if (extension.Trim() == ".xlsx")
                {
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    exceldt = ExcelToDatatable.ConvertXSLXtoDataTable(path1, connString);
                }

                if (exceldt==null || exceldt.Rows.Count<=0)
                {
                    result = "No record found to upload";
                    return false;
                }

                return conversionDAL.Insert(exceldt, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "companyBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public IEnumerable<CurrencyConversion> GetList(int pPageIndex, int pPageSize, string pOrderBy, int pOrder, int? pCountryID, int? pCityID, int? pisActive, string SearchKey, int LoggedinUserID, out int totalCount)
        {
            totalCount = 0;

            try
            {
                IEnumerable<CurrencyConversion> List = conversionDAL.GetList(pPageIndex, pPageSize, pOrderBy, pOrder, pCountryID, pCityID, pisActive, SearchKey, LoggedinUserID, out totalCount);

                return List;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CompanyBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }


    }
}