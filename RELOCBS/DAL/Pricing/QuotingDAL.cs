using Newtonsoft.Json;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace RELOCBS.DAL.Pricing
{
    public class QuotingDAL
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

        public bool InsertQuoting(QuotingViewModel SaveRate, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SaveRate.HFVQuotingList, "CostHeadwiseDetails");
                        string QuotingHeadXml = node.ToString();


                        conn.AddCommand("[Quote].[AddEditQuote]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, -1, ParameterDirection.Input, (QuotingHeadXml));
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
                throw new DataAccessException(Convert.ToString(LoginID), "QuotingDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public IEnumerable<Quoting> GetForGrid(int LoginID, int RateComponetID, int? Page = 1)
        {

            try
            {
                string query = string.Format("exec [Rate].[ForGrid_GetUpdatedAgentRate]  @SP_LoginID={0},@SP_OrgCityID={1},@SP_OrgPortID={2},@SP_DestPortID={3},@SP_DestCityID={4}", Convert.ToString(LoginID), CSubs.QSafeValue(Convert.ToString(RateComponetID)));
                //@SP_AgentID,@SP_Mode ,@Sp_RMCID ,@SP_BussLineID ,@SP_GoodsDescID ,@SP_CompanyID ,

                DataTable dataTable = CSubs.GetDataTable(query);


                var result = (from rw in dataTable.AsEnumerable()
                              select new Quoting()
                              {
                                  WeightUnitFrom = Convert.ToInt64(rw["WeightFrom"]),
                                  WeightUnitTo = Convert.ToInt64(rw["WeightTo"]),
                                  RateCurrencyName = Convert.ToString(rw["RateCurr"]),
                                  RateReceived = Convert.ToString(rw["RateReceived"]),
                                  BaseCurrencyRateName = Convert.ToString(rw["BaseCurr"]),
                                  ConversionRate = Convert.ToUInt64(rw["BaseCurrConversRate"]),
                                  FromLocationName = Convert.ToString(rw["FromLoc"]),
                                  ToLocationName = Convert.ToString(rw["ToLoc"]),
                                  RMCName = Convert.ToString(rw["RMCName"]),
                                  AgentName = Convert.ToString(rw["AgentName"]),
                                  BusinessLineName = Convert.ToString(rw["AgentName"]),
                                  GoodsDescriptionName = Convert.ToString(rw["GoodsDescName"]),
                                  WeightUnitName = Convert.ToString(rw["WeightUnitName"])

                              }).ToList();


                return result;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "QuotingDAL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetDetailById(int? SurveyID, int? RateCompRateWtID, int LoginID,int RateCompRateBatchId)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("exec [Quote].[QuoteDetailsForSurvey] @SP_SurveryID={0},@SP_RateCompRateWtID={1},@SP_LoginID={2},@SP_RateCompRateBatchId={3}",
                 CSubs.QSafeValue(Convert.ToString(SurveyID)), CSubs.QSafeValue(Convert.ToString(RateCompRateWtID)), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(RateCompRateBatchId)));
                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "QuotingDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


            return Dtobj;
        }

        public bool UpdateUseForJobStatus(int SurveyID,int RateCompRateWtID,int LoginID, int RateCompRateBatchID,  out string message)
        {
            message = string.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        conn.AddCommand("[Quote].[UpdateQuoteReadyForJob]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyID", SqlDbType.BigInt, 0, ParameterDirection.Input, (SurveyID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateWtID", SqlDbType.BigInt, 0, ParameterDirection.Input, (RateCompRateWtID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateIdBatchID", SqlDbType.BigInt, 0, ParameterDirection.Input, RateCompRateBatchID);
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            message = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

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
                throw new DataAccessException(Convert.ToString(LoginID), "QuotingDAL", "UpdateUseForJobStatus", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool Delete(int SurveyID, int RateCompRateWtID, int RateCompRateBatchID, int LoginID, out string message)
        {
            message = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        conn.AddCommand("[Quote].[DeleteQuote]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyID", SqlDbType.BigInt, 0, ParameterDirection.Input, (SurveyID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateWtID", SqlDbType.BigInt, 0, ParameterDirection.Input, (RateCompRateWtID));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateBatchID", SqlDbType.BigInt, 0, ParameterDirection.Input, (RateCompRateBatchID));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            message = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

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
                throw new DataAccessException(Convert.ToString(LoginID), "QuotingDAL", "Delete", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertQuotingPrint(QuotePrint print, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        //System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(print.CostHeadList, "CostHeadwiseDetails");
                        //string QuotingHeadXml = node.ToString();

                        XElement CostHeadXml =
                            new XElement("Msges",
                                (from page in print.CostHeadList.Where(c=>c.Checked==true)
                                 select new XElement("Msg",
                                     new XElement("RateCompRateID", page.RateCompRateID),
                                     new XElement("CostHeadID", page.CostHeadID),
                                     new XElement("CompID", page.RateComponentID),
                                     new XElement("ActMsg", page.CostHeadDescription))
                                 )
                             );


                        var InExTermsList = print.InclusionList
                        .Concat(print.ExclusionList)
                        .Where(c=>c.Checked==true).ToList();

                        InExTermsList.AddRange(print.QuoteTermsList.Where(c=>c.Checked==true)
                                    .Select(t => new QuoteInclusionExclusion
                                    {
                                        CostHeadID = t.TermID,
                                        CostHeadName = t.TermName,
                                        CostHeadDescription=t.TermDescription,
                                        Type="T",
                                        Checked=t.Checked

                                     }).ToList());

                        XElement TermsXml =
                            new XElement("Msges",
                                (from page in InExTermsList.Where(c => c.Checked == true)
                                 select new XElement("Msg",
                                     new XElement("TIE", page.Type),
									 new XElement("ActDisplay", page.CostHeadName),
									 new XElement("ActMsg", page.CostHeadDescription))
                                 )
                             );

                        conn.AddCommand("[Quote].[AddEditTermscDetailsForQuote]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QouteTo", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(print.QuoteTo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QtIntroduction", SqlDbType.VarChar, 1000, ParameterDirection.Input, CSubs.PSafeValue(print.QuoteIntro));

                        if (print.QuoteTo.ToUpper()=="SHIPPER")
                        {
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QtTitle", SqlDbType.VarChar, 7, ParameterDirection.Input, CSubs.PSafeValue(print.Shipper_Title));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FName", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(print.ShipperFName));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LName", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(print.ShipperLName));
                        }
                        else
                        {
                            if (print.QuoteTo.ToUpper() == "CORPORATE")
                            {
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AccountClientID", SqlDbType.Int, 0, ParameterDirection.Input, print.AccountID);
                            }
                            else
                            {
                                conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AccountClientID", SqlDbType.Int, 0, ParameterDirection.Input, print.ClientID);
                            }
                        }
                        
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QtAttentionTitle", SqlDbType.VarChar, 7, ParameterDirection.Input, CSubs.PSafeValue(print.Attention));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QtAttention", SqlDbType.VarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(print.AttentionName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QtAddress1", SqlDbType.VarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(print.Address1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QtAddress2", SqlDbType.VarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(print.Address2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QtAddress3", SqlDbType.VarChar, 150, ParameterDirection.Input, CSubs.PSafeValue(print.Address3));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Cityid", SqlDbType.Int, 0, ParameterDirection.Input, print.City);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ZIp_Pin", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(print.Zip));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Insurance", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(print.Insurance));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(print.Remarks));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseMsg", SqlDbType.Xml, -1, ParameterDirection.Input, (CostHeadXml.ToString()));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TermsInclExclMsg", SqlDbType.Xml, -1, ParameterDirection.Input, (TermsXml.ToString()));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyID", SqlDbType.BigInt, 0, ParameterDirection.Input, print.SurveyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateWtD", SqlDbType.BigInt, 0, ParameterDirection.Input, print.RateCompRateWtID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateBatchId", SqlDbType.BigInt, 0, ParameterDirection.Input, print.RateCompRateWtBatchID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QtSubject", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(print.Subject));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QtCrr", SqlDbType.Int, 0, ParameterDirection.Input, print.Quoted_Curr);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QtCrrExchageRate", SqlDbType.Float, 0, ParameterDirection.Input, print.QuotedExchange_rate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsLumSum", SqlDbType.Bit, 0, ParameterDirection.Input, print.IsLumsum);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SentByID", SqlDbType.Int, 0, ParameterDirection.Input, print.SentBy);
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
                throw new DataAccessException(Convert.ToString(LoginID), "QuotingDAL", "InsertQuotingPrint", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

		public DataSet GetQuotingPrintDetail(int? SurveyID, int? RateCompRateWtID, int? RateCompRateWtBatchID, bool? IsLumsum, int CompanyID, int LoginID)
		{
			DataSet Dtobj = new DataSet();

			try
			{
				string query = string.Format("exec [Quote].[GetInclExlTermscDetailsForQuote] @SP_SurveyID={0},@SP_CompanyID={1},@SP_RateCompRateWtID={2},@SP_IsLumSum={3},@SP_LoginID={4},@SP_RateCompRateBatchId={5}",
				 CSubs.QSafeValue(Convert.ToString(SurveyID)),
				 CSubs.QSafeValue(Convert.ToString(CompanyID)),
				 CSubs.QSafeValue(Convert.ToString(RateCompRateWtID)),
				 CSubs.QSafeValue(Convert.ToString(IsLumsum)),
				 CSubs.QSafeValue(Convert.ToString(LoginID)),
				 CSubs.QSafeValue(Convert.ToString(RateCompRateWtBatchID))
				 );
				Dtobj = CSubs.GetDataSet(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "QuotingDAL", "GetQuotingPrintDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}


			return Dtobj;
		}

		public DataSet GetQuotingPrint(int? SurveyID, int? RateCompRateWtID, int? RateCompRateWtBatchID, bool? IsLumsum, int CompanyID, int LoginID,Int16 IsEmail)
		{
			DataSet Dtobj = new DataSet();

			try
			{
				string query = string.Format("exec [Quote].[GetInclExlTermscDetailsForQuotePrint] @SP_SurveyID={0},@SP_CompanyID={1},@SP_RateCompRateWtID={2},@SP_IsLumSum={3},@SP_LoginID={4},@SP_RateCompRateBatchId={5},@SP_IsEmail={6}",
				 CSubs.QSafeValue(Convert.ToString(SurveyID)),
				 CSubs.QSafeValue(Convert.ToString(CompanyID)),
				 CSubs.QSafeValue(Convert.ToString(RateCompRateWtID)),
				 CSubs.QSafeValue(Convert.ToString(IsLumsum)),
				 CSubs.QSafeValue(Convert.ToString(LoginID)),
				 CSubs.QSafeValue(Convert.ToString(RateCompRateWtBatchID)),
                 CSubs.QSafeValue(Convert.ToString(IsEmail))
                 );
				Dtobj = CSubs.GetDataSet(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "QuotingDAL", "GetQuotingPrint", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}


			return Dtobj;
		}

		public DataTable GetCompareQuote(int SurveyID, int LoginID)
        {
            string querys = string.Format("EXEC [Quote].[GetComparableQuotes] @SP_SurveyID={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(SurveyID)),
             CSubs.QSafeValue(Convert.ToString(LoginID)));

            DataTable data = CSubs.GetDataTable(querys);

            return data;

        }

        public bool ApproveQuote(int SurveyID,bool IsRemoveApproval, bool QuoteSentApprove, int? QuoteSenttoApproveUser, int LoginID, int RateCompRateBatchID, out string message)
        {
            message = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        conn.AddCommand("[Quote].[ApproveQuote]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyID", SqlDbType.BigInt, 0, ParameterDirection.Input, (SurveyID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRemoveApproval", SqlDbType.Bit, 0, ParameterDirection.Input, (IsRemoveApproval));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsQuoteSentApprove", SqlDbType.Bit, 0, ParameterDirection.Input, (QuoteSentApprove));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ApprovalUser", SqlDbType.Int, 0, ParameterDirection.Input, QuoteSenttoApproveUser); 
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateBatchID", SqlDbType.Int, 0, ParameterDirection.Input, RateCompRateBatchID); 
						conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            message = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

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
                throw new DataAccessException(Convert.ToString(LoginID), "QuotingDAL", "ApproveQuote", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}