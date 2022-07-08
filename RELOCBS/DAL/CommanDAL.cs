using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace RELOCBS.DAL
{
    public class CommanDAL
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


        //page = "Enquiry",
        public DataTable GetStatusById(Int64 id, string page, string StatusType, int LoginID)
        {
            DataTable dtStatusDetail = new DataTable();

            try
            {
                string param = string.Empty;
                if (page.Equals("Enquiry"))
                    param = "@SP_EnqID={1}";
                else if (page.Equals("EnquiryDetail"))
                    param = "@SP_EnqDetailID={1}";
                else if (page.Equals("Survey"))
                    param = "@SP_SurveyID={1}";
                else if (page.Equals("Move"))
                    param = "@SP_MoveID={1}";
                string query = string.Format("exec [Enq].[GetEnqSurveyMoveStatus] @SP_LoginID={0}," +
                 param+",@SP_StatusType={2}",
                 CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(id)), StatusType);
                dtStatusDetail = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "CompanyBranchDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return dtStatusDetail;

        }

		public DataTable GetBaseCurrByRMC(bool IsRMC = false, int? RMCID = null, int? CompID = null, int? LoginID = null,int? MoveID=0)
		{
			DataTable dtStatusDetail = new DataTable();

			try
			{
				
				string query = string.Format("exec [Comm].[GetBaseCurrency] @SP_IsRMC={0},@SP_RMCID={1},@SP_CompID={2},@SP_Loginid={3},@SP_MoveID={4}",
				 CSubs.QSafeValue(Convert.ToString(IsRMC)), CSubs.QSafeValue(Convert.ToString(RMCID)), CSubs.QSafeValue(Convert.ToString(CompID)), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(MoveID)));
				dtStatusDetail = CSubs.GetDataTable(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "CompanyBranchDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return dtStatusDetail;
		}

		public DataTable IsSubCostHead(int CostHeadID, int LoginID,bool RMCBuss)
		{
			DataTable dtStatusDetail = new DataTable();

			try
			{
				string query = string.Format("exec [Comm].[GetIsSubCost] @SP_CostHeadID={0},@SP_ISRMC={1}",
				CSubs.QSafeValue(Convert.ToString(CostHeadID)),
				RMCBuss);
				dtStatusDetail = CSubs.GetDataTable(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "CompanyBranchDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return dtStatusDetail;
		}

		public DataTable GetConvRate(int FromCurrID, int ToCurrID, string FromPage, int CompID,int LoginID)
		{
			DataTable dtStatusDetail = new DataTable();

			try
			{
				string query = string.Format("exec [Comm].[GetCurrConvRate] @SP_FromCurrencyID={0},@SP_ToCurrencyID={1},@SP_companyID={2}, @SP_FromPage={3}",
				CSubs.QSafeValue(Convert.ToString(FromCurrID)),
				CSubs.QSafeValue(Convert.ToString(ToCurrID)),
				CSubs.QSafeValue(Convert.ToString(CompID)),
				CSubs.QSafeValue(Convert.ToString(FromPage)));
				dtStatusDetail = CSubs.GetDataTable(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "CommanDAL", "GetConvRate", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return dtStatusDetail;
		}
		

		public DataTable GetClientByRMC(bool IsRMC = false, int? RMCID = null, int CompID = 0, int LoginID = 0)
		{
			DataTable dtClientDetail = new DataTable();

			try
			{
				string query = string.Format("[Comm].[ForCombo_RMC] @SP_Type={0},@SP_Loginid={1},@SP_RMCID={2}", CSubs.QSafeValue("SINGLE"), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(RMCID)));
				dtClientDetail = CSubs.GetDataTable(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "CommanDAL", "GetClientByRMC", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return dtClientDetail;
		}

		public DataTable GetSubCostDetails(int CostHeadID, int LoginID, int SurveyID, int RateCompRateID, int RateCompRateBatchID, int RateCompID,Int64 MoveID, int CompID )
		{
			DataTable dt = new DataTable();

			try
			{
				string query = string.Format("[Comm].[GetSubCostList] @SP_CostHeadID={0},@SP_SurveyID={1},@SP_RateCompRateID={2}," +
					"@SP_RateCompRateBatchID={3},@SP_RateCompID={4},@SP_Moveid={5},@SP_CompID={6}", 
					CSubs.QSafeValue(Convert.ToString(CostHeadID)), 
					CSubs.QSafeValue(Convert.ToString(SurveyID)),
					CSubs.QSafeValue(Convert.ToString(RateCompRateID)),
					CSubs.QSafeValue(Convert.ToString(RateCompRateBatchID)),
					CSubs.QSafeValue(Convert.ToString(RateCompID)),
					CSubs.QSafeValue(Convert.ToString(MoveID)),
					CSubs.QSafeValue(Convert.ToString(CompID)));
				dt = CSubs.GetDataTable(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(LoginID), "CommanDAL", "GetClientByRMC", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return dt;
		}

		public DataTable GetLoginByEmployeeDropdown(int EmpID)
		{
			DataTable dt = new DataTable();

			try
			{
				string query = string.Format("[Comm].[GetLoginByEmployeeDropdown] @SP_EmpID={0}",
					CSubs.QSafeValue(Convert.ToString(EmpID)));
				dt = CSubs.GetDataTable(query);
			}
			catch (Exception ex)
			{
				throw new DataAccessException(Convert.ToString(EmpID), "CommanDAL", "GetLoginByEmployeeDropdown", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
			}

			return dt;
		}
		

	}
}