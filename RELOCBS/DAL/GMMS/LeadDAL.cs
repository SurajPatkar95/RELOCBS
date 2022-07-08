using Newtonsoft.Json;
using RELOCBS.App_Code;
using RELOCBS.Common;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL.GMMS
{
    public class LeadDAL
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


        public bool InsertLead(LeadViewModel lead, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[GMMS].[AddEditLead]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ForRMCID", SqlDbType.Int, 0, ParameterDirection.Input, lead.RMCID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCityID", SqlDbType.Int, 0, ParameterDirection.Input, lead.FromCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestCityID", SqlDbType.Int, 0, ParameterDirection.Input, lead.ToCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
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
                throw new DataAccessException(Convert.ToString(LoginID), "LeadDAL", "InsertLead", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertJob(LeadViewModel lead, int LoginID, out string result, out Int64 MoveId)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(lead.HFEnqDet, "EnqDetails");

                        //xml.OuterXml;
                        string EnquiryDetailsXml = node.ToString();
                        conn.AddCommand("[RMC].[OpeningJobByPricing]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FSFRLeadDetailMasterID", SqlDbType.BigInt, 0, ParameterDirection.Input, lead.manageDet.FSFRLeadDetailsID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_UpdatedBatchID", SqlDbType.BigInt, 0, ParameterDirection.Input, lead.manageDet.UpdatedBatchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AddEditDelete", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("E"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqID", SqlDbType.BigInt, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompId", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqSourceID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.EnqSourceID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqDate", SqlDbType.Date, 0, ParameterDirection.Input, DateTime.Now);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqFrom", SqlDbType.VarChar, 100, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqReceivedby", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FollowupDate", SqlDbType.Date, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BussinessLineID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.BussinessLineID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.ClientID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AccountID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.AccountID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveQuoteID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.MoveQuoteID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RevenueBrId", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.RevenueBrId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChangeAcctMgrID", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperFName", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(lead.manageDet.ShipperFName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperLName", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(lead.manageDet.ShipperLName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipCategoryID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.ShipCategoryID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipTypeID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.ShipTypeID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 200, ParameterDirection.Input, CSubs.PSafeValue(lead.manageDet.Address1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 200, ParameterDirection.Input, CSubs.PSafeValue(lead.manageDet.Address2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Email", SqlDbType.VarChar, 100, ParameterDirection.Input, lead.manageDet.Email);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AddressCityID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.AddressCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PIN", SqlDbType.VarChar, 10, ParameterDirection.Input, lead.manageDet.PIN);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone1", SqlDbType.VarChar, 15, ParameterDirection.Input, CSubs.PSafeValue(lead.manageDet.Phone1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone2", SqlDbType.VarChar, 15, ParameterDirection.Input, CSubs.PSafeValue(lead.manageDet.Phone2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(lead.manageDet.Remarks));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Isactive", SqlDbType.Bit, 0, ParameterDirection.Input, true);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LostDate", SqlDbType.Date, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LostToCompitID", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LostReasonID", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LostRemarks", SqlDbType.VarChar, 500, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClientRef", SqlDbType.VarChar, 500, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClientCP", SqlDbType.VarChar, 500, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CPEmail", SqlDbType.VarChar, 500, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TendativeMoveDate", SqlDbType.Date, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContactDate", SqlDbType.Date, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperTitle", SqlDbType.VarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(lead.manageDet.ShipperTitle));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqDetailsXML", SqlDbType.Xml, 0, ParameterDirection.Input, EnquiryDetailsXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperDOB", SqlDbType.Date, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperDesig", SqlDbType.VarChar, 25, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperNationality", SqlDbType.VarChar, 25, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRmcBuss", SqlDbType.Bit, 0, ParameterDirection.Input, true);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.InputOutput, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyID", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCityID", SqlDbType.Int, 0, ParameterDirection.Input, lead.FromCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPortID", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPortID", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestCityID", SqlDbType.Int, 0, ParameterDirection.Input, lead.ToCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ModeID", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_ShipingLineID", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, lead.RMCID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BussLineID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.BussinessLineID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SeaGoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.SeaCommodity);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AirGoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.AirCommodity);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RoadGoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.RoadCommodity);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCurrId", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightFm", SqlDbType.Float, 0, ParameterDirection.Input, null);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightTo", SqlDbType.Float, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SubCostHeadwiseInfo", SqlDbType.Xml, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateID", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompanyRateID", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompanyRatewtID", SqlDbType.Int, 0, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClientID", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.ClientID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AirWeight", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.AirWt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SeaWeight", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.SearWt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RoadWeight", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.RoadWt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AirWtUnitId", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.AirWtUnitId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SeaWtUnitId", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.SeaWtUnitId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RoadWtUnitId", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.RoadWtUnitId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShippingLineId", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.SeaShippingLineId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCFileNo", SqlDbType.VarChar, 30, ParameterDirection.Input, lead.manageDet.RMCFileNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WorkOrderNo", SqlDbType.VarChar, 30, ParameterDirection.Input, lead.manageDet.WKNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobAssignTo", SqlDbType.Int, 0, ParameterDirection.Input, lead.manageDet.MoveCoordinatorID);




                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                MoveId = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
                                return true;
                            }
                            else
                            {
                                MoveId = 0;
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
                throw new DataAccessException(Convert.ToString(LoginID), "LeadDAL", "InsertJob", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }


        public IQueryable<Lead> GetForGrid(int LoginID, int? RMCID, int? FromCityID, int? ToCityID, int? UpdatedBatchId ,bool isRoad = false)
        {

            try
            {
                string query = string.Empty;
                bool GetFullDetails = UpdatedBatchId != null;

                query = string.Format("EXEC [RMC].[GetFSFR_ForLead_ForGrid] @SP_LoginID={0},@SP_FromCityID={1},@SP_ToCityID={2},@SP_CompanyID={3},@SP_RMCID={4},@SP_GetFullDetails={5},@SP_UpdatedBatchID={6},@SP_IsRoad={7}",
                    Convert.ToString(LoginID), CSubs.QSafeValue(Convert.ToString(FromCityID)),
                    CSubs.QSafeValue(Convert.ToString(ToCityID)),
                    CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID)),
                    CSubs.QSafeValue(Convert.ToString(RMCID)), CSubs.QSafeValue(Convert.ToString(GetFullDetails)),
                    CSubs.QSafeValue(Convert.ToString(UpdatedBatchId)), isRoad);

                DataTable dataTable = CSubs.GetDataTable(query);
                IQueryable<Lead> data = null;
                //var result = null;
                if (!GetFullDetails)
                {
                    var result = (from rw in dataTable.AsEnumerable()
                                  select new Lead()
                                  {
                                      pricing = new PricingDet()
                                      {
                                          FSFRLeadDetailsID = Convert.ToInt32(rw["FSFRLeadDetailsID"]),
                                          FSFRLeadDetailMastrID = Convert.ToInt32(rw["FSFRLeadDetailMastrID"]),
                                          UpdatedBatchID = Convert.ToInt32(rw["UpdatedBatchID"]),
                                          ModeID = Convert.ToInt32(rw["ModeID"]),
                                          FSFR = Convert.ToString(rw["FSFR"]),
                                          WeightFrom = Convert.ToString(rw["WeightFrom"]),
										  TransitTime = Convert.ToString(rw["TransitDaysFrom"]) + " - " + Convert.ToString(rw["TransitDaysFrom"]),
										  
									  },
                                      CreatedDate = Convert.ToDateTime(rw["createddate"]),
									  
								  }).ToList();
                    data = result.AsQueryable<Lead>();
                }
                else
                {
                    var result = (from rw in dataTable.AsEnumerable()
                                  select new Lead()
                                  {
                                      pricing = new PricingDet()
                                      {
                                          FSFRLeadDetailsID = Convert.ToInt32(rw["FSFRLeadDetailsID"]),
                                          UpdatedBatchID = Convert.ToInt32(rw["UpdatedBatchID"]),
                                          ModeID = Convert.ToInt32(rw["ModeID"]),
                                          FSFR = Convert.ToString(rw["FSFR"]),
                                          TransportModeName = Convert.ToString(rw["TransportModeName"]),
                                          OrgCost = Convert.ToString(rw["OrgCost"]),
                                          WeightFrom = Convert.ToString(rw["WeightFrom"]),
                                          FrtCost = Convert.ToString(rw["FrtCost"]),
                                          DestCost = Convert.ToString(rw["DestCost"]),
                                          DtDCost = Convert.ToString(rw["DtDCost"]),
                                          Buff = Convert.ToString(rw["Buff"]),
                                          SFRAmt = Convert.ToString(rw["SFRAmt"]),
                                          SFR = Convert.ToString(rw["SFR"]),
                                          RevSFR = Convert.ToString(rw["RevSFR"]),
                                          FSFRAmt = Convert.ToString(rw["FSFRAmt"]),
                                          NetRev = Convert.ToString(rw["NetRev"]),
                                          GPVal = Convert.ToString(rw["GPVal"]),
                                          GPPercent = Convert.ToString(rw["GPPercent"]),
										  TransitTime = Convert.ToString(rw["TransitDaysFrom"])+" - "+ Convert.ToString(rw["TransitDaysTo"]),
									  },
                                      CreatedDate = Convert.ToDateTime(rw["createddate"]),
									  OrgAgent = Convert.ToString(rw["OrgAgent"]),
									  DestAgent = Convert.ToString(rw["DestAgent"]),
									  OrgPort = Convert.ToString(rw["OrgPort"]),
									  DestPort= Convert.ToString(rw["DestPort"])
								  }).ToList();
                    data = result.AsQueryable<Lead>();
                }
				//using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
				//{
				//	if (conn.Connect())
				//	{
				//		conn.AddCommand("[RMC].[GetFSFR_ForLead_ForGrid]", QueryType.Procedure);
				//		conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.BigInt, 0, ParameterDirection.Input, LoginID);
				//		conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromCityID", SqlDbType.Int, 0, ParameterDirection.Input, FromCityID);
				//		conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToCityID", SqlDbType.Int, 0, ParameterDirection.Input, ToCityID);
				//		conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
				//		conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, RMCID);
				//		conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GetFullDetails", SqlDbType.Bit, 0, ParameterDirection.Input, GetFullDetails);
				//		conn.AddParameter(ParameterOF.PROCEDURE, "@SP_UpdatedBatchID", SqlDbType.Int, 0, ParameterDirection.Input, UpdatedBatchId);
				//		conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRoad", SqlDbType.Bit, 0, ParameterDirection.Input, isRoad);
				//		DataTable dataTable = (DataTable)conn.ExecuteProcedure(ProcedureReturnType.DataTable);

				//		//DataTable dataTable = CSubs.GetDataTable(query);

				//		//var result = null;
				//		if (!GetFullDetails)
				//		{
				//			var result = (from rw in dataTable.AsEnumerable()
				//						  select new Lead()
				//						  {
				//							  pricing = new PricingDet()
				//							  {
				//								  FSFRLeadDetailsID = Convert.ToInt32(rw["FSFRLeadDetailsID"]),
				//								  FSFRLeadDetailMastrID = Convert.ToInt32(rw["FSFRLeadDetailMastrID"]),
				//								  UpdatedBatchID = Convert.ToInt32(rw["UpdatedBatchID"]),
				//								  ModeID = Convert.ToInt32(rw["ModeID"]),
				//								  FSFR = Convert.ToString(rw["FSFR"]),
				//								  WeightFrom = Convert.ToString(rw["WeightFrom"])
				//							  },
				//							  CreatedDate = Convert.ToDateTime(rw["createddate"])
				//						  }).ToList();
				//			data = result.AsQueryable<Lead>();
				//		}
				//		else
				//		{
				//			var result = (from rw in dataTable.AsEnumerable()
				//						  select new Lead()
				//						  {
				//							  pricing = new PricingDet()
				//							  {
				//								  FSFRLeadDetailsID = Convert.ToInt32(rw["FSFRLeadDetailsID"]),
				//								  UpdatedBatchID = Convert.ToInt32(rw["UpdatedBatchID"]),
				//								  ModeID = Convert.ToInt32(rw["ModeID"]),
				//								  FSFR = Convert.ToString(rw["FSFR"]),
				//								  TransportModeName = Convert.ToString(rw["TransportModeName"]),
				//								  OrgCost = Convert.ToString(rw["OrgCost"]),
				//								  WeightFrom = Convert.ToString(rw["WeightFrom"]),
				//								  FrtCost = Convert.ToString(rw["FrtCost"]),
				//								  DestCost = Convert.ToString(rw["DestCost"]),
				//								  DtDCost = Convert.ToString(rw["DtDCost"]),
				//								  Buff = Convert.ToString(rw["Buff"]),
				//								  SFRAmt = Convert.ToString(rw["SFRAmt"]),
				//								  SFR = Convert.ToString(rw["SFR"]),
				//								  RevSFR = Convert.ToString(rw["RevSFR"]),
				//								  FSFRAmt = Convert.ToString(rw["FSFRAmt"]),
				//								  NetRev = Convert.ToString(rw["NetRev"]),
				//								  GPVal = Convert.ToString(rw["GPVal"]),
				//								  GPPercent = Convert.ToString(rw["GPPercent"])
				//							  },
				//							  CreatedDate = Convert.ToDateTime(rw["createddate"])
				//						  }).ToList();
				//			data = result.AsQueryable<Lead>();
				//		}
				//	}
				//	else
				//		throw new Exception(conn.ErrorMessage);
				//}
				return data;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "LeadDAL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataTable GetDetailById(int LoginID, int LeadID)
        {
            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("exec [GMMS].[GetLeadDetailsDetailsForDisplay] @SP_LeadID={0},@SP_LoginID={1}",
                 CSubs.QSafeValue(Convert.ToString(LeadID)), CSubs.QSafeValue(Convert.ToString(LoginID)));

                Dtobj = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "LeadDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


            return Dtobj;
        }

		public DataTable GetRMCType(int? RMCID,int LoginID)
		{
			DataTable Dtobj = new DataTable();

			try
			{
				string query = string.Format("exec Comm.GetRMCType @SP_RMCID={0}",
				 CSubs.QSafeValue(Convert.ToString(RMCID)));

				Dtobj = CSubs.GetDataTable(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "LeadDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}


			return Dtobj;
		}

	}
}