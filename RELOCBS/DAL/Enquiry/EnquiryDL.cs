using Newtonsoft.Json;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL.Enquiry
{
	public class EnquiryDL
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
		public bool Insert(RELOCBS.Entities.Enquiry enquiry, out string result)
		{
			result = string.Empty;

			try
			{

				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
						System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(enquiry.EnquiryListHidden, "EnqDetails");

						//xml.OuterXml;
						string EnquiryDetailsXml = node.ToString();

						conn.AddCommand("[Enq].[AddEditEnq]", QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AddEditDelete", SqlDbType.VarChar, 1, ParameterDirection.Input, ((enquiry.EnqID > 0) ? "E" : "I"));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ReloSmrtEnqNo", SqlDbType.VarChar, 20, ParameterDirection.Input, enquiry.ReloSmrtEnqNo);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, enquiry.EnqID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqNo", SqlDbType.VarChar, 20, ParameterDirection.Input, (enquiry.EnqID > 0) ? enquiry.RevenueBr.Substring(0, 3) + enquiry.BussLineName.Substring(0, 1) + enquiry.EnqID : enquiry.RevenueBr.Substring(0, 3) + enquiry.BussLineName.Substring(0, 1));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqSourceID", SqlDbType.Int, 0, ParameterDirection.Input, enquiry.EnqSourceID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (enquiry.EnqDate != null ? enquiry.EnqDate : DateTime.Now));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqFrom", SqlDbType.VarChar, 100, ParameterDirection.Input, (enquiry.EnqFrom));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqReceivedby", SqlDbType.VarChar, 0, ParameterDirection.Input, (enquiry.EnqReceivedby));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FollowupDate", SqlDbType.DateTime, 0, ParameterDirection.Input, enquiry.ContactDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BussinessLineID", SqlDbType.Int, 0, ParameterDirection.Input, (enquiry.BussinessLineID));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentID", SqlDbType.Int, 0, ParameterDirection.Input, enquiry.AgentID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveQuoteID", SqlDbType.Int, 0, ParameterDirection.Input, (enquiry.MoveQuoteID));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RevenueBrId", SqlDbType.Int, 0, ParameterDirection.Input, (enquiry.RevenueBrId));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChangeAcctMgrID", SqlDbType.Int, 0, ParameterDirection.Input, (enquiry.ChangeAcctMgrID));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperFName", SqlDbType.VarChar, 30, ParameterDirection.Input, (enquiry.ShipperFName));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperLName", SqlDbType.VarChar, 30, ParameterDirection.Input, (enquiry.ShipperLName));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipTypeID", SqlDbType.Int, 0, ParameterDirection.Input, enquiry.ShipTypeID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipCategoryID", SqlDbType.Int, 0, ParameterDirection.Input, enquiry.ShipCategoryID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 500, ParameterDirection.Input, (enquiry.Address1));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 500, ParameterDirection.Input, (enquiry.Address2));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Email", SqlDbType.VarChar, 500, ParameterDirection.Input, (enquiry.Email));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AddressCityID", SqlDbType.Int, 0, ParameterDirection.Input, (enquiry.AddressCityID));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PIN", SqlDbType.VarChar, 10, ParameterDirection.Input, (enquiry.PIN));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone1", SqlDbType.VarChar, 50, ParameterDirection.Input, (enquiry.Phone1));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone2", SqlDbType.VarChar, 50, ParameterDirection.Input, (enquiry.Phone2));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 500, ParameterDirection.Input, (enquiry.Remarks));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Isactive", SqlDbType.Bit, 0, ParameterDirection.Input, true);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LostDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (enquiry.LostDate));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LostToCompitID", SqlDbType.Int, 0, ParameterDirection.Input, (enquiry.LostToCompitID));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LostToText", SqlDbType.VarChar, 100, ParameterDirection.Input, (enquiry.LostTo));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LostReasonID", SqlDbType.Int, 0, ParameterDirection.Input, (enquiry.LostReasonID));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LostRemarks", SqlDbType.VarChar, 100, ParameterDirection.Input, (enquiry.LostRemarks));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqDetailsXML", SqlDbType.Xml, -1, ParameterDirection.Input, EnquiryDetailsXml);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClientRef", SqlDbType.VarChar, 500, ParameterDirection.Input, enquiry.ClientRef);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClientCP", SqlDbType.VarChar, 500, ParameterDirection.Input, enquiry.ClientCP);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CPEmail", SqlDbType.VarChar, 500, ParameterDirection.Input, enquiry.CPEmail);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TendativeMoveDate", SqlDbType.DateTime, 500, ParameterDirection.Input, enquiry.TentativeDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContactDate", SqlDbType.DateTime, 500, ParameterDirection.Input, enquiry.ContactDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperTitle", SqlDbType.VarChar, 10, ParameterDirection.Input, enquiry.ShipperTitle);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperDOB", SqlDbType.DateTime, 0, ParameterDirection.Input, null);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperNationality", SqlDbType.VarChar, 25, ParameterDirection.Input, enquiry.Nationality);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperDesig ", SqlDbType.VarChar, 25, ParameterDirection.Input, enquiry.Designation);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AccountID ", SqlDbType.Int, 25, ParameterDirection.Input, enquiry.ClientDetails.Client);
						conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
						conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

						if (!conn.IsError)
						{
							int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
							result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

							if (ReturnStatus == 0)
							{
								enquiry.EnqID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_EnqID"));
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
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EnquiryDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
			//return true;
		}

		public DataSet GetDetailById(int EnqDetailID, int EnquiryID = 0)
		{
			DataSet EnquiryDetailDt = new DataSet();

			try
			{
				string query = string.Format("EXEC [Enq].[GetEnqueryDetailsForDisplay] @SP_LoginID={0}, @SP_EnqID={1}",
				UserSession.GetUserSession().LoginID, EnquiryID);
				EnquiryDetailDt = CSubs.GetDataSet(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EnquiryDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return EnquiryDetailDt;

		}

		public IQueryable<Entities.Enquiry> GetEnquiryList(DateTime? FromDate, DateTime? Todate, string EnqID = null, bool? RMCBuss = false, string Shipper = null)
		{
			int LoggedinUserID = UserSession.GetUserSession().LoginID;
			try
			{

				IQueryable<Entities.Enquiry> data = null;

				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
						conn.AddCommand("[Enq].[GetEnqueryForGrid]", QueryType.Procedure);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqFromDate", SqlDbType.DateTime, 1, ParameterDirection.Input, FromDate);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqToDate", SqlDbType.DateTime, 0, ParameterDirection.Input, Todate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 8, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqID", SqlDbType.VarChar, 20, ParameterDirection.Input, EnqID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 8, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Shipper", SqlDbType.VarChar, 50, ParameterDirection.Input, Shipper);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBuss", SqlDbType.Bit, 0, ParameterDirection.Input, RMCBuss);
						DataTable dt = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

						if (dt != null)
						{
							var result = (from rw in dt.AsEnumerable()
										  select new Entities.Enquiry()
										  {
											  EnqID = rw["EnqID"] == DBNull.Value ? (Int64)0 : Convert.ToInt64(rw["EnqID"]),
											  EnqNo = rw["EnqNo"] == DBNull.Value ? null : Convert.ToString(rw["EnqNo"]),
											  EnqDate = rw["EnqDate"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(rw["EnqDate"]),
											  BussLineName = rw["BussLineName"] == DBNull.Value ? null : Convert.ToString(rw["BussLineName"]),
											  AgentName = rw["AgentName"] == DBNull.Value ? null : Convert.ToString(rw["AgentName"]),
											  //FollowupDate = rw["FollowupDate"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(rw["FollowupDate"]),
											  ShipperFName = rw["ShipperFName"] == DBNull.Value ? null : Convert.ToString(rw["ShipperFName"]) + Convert.ToString(rw["ShipperLName"]),
											  //ShipperLName = rw["ShipperLName"] == DBNull.Value ? null : Convert.ToString(rw["ShipperLName"]),
											  City = rw["CityName"] == DBNull.Value ? null : Convert.ToString(rw["CityName"]),
											  Phone1 = rw["Phone1"] == DBNull.Value ? null : Convert.ToString(rw["Phone1"]),
											  RevenueBr = rw["RevenueBr"] == DBNull.Value ? null : Convert.ToString(rw["RevenueBr"]),
											  Remarks = rw["EnqStatus"] == DBNull.Value ? null : Convert.ToString(rw["EnqStatus"]),
											  ClientDetails = new Entities.ClientDetails
											  {
												  Account = rw["Account"] == DBNull.Value ? null : Convert.ToString(rw["Account"])
												  //Client = rw["AccountID"] == DBNull.Value ? (Int32)0 : Convert.ToInt32(rw["AccountID"])
											  },
											  EnqStatus = !string.IsNullOrWhiteSpace(Convert.ToString(rw["EnqID"])) ? new CommanBL().GetStatusById(Convert.ToInt64(rw["EnqID"]), "Enquiry", "Main").StatusName : ""
										  }).ToList();
							data = result.AsQueryable<Entities.Enquiry>();
						}
					}
					else
						throw new Exception(conn.ErrorMessage);
				}
				return data;
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoggedinUserID), "EnquiryDAL", "GetEnquiryList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			//return null;

		}

		public DataTable GetProjectByServiceLine(int ServiceLineID)
		{
			DataTable ProjectDt = new DataTable();

			try
			{
				string query = string.Format("EXEC [Comm].[GetServiceLineProject] @SP_ServiceLineID={0}",
				ServiceLineID);
				ProjectDt = CSubs.GetDataTable(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EnquiryDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return ProjectDt;
		}

		
		public bool SentToMobile(int? EnqID, int? EnquiryDetailID, out string result)
		{
			result = string.Empty;

			try
			{

				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{
						conn.AddCommand("[Voxme].[UpdateEnqMastVox]", QueryType.Procedure);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqID", SqlDbType.BigInt, 0, ParameterDirection.Input, EnqID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqDtlID", SqlDbType.BigInt, 0, ParameterDirection.Input, EnquiryDetailID);
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
				throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EnquiryDAL", "SentToMobile", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
			//return true;
		}

        public bool UpdateSurveyorIdGMMS(JobSurveyForMobile model,int LoginID, out string result)
        {
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Voxme].[UpdateSurveyorIdGMMS]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveryorID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.SurveyerID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqDtlID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.EnqDetailID);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EnquiryDAL", "UpdateSurveyorIdGMMS", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

		public bool InsertFollowUpDetials(Entities.Enquiry SaveData, int LoginID, out string result)
		{
			result = String.Empty;

			try
			{
				using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				{
					if (conn.Connect())
					{

						conn.AddCommand("[Enq].[AddEditEnqFollowupInfo]", QueryType.Procedure);
						//conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqDetID", SqlDbType.Int, 0, ParameterDirection.Input, SaveData.FollowUp.EnqDetID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FollowupDate", SqlDbType.Date, 0, ParameterDirection.Input, SaveData.FollowUp.FollowUpDate);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FollowupRemarks", SqlDbType.VarChar, 1000, ParameterDirection.Input, SaveData.FollowUp.FollowUpRemark);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsClose", SqlDbType.Bit, 0, ParameterDirection.Input, SaveData.FollowUp.IsClose);
						conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
						conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
						conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

						if (!conn.IsError)
						{
							int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
							result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

							if (ReturnStatus == 0)
							{
								//SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
				return true;

			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "EnquiryDAL", "InsertFollowUpDetials", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}
		}
	}
}