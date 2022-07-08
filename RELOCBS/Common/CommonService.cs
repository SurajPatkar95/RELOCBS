using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RELOCBS.Common
{
    public class CommonService
    {

        private enum ControlName
        {
            Loosecased, LCLFCL, YesNo, Title, BillingOn, PaymentType, RMC, VehicleType, InvoiceStatus,
            WOSInvoiceStatus, AccessType, FreightLCLFCL, Reports, InvCredit, CreditCustApprovalType, CRPriority,
            NormalRev, MSME, CAFStatus
        }

        public enum RateComp
        {
            Origin = 1, Freight = 2, Destination = 3
        }

        public enum RMCBuss
        {
            RMC = 1, NonRMC = 0
        }
        public static IEnumerable<SelectListItem> Loosecased { get { return GetCustomDropDown(ControlName.Loosecased); } }
        public static IEnumerable<SelectListItem> LCLFCL { get { return GetCustomDropDown(ControlName.LCLFCL); } }
        public static IEnumerable<SelectListItem> FreightLCLFCL { get { return GetCustomDropDown(ControlName.FreightLCLFCL); } }
        public static IEnumerable<SelectListItem> YesNo { get { return GetCustomDropDown(ControlName.YesNo); } }
        public static IEnumerable<SelectListItem> Title { get { return GetCustomDropDown(ControlName.Title); } }
        public static IEnumerable<SelectListItem> BillingOn { get { return GetCustomDropDown(ControlName.BillingOn); } }
        public static IEnumerable<SelectListItem> PaymentType { get { return GetCustomDropDown(ControlName.PaymentType); } }
        public static IEnumerable<SelectListItem> RMC { get { return GetCustomDropDown(ControlName.RMC); } }
        public static IEnumerable<SelectListItem> VehicleType { get { return GetCustomDropDown(ControlName.VehicleType); } }
        public static IEnumerable<SelectListItem> InvoiceStatus { get { return GetCustomDropDown(ControlName.InvoiceStatus); } }
        public static IEnumerable<SelectListItem> CAFStatus { get { return GetCustomDropDown(ControlName.CAFStatus); } }
        public static IEnumerable<SelectListItem> WOSInvoiceStatus { get { return GetCustomDropDown(ControlName.WOSInvoiceStatus); } }
        public static IEnumerable<SelectListItem> AccessType { get { return GetCustomDropDown(ControlName.AccessType); } }
        public static IEnumerable<SelectListItem> Reports { get { return GetCustomDropDown(ControlName.Reports); } }
        public static IEnumerable<SelectListItem> InvCredit { get { return GetCustomDropDown(ControlName.InvCredit); } }

        public static IEnumerable<SelectListItem> CreditCustApprovalType { get { return GetCustomDropDown(ControlName.CreditCustApprovalType); } }

        public static IEnumerable<SelectListItem> CRPriority { get { return GetCustomDropDown(ControlName.CRPriority); } }
        public static IEnumerable<SelectListItem> NormalRev { get { return GetCustomDropDown(ControlName.NormalRev); } }
        public static IEnumerable<SelectListItem> MSME { get { return GetCustomDropDown(ControlName.MSME); } }


        private static IEnumerable<SelectListItem> GetCustomDropDown(ControlName Type)
        {
            //string Regex = string.Empty;
            switch (Type)
            {
                case ControlName.Loosecased:
                    return new[]{ new SelectListItem { Text = "Loose", Value = "Loose" }, new SelectListItem { Text = "Cased", Value = "Cased" },
                    new SelectListItem { Text = "LiftVan", Value = "LiftVan" }, new SelectListItem { Text = " ", Value = " " }};
                case ControlName.LCLFCL:
                    return new[]{ new SelectListItem { Text = "LCL", Value = "LCL" }, new SelectListItem { Text = "FCL", Value = "FCL" },
                        new SelectListItem { Text = "Cased", Value = "Cased" },new SelectListItem { Text = " ", Value = " " },new SelectListItem { Text = "Part Load", Value = "Part" },new SelectListItem { Text = "Direct", Value = "Direct" },new SelectListItem { Text = "GRPG", Value = "GRPG" } };
                case ControlName.FreightLCLFCL:
                    return new[] { new SelectListItem { Text = "LCL", Value = "LCL" }, new SelectListItem { Text = "FCL", Value = "FCL" },
                        new SelectListItem { Text = "GRPG", Value = "GRPG" } };
                case ControlName.YesNo:
                    return new[] { new SelectListItem { Text = "Yes", Value = "1" }, new SelectListItem { Text = "No", Value = "0" } };
                case ControlName.Title:
                    return new[]{ new SelectListItem { Text = "Mr", Value = "Mr" }, new SelectListItem { Text = "Mrs", Value = "Mrs" },
                                   new SelectListItem { Text = "Miss", Value = "Miss" }, new SelectListItem { Text = "Master", Value = "Master" },
                                   new SelectListItem { Text = "Dr", Value = "Dr" }};
                case ControlName.BillingOn:
                    return new[]{ new SelectListItem { Text = "Client", Value = "Client" }, new SelectListItem { Text = "Account", Value = "Corporate" },
                                    new SelectListItem { Text = "Shipper", Value = "Shipper" }};
                case ControlName.PaymentType:
                    return new[] { new SelectListItem { Text = "Full", Value = "Full" }, new SelectListItem { Text = "Balance", Value = "Balance" } };
                case ControlName.RMC:
                    return new[] { new SelectListItem { Text = "RMC-BUSINESS", Value = "RMC-BUSINESS" }, new SelectListItem { Text = "NON RMC-BUSINESS", Value = "NON RMC-BUSINESS" },
                        new SelectListItem { Text = "ORIENTATION SERVICE", Value = "ORIENTATION SERVICE" } };
                case ControlName.VehicleType:
                    return new[] { new SelectListItem { Text = "Owned", Value = "O" }, new SelectListItem { Text = "Hired", Value = "H" } };
                case ControlName.InvoiceStatus:
                    return new[] {new SelectListItem { Text = "Draft", Value = "Draft" },
                        new SelectListItem { Text = "Send To Finance", Value = "Send To Finance" }, new SelectListItem { Text = "Send To SD", Value = "Send To SD" },
                        new SelectListItem { Text = "Approved", Value = "Approved" }, new SelectListItem { Text = "Finalized", Value = "Finalized" },
                        new SelectListItem { Text = "Exported", Value = "Exported" }
                    };
                case ControlName.CAFStatus:
                    return new[] {new SelectListItem { Text = "Send for approval", Value = "Send for approval" },
                        new SelectListItem { Text = "APPROVED", Value = "APPROVED" },
                        new SelectListItem { Text = "Approval Awaiting", Value = "Approval Awaiting" },
                        new SelectListItem { Text = "Rejected", Value = "Rejected" },
                        new SelectListItem { Text = "REMOVE APPROVAL AWAITING", Value = "REMOVE APPROVAL AWAITING" }
                        //new SelectListItem { Text = "Exported", Value = "Exported" }
                    };
                case ControlName.WOSInvoiceStatus:
                    return new[] {new SelectListItem { Text = "Draft", Value = "Draft" },
                        new SelectListItem { Text = "Send To Finance", Value = "Send To Finance" }, new SelectListItem { Text = "Send To Consultant", Value = "Send To Consultant" },
                        new SelectListItem { Text = "Approved", Value = "Approved" }, new SelectListItem { Text = "Finalized", Value = "Finalized" },
                        new SelectListItem { Text = "Exported", Value = "Exported" }
                    };
                case ControlName.AccessType:
                    return new[] { new SelectListItem { Text = "Origin", Value = "Origin" }, new SelectListItem { Text = "Destination", Value = "Destination" }, new SelectListItem { Text = "Via", Value = "Via" } };
                case ControlName.Reports:
                    return new[] { new SelectListItem { Text = "Enquiry Report", Value = "1" }, new SelectListItem { Text = "Survey Report", Value = "2" },
                        new SelectListItem { Text = "Estimate Report", Value = "3" }, new SelectListItem { Text = "Quote Report", Value = "4" },
                        new SelectListItem { Text= "Voxme Report",Value="5"}, new SelectListItem { Text = "B&C Report", Value = "6" },
                        new SelectListItem { Text = "PJR", Value = "7" }, new SelectListItem { Text = "DJR", Value = "8" },
                        new SelectListItem { Text = "GDPR Concent Form", Value = "9" }, new SelectListItem { Text = "GDPR Privacy Policy", Value = "10" }};
                case ControlName.InvCredit:
                    return new[] { new SelectListItem { Text = "Invoice", Value = "I" }, new SelectListItem { Text = "Credit Note", Value = "C" } };
                case ControlName.CreditCustApprovalType:
                    return new[] { new SelectListItem { Text = "Email approval", Value = "Email approval" }, new SelectListItem { Text = "PO based", Value = "PO based" } };
                case ControlName.CRPriority:
                    return new[] { new SelectListItem { Text = "Critical", Value = "Critical" }, new SelectListItem { Text = "High", Value = "High" }, new SelectListItem { Text = "Medium", Value = "Medium" }, new SelectListItem { Text = "Low", Value = "Low" } };
                case ControlName.NormalRev:
                    return new[] { new SelectListItem { Text = "Reverse", Value = "Rev" }, new SelectListItem { Text = "Normal", Value = "Normal" } };

                case ControlName.MSME:
                    return new[] { new SelectListItem { Text = "MSME", Value = "MSME" }, new SelectListItem { Text = "No", Value = "No" } };
                default:
                    return new List<SelectListItem>();

            }
        }

        //Generate encrypted QueryString
        public static string GenerateQueryString(string param, string[] valuearr)
        {
            string newText = string.Empty;
            for (int i = 0; i < valuearr.Length; i++)
            {
                string regex = "ParamValue" + i;
                param = param.Replace(regex, valuearr[i].ToString());
            }
            param = GetCrypt(param, 1);
            return param;
        }

        //get decrypted QueryString as Dictionary
        public static Dictionary<string, string> GetQueryString(string value)
        {
            value = GetCrypt(value, 2);
            string[] arr = value.Split(new string[] { "&" }, StringSplitOptions.None);
            KeyValuePair<string, string> a = new KeyValuePair<string, string>();
            var opts = new Dictionary<string, string>();
            for (int i = 0; i < arr.Length; i++)
            {
                var b = arr[i].Split('=');
                opts.Add(b[0], b[1]);
            }
            return opts;
        }

        //Flag (1=Encrypt, 2=Decrypt)
        public static string GetCrypt(string prmStr, int flag)
        {
            string convert_str = prmStr;
            if (flag == 1)
                convert_str = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(prmStr));
            else if (flag == 2)
                convert_str = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(prmStr));
            return convert_str;


        }

        public static Dictionary<string, string> Decrypt(string encryptedText)
        {
            encryptedText = encryptedText.Replace('-', '+').Replace('_', '/').Replace(',', '=');
            byte[] bytes = Convert.FromBase64String(encryptedText);
            string decryptedString = Encoding.UTF8.GetString(bytes);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string[] keyValuePair = decryptedString.Split('-');

            foreach (string key in keyValuePair)
            {
                string[] keyValue = key.Split('+');
                dictionary.Add(keyValue[0], keyValue[1]);
            }

            return dictionary;
        }

        public static string Encypt(Dictionary<string, string> dictionary)
        {
            byte[] queryString = Encoding.UTF8.GetBytes(string.Join("-", dictionary.Select(d => d.Key + "+" + d.Value)));
            string strBase64 = Convert.ToBase64String(queryString);
            return strBase64.Replace('+', '-').Replace('/', '_').Replace('=', ',');
        }
        //public static string FN()
        //{

        //    GetRegex(RegexType.Phone);
        //}

        public static bool ValidateJSON(string s)
        {
            try
            {
                JToken.Parse(s);
                return true;
            }
            catch (JsonReaderException ex)
            {
                //Trace.WriteLine(ex);
                return false;
            }
        }

        public static bool IsAllColumnExist(DataTable tableNameToCheck, List<String> columnsNames)
        {
            bool iscolumnExist = true;
            try
            {
                if (null != tableNameToCheck && tableNameToCheck.Columns != null)
                {
                    foreach (string columnName in columnsNames)
                    {
                        if (!tableNameToCheck.Columns.Contains(columnName))
                        {
                            iscolumnExist = false;
                            break;
                        }
                    }
                }
                else
                {
                    iscolumnExist = false;
                }
            }
            catch (Exception ex)
            {

            }
            return iscolumnExist;
        }

        public static bool GenerateExcel(HttpResponseBase Response, string excelName, string spName, Dictionary<string, string> exptoExlParameters)
        {
            try
            {
                var gv = new GridView();
                DataTable dtgridData = new DataTable();
                CommonSubs subs = new CommonSubs();
                string param = string.Join(",", exptoExlParameters.Select(x => x.Key + "=" + subs.QSafeValue(x.Value)).ToArray()).TrimEnd(',');
                string query = string.Format("EXEC {0} {1}", spName, param);
                dtgridData = subs.GetDataTable(query);
                //_spService.GetExportToExcelData(exptoExlParameters, spName);
                if (dtgridData.Rows.Count > 0)
                {
                    gv.DataSource = dtgridData;
                }
                else
                {

                    return false;

                    //dtgridData = new DataTable();
                    //dtgridData.Columns.Add("Message", typeof(string));
                    //dtgridData.Rows.Add("There are no items to display! ");
                    //gv.DataSource = dtgridData;
                }
                gv.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                System.IO.StringWriter objStringWriter = new System.IO.StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();

                return true;
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CommonService", "GenerateExcel", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public static string GetAbsoluteUrl(string relativeUrl)
        {
            relativeUrl = relativeUrl.Replace("~/", string.Empty);
            string[] splits = HttpContext.Current.Request.Url.AbsoluteUri.Split('/');
            if (splits.Length >= 2)
            {
                string url = splits[0] + "//";
                for (int i = 2; i < splits.Length - 1; i++)
                {
                    url += splits[i];
                    url += "/";
                }

                return url + relativeUrl;
            }
            return relativeUrl;
        }

        public async static Task<string> GetImageAsBase64Url(string url)
        {
            //var credentials = new NetworkCredential(user, pw);
            using (var handler = new HttpClientHandler())/*{ Credentials = credentials }*/
            using (var client = new HttpClient(handler))
            {
                var bytes = await client.GetByteArrayAsync(url);
                return "image/jpeg;base64," + Convert.ToBase64String(bytes);
            }
        }

        public static string GetBase64StringForImage(string imgPath)
        {
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            return "data:image/png;base64," + Convert.ToBase64String(imageBytes, 0, imageBytes.Length);
        }

        public static string AmountINWords(double value)
        {
            App_Code.CommonSubs CS = new App_Code.CommonSubs();
            string result = CS.NumberInWords(value.ToString());
            return result;
        }

        public static bool IsViewFileButton(string FileName)
        {
            List<string> AllowedExtensions = new List<string> { ".jpg", ".jpeg", ".jpe", ".BMP", ".gif", ".png", ".pdf" };
            bool exist = false;
            string extension = Path.GetExtension(FileName).ToLower();
            if (AllowedExtensions.Contains(extension))
            {
                return true;
            }
            return false;
        }

        public static bool GenerateExcel(HttpResponseBase Response, DataTable dataTable, string excelName)
        {
            try
            {
                var gv = new GridView();
                gv.AutoGenerateColumns = false;
                foreach (DataColumn dataCol in dataTable.Columns)
                {
                    BoundField boundCol = new BoundField()
                    {
                        DataField = dataCol.ColumnName,
                        HeaderText = dataCol.ColumnName,
                        HtmlEncode = false
                    };

                    gv.Columns.Add(boundCol);
                }

                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    gv.DataSource = dataTable;
                }
                else
                {

                    return false;
                }

                gv.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xls");
                Response.ContentType = "application/vnd.ms-excel";

                Response.Charset = "";
                System.IO.StringWriter objStringWriter = new System.IO.StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
                gv.RenderControl(objHtmlTextWriter);
                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();

                return true;
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CommonService", "GenerateExcel2", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


        }

        public static string MakeHtmlTable(DataTable data)
        {
            string[] table = new string[data.Rows.Count];
            long counter = 1;
            string htmlstring = "<tr>";
            foreach (DataColumn col in data.Columns)
            {
                htmlstring += "<th bgcolor=\"#DCDCDC\">" + col.ColumnName + "</th>";
            }
            htmlstring += "</tr>";
            foreach (DataRow row in data.Rows)
            {
                table[counter - 1] = "<tr><td>" + String.Join("</td><td>", (from o in row.ItemArray select o.ToString().Trim()).ToArray()) + "</td></tr>";

                counter += 1;
            }

            return "<table border=\"2px\">" + htmlstring + String.Join("", table) + "</table>";
        }

        public static IEnumerable<SelectListItem> GetSelectListItemsFromDatatable(DataTable dtTable, bool PrmAddAll = false)
        {
            List<SelectListItem> listItems = new List<SelectListItem>();

            try
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
                        SelectListItem item = new SelectListItem() { Value = dtTable.Rows[i][0].ToString(), Text = dtTable.Rows[i][1].ToString() };
                        listItems.Add(item);
                        //item.DataBind();
                    }
                }
            }
            catch (Exception EX)
            {
                //LogError(_Page, "AttachRadCombo", EX.ToString());
            }
            finally
            {
                if (PrmAddAll)
                    listItems.Insert(0, new SelectListItem() { Value = "-1", Text = "*All Items*" });

            }

            return listItems;

        }

        public static bool CheckValidApprovalLink(int LoginID, string Code)
        {
            bool result = false;

            try
            {
                DataTable dt = new CommonSubs().GetDataTable(string.Format("EXEC [Access].[CheckValidApprovalLinkTracker] @sp_code={0},@sp_loginid={1}", Code, Convert.ToString(LoginID)));

                if (dt != null && dt.Rows.Count > 0)
                {

                    return Convert.ToBoolean(Convert.ToInt16(dt.Rows[0]["IsValidLink"]));
                }

            }
            catch (Exception ex)
            {
            }

            return result;

        }
    }
}