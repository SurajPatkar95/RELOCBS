using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Dynamic;
using RELOCBS.DAL.CreditApproval;
using RELOCBS.Common.ExceptionHandling;
using System.Data;
using RELOCBS.Common;
using System.Web.Mvc;

namespace RELOCBS.BL.CreditApproval
{
    public class CreditApprovalBL
    {
        private CreditApprovalDAL _creditDAL;

        public CreditApprovalDAL creditDAL
        {

            get
            {
                if (this._creditDAL == null)
                    this._creditDAL = new CreditApprovalDAL();
                return this._creditDAL;
            }
        }

        public IEnumerable<CreditApprovalGrid> GetBusinessEntityGrid(string search ,string Status,int CorporateId, string sort,string sortdir,int skip,int pageSize, out int totalRecord)
        {
            totalRecord = 0;
            try
            {
                
                IQueryable<Entities.CreditApprovalGrid>  Grid = creditDAL.GetBusinessEntityGrid(search, Status, CorporateId);
                if (Grid != null)
                {
                    totalRecord = Grid.Count();

                    if (pageSize > 1)
                    {
                        Grid = Grid.Skip((skip * (pageSize - 1))).Take(skip);
                    }
                    else
                    {
                        Grid = Grid.Take(skip);
                    }

                    Grid = Grid.OrderBy(sort + " " + sortdir);

                    return Grid.ToList();
                }
                else
                {
                    return new List<Entities.CreditApprovalGrid>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CreditApprovalBL", "GetCreditApprovalCompanyGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }


        public CreditLimitEntity GetDetails(int EntityID,bool isFromMail=false,int? UserID=0)
        {
            CreditLimitEntity model = new CreditLimitEntity();
            int LoginID = isFromMail ? Convert.ToInt32(UserID) : UserSession.GetUserSession().LoginID;
            try
            {
                DataSet data = creditDAL.GetDetails(LoginID, EntityID);

                if (data != null && data.Tables.Count > 0)
                {
                    if (data.Tables.Count > 0 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                    {

                        

                        model = (from item in data.Tables[0].AsEnumerable()
                                 select new CreditLimitEntity()
                                 {
                                     CreditLimitEntityID = Convert.ToInt32(item["CreditLimitEntityID"]),
                                     CorporateID =  Convert.ToInt32(item["CreditLimitCorporateID"]),
                                     Cust_Contact_Name = Convert.ToString(item["Cust_Contact_Name"]),
                                     Cust_Contact_Number = Convert.ToString(item["Cust_Contact_Number"]),
                                     Cust_Contact_Designation = Convert.ToString(item["Cust_Contact_Designation"]),
                                     Cust_Contact_Email = Convert.ToString(item["Cust_Contact_Email"]),
                                     Address = Convert.ToString(item["CorpAddress"]),
                                     Turnover_Amt = !string.IsNullOrWhiteSpace(Convert.ToString(item["TurnoverAmt"])) ? Convert.ToDecimal(item["TurnoverAmt"]) : (decimal?)null,
                                     CityId = Convert.ToInt32(item["CityId"]),
                                     IsActive = Convert.ToBoolean(item["IsActive"]),
                                     GSTIN_No = Convert.ToString(item["GSTNO"]),
                                     TabList = new List<TabList>(),
                                     CreatedByName= Convert.ToString(item["CreatedByName"]),
                                     CreatedDate = Convert.ToDateTime(item["LastCreatedDate"]),
                                     //advance_collect_Percent= !string.IsNullOrWhiteSpace(Convert.ToString(item["AdvCollected_Percent"])) ? Convert.ToDecimal(item["AdvCollected_Percent"]) : (decimal?)null,
                                     ClientMap = new EntityClientMap() { CreditLimitEntityID = Convert.ToInt32(item["CreditLimitEntityID"]),Remark = Convert.ToString(item["ClientRemark"]) },
                                     ApproveDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["ApprovedDate"])) ? Convert.ToDateTime(item["ApprovedDate"]) : (DateTime?)null,
                                     ApproveByName= Convert.ToString(item["ApprovedByName"]),
                                     ApproveType = Convert.ToString(item["StatusName"]),
                                     ApproveRemark = Convert.ToString(item["ApprovedRemark"]),
                                     Status = Convert.ToString(item["StatusName"]),
                                     StatusID = Convert.ToInt32(item["StatusID"]),
                                     CompID = Convert.ToInt32(item["CompID"]),
                                     IsRMC = Convert.ToBoolean(item["IsRMC"]),
                                     FromMail =isFromMail,
                                     EffectiveTo = Convert.ToDateTime(item["EffectiveTo"])
                                 }).FirstOrDefault();


                        if (data.Tables.Count > 1 && data.Tables[0] != null && data.Tables[0].Rows.Count > 0)
                        {
                            #region TabList
                            //CostObj.ShowAdvanceCaution = Convert.ToBoolean(CostDs.Tables[destttable].Rows[0]["ShowAdvanceCaution"]);
                            for (int i = 0; i < data.Tables[1].Columns.Count; i++)
                            {

                                TabList tab = new TabList();
                                tab.TabIndex = Convert.ToInt32(data.Tables[1].Rows[0][i].ToString());
                                //if (tab.TabIndex !=0)
                                //{
                                //	
                                //}
                                model.TabList.Add(tab);

                            }
                            #endregion

                        }

                        if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
                        {
                            model.buss_Dev = (from dataRow in data.Tables[2].AsEnumerable()
                            select new Buss_Dev_Feedback()
                            {
                                CreditLimitEntityID = Convert.ToInt32(dataRow["CreditLimitEntityID"]),
                                Buss_Dev_FeedbackID = Convert.ToInt64(dataRow["Buss_Dev_FeedbackID"]),
                                Credit_Amount = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["Credit_Approval_limit"])) ? Convert.ToInt64(dataRow["Credit_Approval_limit"]) : 0,
                                Credit_Amount_Display=String.Format("{0:n0}", !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["Credit_Approval_limit"])) ? Convert.ToInt64(dataRow["Credit_Approval_limit"]) : 0),
                                ProjectID = Convert.ToInt32(dataRow["ProjectID"]),
                                Project = Convert.ToString(dataRow["ProjectName"]),
                                ServiceLine = Convert.ToString(dataRow["ServiceLine"]),
                                ServiceLineID = Convert.ToInt32(dataRow["ServiceLineID"]),
                                BillingInstructions_Remark = Convert.ToString(dataRow["BillingInstructions_Remark"]),
                                CurrencyName =Convert.ToString(dataRow["CurrencyName"]),
                                CurrID = Convert.ToInt32(dataRow["CurrID"]),
                                Credit_period_basisID = Convert.ToInt32(dataRow["CreditPeriodBasisID"]),
                                Credit_period_basis = Convert.ToString(dataRow["CreditPeriodBasis"]),
                                CreditDays = Convert.ToInt32(dataRow["CreditDays"]),
                                Adv_Percent = Convert.ToInt32(dataRow["AdvCollectPercent"]),
                            }).ToList();

                            model.TotalCreditLimit = model.buss_Dev.Sum(d => d.Credit_Amount);
                            model.TotalCreditLimitDisplay = string.Format("{0:n0}", model.TotalCreditLimit);
                        }
                        
                        model.ClientMap.UnmapClientNameList = new List<SelectListItem>();
                        model.ClientMap.MappedClientNameList = new List<SelectListItem>();
                        if (data.Tables.Count > 3 && data.Tables[3] != null && data.Tables[3].Rows.Count > 0)
                        {
                            model.ClientMap.UnmapClientNameList = CommonService.GetSelectListItemsFromDatatable(data.Tables[3], false);

                            model.ClientMap.UnMappedClientList = data.Tables[3].AsEnumerable().Select(r=> r.Field<Int64>("ID")).ToArray();
                        }

                        if (data.Tables.Count > 4 && data.Tables[4] != null && data.Tables[4].Rows.Count > 0)
                        {
                            model.ClientMap.MappedClientNameList = CommonService.GetSelectListItemsFromDatatable(data.Tables[4], false);
                            model.ClientMap.MappedClientList = data.Tables[4].AsEnumerable().Select(r => r.Field<Int64>("ID")).ToArray();
                        }

                        if (data.Tables.Count > 5 && data.Tables[5] != null && data.Tables[5].Rows.Count > 0)
                        {
                            DataRow item = data.Tables[5].Rows[0];
                            model.IsApprover = Convert.ToInt16(item["IsApprover"])==1 ? true :false;
                            model.FromAmount = Convert.ToInt64(item["FromAmount"]);
                            model.ToAmount = Convert.ToInt64(item["ToAmount"]);
                        }
                        
                        #region temp
                        /**
                        
                        //if (data.Tables.Count > 4 && data.Tables[4] != null && data.Tables[4].Rows.Count > 0)
                        //{
                        //    model.Approvalfiles = (from item in data.Tables[4].AsEnumerable()
                        //                           select new CustApprovalFiles()
                        //                           {
                        //                               FileID = Convert.ToInt64(item["FileID"]),
                        //                               Buss_Dev_FeedbackID = Convert.ToInt64(item["Buss_Dev_FeedbackID"]),
                        //                               FileName = Convert.ToString(item["FileName"]),
                        //                               FilePath = Convert.ToString(item["SavedFilePath"]),
                        //                               ApprovalType = Convert.ToString(item["Approval_type"]),
                        //                               DocDescription = Convert.ToString(item["FileDescription"]),

                        //                           }).ToList();
                        //}

                        if (data.Tables.Count > 4 && data.Tables[4] != null && data.Tables[4].Rows.Count > 0)
                        {
                            model.docUpload.docLists = (from item in data.Tables[4].AsEnumerable()
                                                        select new DocList()
                                                        {
                                                            //ActivityID = Convert.ToInt64(item["ActivityID"]),
                                                            DocID = Convert.ToInt64(item["DocID"]),
                                                            DocTypeID = Convert.ToInt32(item["DocTypeID"]),
                                                            DocType = Convert.ToString(item["DocTypeName"]),
                                                            DocumentName = Convert.ToString(item["DocumentName"]),
                                                            //file = (HttpPostedFileBase)new MemoryPostedFile(System.IO.File.ReadAllBytes(Convert.ToString(item["DocumentPath"])))

                                                        }).ToList();

                        }
                        **/
                        #endregion
                    }

                }

                
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "CreditApprovalBL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

            return model;
        }

        public CustApprovalFiles GetCustApprovalFile(int FileID,int EntityID,int FeedbackID)
        {
            CustApprovalFiles model = new CustApprovalFiles();
            try
            {
                DataTable dt = creditDAL.GetCustApprovalFile(UserSession.GetUserSession().LoginID,FileID, EntityID,FeedbackID);

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow item = dt.Rows[0];
                    model.FileID = Convert.ToInt64(item["FileID"]);
                    model.Buss_Dev_FeedbackID = Convert.ToInt64(item["Buss_Dev_FeedbackID"]);
                    model.FileName = Convert.ToString(item["FileName"]);
                    model.FilePath = Convert.ToString(item["SavedFilePath"]);
                    model.ApprovalType = Convert.ToString(item["Approval_type"]);
                }

                return model;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CreditApprovalBL", "GetDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }



        }

        public bool InsertDocument(CustApprovalUpload CustApprovalUpload, out string message)
        {
            try
            {
                return creditDAL.InsertDocument(CustApprovalUpload, UserSession.GetUserSession().LoginID, out message);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CreditApprovalBL", "InsertDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool DeleteDocument(Int64 FileID,Int32 EntityID,Int64 FeedbackID, out string message)
        {
            try
            {
                return creditDAL.DeleteDocument(FileID, EntityID, FeedbackID, UserSession.GetUserSession().LoginID, out message);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CreditApprovalBL", "DeleteDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool InsetCreditEntity(CreditLimitEntity model, out string result)
        {
            try
            {
                return creditDAL.InsetCreditEntity(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CreditApprovalBL", "InsetCreditEntity", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }
        
        public bool UpdateApprovalStatus(CreditLimitEntity model,  out string result, bool isFromMail = false, int? UserID = 0)
        {
            int LoginID = isFromMail ? Convert.ToInt32(UserID) : UserSession.GetUserSession().LoginID;
            try
            {
                return creditDAL.UpdateApprovalStatus(model, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "CreditApprovalBL", "UpdateApprovalStatus", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool InsertClientEntityMap(EntityClientMap model, out string result)
        {
            try
            {
                return creditDAL.InsertClientEntityMap(model, UserSession.GetUserSession().LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CreditApprovalBL", "InsertClientEntityMap", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public CFAPrint GetPrint(int Id)
        {
            CFAPrint print = new CFAPrint();
            try
            {
                DataSet data = creditDAL.GetPrint(UserSession.GetUserSession().LoginID, Id);

                if (data != null && data.Tables.Count > 0)
                {

                    print = (from item in data.Tables[0].AsEnumerable()
                             select new CFAPrint()
                             {
                                 CreditLimitEntityID = Convert.ToInt32(item["CreditLimitEntityID"]),
                                 CorporateName = Convert.ToString(item["CorpName"]),
                                 Cust_Contact_Name = Convert.ToString(item["Cust_Contact_Name"]),
                                 Cust_Contact_Number = Convert.ToString(item["Cust_Contact_Number"]),
                                 Cust_Contact_Designation = Convert.ToString(item["Cust_Contact_Designation"]),
                                 Cust_Contact_Email = Convert.ToString(item["Cust_Contact_Email"]),
                                 Address = Convert.ToString(item["CorpAddress"]),
                                 Turnover_Amt = !string.IsNullOrWhiteSpace(Convert.ToString(item["TurnoverAmt"])) ? Convert.ToDecimal(item["TurnoverAmt"]) : (decimal?)null,
                                 CityName = Convert.ToString(item["CityName"]),
                                 GSTIN_No = Convert.ToString(item["GSTNO"]),
                                 CreatedByName = Convert.ToString(item["CreatedByName"]),
                                 CreatedDate = Convert.ToDateTime(item["LastCreatedDate"]),
                                 //advance_collect_Percent= !string.IsNullOrWhiteSpace(Convert.ToString(item["AdvCollected_Percent"])) ? Convert.ToDecimal(item["AdvCollected_Percent"]) : (decimal?)null,
                                 Clients = new CFAClientMap() { CreditLimitEntityID = Convert.ToInt32(item["CreditLimitEntityID"]), Remark = Convert.ToString(item["ClientRemark"]) },
                                 ApproveDate = !string.IsNullOrWhiteSpace(Convert.ToString(item["ApprovedDate"])) ? Convert.ToDateTime(item["ApprovedDate"]) : (DateTime?)null,
                                 ApproveByName = Convert.ToString(item["ApprovedByName"]),
                                 ApproveType = Convert.ToString(item["StatusName"]),
                                 ApproveRemark = Convert.ToString(item["ApprovedRemark"]),
                                 Status = Convert.ToString(item["StatusName"]),
                                 EffectiveTo= Convert.ToDateTime(item["EffectiveTo"])
                             }).FirstOrDefault();


                    if (data.Tables.Count > 1 && data.Tables[1] != null && data.Tables[1].Rows.Count > 0)
                    {
                        print.buss_Dev = (from dataRow in data.Tables[1].AsEnumerable()
                                          select new Buss_Dev_Feedback()
                                          {
                                              CreditLimitEntityID = Convert.ToInt32(dataRow["CreditLimitEntityID"]),
                                              Buss_Dev_FeedbackID = Convert.ToInt64(dataRow["Buss_Dev_FeedbackID"]),
                                              Credit_Amount = !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["Credit_Approval_limit"])) ? Convert.ToInt64(dataRow["Credit_Approval_limit"]) : 0,
                                              Credit_Amount_Display = String.Format("{0:n0}", !string.IsNullOrWhiteSpace(Convert.ToString(dataRow["Credit_Approval_limit"])) ? Convert.ToInt64(dataRow["Credit_Approval_limit"]) : 0),
                                              ProjectID = Convert.ToInt32(dataRow["ProjectID"]),
                                              Project = Convert.ToString(dataRow["ProjectName"]),
                                              ServiceLine = Convert.ToString(dataRow["ServiceLine"]),
                                              ServiceLineID = Convert.ToInt32(dataRow["ServiceLineID"]),
                                              BillingInstructions_Remark = Convert.ToString(dataRow["BillingInstructions_Remark"]),
                                              CurrencyName = Convert.ToString(dataRow["CurrencyName"]),
                                              CurrID = Convert.ToInt32(dataRow["CurrID"]),
                                              Credit_period_basisID = Convert.ToInt32(dataRow["CreditPeriodBasisID"]),
                                              Credit_period_basis = Convert.ToString(dataRow["CreditPeriodBasis"]),
                                              CreditDays = Convert.ToInt32(dataRow["CreditDays"]),
                                              Adv_Percent = Convert.ToInt32(dataRow["AdvCollectPercent"]),
                                          }).ToList();
                        
                        print.TotalCreditLimit = string.Format("{0:n0}", print.buss_Dev.Sum(d => d.Credit_Amount));
                    }

                    if (data.Tables.Count > 2 && data.Tables[2] != null && data.Tables[2].Rows.Count > 0)
                    {
                        print.Clients.CorporateName = data.Tables[2].AsEnumerable().Select(r => r.Field<string>("NAME")).ToArray();
                    }
                }

                return print;
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(RELOCBS.Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(UserSession.GetUserSession().LoginID), "CreditApprovalBL", "GetPrint", RELOCBS.Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}