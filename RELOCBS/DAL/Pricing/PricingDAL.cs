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
    public class PricingDAL
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

        public DataTable GetPricingBuffer(int RMCID, int LoginId, int CompId)
        {
            //DataSet data = new DataSet();
            DataTable data = new DataTable();
            //int LoginId = UserSession.GetUserSession().LoginID;
            try
            {

                data = CSubs.GetDataTable(string.Format("[RMC].[GetSlabsForBuffer] @SP_RMCID ={0},@SP_LoginID={1},@SP_CompanyID={2}"
                    , CSubs.QSafeValue(Convert.ToString(RMCID)), CSubs.QSafeValue(Convert.ToString(LoginId)), CSubs.QSafeValue(Convert.ToString(CompId))));

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginId), "PricingDAL", "GetWeightSlab", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return data;
        }

        public DataTable GetFixedRateCharges(int RMCID, int LoginId, int CompId)
        {
            //DataSet data = new DataSet();
            DataTable data = new DataTable();
            //int LoginId = UserSession.GetUserSession().LoginID;
            try
            {

                data = CSubs.GetDataTable(string.Format("[RMC].[GetFixedRatesForBufferScreen] @SP_RMCID ={0},@SP_LoginID={1},@SP_CompanyID={2}"
                    , CSubs.QSafeValue(Convert.ToString(RMCID)), CSubs.QSafeValue(Convert.ToString(LoginId)), CSubs.QSafeValue(Convert.ToString(CompId))));

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginId), "PricingDAL", "GetWeightSlab", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return data;
        }

        public DataTable GetPricingCombination(int LoginId, RMCPricing model, bool bufferflag,bool IsRoad)//bufferflag = false(without buffer)
        {
            DataTable dt = new DataTable();

            var charges = new XElement("FixedCostDetails");
            var buffer = new XElement("BufferDetails"); 

            if (model.RMCFees != null)
            {
                charges = new XElement("FixedCostDetails",
                from emp in model.RMCFees
                select new XElement("FixedCostDetail",
                               new XElement("FixedCostID", emp.CostHeadId),
                               new XElement("Amt", emp.Amount),
                               new XElement("PercentVal", emp.Percent)
                           ));
            }

            if (model.PricingSeaBuffer != null || model.PricingAirBuffer != null)
            {
                buffer = new XElement("BufferDetails",
                from emp in model.PricingSeaBuffer.Union(model.PricingAirBuffer)
                select new XElement("BufferDetail",
                               new XElement("Slab", emp.BufferSlab),
                               new XElement("Buff", emp.BufferCost),
                               new XElement("Mode", emp.ModeId)
                           ));
            }
            try
            {
                dt = CSubs.GetDataTable(string.Format("[RMC].[CalculateSFR] @SP_LoginID={0},@SP_BufferWtandAmt='{1}',@SP_FromCityID={2},@Sp_ToCityID={3},@SP_CompID={4},@SP_RMCID={5},@SP_isRoad={6},@SP_SFRCalcMethod={7},@SP_FixedCostWithID={8},@SP_NormalRev={9}",
                   CSubs.QSafeValue(Convert.ToString(LoginId)),
                   buffer,
                   CSubs.QSafeValue(Convert.ToString(model.FromCityId)),
                   CSubs.QSafeValue(Convert.ToString(model.ToCityId)),
                   CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID)),
                   CSubs.QSafeValue(Convert.ToString(model.RMCID)),
                   IsRoad,
                   model.CalculationMethod,
                   "'" + charges + "'",
				   CSubs.QSafeValue(Convert.ToString(model.NormalRev))));

                
            }
            catch (Exception ex)
            {

                //throw;
            }
            return dt;
        }

        public bool InsertOriginRate(List<SaveOriginRate> SaveOriginRate, RateUpload R, int LoginID)
        {

            try
            {

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "PricingDAL", "InsertOriginRate", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return true;
        }

        public bool InsertFreightRate(List<SaveFreightRate> SaveOriginRate, RateUpload R, int LoginID)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "PricingDAL", "InsertFreightRate", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return true;
        }

        public bool InsertDestinationRate(List<SaveDestinationRate> SaveOriginRate, RateUpload R, int LoginID)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "PricingDAL", "InsertDestinationRate", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return true;

        }

        public bool InsertBlanketRate(List<SaveBlanketRate> SaveOriginRate, RateUpload R, int LoginID)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "PricingDAL", "InsertBlanketRate", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return true;

        }

        public DataTable GetWeightSlab(int RateComponentID, int RMCID, int LoginID)
        {
            DataTable data = new DataTable();

            try
            {
                data = CSubs.GetDataTable(string.Format("[dbo].[GetUploadWeightSlabDetail] @SP_RMCID={0},@SP_RateCompoentID={1},@SP_LOGINID={2}"
                    , CSubs.QSafeValue(Convert.ToString(RMCID)), CSubs.QSafeValue(Convert.ToString(RateComponentID)), CSubs.QSafeValue(Convert.ToString(LoginID))));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "PricingDAL", "GetWeightSlab", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return data;
        }

        public bool SaveAmendRates(RMCPricing model, XElement ModeXML, XElement charges, string SFRListXml, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[RMC].[AddEditFSFR]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromCityID", SqlDbType.Int, 0, ParameterDirection.Input, model.FromCityId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToCityID", SqlDbType.Int, 0, ParameterDirection.Input, model.ToCityId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, model.RMCID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ModeXMLID", SqlDbType.Xml, 0, ParameterDirection.Input, ModeXML.HasElements ? Convert.ToString(ModeXML):null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WtUnitID", SqlDbType.Int, 0, ParameterDirection.Input, 1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityPortCombRef", SqlDbType.NVarChar, 50, ParameterDirection.Input, model.CombinationNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FixedCostHead", SqlDbType.Xml, 0, ParameterDirection.Input, charges.HasElements ? Convert.ToString(charges):null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FSFRData", SqlDbType.Xml, 0, ParameterDirection.Input, SFRListXml);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Calc_Type", SqlDbType.VarChar, 10, ParameterDirection.Input, model.NormalRev);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "PricingDAL", "SaveAmendRates", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public DataTable GetMethodBufferUnitMapping(int RMCID, int LoginId, int CompId)
        {
            //DataSet data = new DataSet();
            DataTable data = new DataTable();
            //int LoginId = UserSession.GetUserSession().LoginID;
            try
            {

                data = CSubs.GetDataTable(string.Format("[RMC].[GetSlabsForBuffer] @SP_RMCID ={0},@SP_LoginID={1},@SP_CompanyID={2}"
                    , CSubs.QSafeValue(Convert.ToString(RMCID)), CSubs.QSafeValue(Convert.ToString(LoginId)), CSubs.QSafeValue(Convert.ToString(CompId))));

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginId), "PricingDAL", "GetWeightSlab", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return data;
        }

		public DataTable GetPricingByJob(int UpdatedBatchID)
		{
			DataTable dt = new DataTable();
			
			try
			{
				dt = CSubs.GetDataTable(string.Format("[MoveMan].[GetAmendRatesForJob] @SP_UpdatedBatchID={0}",
				   CSubs.QSafeValue(Convert.ToString(UpdatedBatchID))));


			}
			catch (Exception ex)
			{

				//throw;
			}
			return dt;
		}
	}
}