using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.BL
{
	public class CommanBL
	{
		private CommanDAL _commanDAL;

		public CommanDAL commanDAL
		{

			get
			{
				if (this._commanDAL == null)
					this._commanDAL = new CommanDAL();
				return this._commanDAL;
			}
		}

		public Status GetStatusById(Int64 id, string page, string StatusType)
		{
			Status objStatus = new Status();
			try
			{
				DataTable dtStatusDetail = commanDAL.GetStatusById(id, page, StatusType, UserSession.GetUserSession().LoginID);

				if (dtStatusDetail.Rows.Count > 0)
				{
					/*objStatus = dtStatusDetail.Rows.
                            MenuId = Convert.ToInt32(dataRow["MenuId"]),
                            MenuName = Convert.ToString(dataRow["MenuName"]),
                            Allow_Add = Convert.ToBoolean(dataRow["Allow_Add"]),
                            Allow_Edit = Convert.ToBoolean(dataRow["Allow_Edit"]),
                            Allow_Delete = Convert.ToBoolean(dataRow["Allow_Delete"]),
                            Allow_View = Convert.ToBoolean(dataRow["Allow_View"]),
                        }).ToList();*/
					objStatus.StatusName = Convert.ToString(dtStatusDetail.Rows[0]["Status"]);
					objStatus.StatusDate = string.IsNullOrWhiteSpace(Convert.ToString(dtStatusDetail.Rows[0]["StatusDate"])) ? DateTime.Now : Convert.ToDateTime(dtStatusDetail.Rows[0]["StatusDate"]);
				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CommanBL", "GetStatusById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
			return objStatus;
		}

		public int GetBaseCurrByRMC(bool IsRMC = false, int? RMCID = null, int? CompID = null, int? LoginID = null,int? MoveID = null)
		{
			int BaseCurrID = 0;
			try
			{
				
				DataTable dtBaseCurr = commanDAL.GetBaseCurrByRMC(IsRMC, RMCID, CompID, LoginID, MoveID);

				if (dtBaseCurr!=null && dtBaseCurr.Rows.Count == 1)
				{
					BaseCurrID = Convert.ToInt32(dtBaseCurr.Rows[0]["BaseCurrID"]);
				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CommanBL", "GetBaseCurrByRMC", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
			return BaseCurrID;
		}

		public int GetClientByRMC(bool IsRMC = false, int? RMCID = null)
		{
			int ClientID = 0;
			try
			{
				DataTable dtClientDetails = commanDAL.GetClientByRMC(IsRMC, RMCID, UserSession.GetUserSession().CompanyID, UserSession.GetUserSession().LoginID);

				if (dtClientDetails.Rows.Count == 1)
				{
					ClientID = Convert.ToInt32(dtClientDetails.Rows[0]["AgentID"]);
				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CommanBL", "GetStatusById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
			return ClientID;
		}

		public List<Entities.SubCosthead> GetSubCostDetails(int CostHeadID = 0, int SurveyID = 0, int RateCompRateID = 0, int RateCompRateBatchID = 0,int RateCompID=0,int MoveID=0)
		{
			List<Entities.SubCosthead> objSubCosthead = new List<Entities.SubCosthead>();
			int loginID = UserSession.GetUserSession().LoginID;
			int CompanyID = UserSession.GetUserSession().CompanyID;
			DataTable dt = commanDAL.GetSubCostDetails(CostHeadID, loginID,SurveyID,RateCompRateID, RateCompRateBatchID,RateCompID,MoveID, CompanyID);
			if (dt.Rows.Count > 0)
			{
				objSubCosthead = (from item in dt.AsEnumerable()
								  select new Entities.SubCosthead()
								  {
									  CostHeadID = item["CostHeadID"] == DBNull.Value ? 0 : Convert.ToInt32(item["CostHeadID"]),
									  CostHeadName = item["SubCostHeadName"] == DBNull.Value ? null : Convert.ToString(item["SubCostHeadName"]),
									  RateCompID = item["RateCompID"] == DBNull.Value ? 0 : Convert.ToInt32(item["RateCompID"]),
									  SubCostID = item["SubCostHeadID"] == DBNull.Value ? 0 : Convert.ToInt32(item["SubCostHeadID"]),
									  RateCurrID = item["RateCurrID"] == DBNull.Value ? 0 : Convert.ToInt32(item["RateCurrID"]),
									  RateCurr = item["RateCurr"] == DBNull.Value ? null : Convert.ToString(item["RateCurr"]),
									  RateValue = item["RateValue"] == DBNull.Value ? 0 : Convert.ToDecimal(item["RateValue"]),
									  ConvRate = item["ConvRate"] == DBNull.Value ? 0 : Convert.ToDecimal(item["ConvRate"]),
									  Value = item["Amt"] == DBNull.Value ? 0 : Convert.ToDecimal(item["Amt"])
								  }).ToList();
			}
			return objSubCosthead;
		}

		public bool IsSubCostHead(int CostHeadID)
		{
			bool IsSubCost = false;
			try
			{
				bool RMCBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
				DataTable dtSubCost = commanDAL.IsSubCostHead(CostHeadID,UserSession.GetUserSession().LoginID, RMCBuss);

				if (dtSubCost.Rows.Count == 1)
				{
					IsSubCost = Convert.ToBoolean(dtSubCost.Rows[0]["IsSubCostHEad"]);
				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CommanBL", "IsSubCostHead", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
			return IsSubCost;
		}

		public decimal GetConvRate(int FromCurrID, int toCurrID, string FromPage)
		{
			decimal ConvRate = 0;
			try
			{
				int CompID = UserSession.GetUserSession().CompanyID;
				int LoginID = UserSession.GetUserSession().LoginID;
				DataTable dtConvRate = commanDAL.GetConvRate(FromCurrID, toCurrID, FromPage, CompID, LoginID);

				if (dtConvRate.Rows.Count == 1)
				{
					ConvRate = /*Convert.ToDecimal("2.30");*/dtConvRate.Rows[0]["ConversionRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dtConvRate.Rows[0]["ConversionRate"]);
				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CommanBL", "GetConvRate", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
			return ConvRate;
		}

		public int GetLoginByEmployeeDropdown(int EmpID)
		{
			int LoginID = 0;
			try
			{
				
				DataTable dtLogin = commanDAL.GetLoginByEmployeeDropdown(EmpID);

				if (dtLogin.Rows.Count == 1)
				{
					LoginID = Convert.ToInt32(dtLogin.Rows[0]["LoginID"]);
				}
			}
			catch (DataAccessException ex)
			{
				throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
			}
			catch (Exception ex)
			{
				throw new BussinessLogicException(Convert.ToString(EmpID), "CommanBL", "GetLoginByEmployeeDropdown", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
			}
			return LoginID;
		}
	}


}