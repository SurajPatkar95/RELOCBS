using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace WSGCBS.App_Code
{
    public class SalesForceIntgr
    {
    }

    public class SalesforceClient
    {
        string LOGIN_ENDPOINT = System.Configuration.ConfigurationManager.ConnectionStrings["SFLOGINENDPOINT"].ToString();
        private string API_ENDPOINT = System.Configuration.ConfigurationManager.ConnectionStrings["SFAPIENDPOINT"].ToString();

        public string Username { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthToken { get; set; }
        public string InstanceUrl { get; set; }

        //static SalesforceClient()
        //{
        //    // SF requires TLS 1.1 or 1.2
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;
        //}
        // TODO: use RestSharps
        public DataTable SalesforceClientLogin(DataTable DTdetail, int ModuleID)
        {
            //DTdetail = DatagetDetails("", "", 0, 0);
            DataTable resDTdetail = null;
            if (DTdetail != null && DTdetail.Rows.Count != 0)
            {
                for (int i = 0; i < DTdetail.Rows.Count; i++)
                {
                    ClientId = DTdetail.Rows[i]["client_id"].ToString();
                    ClientSecret = DTdetail.Rows[i]["client_secret"].ToString();
                    Username = DTdetail.Rows[i]["username"].ToString();
                    Password = DTdetail.Rows[i]["password"].ToString();
                    LOGIN_ENDPOINT = DTdetail.Rows[i]["URL"].ToString();

                    String jsonResponse;
                    using (var client = new HttpClient())
                    {
                        var request = new FormUrlEncodedContent(new Dictionary<string, string>
                            {
                                {"grant_type", "password"},
                                {"client_id", ClientId},
                                {"client_secret", ClientSecret},
                                {"username", Username},
                                {"password", Password + Token}
                            });
                        request.Headers.Add("X-PrettyPrint", "1");
                        var response = client.PostAsync(LOGIN_ENDPOINT, request).Result;
                        jsonResponse = response.Content.ReadAsStringAsync().Result;
                    }
                    //Console.WriteLine($"Response: {jsonResponse}");
                    var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);
                    AuthToken = values["access_token"];
                    InstanceUrl = values["instance_url"];

                    resDTdetail = DatagetDetails(AuthToken, InstanceUrl, 1, Convert.ToInt32(DTdetail.Rows[i]["TokenMasterID"].ToString()), ModuleID);
                }
            }
            return resDTdetail;
        }

        //public string QueryEndpoints()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        string restQuery = InstanceUrl + API_ENDPOINT;
        //        var request = new HttpRequestMessage(HttpMethod.Get, restQuery);
        //        request.Headers.Add("Authorization", "Bearer " + AuthToken);
        //        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        request.Headers.Add("X-PrettyPrint", "1");
        //        var response = client.SendAsync(request).Result;
        //        return response.Content.ReadAsStringAsync().Result;
        //    }
        //}

        //public string Describe(string sObject)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        string restQuery = InstanceUrl + API_ENDPOINT + "sobjects/" + sObject;
        //        var request = new HttpRequestMessage(HttpMethod.Get, restQuery);
        //        request.Headers.Add("Authorization", "Bearer " + AuthToken);
        //        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        request.Headers.Add("X-PrettyPrint", "1");
        //        var response = client.SendAsync(request).Result;
        //        return response.Content.ReadAsStringAsync().Result;
        //    }
        //}

        //public string Query(string soqlQuery)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        string restRequest = InstanceUrl + API_ENDPOINT + "query/?q=" + soqlQuery;
        //        var request = new HttpRequestMessage(HttpMethod.Get, restRequest);
        //        request.Headers.Add("Authorization", "Bearer " + AuthToken);
        //        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        request.Headers.Add("X-PrettyPrint", "1");
        //        var response = client.SendAsync(request).Result;
        //        return response.Content.ReadAsStringAsync().Result;
        //    }
        //}

        public DataTable DatagetDetails(string access_token, string instance_url, int SituationID, int TokenMasterID, int ModuleID)
        {
            DataTable dtresult = null;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["WSGDB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("usp_tbl_SalesforceTokenMaster", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@access_token", SqlDbType.NVarChar, 800, ParameterDirection.Input, access_token);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@instance_url", SqlDbType.NVarChar, 800, ParameterDirection.Input, instance_url);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@TokenMasterID", SqlDbType.Int, 0, ParameterDirection.Input, TokenMasterID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SituationID", SqlDbType.Int, 0, ParameterDirection.Input, SituationID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@ModuleID", SqlDbType.Int, 0, ParameterDirection.Input, ModuleID);

                        DataTable dt = new DataTable();
                        dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);
                        if (!conn.IsError)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                dtresult = dt;
                            }
                        }
                        else
                            throw new Exception(conn.ErrorMessage);
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (System.Threading.ThreadAbortException EX)
            {
                dtresult = null;
            }
            return dtresult;
        }

        public void Opportunitylineitem(string OpportunityID, int LoginID, string Module)
        {
            DataTable DTdetail, DTdetails, DTSalesforceDetails;

            Opportunitylineitem aa = new Opportunitylineitem();
            List<Record> bb = new List<Record>();

            DTSalesforceDetails = Request_SalesforceDetails(1, OpportunityID, Module);

            if (DTSalesforceDetails != null && DTSalesforceDetails.Rows.Count != 0)
            {
                for (int i = 0; i < DTSalesforceDetails.Rows.Count; i++)
                {
                    Attributes cc = new Attributes()
                    {
                        method = DTSalesforceDetails.Rows[i]["method"].ToString(),
                        type = DTSalesforceDetails.Rows[i]["type"].ToString(),
                        referenceId = DTSalesforceDetails.Rows[i]["referenceId"].ToString()
                    };

                    bb.Add(new Record()
                    {
                        attributes = cc,
                        OpportunityId = DTSalesforceDetails.Rows[i]["OpportunityId"].ToString(),
                        Quantity = DTSalesforceDetails.Rows[i]["Quantity"].ToString(),
                        pricebookentryId = DTSalesforceDetails.Rows[i]["pricebookentryId"].ToString(),
                        Product2Id = DTSalesforceDetails.Rows[i]["Product2Id"].ToString(),
                        UnitPrice = DTSalesforceDetails.Rows[i]["UnitPrice"].ToString(),
                        RGP__c = DTSalesforceDetails.Rows[i]["RGP__c"].ToString(),
                        External_ID__c = DTSalesforceDetails.Rows[i]["External_ID__c"].ToString(),
                        Feasibility_Remark__c = DTSalesforceDetails.Rows[i]["Feasibility_Remark__c"].ToString(),
                        Feasibility_Status__c = DTSalesforceDetails.Rows[i]["Feasibility_Status__c"].ToString(),
                        Frequency__c = DTSalesforceDetails.Rows[i]["Frequency__c"].ToString(),
                        LOI_Recieved__c = DTSalesforceDetails.Rows[i]["LOI_Recieved__c"].ToString(),
                        Operation_Started__c = DTSalesforceDetails.Rows[i]["Operation_Started__c"].ToString(),
                        Pickup_Address__c = DTSalesforceDetails.Rows[i]["Pickup_Address__c"].ToString(),
                        Pickup_Code__c = DTSalesforceDetails.Rows[i]["Pickup_Code__c"].ToString(),
                        Pincode__c = DTSalesforceDetails.Rows[i]["Pincode__c"].ToString(),
                        ServiceDate = DTSalesforceDetails.Rows[i]["ServiceDate"].ToString(),
                        Cash_Pickup_Limit__c = DTSalesforceDetails.Rows[i]["Cash_Pickup_Limit__c"].ToString()
                    });
                }

                aa.records = bb;
                int ModuleID = Convert.ToInt32(DTSalesforceDetails.Rows[0]["ModuleID"].ToString());
                DTdetail = DatagetDetails("", "", 0, 0, ModuleID);

                if (Convert.ToBoolean(DTdetail.Rows[0]["TokenExpire"].ToString()) == true)
                {
                    DTdetails = SalesforceClientLogin(DTdetail, ModuleID);
                }
                else
                {
                    DTdetails = DTdetail;
                }

                string URL = DTdetail.Rows[0]["ModulePath"].ToString();
                string varMethod = DTdetail.Rows[0]["Method"].ToString();
                string AuthToken = DTdetail.Rows[0]["access_token"].ToString();
                string Message = JsonConvert.SerializeObject(aa);

                string varErrorOrNot = "";

                string Retult = APICall(URL, Message.ToString(), AuthToken, varMethod, ModuleID, out varErrorOrNot);

                bool hasErrors = false;

                if (varErrorOrNot == "Successful")
                {
                    var dt = JsonConvert.DeserializeObject<dynamic>(Retult);
                    hasErrors = Convert.ToBoolean(dt.hasErrors.Value);

                    for (int i = 0; i < DTSalesforceDetails.Rows.Count; i++)
                    {
                        foreach (var item in dt.results)
                        {
                            if (item.referenceId.Value == DTSalesforceDetails.Rows[i]["referenceId"].ToString())
                            {
                                USPATMOpportunityTimeLine(DTSalesforceDetails.Rows[i]["referenceId"].ToString(), OpportunityID, item.referenceId.Value, item.id.Value, LoginID, false, 0, Module);
                            }
                        }
                    }
                }
                Response_SalesforceDetails(ModuleID, Message.ToString(), Retult, OpportunityID, hasErrors, Module);
            }
        }

        public void Composite(string OpportunityID, int LoginID, string Module)
        {
            DataTable DTdetail, DTdetails, DTSalesforceDetails;

            Composite aa = new Composite();
            List<CompositeRequest> bb = new List<CompositeRequest>();

            DTSalesforceDetails = Request_SalesforceDetails(2, OpportunityID, Module);

            if (DTSalesforceDetails != null && DTSalesforceDetails.Rows.Count != 0)
            {

                for (int i = 0; i < DTSalesforceDetails.Rows.Count; i++)
                {
                    Body cc = new Body()
                    {
                        LOI_Recieved__c = DTSalesforceDetails.Rows[i]["LOI_Recieved__c"].ToString(),
                        Feasibility_Status__c = DTSalesforceDetails.Rows[i]["Feasibility_Status__c"].ToString()
                    };

                    bb.Add(new CompositeRequest()
                    {
                        method = DTSalesforceDetails.Rows[i]["method"].ToString(),
                        url = DTSalesforceDetails.Rows[i]["url"].ToString(),
                        referenceId = DTSalesforceDetails.Rows[i]["referenceId"].ToString(),
                        body = cc
                    });
                }
                aa.compositeRequest = bb;
                int ModuleID = Convert.ToInt32(DTSalesforceDetails.Rows[0]["ModuleID"].ToString());
                DTdetail = DatagetDetails("", "", 0, 0, ModuleID);

                if (Convert.ToBoolean(DTdetail.Rows[0]["TokenExpire"].ToString()) == true)
                {
                    DTdetails = SalesforceClientLogin(DTdetail, ModuleID);
                }
                else
                {
                    DTdetails = DTdetail;
                }

                string URL = DTdetail.Rows[0]["ModulePath"].ToString();
                string varMethod = DTdetail.Rows[0]["Method"].ToString();
                string AuthToken = DTdetail.Rows[0]["access_token"].ToString();
                string Message = JsonConvert.SerializeObject(aa);

                string varErrorOrNot = "";
                string Retult = APICall(URL, Message.ToString(), AuthToken, varMethod, ModuleID, out varErrorOrNot);

                bool hasErrors = false;

                if (varErrorOrNot == "Successful")
                {
                    var dt = JsonConvert.DeserializeObject<dynamic>(Retult);
                    hasErrors = false;

                    for (int i = 0; i < DTSalesforceDetails.Rows.Count; i++)
                    {
                        foreach (var item in dt.compositeResponse)
                        {
                            if (item.body.id.Value == DTSalesforceDetails.Rows[i]["ID"].ToString() && item.body.success.Value)
                            {
                                USPATMOpportunityTimeLine(DTSalesforceDetails.Rows[i]["referenceId"].ToString(), DTSalesforceDetails.Rows[i]["OpportunityID"].ToString(), DTSalesforceDetails.Rows[i]["referenceId"].ToString(), item.body.id.Value, LoginID, item.body.success.Value, 1, Module);
                            }
                        }
                    }
                }
                Response_SalesforceDetails(ModuleID, Message.ToString(), Retult, OpportunityID, hasErrors, Module);
            }
        }

        public string APICall(string URL, string Message, string AuthToken, string varMethod, int ModuleID, out string varErrorOrNot)
        {
            string StrReturn = "";

            var request = WebRequest.Create(URL) as HttpWebRequest;
            request.KeepAlive = true;
            request.Method = varMethod; // "POST";
            request.ContentType = "application/json";

            request.Headers.Add("authorization", "Bearer " + AuthToken);

            byte[] byteArray;
            byteArray = Encoding.UTF8.GetBytes(Message.ToString());

            string responseContent = null;
            try
            {
                using (var writer = request.GetRequestStream())
                {
                    writer.Write(byteArray, 0, byteArray.Length);
                }
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }

                StrReturn = responseContent;
                varErrorOrNot = "Successful";
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse err = ex.Response as HttpWebResponse;
                    if (err != null)
                    {
                        StrReturn = new StreamReader(err.GetResponseStream()).ReadToEnd();
                    }
                }
                varErrorOrNot = "Failed";
            }
            catch (Exception ex)
            {
                StrReturn = ex.Message;
                varErrorOrNot = "Failed";
            }
            return StrReturn;
        }

        public DataTable Request_SalesforceDetails(int SituationID, string OpportunityID, string Module)
        {
            DataTable dtresult = null;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["WSGDB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("usp_Request_Salesforce", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SituationID", SqlDbType.Int, 0, ParameterDirection.Input, SituationID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@OpportunityID", SqlDbType.NVarChar, 300, ParameterDirection.Input, OpportunityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Module", SqlDbType.NVarChar, 300, ParameterDirection.Input, Module);

                        DataTable dt = new DataTable();
                        dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);
                        if (!conn.IsError)
                        {
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                dtresult = dt;
                            }
                        }
                        else
                            throw new Exception(conn.ErrorMessage);
                    }
                    else
                        throw new Exception(conn.ErrorMessage);
                }
            }
            catch (System.Threading.ThreadAbortException EX)
            {
                dtresult = null;
            }

            return dtresult;
        }

        public string Response_SalesforceDetails(int ModuleID, string StrRequest, string StrResponse, string OpportunityID, bool IsError, string Module)
        {
            string Result = "";
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WSGDB"].ToString());
            SqlCommand cmd = new SqlCommand("usp_tbl_SalesforceResponse", con);
            try
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@ModuleID", ModuleID));
                cmd.Parameters.Add(new SqlParameter("@StrRequest", StrRequest));
                cmd.Parameters.Add(new SqlParameter("@StrResponse", StrResponse));
                cmd.Parameters.Add(new SqlParameter("@OpportunityID", OpportunityID));
                cmd.Parameters.Add(new SqlParameter("@IsError", IsError));
                cmd.Parameters.Add(new SqlParameter("@Module", Module));


                SqlParameter outputParams = new SqlParameter("@MESSAGE", SqlDbType.VarChar, 500);
                outputParams.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParams);
                cmd.ExecuteNonQuery();
                Result = outputParams.Value.ToString();
            }
            catch (SqlException e)
            {
                Result = e.ToString();
            }
            finally
            {
                con.Close();
            }
            return Result;
        }


        public string USPATMOpportunityTimeLine(string FeasibilityID, string OpportunityID, string RefID, string OpportunityTimeLineID, int LOGINID, bool IsComposite, int SituationID, string Module)
        {
            string Result = "";
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["WSGDB"].ToString());
            SqlCommand cmd = new SqlCommand("ATM_ATMOpportunityTimeLine", con);
            try
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SP_FeasibilityID", FeasibilityID));
                cmd.Parameters.Add(new SqlParameter("@SP_OpportunityID", OpportunityID));
                cmd.Parameters.Add(new SqlParameter("@SP_RefID", RefID));
                cmd.Parameters.Add(new SqlParameter("@SP_OpportunityTimeLineID", OpportunityTimeLineID));
                cmd.Parameters.Add(new SqlParameter("@SP_LOGINID", LOGINID));
                cmd.Parameters.Add(new SqlParameter("@SP_IsComposite", IsComposite));
                cmd.Parameters.Add(new SqlParameter("@Sp_SituationID", SituationID));
                cmd.Parameters.Add(new SqlParameter("@Sp_Module", Module));


                SqlParameter outputParams = new SqlParameter("@MESSAGE", SqlDbType.VarChar, 500);
                outputParams.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParams);
                cmd.ExecuteNonQuery();
                Result = outputParams.Value.ToString();
            }
            catch (SqlException e)
            {
                Result = e.ToString();
            }
            finally
            {
                con.Close();
            }
            return Result;
        }
    }

    public class Attributes
    {
        public string method { get; set; }
        public string type { get; set; }
        public string referenceId { get; set; }
    }

    public class Record
    {
        public Attributes attributes { get; set; }
        public string OpportunityId { get; set; }
        public string Quantity { get; set; }
        public string pricebookentryId { get; set; }
        public string Product2Id { get; set; }
        public string UnitPrice { get; set; }
        public string RGP__c { get; set; }
        public string External_ID__c { get; set; }
        public string Feasibility_Remark__c { get; set; }
        public string Feasibility_Status__c { get; set; }
        public string Frequency__c { get; set; }
        public string LOI_Recieved__c { get; set; }
        public string Operation_Started__c { get; set; }
        public string Pickup_Address__c { get; set; }
        public string Pickup_Code__c { get; set; }
        public string Pincode__c { get; set; }
        public string ServiceDate { get; set; }
        public string Cash_Pickup_Limit__c { get; set; }
    }

    public class Opportunitylineitem
    {
        public List<Record> records { get; set; }
    }

    #region Composite DTO

    public class Body
    {
        public string LOI_Recieved__c { get; set; }
        public string Feasibility_Status__c { get; set; }
    }

    public class CompositeRequest
    {
        public string method { get; set; }
        public string url { get; set; }
        public string referenceId { get; set; }
        public Body body { get; set; }
    }

    public class Composite
    {
        public List<CompositeRequest> compositeRequest { get; set; }
    }

    #endregion
}