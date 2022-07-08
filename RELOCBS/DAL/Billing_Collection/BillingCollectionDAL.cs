using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Utility;
using System;
using System.Data;

namespace RELOCBS.DAL.Billing_Collection
{

    public class BillingCollectionDAL
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
        public bool Insert(Entities.Billing_Collection bnc, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string enqid = bnc.EnquiryDetailIds != null ? "," + string.Join(",", bnc.EnquiryDetailIds): string.Empty;
                        conn.AddCommand("[MoveMan].[AddEditBillingCollection]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqDetailIDs", SqlDbType.VarChar, 20, ParameterDirection.Input, bnc.EnqDetailID+ enqid);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AccountMgrID", SqlDbType.Int, 0, ParameterDirection.Input, bnc.chgAccountMgr);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillingOn", SqlDbType.VarChar, 50, ParameterDirection.Input, bnc.BillingOn);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillingOnClientId", SqlDbType.Int, 0, ParameterDirection.Input, bnc.BillingOnClientId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CreditApproved", SqlDbType.Bit, 0, ParameterDirection.Input, bnc.CreditApproved);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Advance", SqlDbType.Bit, 0, ParameterDirection.Input, bnc.Advance);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Amount", SqlDbType.Float, 0, ParameterDirection.Input, bnc.Amount);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PaymentPostDelivery", SqlDbType.VarChar, 10, ParameterDirection.Input, bnc.PaymentPostDelivery);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PaymentPreDelivery", SqlDbType.VarChar, 10, ParameterDirection.Input, bnc.PaymentPreDelivery);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoDays", SqlDbType.Int, 0, ParameterDirection.Input, bnc.NoDays);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PurchaseOrder", SqlDbType.Bit, 0, ParameterDirection.Input, bnc.PurchaseOrder);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AthorizeQuote", SqlDbType.Bit, 0, ParameterDirection.Input, bnc.AthorizeQuote);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Others", SqlDbType.Bit, 0, ParameterDirection.Input, bnc.Others);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SpecifyOth", SqlDbType.VarChar, 500, ParameterDirection.Input, bnc.Specify);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 500, ParameterDirection.Input, bnc.Remarks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperTitle", SqlDbType.VarChar, 10, ParameterDirection.Input, bnc.Shipper.Title);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperFName", SqlDbType.VarChar, 20, ParameterDirection.Input, bnc.Shipper.ShipperFName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperLName", SqlDbType.VarChar, 20, ParameterDirection.Input, bnc.Shipper.ShipperLName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 200, ParameterDirection.Input, bnc.Shipper.Address1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 200, ParameterDirection.Input, bnc.Shipper.Address2);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Email", SqlDbType.VarChar, 100, ParameterDirection.Input, bnc.Shipper.Email);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AddressCityId", SqlDbType.Int, 0, ParameterDirection.Input, bnc.Shipper.AddressCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PIN", SqlDbType.VarChar, 10, ParameterDirection.Input, bnc.Shipper.PIN);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone1", SqlDbType.VarChar, 50, ParameterDirection.Input, bnc.Shipper.Phone1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone2", SqlDbType.VarChar, 50, ParameterDirection.Input, bnc.Shipper.Phone2);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipCategoryID", SqlDbType.Int, 0, ParameterDirection.Input, bnc.Shipper.ShipCategoryID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperDOB", SqlDbType.Date, 0, ParameterDirection.Input, bnc.Shipper.DOB);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperDesig", SqlDbType.VarChar, 25, ParameterDirection.Input, bnc.Shipper.Designation);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperNationality", SqlDbType.VarChar, 25, ParameterDirection.Input, bnc.Shipper.Nationality);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
						conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Attention", SqlDbType.VarChar, 100, ParameterDirection.Input, bnc.Attention);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EnquiryDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }


        public DataSet GetDetailById(int EnqDetailID)
        {
            DataSet EnquiryDetailDt = new DataSet();

            try
            {
                string query = string.Format("EXEC [MoveMan].[GetBillingCollectionDetails] @SP_BillCollId={0}, @SP_EnqDetailID={1}, @SP_LoginID={2}",
                0, EnqDetailID,UserSession.GetUserSession().LoginID);
                EnquiryDetailDt = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "EnquiryDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return EnquiryDetailDt;

        }
    }
}