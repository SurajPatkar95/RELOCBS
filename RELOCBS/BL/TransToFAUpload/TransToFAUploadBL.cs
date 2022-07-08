using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.TransToFAUpload;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;

namespace RELOCBS.BL.TransToFAUpload
{
    public class TransToFAUploadBL
    {

        private TransToFAUploadDAL _transToFAUploadDAL;

        public TransToFAUploadDAL transToFAUploadDAL
        {

            get
            {
                if (this._transToFAUploadDAL == null)
                    this._transToFAUploadDAL = new TransToFAUploadDAL();
                return this._transToFAUploadDAL;
            }
        }

        public bool UploadTransFAOther(TransToFAUploadVM model, string fileLocation, out string result)
        {
            try
            {
                HttpPostedFileBase File = model.file;
                string fileExtension = System.IO.Path.GetExtension(File.FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx" || fileExtension==".csv")
                {
                     fileLocation = Path.Combine(fileLocation,File.FileName.Replace(fileExtension,"")+ "_" + Guid.NewGuid().ToString() + fileExtension);

                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.SetAttributes(fileLocation, FileAttributes.Normal);
                        //   System.IO.File.Delete(fileLocation);
                    }
                    File.SaveAs(fileLocation);
                    
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                    fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xlsx file format.
                    if (fileExtension == ".xlsx")
                    {
                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                        fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    else if(fileExtension == ".csv")
                    {
                        excelConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                            fileLocation +";Extended Properties=\"Text;HDR=Yes\"";

                    }

                    DataTable dt = new DataTable();
                    //Create Connection to Excel work book and add oledb namespace
                    using (OleDbConnection excelConnection = new OleDbConnection(excelConnectionString))
                    {
                        excelConnection.Open();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        string query = "select * from " + "[" + excelSheets[0] + "]" + " ";
                        string loopCoulmnns = "";
                        DataSet dst = GetExcelConnString(fileLocation, excelSheets[0], ref excelConnectionString, ref query, fileLocation);

                        for (int i = 0; i < dst.Tables[0].Columns.Count; i++)
                        {

                            if (dst.Tables[0].Columns[i].ColumnName.ToUpper().Contains("ITEM CODE") || dst.Tables[0].Columns[i].ColumnName.ToUpper().Contains("MISCELLANEOUS") || dst.Tables[0].Columns[i].ColumnName.ToUpper().Contains("JOB NO/ PROJECT /REVENUE BRANCH"))
                            {
                                loopCoulmnns = loopCoulmnns + "," + "Format([" + dst.Tables[0].Columns[i].ColumnName + "],\"#####\") as [" + dst.Tables[0].Columns[i].ColumnName + "] ";
                            }
                            else if (dst.Tables[0].Columns[i].ColumnName.ToUpper().Contains("DATE"))
                            {
                                loopCoulmnns = loopCoulmnns + "," + "Format([" + dst.Tables[0].Columns[i].ColumnName + "],\"dd mmm yyyy\") as [" + dst.Tables[0].Columns[i].ColumnName + "] ";
                            }
                            else
                            {
                                //Format([MobileNo], \"#####\")
                                loopCoulmnns = loopCoulmnns + "," + "[" + dst.Tables[0].Columns[i].ColumnName + "]";

                            }
                            
                        }

                        if (loopCoulmnns.StartsWith(","))
                        {
                            loopCoulmnns = loopCoulmnns.Remove(0, 1);
                        }
                        if (loopCoulmnns.EndsWith(","))
                        {
                            int length = loopCoulmnns.Length;
                            loopCoulmnns = loopCoulmnns.Remove(length - 1, 1);
                        }

                         query = "select " + loopCoulmnns + " from " + "[" + excelSheets[0] + "]" + " ";
                        
                        using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection))
                        {
                            model.dtTable = new DataTable();
                            int AppID = model.AppID != 3 && model.AppID != 2 ? 1 : model.AppID;
                            Dictionary<int, String> tblName = new Dictionary<int, string>();
                            tblName.Add(1, "TransFAOther");
                            tblName.Add(2, "UT_TransFA_ReloSmart");
                            tblName.Add(3, "UT_TransFA_ReloTrack");
                            model.dtTable.TableName = tblName[AppID];
                            dataAdapter.Fill(model.dtTable);
                        }
                        if (excelConnection.State == ConnectionState.Open)
                        {
                            excelConnection.Close();
                        }
                    }

                }

                model.dtTable.CaseSensitive = false;

                string BillNoColumnName = "SALES INVOICE NUMBER"; 

                if (model.dtTable.Columns.Contains(BillNoColumnName))
                {
                    var x = (from r in model.dtTable.AsEnumerable() select r[BillNoColumnName]).Distinct().ToList();

                    if (x.Count > 20)
                    {
                        result = "Maximum 20 invoices can be uploaded in one upload";
                        return false;
                    }

                }
                else
                {
                    if (model.dtTable.Rows.Count > 50)
                    {
                        result = "Maximum 20 invoices can be uploaded in one upload";
                        return false;
                    }
                }
                
                return transToFAUploadDAL.UploadTransFAOther(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "TransToFAUploadBL", "UploadTransFAOther", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        private DataSet GetExcelConnString(string strFileName, string strSheet, ref string strConnString, ref string strQuery, string strFilepath)
        {
            DataSet dst = new DataSet();
            //string m_strFileType;
            if (!string.IsNullOrEmpty(strSheet))
            {
                strQuery = string.Format("SELECT * FROM [" + strSheet + "]");
            }
            else
            {
                strQuery = string.Format("SELECT *  FROM [Sheet1$]");
            }
            try
            {
                //if (strFileName.EndsWith("xls"))
                //{
                //    //strConnString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & strFilename & ";Extended Properties=Excel 8.0", Path.GetDirectoryName(strFilename))
                //    //strConnString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFileName + ";Extended Properties=Excel 8.0;", Path.GetDirectoryName(strFileName));
                //    //m_strFileType = Convert.ToString(SynapseEnums.SynapseEnums.FILETYPE.EXCEL);
                //}
                //else if (strFileName.EndsWith("xlsx"))
                //{
                //    //strConnString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strFileName + ";Extended Properties=Excel 12.0;", Path.GetDirectoryName(strFileName));
                //    //m_strFileType = Convert.ToString(SynapseEnums.SynapseEnums.FILETYPE.EXCEL);
                //}
                //else if (strFileName.EndsWith("csv"))
                //{
                //    //strConnString = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Text;", Path.GetDirectoryName(strFilename))
                //    //strConnString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Text;", Path.GetDirectoryName(strFileName));
                //    //strQuery = String.Format("SELECT * FROM {0}", Path.GetFileName(strFilename))

                //    //strQuery = "SELECT * FROM [" + Path.GetFileName(strFileName) + "]";
                //    //m_strFileType = Convert.ToString(SynapseEnums.SynapseEnums.FILETYPE.NOTEPAD);
                //}
                
                OleDbDataAdapter datadapt = new OleDbDataAdapter(strQuery, strConnString);
                datadapt.Fill(dst);
                
            }
            catch (Exception ex)
            {

            }

            return dst;
        }

        public CostUploadFormat GetUploadFormat(int AppID)
        {
            try
            {
                DataTable dataTable = transToFAUploadDAL.GetUploadFormat(AppID, UserSession.GetUserSession().LoginID);

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    var result = (from rw in dataTable.AsEnumerable()
                                  select new CostUploadFormat()
                                  {

                                      FileName = Convert.ToString(rw["FileName"]),
                                      ResourceName = Convert.ToString(rw["NAME"]),
                                      FileID = Convert.ToInt32(rw["ID"])
                                  }).FirstOrDefault();

                    return result;
                }

                return new CostUploadFormat();
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "TransToFAUploadBL", "GetUploadFormat", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}