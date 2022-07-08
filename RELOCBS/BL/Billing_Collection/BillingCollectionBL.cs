using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Billing_Collection;
using RELOCBS.Utility;

namespace RELOCBS.BL.Billing_Collection
{
    public class BillingCollectionBL
    {
        private BillingCollectionDAL _bncDAL;

        public BillingCollectionDAL bncDAL
        {

            get
            {
                if (this._bncDAL == null)
                    this._bncDAL = new BillingCollectionDAL();
                return this._bncDAL;
            }
        }
        public bool Insert(Entities.Billing_Collection bnc, out string result)
        {
            try
            {
                return bncDAL.Insert(bnc, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public Entities.Billing_Collection GetDetailById(int EnqDetailID, out List<SelectListItem> enquirydetList)
        {
            Entities.Billing_Collection surveyObj = new Entities.Billing_Collection();
            enquirydetList = null;
            try
            {
                DataSet bncDt = bncDAL.GetDetailById(EnqDetailID);
                if (bncDt != null && bncDt.Tables.Count > 0 && bncDt.Tables[0].Rows.Count > 0)
                {
                    surveyObj = (from rw in bncDt.Tables[0].AsEnumerable()
                                 select new Entities.Billing_Collection()
                                 {
                                     EnqID = rw["EnqId"] == DBNull.Value ? (Int64)0 : Convert.ToInt64(rw["EnqId"]),
									 EnqNo = rw["EnqNo"] == DBNull.Value ? null : Convert.ToString(rw["EnqNo"]),
									 EnqDetailID = rw["EnqDetailID"] == DBNull.Value ? (Int64)0 : Convert.ToInt64(rw["EnqDetailID"]),
									 EnqSeqID = rw["EnqDetSequenceID"] == DBNull.Value ? (int)0 : Convert.ToInt32(rw["EnqDetSequenceID"]),
									 chgAccountMgr = rw["AccMgrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["AccMgrID"]),
                                     BillingOn = rw["BillingOn"] == DBNull.Value ? null : Convert.ToString(rw["BillingOn"]),
                                     BillingOnClientId = rw["billingonClientID"] == DBNull.Value ? (int)0 : Convert.ToInt32(rw["billingonClientID"]),
                                     AccountId = rw["AccountId"] == DBNull.Value ? (int)0 : Convert.ToInt32(rw["AccountId"]),
                                     ClientId = rw["ClientId"] == DBNull.Value ? (int)0 : Convert.ToInt32(rw["ClientId"]),
                                     CreditApproved = rw["creditapproved"] == DBNull.Value ? 0 : Convert.ToInt32(rw["creditapproved"]),
                                     Advance = rw["Advance"] == DBNull.Value ? false : Convert.ToBoolean(rw["Advance"]),
                                     Amount = rw["Amount"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["Amount"]),
                                     PaymentPostDelivery = rw["PaymentPostDelivery"] == DBNull.Value ? null : Convert.ToString(rw["PaymentPostDelivery"]),
                                     PaymentPreDelivery = rw["PaymentPreDelivery"] == DBNull.Value ? null : Convert.ToString(rw["PaymentPreDelivery"]),
                                     NoDays = rw["NoDays"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["NoDays"]),
                                     PurchaseOrder = rw["PurchaseOrder"] == DBNull.Value ? false : Convert.ToBoolean(rw["PurchaseOrder"]),
                                     AthorizeQuote = rw["AthorizeQuote"] == DBNull.Value ? false : Convert.ToBoolean(rw["AthorizeQuote"]),
                                     Others = rw["Others"] == DBNull.Value ? false : Convert.ToBoolean(rw["Others"]),
                                     Specify = rw["SpecifyOth"] == DBNull.Value ? null : Convert.ToString(rw["SpecifyOth"]),
                                     ServiceLine = rw["ServiceLine"] == DBNull.Value ? null : Convert.ToString(rw["ServiceLine"]),
                                     RevenueBr = rw["RevenueBr"] == DBNull.Value ? null : Convert.ToString(rw["RevenueBr"]),
                                     PreparedBy = rw["CreatedBy"] == DBNull.Value ? UserSession.GetUserSession().UserName : Convert.ToString(rw["CreatedBy"]),
                                     Entrydate = rw["CreatedDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(rw["CreatedDate"]),
									 Attention=rw["Attention"] == DBNull.Value ? null : Convert.ToString(rw["Attention"]),
                                     Billing_ContactPerson = rw["BillingClientName"] == DBNull.Value ? null : Convert.ToString(rw["BillingClientName"]),
                                     Billing_Address = rw["BillingAddress"] == DBNull.Value ? null : Convert.ToString(rw["BillingAddress"]),
                                     Billing_Email = rw["BillingEmail"] == DBNull.Value ? null : Convert.ToString(rw["BillingEmail"]),
                                     Billing_Tel = rw["BillingTel"] == DBNull.Value ? null : Convert.ToString(rw["BillingTel"]),
                                     ClientType = rw["ClientType"] == DBNull.Value ? null : Convert.ToString(rw["ClientType"]),
                                     AccountMgr = rw["AccMgrName"] == DBNull.Value ? null : Convert.ToString(rw["AccMgrName"]),
                                     
                                     Shipper = new Entities.ShipperDetail
                                     {
                                         Title = rw["ShipperTitle"] == DBNull.Value ? null : Convert.ToString(rw["ShipperTitle"]),
                                         ShipperFName= rw["ShipperFName"] == DBNull.Value ? null : Convert.ToString(rw["ShipperFName"]),
                                         ShipperLName = rw["ShipperLName"] == DBNull.Value ? null : Convert.ToString(rw["ShipperLName"]),
                                         Address1 = rw["Address1"] == DBNull.Value ? null : Convert.ToString(rw["Address1"]),
                                         Address2 = rw["Address2"] == DBNull.Value ? null : Convert.ToString(rw["Address2"]),
                                         Email = rw["Email"] == DBNull.Value ? null : Convert.ToString(rw["Email"]),
                                         AddressCityID = rw["AddressCityId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["AddressCityId"]),
                                         PIN = rw["PIN"] == DBNull.Value ? null : Convert.ToString(rw["PIN"]),
                                         Phone1 = rw["Phone1"] == DBNull.Value ? null : Convert.ToString(rw["Phone1"]),
                                         Phone2 = rw["Phone2"] == DBNull.Value ? null : Convert.ToString(rw["Phone2"]),
                                         ShipCategoryID = rw["ShipCategoryID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["ShipCategoryID"]),
                                         DOB = rw["ShipperDOB"] == DBNull.Value ? default(DateTime) : Convert.ToDateTime(rw["ShipperDOB"]),
                                         Designation = rw["ShipperDesig"] == DBNull.Value ? null : Convert.ToString(rw["ShipperDesig"]),
                                         Nationality = rw["ShipperNationality"] == DBNull.Value ? null : Convert.ToString(rw["ShipperNationality"]),
                                         ShipCategory = rw["ShipperCategory"] == DBNull.Value ? null : Convert.ToString(rw["ShipperCategory"]),
                                         
                                     }
                                 }).First();
                    
                }
                if (bncDt.Tables.Count>1 && bncDt.Tables[1] != null && bncDt.Tables[1].Rows.Count > 0)
                {
                    enquirydetList = bncDt.Tables[1].AsEnumerable().
                        Select(dataRow => new SelectListItem
                        {
                            Value = Convert.ToString(dataRow["enqdetailID"]),
                            Text = Convert.ToString(dataRow["Enquiry"]),
                        }).Where(x => x.Value != EnqDetailID.ToString()).ToList();
                }

                return surveyObj;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyBL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return surveyObj;

        }
    }
}