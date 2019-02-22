using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using RELOCBS.Utility;
using System.Web.Mvc;

namespace RELOCBS.App_Code
{
    public class CommonSubs
    {
        private System.Web.Mvc.Controller _Page;

        public CommonSubs()
        {
            _Page = null;
        }

        public CommonSubs(System.Web.Mvc.Controller prmPage)
        {
            _Page = prmPage;
        }

        public string Encrypt(string prmStr)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(prmStr));
        }

        public string Decrypt(string prmStr)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(prmStr));
        }

        public string EncryptURL(string prmStr)
        {
            return HttpUtility.UrlEncode(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(prmStr)));
        }

        public string DecryptURL(string prmStr)
        {
            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(prmStr));
        }

        public void LogError(object objPage, string Module, string ErrorMsg)
        {
            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string LoginID = Convert.ToString(UserSession.GetUserSession().LoginID);
                string FullMessage = "UserID : " + LoginID + Environment.NewLine +
                                     "DateTime  : " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm:ss") + Environment.NewLine +
                                     "Page      : " + objPage.ToString() + Environment.NewLine +
                                     "Module    : " + Module.ToString() + Environment.NewLine +
                                     "Error     : " + Environment.NewLine + Environment.NewLine +
                                     ErrorMsg + Environment.NewLine + Environment.NewLine +
                                     "****************************************************************************************"
                                     + Environment.NewLine;

                try
                {
                    string ErrorLogFolder = System.Configuration.ConfigurationManager.AppSettings["LogFolder"];
                    if (Directory.Exists(ErrorLogFolder) == false)
                    {
                        Directory.CreateDirectory(ErrorLogFolder);
                    }
                    string ErrorFilename = "RELOCBS_LOG_" + DateTime.Now.ToString("dd_MM_yyyy") + ".log";
                    string FullFileName = Path.Combine(ErrorLogFolder, ErrorFilename);
                    File.AppendAllText(FullFileName, FullMessage);
                }
                catch { }

                try
                {
                    ExecuteQuery(String.Format("INSERT INTO Access.ErrorLog ([LoginID], ErrorPage, ErrorModule, ErrorDateTime, ErrorDescription) VALUES ({0},{1},{2},GETDATE(),{3})"
                                , QSafeValue(LoginID)
                                , QSafeValue(objPage.ToString())
                                , QSafeValue(Module)
                                , QSafeValue(ErrorMsg)
                                ));
                }
                catch { }


            }
            catch { }
        }

        public string SafeText(String strConvert, SafeTextType intFlag)
        {
            if (intFlag == SafeTextType.TEXTTOXML)
            {
                strConvert = strConvert.Trim();
                strConvert = strConvert.Replace("<", "&lt;");
                strConvert = strConvert.Replace(">", "&gt;");
                strConvert = strConvert.Replace("&", "&amp;");
            }
            else if (intFlag == SafeTextType.XMLTOTEXT)
            {
                strConvert = strConvert.Trim();
                strConvert = strConvert.Replace("&lt;", "<");
                strConvert = strConvert.Replace("&gt;", ">");
                strConvert = strConvert.Replace("&amp;", "&");
            }
            else if (intFlag == SafeTextType.WITHOUTCTRLCHAR)
            {
                strConvert = strConvert.Trim();
                strConvert = strConvert.Replace("'", "''");
                System.Text.StringBuilder strCtrlChr = new System.Text.StringBuilder(strConvert.Length);
                for (Int32 Index = 0; Index < strConvert.Length; Index++)
                {
                    if (Char.IsControl(strConvert, Index) == false)
                        strCtrlChr.Append(strConvert[Index]);
                }
                strConvert = strCtrlChr.ToString();
            }
            else if (intFlag == SafeTextType.TRIMMEDTEXT)
            {
                strConvert = strConvert.Trim();
                strConvert = strConvert.Replace("'", "''");
            }
            else
            {
                strConvert = strConvert.Replace("'", "''");
            }
            return strConvert;
        }

        public string GetRandomString(Int16 prmLength)
        {
            Random num = new Random();
            String SpecialChars = "@$*";
            String RandomString = "";
            for (Int32 i = 1; i <= prmLength; i++)
            {
                switch (num.Next(1, 5))
                {
                    case 1:
                        RandomString += (char)(num.Next(65, 91));
                        break;
                    case 2:
                        RandomString += (char)(num.Next(48, 58));
                        break;
                    case 3:
                        RandomString += (char)(num.Next(97, 123));
                        break;
                    case 4:
                        RandomString += SpecialChars[num.Next(0, SpecialChars.Length)];
                        break;
                }
            }
            return RandomString;
        }

        public string GetRandomInt(Int16 length)
        {
            const string valid = "1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public bool IsValidTime(string prmStrTime)
        {
            if (Regex.IsMatch(prmStrTime, "\\d\\d\\:\\d\\d") == true)
            {
                Int16 hr = Convert.ToInt16(prmStrTime.Substring(0, 2));
                Int16 min = Convert.ToInt16(prmStrTime.Substring(3, 2));
                if (min >= 60) return false;
                if (hr > 23) return false;

                return true;
            }
            return false;
        }

        public bool IsValidDate(string prmStrDate, out DateTime prmDate)
        {
            return DateTime.TryParseExact(prmStrDate, "dd\\/MM\\/yyyy", null, System.Globalization.DateTimeStyles.None, out prmDate);
        }

        #region safevalues
        public object PSafeValue(string prmStr, EValueType prmValueType = EValueType.STRING)
        {
            object ReturnValue = DBNull.Value;
            try
            {
                prmStr = prmStr.Trim();
                if (!String.IsNullOrWhiteSpace(prmStr))
                {
                    if (prmValueType == EValueType.DATE)
                    {
                        DateTime date;
                        if (IsValidDate(prmStr, out date) == true)
                            ReturnValue = date;
                    }

                    else if (prmValueType == EValueType.INT)
                    {
                        ReturnValue = Convert.ToInt32(prmStr);
                    }
                    else if (prmValueType == EValueType.DOUBLE)
                    {
                        ReturnValue = Convert.ToDouble(prmStr);
                    }
                    else
                    {
                        ReturnValue = prmStr.Replace("'", "''");
                    }
                }
            }
            catch { }
            return ReturnValue;
        }

        public string QSafeValue(string prmStr, EValueType prmValueType = EValueType.STRING)
        {
            string ReturnValue = "null";
            try
            {
                prmStr = prmStr.Trim();
                if (!String.IsNullOrWhiteSpace(prmStr))
                {
                    if (prmValueType == EValueType.DATE)
                    {
                        DateTime date;
                        if (IsValidDate(prmStr, out date) == true)
                            ReturnValue = "'" + date.ToString("dd MMM yyyy") + "'";
                    }
                    else if (prmValueType == EValueType.INT)
                    {
                        ReturnValue = SafeInt(prmStr).ToString();
                    }
                    else if (prmValueType == EValueType.DOUBLE)
                    {
                        ReturnValue = SafeDouble(prmStr).ToString();
                    }
                    else
                    {
                        ReturnValue = "'" + prmStr.Replace("'", "''") + "'";
                    }
                }
            }
            catch { }
            return ReturnValue;
        }

        public Double SafeDouble(string prmValue)
        {
            Double retval;
            if (prmValue == null || prmValue.Trim() == "")
            {
                return 0;
            }
            else if (Double.TryParse(prmValue, out retval) == true)
            {
                return retval;
            }
            return 0;
        }

        public int SafeInt(string prmValue)
        {
            Int32 retval;
            if (prmValue == null || prmValue.Trim() == "")
            {
                return 0;
            }
            else if (Int32.TryParse(prmValue, out retval) == true)
            {
                return retval;
            }
            return 0;
        }

        public Decimal SafeDecimal(string prmValue)
        {
            Decimal retval;
            if (prmValue == null || prmValue.Trim() == "")
            {
                return 0;
            }
            else if (Decimal.TryParse(prmValue, out retval) == true)
            {
                return retval;
            }
            return 0;
        }

        public float SafeFloat(string prmValue)
        {
            float retval;
            if (prmValue == null || prmValue.Trim() == "")
            {
                return 0;
            }
            else if (float.TryParse(prmValue, out retval) == true)
            {
                return retval;
            }
            return 0;
        }
        #endregion

        #region Sql functions
        public Object GetValue(string prmQuery)
        {
            Object value = null;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand(prmQuery, QueryType.QueryText);
                        value = conn.ExecuteQuery(QueryReturnType.SingleValue);
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception EX)
            {
                LogError(this._Page, "GetValue", EX.ToString());
            }
            return value;
        }

        public DataTable GetDataTable(string prmQuery)
        {
            DataTable dtTable = null;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand(prmQuery, QueryType.QueryText);
                        dtTable = (DataTable)conn.ExecuteQuery(QueryReturnType.DataTable);
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception EX)
            {
                LogError(this._Page, "GetDataTable", EX.ToString());
            }
            return dtTable;
        }

        public DataTable GetDataTable(string prmQuery, string Constr)
        {
            DataTable dtTable = null;
            try
            {
                using (CDALSQL conn = new CDALSQL(Constr))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand(prmQuery, QueryType.QueryText);
                        dtTable = (DataTable)conn.ExecuteQuery(QueryReturnType.DataTable);
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception EX)
            {
                LogError(this._Page, "GetDataTable", EX.ToString());
            }
            return dtTable;
        }

        public DataSet GetDataSet(string prmQuery)
        {
            DataSet dstSet = null;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand(prmQuery, QueryType.QueryText);
                        dstSet = (DataSet)conn.ExecuteQuery(QueryReturnType.DataSet);
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception EX)
            {
                LogError(this._Page, "GetDataSet", EX.ToString());
            }
            return dstSet;
        }

        public DataSet GetDataSet(string prmQuery, string Constr)
        {
            DataSet dstSet = null;
            try
            {
                using (CDALSQL conn = new CDALSQL(Constr))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand(prmQuery, QueryType.QueryText);
                        dstSet = (DataSet)conn.ExecuteQuery(QueryReturnType.DataSet);
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception EX)
            {
                LogError(this._Page, "GetDataSet", EX.ToString());
            }
            return dstSet;
        }

        public Boolean ExecuteQuery(string prmQuery)
        {
            Boolean result = false;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand(prmQuery, QueryType.QueryText);
                        conn.ExecuteQuery(QueryReturnType.SingleValue);
                        if (conn.IsError == false)
                            result = true;
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (Exception EX)
            {
                LogError(this._Page, "ExecuteQuery", EX.ToString());
            }
            return result;
        }
        #endregion

        #region PageRights
        public bool CheckPageRights(string PrmPageID, PermissionType PrmPermission)
        {
            Boolean result = false;

            try
            {
                using (DataTable dtPermissions = UserSession.GetUserMenuTable())
                {
                    DataRow[] dr = dtPermissions.Select("MENUID=" + PrmPageID);
                    if (dr.Length > 0)
                    {
                        switch (PrmPermission)
                        {
                            case PermissionType.VIEW:
                                if (dr[0]["ALLOW_VIEW"].ToString() == "1") result = true;
                                break;
                            case PermissionType.ADD:
                                if (dr[0]["ALLOW_ADD"].ToString() == "1") result = true;
                                break;
                            case PermissionType.EDIT:
                                if (dr[0]["ALLOW_EDIT"].ToString() == "1") result = true;
                                break;
                            case PermissionType.DELETE:
                                if (dr[0]["ALLOW_DELETE"].ToString() == "1") result = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch { }

            return result;
        }
        #endregion

        public string NumberInWords(string nStr)
        {
            int lenStr;
            nStr = (Math.Round(SafeDecimal(nStr.Trim()), 2)).ToString();
            lenStr = nStr.Length;
            if (!nStr.Contains("."))
            {
                return InWords(Mid(nStr, Convert.ToUInt32(1), Convert.ToUInt32(lenStr)));
            }
            else
            {
                if ((nStr.IndexOf(".") + 2) == lenStr)
                {
                    nStr = nStr + "0";
                }
                if (SafeInt(Mid(nStr, Convert.ToUInt32(nStr.IndexOf(".")) + 2, Convert.ToUInt32(lenStr))) > 0)
                    return InWords(Mid(nStr, Convert.ToUInt32(1), Convert.ToUInt32(nStr.IndexOf(".")))) + " And Paise " + InWords(Mid(nStr, Convert.ToUInt32(nStr.IndexOf(".")) + 2, Convert.ToUInt32(lenStr)));
                else
                    return InWords(Mid(nStr, Convert.ToUInt32(1), Convert.ToUInt32(nStr.IndexOf("."))));
            }

        }

        private string InWords(string nStr)
        {
            string[] vOnes = new string[10];
            string[] vTens = new string[10];
            string[] vTeens = new string[10];
            Int16 numLen;
            Int16 nPos;
            Int64 numVal;
            Int16 I;
            string tempInWords;
            Boolean cPos;

            vOnes[1] = "One"; vOnes[2] = "Two"; vOnes[3] = "Three"; vOnes[4] = "Four";
            vOnes[5] = "Five"; vOnes[6] = "Six"; vOnes[7] = "Seven"; vOnes[8] = "Eight";
            vOnes[9] = "Nine";
            vTens[1] = "Ten"; vTens[2] = "Twenty"; vTens[3] = "Thirty"; vTens[4] = "Forty";
            vTens[5] = "Fifty"; vTens[6] = "Sixty"; vTens[7] = "Seventy"; vTens[8] = "Eighty";
            vTens[9] = "Ninety";
            vTeens[1] = "Eleven"; vTeens[2] = "Twelve"; vTeens[3] = "Thirteen"; vTeens[4] = "Forteen";
            vTeens[5] = "Fifteen"; vTeens[6] = "Sixteen"; vTeens[7] = "Seventeen"; vTeens[8] = "Eighteen";
            vTeens[9] = "Ninteen";
            tempInWords = "";
            numLen = Convert.ToInt16(nStr.Length);
            numVal = SafeInt(nStr);
            nPos = numLen;
            cPos = false;

            for (I = 1; I <= numLen; I++)
            {
                if (Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)) != "0")
                {
                    if (nPos == 11)
                    {
                        if (Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)) == "1")
                        {
                            if (Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)) == "0")
                            {
                                tempInWords = tempInWords + "Ten" + " Arab ";
                            }
                            else
                            {
                                tempInWords = tempInWords + (SafeInt(Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1))) == 0 ? "" : vTeens[SafeInt(Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)))] + " Arab ");
                            }
                            cPos = true;
                        }
                        else
                        {
                            tempInWords = tempInWords + vTens[SafeInt(Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)))] + (Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)) == "0" ? " Arab " : " ");
                        }
                    }
                    else if (nPos == 10)
                    {
                        tempInWords = tempInWords + vOnes[SafeInt(Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)))] + " Arab ";
                    }
                    else if (nPos == 9)
                    {
                        if (Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)) == "1")
                        {
                            if (Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)) == "0")
                            {
                                tempInWords = tempInWords + "Ten" + " Crore ";
                            }
                            else
                            {
                                tempInWords = tempInWords + (SafeInt(Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1))) == 0 ? "" : vTeens[SafeInt(Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)))] + " Crore ");
                            }
                            cPos = true;
                        }
                        else
                        {
                            tempInWords = tempInWords + vTens[SafeInt(Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)))] + (Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)) == "0" ? " Crore " : " ");
                        }
                    }
                    else if (nPos == 8)
                    {
                        tempInWords = tempInWords + vOnes[SafeInt(Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)))] + " Crore ";
                    }
                    else if (nPos == 7)
                    {
                        if (Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)) == "1")
                        {
                            if (Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)) == "0")
                            {
                                tempInWords = tempInWords + "Ten" + " Lacs ";
                            }
                            else
                            {
                                tempInWords = tempInWords + (SafeInt(Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1))) == 0 ? "" : vTeens[SafeInt(Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)))] + " Lacs ");
                            }
                            cPos = true;
                        }
                        else
                        {
                            tempInWords = tempInWords + vTens[SafeInt(Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)))] + (Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)) == "0" ? " Lacs " : " ");
                        }
                    }
                    else if (nPos == 6)
                    {
                        tempInWords = tempInWords + vOnes[SafeInt(Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)))] + " Lacs ";
                    }
                    else if (nPos == 5)
                    {
                        if (Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)) == "1")
                        {
                            if (Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)) == "0")
                            {
                                tempInWords = tempInWords + "Ten" + " Thousand ";
                            }
                            else
                            {
                                tempInWords = tempInWords + (SafeInt(Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1))) == 0 ? "" : vTeens[SafeInt(Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)))] + " Thousand ");
                            }
                            cPos = true;
                        }
                        else
                        {
                            tempInWords = tempInWords + vTens[SafeInt(Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)))] + (Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)) == "0" ? " Thousand " : " ");
                        }
                    }
                    else if (nPos == 4)
                    {
                        tempInWords = tempInWords + vOnes[SafeInt(Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)))] + " Thousand ";
                    }
                    else if (nPos == 3)
                    {
                        tempInWords = tempInWords + vOnes[SafeInt(Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)))] + " Hundred ";
                    }
                    else if (nPos == 2)
                    {
                        if (Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)) == "1")
                        {
                            if (Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)) == "0")
                            {
                                tempInWords = tempInWords + "Ten ";
                            }
                            else
                            {
                                tempInWords = tempInWords + (SafeInt(Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1))) == 0 ? "" : vTeens[SafeInt(Mid(nStr, Convert.ToUInt32(I + 1), Convert.ToUInt32(1)))] + " ");
                            }
                            cPos = true;
                        }
                        else
                        {
                            tempInWords = tempInWords + vTens[SafeInt(Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)))] + " ";
                        }
                    }
                    else
                    {
                        tempInWords = tempInWords + vOnes[SafeInt(Mid(nStr, Convert.ToUInt32(I), Convert.ToUInt32(1)))] + " ";
                    }
                }
                if (cPos == true)
                {
                    I = Convert.ToInt16(I + 1);
                    nPos = Convert.ToInt16(nPos - 1);
                    cPos = false;
                }
                nPos = Convert.ToInt16(nPos - 1);
            }
            return tempInWords;
        }

        public string Mid(string prmString, UInt32 prmStart, UInt32 prmLength)
        {
            string retVal = "";
            try
            {
                if (String.IsNullOrEmpty(prmString))
                {
                    retVal = "";
                }
                else
                {
                    if (prmStart > 0)
                    {
                        if (prmLength > 0)
                        {
                            if ((prmStart + prmLength - 1) > prmString.Length)
                            {
                                prmLength = Convert.ToUInt32(Math.Abs((prmString.Length - prmStart) + 1));
                                //prmLength = Math.Abs((Program.SafeInt(prmString.Length.ToString())-Program.SafeInt(prmStart.ToString()))+1);
                            }
                            retVal = prmString.Substring(SafeInt(prmStart.ToString()) - 1, SafeInt(prmLength.ToString()));
                        }
                        else
                        {
                            throw (new Exception("Length cannot be less then 1"));
                        }
                    }
                    else
                    {
                        throw (new Exception("Start cannot be less then 1"));
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(this._Page, "Mid", ex.ToString());
            }
            return (retVal);
        }

        #region Compress Image

        public static Byte[] Compressimage(Stream sourcePath, String filename)
        {
            Byte[] data = null;

            try
            {
                using (var image = Image.FromStream(sourcePath))
                {
                    float maxHeight = 800.0f;
                    float maxWidth = 800.0f;
                    int newWidth;
                    int newHeight;
                    string extension;
                    Bitmap originalBMP = new Bitmap(image);
                    int originalWidth = originalBMP.Width;
                    int originalHeight = originalBMP.Height;

                    if (originalWidth > maxWidth || originalHeight > maxHeight)
                    {

                        // To preserve the aspect ratio  
                        float ratioX = (float)maxWidth / (float)originalWidth;
                        float ratioY = (float)maxHeight / (float)originalHeight;
                        float ratio = Math.Min(ratioX, ratioY);
                        newWidth = (int)(originalWidth * ratio);
                        newHeight = (int)(originalHeight * ratio);
                    }
                    else
                    {
                        newWidth = (int)originalWidth;
                        newHeight = (int)originalHeight;

                    }
                    Bitmap bitMAP1 = new Bitmap(originalBMP, newWidth, newHeight);
                    Graphics imgGraph = Graphics.FromImage(bitMAP1);
                    extension = Path.GetExtension(filename);
                    //if (extension == ".png" || extension == ".gif")
                    //{
                    //    imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                    //    imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    //    imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);


                    //    bitMAP1.Save(targetPath, image.RawFormat);

                    //    bitMAP1.Dispose();
                    //    imgGraph.Dispose();
                    //    originalBMP.Dispose();
                    //}
                    //else 
                    //if (extension == ".jpg")
                    //{

                    imgGraph.SmoothingMode = SmoothingMode.AntiAlias;
                    imgGraph.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    imgGraph.DrawImage(originalBMP, 0, 0, newWidth, newHeight);
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 90L);
                    myEncoderParameters.Param[0] = myEncoderParameter;

                    // bitMAP1.Save(@"D:\LogFolder\"+filename, jpgEncoder, myEncoderParameters);


                    using (var memoryStream = new MemoryStream())
                    {
                        bitMAP1.Save(memoryStream, jpgEncoder, myEncoderParameters);

                        data = memoryStream.ToArray();
                    }
                    bitMAP1.Dispose();
                    imgGraph.Dispose();
                    originalBMP.Dispose();

                    //}


                }

            }
            catch (Exception)
            {
                throw;

            }
            return data;
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        #endregion

        #region

        public IEnumerable<SelectListItem> BindDropdown(string PrmQuery, bool PrmAddAll = false)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();

            try
            {
                
                using (DataTable dtTable = GetDataTable(PrmQuery))
                {
                    if (dtTable != null)
                    {
                        

                        Int32 itemOffset;
                        Int32 endOffset;

                        new SelectListItem
                        {
                            Value = null,
                            Text = " "
                        };

                        itemOffset = 0;
                        endOffset = dtTable.Rows.Count;

                        for (Int32 i = itemOffset; i < endOffset; i++)
                        {
                            SelectListItem item = new SelectListItem(){ Value=dtTable.Rows[i][0].ToString(), Text=dtTable.Rows[i][1].ToString() };
                            listItems.Add(item);
                            //item.DataBind();
                        }
                    }
                }
            }
            catch (Exception EX)
            {
                LogError(_Page, "AttachRadCombo", EX.ToString());
            }
            finally
            {
                if (PrmAddAll)
                    listItems.Insert(0, new SelectListItem() { Value= "-1",Text="*All Items*" });
                
            }

            return listItems;
        }


        #endregion
    }
}