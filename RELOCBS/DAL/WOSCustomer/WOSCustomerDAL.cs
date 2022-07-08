using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace RELOCBS.DAL.WOSCustomer
{
    public class WOSCustomerDAL
    {
        private CommonSubs _CSubs;
        public CommonSubs CSubs
        {
            get
            {
                if (_CSubs == null)
                    _CSubs = new CommonSubs();
                return _CSubs;
            }
        }

        public DataSet GetCustomerServiceMapping(int LoginID, string CustomerName, bool? IsRMC, int? RMCID, int? ClientID, int? AccountID, DateTime? EffectiveFrom)
        {
            DataSet CustomerServiceMapDs = null;
            try
            {
                string query = string.Format("EXEC [WOS].[GetClientServiceMapingList] @SP_LoginID={0}, @SP_CustomerName={1}, @SP_IsRMC={2}, @SP_RMCID={3}, @SP_ClientID={4}, @SP_AccountID={5}, @SP_EffectiveFrom={6}",
                    CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(CustomerName), CSubs.QSafeValue(Convert.ToString(IsRMC)), CSubs.QSafeValue(Convert.ToString(RMCID)),
                    CSubs.QSafeValue(Convert.ToString(ClientID)), CSubs.QSafeValue(Convert.ToString(AccountID)), CSubs.QSafeValue(Convert.ToString(EffectiveFrom)));

                CustomerServiceMapDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSSubServiceDAL", "GetCustomerServiceMapping", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return CustomerServiceMapDs;
        }

        public DataSet GetClientServiceMapingDetailsById(int LoginID, Int64 CustServMapMasterID)
        {
            DataSet DebitNoteDetailsDs = new DataSet();
            try
            {
                string query = string.Format("EXEC [WOS].[GetClientServiceMapingDetails] @SP_LoginID={0}, @SP_CustServMapMasterID={1}",
                    CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CustServMapMasterID)));

                DebitNoteDetailsDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSSubServiceDAL", "GetClientServiceMapingDetailsById", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return DebitNoteDetailsDs;
        }

        public bool SaveCustomerServiceMap(Entities.WOSCustomer WOSCustomerObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        var ServiceLists = new XElement("ServiceLists",
                            from m in WOSCustomerObj.WOSSubServiceList
                            where m.IsChecked == true
                            select new XElement("ServiceList",
                            new XElement("SubServiceMastID", m.SubServiceMastID),
                            new XElement("CostAmount", m.MastCostAmount),
                            new XElement("RevenueAmount", m.MastRevenueAmount),
                            new XElement("IsChecked", m.IsChecked)
                            ));
                        string ServiceListXmlString = ServiceLists.HasElements ? Convert.ToString(ServiceLists) : null;

                        conn.AddCommand("[WOS].[AddEditCustomerServiceMap]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CustServMapMasterID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, WOSCustomerObj.CustServMapMasterID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMC", SqlDbType.Bit, 0, ParameterDirection.Input, WOSCustomerObj.IsRMC);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClientID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSCustomerObj.ClientID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AccountID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSCustomerObj.AccountID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceList", SqlDbType.Xml, 0, ParameterDirection.Input, ServiceListXmlString);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EffectiveFrom", SqlDbType.Date, 0, ParameterDirection.Input, WOSCustomerObj.EffectiveFrom);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, WOSCustomerObj.CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCountryID", SqlDbType.Int, 0, ParameterDirection.Input, WOSCustomerObj.OriginCountryID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestCountryID", SqlDbType.Int, 0, ParameterDirection.Input, WOSCustomerObj.DestinationCountryID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RevnueCurrID", SqlDbType.Int, 0, ParameterDirection.Input, WOSCustomerObj.RevenueCurrencyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostCurrID", SqlDbType.Int, 0, ParameterDirection.Input, WOSCustomerObj.CostCurrencyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.Int, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.VarChar, 100, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                WOSCustomerObj.CustServMapMasterID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_CustServMapMasterID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSCustomerDAL", "SaveCustomerServiceMap", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}