using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.WOSJobOpening;
using RELOCBS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;

namespace RELOCBS.BL.WOSJobOpening
{
    public class WOSJobOpeningBL
    {
        private WOSJobOpeningDAL _WOSJobOpeningDAL;
        public WOSJobOpeningDAL WOSJobOpeningDAL
        {
            get
            {
                if (_WOSJobOpeningDAL == null)
                    _WOSJobOpeningDAL = new WOSJobOpeningDAL();
                return _WOSJobOpeningDAL;
            }
        }

        public IEnumerable<Entities.WOSJobOpening> GetWOSJobOpeningList(string Sort, string SortDir, int Skip, int PageSize, DateTime? FromDate, DateTime? ToDate,
            int? LoginID, int? CompId, bool? IsJobDate, bool? IsRMCBuss, string AssigneeName, string SearchType, string Search, out int TotalCount)
        {
            IQueryable<Entities.WOSJobOpening> WOSJobOpeningList;
            TotalCount = 0;
            try
            {
                DataSet WOSJobOpeningDs = WOSJobOpeningDAL.GetWOSJobOpeningList(FromDate, ToDate, LoginID, CompId, IsJobDate, IsRMCBuss, AssigneeName, SearchType, Search);

                if (WOSJobOpeningDs != null && WOSJobOpeningDs.Tables.Count > 0 && WOSJobOpeningDs.Tables[0].Rows.Count > 0)
                {
                    var result = (from rw in WOSJobOpeningDs.Tables[0].AsEnumerable()
                                  select new Entities.WOSJobOpening()
                                  {
                                      WOSMoveID = rw["WOSMoveID"] == DBNull.Value ? 0 : Convert.ToInt64(rw["WOSMoveID"]),
                                      WOSJobID = rw["WOSJobID"] == DBNull.Value ? null : Convert.ToString(rw["WOSJobID"]),
                                      WOSJobOpenedDate = rw["WOSJobOpenedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["WOSJobOpenedDate"]),
                                      AssigneeName = rw["AssigneeName"] == DBNull.Value ? null : Convert.ToString(rw["AssigneeName"]),
                                      ClientName = rw["Client"] == DBNull.Value ? null : Convert.ToString(rw["Client"]),
                                      AccountName = rw["Account"] == DBNull.Value ? null : Convert.ToString(rw["Account"]),
                                      JobStatus = rw["JobStatus"] == DBNull.Value ? null : Convert.ToString(rw["JobStatus"])
                                  }).ToList();
                    WOSJobOpeningList = result.AsQueryable();

                    TotalCount = WOSJobOpeningList.Count();
                    WOSJobOpeningList = WOSJobOpeningList.OrderBy(Sort + " " + SortDir);
                    if (PageSize > 0)
                    {
                        WOSJobOpeningList = WOSJobOpeningList.Skip(Skip).Take(PageSize);
                    }
                    return WOSJobOpeningList.ToList();
                }
                else
                {
                    return new List<Entities.WOSJobOpening>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "GetWOSJobOpeningList", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Entities.WOSJobOpening GetWOSJobDetailsById(Int64 WOSMoveID, int LoginID, out IEnumerable<Entities.WOSJobOpening> WOSJobOpeningList)
        {
            Entities.WOSJobOpening WOSJobOpeningObj = new Entities.WOSJobOpening();
            WOSJobOpeningList = null;
            try
            {
                DataSet WOSJobOpeningDs = WOSJobOpeningDAL.GetWOSJobDetailsById(WOSMoveID, LoginID);

                if (WOSJobOpeningDs != null)
                {
                    if (WOSJobOpeningDs.Tables.Count > 0 && WOSJobOpeningDs.Tables[0].Rows.Count > 0)
                    {
                        WOSJobOpeningObj = (from rw in WOSJobOpeningDs.Tables[0].AsEnumerable()
                                            select new Entities.WOSJobOpening()
                                            {
                                                WOSMoveID = rw["WOSMoveID"] == DBNull.Value ? 0 : Convert.ToInt64(rw["WOSMoveID"]),
                                                MoveID = rw["MoveID"] == DBNull.Value ? 0 : Convert.ToInt64(rw["MoveID"]),
                                                WOSJobID = rw["WOSJobID"] == DBNull.Value ? null : Convert.ToString(rw["WOSJobID"]),
                                                WOSJobOpenedDate = rw["WOSJobOpenedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["WOSJobOpenedDate"]),
                                                IsRMCBus = rw["IsRMCBus"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(rw["IsRMCBus"]),
                                                AssigneeFName = rw["AssigneeFName"] == DBNull.Value ? null : Convert.ToString(rw["AssigneeFName"]),
                                                AssigneeLName = rw["AssigneeLName"] == DBNull.Value ? null : Convert.ToString(rw["AssigneeLName"]),
                                                FromCityID = rw["FromCityID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["FromCityID"]),
                                                FromCityName = rw["FromCityName"] == DBNull.Value ? null : Convert.ToString(rw["FromCityName"]),
                                                ToCityID = rw["ToCityID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["ToCityID"]),
                                                ToCityName = rw["ToCityName"] == DBNull.Value ? null : Convert.ToString(rw["ToCityName"]),
                                                FileNo = rw["FileNo"] == DBNull.Value ? null : Convert.ToString(rw["FileNo"]),
                                                WONumber = rw["WONumber"] == DBNull.Value ? null : Convert.ToString(rw["WONumber"]),
                                                OldJobNo = rw["OldJobNo"] == DBNull.Value ? null : Convert.ToString(rw["OldJobNo"]),
                                                TentativeBillingMonth = rw["TentativeBillingMonth"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["TentativeBillingMonth"]),
                                                JobRemarks = rw["JobRemarks"] == DBNull.Value ? null : Convert.ToString(rw["JobRemarks"]),
                                                ServiceLineID = rw["ServiceLineID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["ServiceLineID"]),
                                                RevBranchID = rw["RevBranchID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["RevBranchID"]),
                                                RevBrName = rw["RevBrName"] == DBNull.Value ? null : Convert.ToString(rw["RevBrName"]),
                                                JobBrID = rw["JobBrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["JobBrID"]),
                                                JobBrName = rw["JobBrName"] == DBNull.Value ? null : Convert.ToString(rw["JobBrName"]),
                                                SDId = rw["SDId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["SDId"]),
                                                SDName = rw["SDName"] == DBNull.Value ? null : Convert.ToString(rw["SDName"]),
                                                SRId = rw["SRId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["SRId"]),
                                                SRName = rw["SRName"] == DBNull.Value ? null : Convert.ToString(rw["SRName"]),
                                                KAM = rw["KAM"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["KAM"]),
                                                KAMName = rw["KAMName"] == DBNull.Value ? null : Convert.ToString(rw["KAMName"]),
                                                ClientName = rw["ClientName"] == DBNull.Value ? null : Convert.ToString(rw["ClientName"]),
                                                ClientID = rw["ClientID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["ClientID"]),
                                                AccountName = rw["AccountName"] == DBNull.Value ? null : Convert.ToString(rw["AccountName"]),
                                                AccountID = rw["AccountID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["AccountID"]),
                                                JobCancelRemark = rw["JobCancelRemark"] == DBNull.Value ? null : Convert.ToString(rw["JobCancelRemark"]),
                                                JobStatus = rw["JobStatus"] == DBNull.Value ? null : Convert.ToString(rw["JobStatus"]),
                                                EmailTo = rw["EmailTo"] == DBNull.Value ? null : Convert.ToString(rw["EmailTo"]),
                                                EmailCc = rw["EmailCc"] == DBNull.Value ? null : Convert.ToString(rw["EmailCc"]),
                                                EmailBcc = rw["EmailBcc"] == DBNull.Value ? null : Convert.ToString(rw["EmailBcc"])
                                            }).First();
                    }
                    if (WOSJobOpeningDs.Tables.Count > 1 && WOSJobOpeningDs.Tables[1].Rows.Count > 0)
                    {
                        WOSJobOpeningObj.WOSJobDocUploadList = (from rw in WOSJobOpeningDs.Tables[1].AsEnumerable()
                                                                select new WOSJobDocUpload()
                                                                {
                                                                    FileID = rw["FileID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["FileID"]),
                                                                    DocTypeID = rw["DocTypeID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DocTypeID"]),
                                                                    DocTypeText = rw["DocTypeName"] == DBNull.Value ? null : Convert.ToString(rw["DocTypeName"]),
                                                                    DocNameID = rw["DocNameID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DocNameID"]),
                                                                    DocNameText = rw["DocName"] == DBNull.Value ? null : Convert.ToString(rw["DocName"]),
                                                                    DocDescription = rw["Description"] == DBNull.Value ? null : Convert.ToString(rw["Description"]),
                                                                    IsShowToAssignee = rw["IsShowToAssignee"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsShowToAssignee"]),
                                                                    FileName = rw["DocFileName"] == DBNull.Value ? null : Convert.ToString(rw["DocFileName"]),
                                                                    UploadById = rw["CreatedBy"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CreatedBy"]),
                                                                    UploadBy = rw["UploadBy"] == DBNull.Value ? null : Convert.ToString(rw["UploadBy"]),
                                                                    UploadDate = rw["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["CreatedDate"])
                                                                }).ToList();
                    }
                    if (WOSJobOpeningDs.Tables.Count > 2 && WOSJobOpeningDs.Tables[2].Rows.Count > 0)
                    {
                        DataRow rw = WOSJobOpeningDs.Tables[2].Rows[0];
                        WOSJobOpeningObj.IsCostSheetSaved = rw["IsCostSheetSaved"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(rw["IsCostSheetSaved"]);
                        WOSJobOpeningObj.WOSCustomer.CostCurrencyID = rw["CostCurrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CostCurrID"]);
                        WOSJobOpeningObj.WOSCustomer.CostCurrency = rw["CostCurr"] == DBNull.Value ? null : Convert.ToString(rw["CostCurr"]);
                        WOSJobOpeningObj.WOSCustomer.RevenueCurrencyID = rw["RevenueCurrID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["RevenueCurrID"]);
                        WOSJobOpeningObj.WOSCustomer.RevenueCurrency = rw["RevenueCurr"] == DBNull.Value ? null : Convert.ToString(rw["RevenueCurr"]);
                        WOSJobOpeningObj.WOSCustomer.AuditFee = rw["AuditFee"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["AuditFee"]);
                        WOSJobOpeningObj.WOSCustomer.AdminFee = rw["AdminFee"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["AdminFee"]);
                        WOSJobOpeningObj.WOSCustomer.BillCommissionPercent = rw["BillCommissionPercent"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["BillCommissionPercent"]);
                        WOSJobOpeningObj.CSPreparedBy = rw["CSPreparedBy"] == DBNull.Value ? null : Convert.ToString(rw["CSPreparedBy"]);
                        WOSJobOpeningObj.CSPreparedDate = rw["CSPreparedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["CSPreparedDate"]);
                        WOSJobOpeningObj.CSApprovedBy = rw["CSApprovedBy"] == DBNull.Value ? null : Convert.ToString(rw["CSApprovedBy"]);
                        WOSJobOpeningObj.CSApprovedDate = rw["CSApprovedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["CSApprovedDate"]);
                        WOSJobOpeningObj.IsCSSentToApprove = rw["IsSendToApproval"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(rw["IsSendToApproval"]);
                        WOSJobOpeningObj.CSSentToApproveUser = rw["ApprovalUserID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["ApprovalUserID"]);
                        WOSJobOpeningObj.IsCostSheetApproved = rw["IsCostSheetApproved"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(rw["IsCostSheetApproved"]);
                        WOSJobOpeningObj.CostSheetApproved = WOSJobOpeningObj.IsCostSheetApproved == true ? "Approved" : "Pending";
                    }
                    if (WOSJobOpeningDs.Tables.Count > 3 && WOSJobOpeningDs.Tables[3].Rows.Count > 0)
                    {
                        WOSJobOpeningObj.WOSHouseDetails = (from rw in WOSJobOpeningDs.Tables[3].AsEnumerable()
                                                            select new WOSHouseDetails()
                                                            {
                                                                WOSMoveID = rw["WOSMoveID"] == DBNull.Value ? 0 : Convert.ToInt64(rw["WOSMoveID"]),
                                                                HouseMasterID = rw["HouseMasterID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["HouseMasterID"]),
                                                                HouseJobTransID = rw["HouseJobTransID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["HouseJobTransID"]),
                                                                LeaseSignedDate = rw["LeaseSignedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["LeaseSignedDate"]),
                                                                LeaseRenewedUntilDate = rw["LeaseRenewedUntilDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["LeaseRenewedUntilDate"]),
                                                                LeaseExpiryDate = rw["LeaseExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["LeaseExpiryDate"]),
                                                                IsFixedPeriod = rw["IsFixedPeriod"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsFixedPeriod"]),
                                                                FinalArrDate = rw["FinalArrDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["FinalArrDate"]),
                                                                HHGArrDate = rw["HHGArrDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["HHGArrDate"]),
                                                                WorkStartDate = rw["WorkStartDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["WorkStartDate"]),
                                                                OfficialHandoverPropertyDate = rw["OfficialHandoverPropertyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["OfficialHandoverPropertyDate"]),
                                                                InspectionDateIn = rw["InspectionDateIn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["InspectionDateIn"]),
                                                                ShortTermInspDate = rw["ShortTermInspDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ShortTermInspDate"]),
                                                                SettlingDate = rw["SettlingDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["SettlingDate"]),
                                                                ExpMoveOutDate = rw["ExpMoveOutDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ExpMoveOutDate"]),
                                                                ActMoveOutDate = rw["ActMoveOutDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ActMoveOutDate"]),
                                                                InspectionDateOut = rw["InspectionDateOut"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["InspectionDateOut"]),
                                                                ProperyReleaseByDate = rw["ProperyReleaseByDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ProperyReleaseByDate"]),
                                                                OfficialHandbackPropertyDate = rw["OfficialHandbackPropertyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["OfficialHandbackPropertyDate"]),
                                                                InactiveDate = rw["InactiveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["InactiveDate"]),
                                                                IsClosed = rw["IsClosed"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsClosed"])
                                                            }).First();
                    }
                    if (WOSJobOpeningDs.Tables.Count > 4 && WOSJobOpeningDs.Tables[4].Rows.Count > 0)
                    {
                        for (int i = 0; i < WOSJobOpeningDs.Tables[4].Columns.Count; i++)
                        {
                            TabList tabList = new TabList();
                            tabList.TabIndex = Convert.ToInt32(WOSJobOpeningDs.Tables[4].Rows[0][i].ToString());
                            WOSJobOpeningObj.TabList.Add(tabList);
                        }
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "GetWOSJobDetailsById", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return WOSJobOpeningObj;
        }

        public bool SaveWOSJobDetails(Entities.WOSJobOpening WOSJobOpening, int LoginID, out string result)
        {
            try
            {
                return WOSJobOpeningDAL.SaveWOSJobDetails(WOSJobOpening, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "SaveWOSJobDetails", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public Assignee GetAssigneeDetailsById(int LoginID, Int64 WOSMoveID)
        {
            Assignee AssigneeObj = new Assignee();
            AssigneeObj.WOSMoveID = WOSMoveID;
            try
            {
                DataSet AssigneeDt = WOSJobOpeningDAL.GetAssigneeDetailsById(LoginID, WOSMoveID);

                if (AssigneeDt != null)
                {
                    if (AssigneeDt.Tables.Count > 0 && AssigneeDt.Tables[0].Rows.Count > 0)
                    {
                        AssigneeObj = (from rw in AssigneeDt.Tables[0].AsEnumerable()
                                       select new Assignee()
                                       {
                                           WOSMoveID = rw["WOSMoveID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["WOSMoveID"]),
                                           MoveID = rw["MoveID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["MoveID"]),
                                           WOSJobID = rw["WOSJobID"] == DBNull.Value ? null : Convert.ToString(rw["WOSJobID"]),
                                           TransfereeId = rw["TransfereeId"] == DBNull.Value ? 0 : Convert.ToInt32(rw["TransfereeId"]),
                                           LastName = rw["LastName"] == DBNull.Value ? rw["AssigneeLName"] == DBNull.Value ? null : Convert.ToString(rw["AssigneeLName"]) : Convert.ToString(rw["LastName"]),
                                           FirstName = rw["FirstName"] == DBNull.Value ? rw["AssigneeFName"] == DBNull.Value ? null : Convert.ToString(rw["AssigneeFName"]) : Convert.ToString(rw["FirstName"]),
                                           MiddleName = rw["MiddleName"] == DBNull.Value ? null : Convert.ToString(rw["MiddleName"]),
                                           MaidenName = rw["MaidenName"] == DBNull.Value ? null : Convert.ToString(rw["MaidenName"]),
                                           AssigneeTitleId = rw["Title"] == DBNull.Value ? null : Convert.ToString(rw["Title"]),
                                           AssigneeRef = rw["TransfereeRef"] == DBNull.Value ? null : Convert.ToString(rw["TransfereeRef"]),
                                           GenderId = rw["GenderId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["GenderId"]),
                                           IsSmoker = rw["IsSmoker"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsSmoker"]),
                                           BirthDate = rw["BirthDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["BirthDate"]),
                                           Languages = rw["Languages"] == DBNull.Value ? null : Convert.ToString(rw["Languages"]),
                                           Religion = rw["Religion"] == DBNull.Value ? null : Convert.ToString(rw["Religion"]),
                                           EmailId = rw["EmailId"] == DBNull.Value ? null : Convert.ToString(rw["EmailId"]),
                                           Address1 = rw["Address1"] == DBNull.Value ? null : Convert.ToString(rw["Address1"]),
                                           Address2 = rw["Address2"] == DBNull.Value ? null : Convert.ToString(rw["Address2"]),
                                           CityID = rw["CityID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["CityID"]),
                                           City = rw["CityName"] == DBNull.Value ? null : Convert.ToString(rw["CityName"]),
                                           PINCode = rw["Zip_PinCode"] == DBNull.Value ? null : Convert.ToString(rw["Zip_PinCode"]),

                                           //PassportNo = rw["PassportNo"] == DBNull.Value ? null : Convert.ToString(rw["PassportNo"]),
                                           //PassportIssuedBy = rw["PassportIssuedBy"] == DBNull.Value ? null : Convert.ToString(rw["PassportIssuedBy"]),
                                           //PassportIssueDate = rw["PassportIssueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["PassportIssueDate"]),
                                           //PassportExpiryDate = rw["PassportExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["PassportExpiryDate"]),
                                           //CountryId_Nationality = rw["CountryId_Nationality"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CountryId_Nationality"]),
                                           //CountryId_OtherNationality = rw["CountryId_OtherNationality"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CountryId_OtherNationality"]),
                                           //PlaceOfBirth = rw["PlaceOfBirth"] == DBNull.Value ? null : Convert.ToString(rw["PlaceOfBirth"]),
                                           //CountryId_CountryOfBirth = rw["CountryId_CountryOfBirth"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CountryId_CountryOfBirth"]),
                                           //DrivingLicenseNumber = rw["DrivingLicenseNumber"] == DBNull.Value ? null : Convert.ToString(rw["DrivingLicenseNumber"]),
                                           //CountryId_DrivingLicenseIssuedBy = rw["CountryId_DrivingLicenseIssuedBy"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CountryId_DrivingLicenseIssuedBy"]),
                                           //DrivingLicenseIssueDate = rw["DrivingLicenseIssueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["DrivingLicenseIssueDate"]),
                                           //DrivingLicenseExpiryDate = rw["DrivingLicenseExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["DrivingLicenseExpiryDate"]),
                                           //IDCardNo = rw["IDCardNo"] == DBNull.Value ? null : Convert.ToString(rw["IDCardNo"]),
                                           //IDCardIssuedBy = rw["IDCardIssuedBy"] == DBNull.Value ? null : Convert.ToString(rw["IDCardIssuedBy"]),
                                           //IDCardIssueDate = rw["IDCardIssueDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["IDCardIssueDate"]),
                                           //IDCardExpiryDate = rw["IDCardExpiryDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["IDCardExpiryDate"]),
                                           //VAT = rw["VAT"] == DBNull.Value ? null : Convert.ToString(rw["VAT"]),
                                           //SSN = rw["SSN"] == DBNull.Value ? null : Convert.ToString(rw["SSN"]),
                                           //TransfereeWorkReference = rw["TransfereeWorkReference"] == DBNull.Value ? null : Convert.ToString(rw["TransfereeWorkReference"]),

                                           LastName_Partner = rw["LastName_Partner"] == DBNull.Value ? null : Convert.ToString(rw["LastName_Partner"]),
                                           FirstName_Partner = rw["FirstName_Partner"] == DBNull.Value ? null : Convert.ToString(rw["FirstName_Partner"]),
                                           MiddleName_Partner = rw["MiddleName_Partner"] == DBNull.Value ? null : Convert.ToString(rw["MiddleName_Partner"]),
                                           MaidenName_Partner = rw["MaidenName_Partner"] == DBNull.Value ? null : Convert.ToString(rw["MaidenName_Partner"]),
                                           Title_Partner = rw["Title_Partner"] == DBNull.Value ? null : Convert.ToString(rw["Title_Partner"]),
                                           EmailId_Partner = rw["EmailId_Partner"] == DBNull.Value ? null : Convert.ToString(rw["EmailId_Partner"]),
                                           GenderId_Partner = rw["GenderID_Partner"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["GenderID_Partner"]),
                                           IsSmoker_Partner = rw["IsSmoker_Partner"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsSmoker_Partner"]),
                                           BirthDate_Partner = rw["BirthDate_Partner"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["BirthDate_Partner"]),
                                           Languages_Partner = rw["Languages_Partner"] == DBNull.Value ? null : Convert.ToString(rw["Languages_Partner"]),
                                           Religion_Partner = rw["Religion_Partner"] == DBNull.Value ? null : Convert.ToString(rw["Religion_Partner"]),
                                           Occupation_Partner = rw["Occupation_Partner"] == DBNull.Value ? null : Convert.ToString(rw["Occupation_Partner"]),
                                           //PassportNo_Partner = rw["PassportNo_Partner"] == DBNull.Value ? null : Convert.ToString(rw["PassportNo_Partner"]),
                                           //PassportIssuedBy_Partner = rw["PassportIssuedBy_Partner"] == DBNull.Value ? null : Convert.ToString(rw["PassportIssuedBy_Partner"]),
                                           //PassportIssueDate_Partner = rw["PassportIssueDate_Partner"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["PassportIssueDate_Partner"]),
                                           //PassportExpiryDate_Partner = rw["PassportExpiryDate_Partner"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["PassportExpiryDate_Partner"]),
                                           //CountryId_Nationality_Partner = rw["CountryId_Nationality_Partner"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CountryId_Nationality_Partner"]),
                                           //CountryId_OtherNationality_Partner = rw["CountryId_OtherNationality_Partner"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CountryId_OtherNationality_Partner"]),
                                           //PlaceOfBirth_Partner = rw["PlaceOfBirth_Partner"] == DBNull.Value ? null : Convert.ToString(rw["PlaceOfBirth_Partner"]),
                                           //CountryId_CountryOfBirth_Partner = rw["CountryId_CountryOfBirth_Partner"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CountryId_CountryOfBirth_Partner"]),
                                           //DrivingLicenseNumber_Partner = rw["DrivingLicenseNumber_Partner"] == DBNull.Value ? null : Convert.ToString(rw["DrivingLicenseNumber_Partner"]),
                                           //CountryId_DrivingLicenseIssuedBy_Partner = rw["CountryId_DrivingLicenseIssuedBy_Partner"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CountryId_DrivingLicenseIssuedBy_Partner"]),
                                           //DrivingLicenseIssueDate_Partner = rw["DrivingLicenseIssueDate_Partner"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["DrivingLicenseIssueDate_Partner"]),
                                           //DrivingLicenseExpiryDate_Partner = rw["DrivingLicenseExpiryDate_Partner"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["DrivingLicenseExpiryDate_Partner"]),
                                           //IDCardNo_Partner = rw["IDCardNo_Partner"] == DBNull.Value ? null : Convert.ToString(rw["IDCardNo_Partner"]),
                                           //IDCardIssuedBy_Partner = rw["IDCardIssuedBy_Partner"] == DBNull.Value ? null : Convert.ToString(rw["IDCardIssuedBy_Partner"]),
                                           //IDCardIssueDate_Partner = rw["IDCardIssueDate_Partner"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["IDCardIssueDate_Partner"]),
                                           //IDCardExpiryDate_Partner = rw["IDCardExpiryDate_Partner"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["IDCardExpiryDate_Partner"]),
                                           //VAT_Partner = rw["VAT_Partner"] == DBNull.Value ? null : Convert.ToString(rw["VAT_Partner"]),
                                           //SSN_Partner = rw["SSN_Partner"] == DBNull.Value ? null : Convert.ToString(rw["SSN_Partner"]),
                                           //PrivacyAgreementAcceptanceDate = rw["PrivacyAgreementAcceptanceDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["PrivacyAgreementAcceptanceDate"]),

                                           MaritalStatusId = rw["MaritalStatusId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["MaritalStatusId"]),
                                           MarriageDate = rw["MarriageDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["MarriageDate"]),
                                           IsMarriageCertAvailable = rw["IsMarriageCertAvailable"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsMarriageCertAvailable"]),
                                           IsPetsMoving = rw["IsPetsMoving"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsPetsMoving"]),
                                           PetsDescription = rw["PetsDescription"] == DBNull.Value ? null : Convert.ToString(rw["PetsDescription"]),
                                           FamilyDetails = rw["FamilyDetails"] == DBNull.Value ? null : Convert.ToString(rw["FamilyDetails"]),
                                           IsMovingTogether = rw["IsMovingTogether"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsMovingTogether"]),
                                           NextOfKinDetails = rw["NextOfKinDetails"] == DBNull.Value ? null : Convert.ToString(rw["NextOfKinDetails"]),
                                           NoOfChildren = rw["NoOfChildren"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["NoOfChildren"]),

                                           //SchoolTypeId = rw["SchoolTypeId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["SchoolTypeId"]),
                                           //SpecificSchoolDetails = rw["SpecificSchoolDetails"] == DBNull.Value ? null : Convert.ToString(rw["SpecificSchoolDetails"]),
                                           //SpecialSchoolRequirements = rw["SpecialSchoolRequirements"] == DBNull.Value ? null : Convert.ToString(rw["SpecialSchoolRequirements"]),
                                           SpecialHealthProblem = rw["SpecialHealthProblem"] == DBNull.Value ? null : Convert.ToString(rw["SpecialHealthProblem"]),

                                           HomeFindingVisitDate = rw["HomeFindingVisitDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["HomeFindingVisitDate"]),
                                           DestinationServicesDates = rw["DestinationServicesDates"] == DBNull.Value ? null : Convert.ToString(rw["DestinationServicesDates"]),
                                           ArrivalDate = rw["ArrivalDateTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ArrivalDateTime"]),
                                           //ArrivalTime = rw["ArrivalTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ArrivalTime"]),

                                           HotelName = rw["HotelName"] == DBNull.Value ? null : Convert.ToString(rw["HotelName"]),
                                           HotelContactNo = rw["HotelContactNo"] == DBNull.Value ? null : Convert.ToString(rw["HotelContactNo"]),

                                           CurrencyId = rw["CurrencyId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CurrencyId"]),

                                           RentOrPurchase = rw["RentOrPurchase"] == DBNull.Value ? null : Convert.ToString(rw["RentOrPurchase"]),

                                           AllowancePerMonth = rw["AllowancePerMonth"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["AllowancePerMonth"]),

                                           //HouseOrApartment = rw["HouseOrApartment"] == DBNull.Value ? null : Convert.ToString(rw["HouseOrApartment"]),
                                           IsHouse = rw["IsHouse"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsHouse"]),
                                           IsApartment = rw["IsApartment"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsApartment"]),
                                           OtherHousingPreference = rw["OtherHousingPreference"] == DBNull.Value ? null : Convert.ToString(rw["OtherHousingPreference"]),
                                           IsAllowanceIncludesUtilities = rw["IsAllowanceIncludesUtilities"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsAllowanceIncludesUtilities"]),
                                           AllowancePerMonthWithUtilities = rw["AllowancePerMonthWithUtilities"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["AllowancePerMonthWithUtilities"]),
                                           FurnishedOrUnfurnished = rw["FurnishedOrUnfurnished"] == DBNull.Value ? null : Convert.ToString(rw["FurnishedOrUnfurnished"]),
                                           IsAllowanceIncludesFurniture = rw["IsAllowanceIncludesFurniture"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsAllowanceIncludesFurniture"]),
                                           AllowancePerMonthWithFurniture = rw["AllowancePerMonthWithFurniture"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["AllowancePerMonthWithFurniture"]),
                                           EssentialFeatures = rw["EssentialFeatures"] == DBNull.Value ? null : Convert.ToString(rw["EssentialFeatures"]),

                                           NoOfBedrooms = rw["NoOfBedrooms"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["NoOfBedrooms"]),
                                           NoOfBathrooms = rw["NoOfBathrooms"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["NoOfBathrooms"]),
                                           NoOfLivingRooms = rw["NoOfLivingRooms"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["NoOfLivingRooms"]),
                                           NoOfDiningRooms = rw["NoOfDiningRooms"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["NoOfDiningRooms"]),

                                           IsPreferredCommunities = rw["IsPreferredCommunities"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsPreferredCommunities"]),
                                           PreferredCommunitiesDetails = rw["PreferredCommunitiesDetails"] == DBNull.Value ? null : Convert.ToString(rw["PreferredCommunitiesDetails"]),

                                           LocationId = rw["LocationId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["LocationId"]),
                                           MaxCommutingTime = rw["MaxCommutingTime"] == DBNull.Value ? null : Convert.ToString(rw["MaxCommutingTime"]),
                                           ByCarOrPublicTransport = rw["ByCarOrPublicTransport"] == DBNull.Value ? null : Convert.ToString(rw["ByCarOrPublicTransport"]),

                                           WithinEasyReachOfTheProperty = rw["WithinEasyReachOfTheProperty"] == DBNull.Value ? null : Convert.ToString(rw["WithinEasyReachOfTheProperty"]),

                                           IsDriversLicenseAvailable = rw["IsDriversLicenseAvailable"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsDriversLicenseAvailable"]),

                                           CountryId_DrivingLicenseIssuedBy = rw["CountryId_DrivingLicenseIssuedBy"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CountryId_DrivingLicenseIssuedBy"]),

                                           VehiclesImportLeasePurchase = rw["VehiclesImportLeasePurchase"] == DBNull.Value ? null : Convert.ToString(rw["VehiclesImportLeasePurchase"]),
                                           NoOfVehicles = rw["NoOfVehicles"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["NoOfVehicles"]),
                                           TypeOfVehicles = rw["TypeOfVehicles"] == DBNull.Value ? null : Convert.ToString(rw["TypeOfVehicles"]),

                                           IsFinalSubmit = rw["IsFinalSubmit"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsFinalSubmit"])
                                       }).First();
                    }

                    if (AssigneeDt.Tables.Count > 1 && AssigneeDt.Tables[1].Rows.Count > 0)
                    {
                        AssigneeObj.ChildDetailsList = (from rw in AssigneeDt.Tables[1].AsEnumerable()
                                                        select new ChildDetails()
                                                        {
                                                            ChildDetailsId = rw["ChildDetailsId"] == DBNull.Value ? 0 : Convert.ToInt64(rw["ChildDetailsId"]),
                                                            ChildName = rw["ChildName"] == DBNull.Value ? null : Convert.ToString(rw["ChildName"]),
                                                            ChildAge = rw["ChildAge"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["ChildAge"]),
                                                            //SchoolType = rw["SchoolType"] == DBNull.Value ? null : Convert.ToString(rw["SchoolType"]),
                                                            SchoolTypeId = rw["SchoolTypeId"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["SchoolTypeId"]),
                                                            SchoolType = rw["SchoolTypeName"] == DBNull.Value ? null : Convert.ToString(rw["SchoolTypeName"]),
                                                            SpecificSchoolDetails = rw["SpecificSchoolDetails"] == DBNull.Value ? null : Convert.ToString(rw["SpecificSchoolDetails"]),
                                                            SpecialSchoolRequirements = rw["SpecialSchoolRequirements"] == DBNull.Value ? null : Convert.ToString(rw["SpecialSchoolRequirements"])
                                                        }).ToList();
                    }

                    if (AssigneeDt.Tables.Count > 2 && AssigneeDt.Tables[2].Rows.Count > 0)
                    {
                        AssigneeObj.WOSJobDocUploadList = (from rw in AssigneeDt.Tables[2].AsEnumerable()
                                                           select new WOSJobDocUpload()
                                                           {
                                                               FileID = rw["FileID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["FileID"]),//
                                                               JobDocTypeId = rw["JobDocTypeId"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["JobDocTypeId"]),
                                                               DocTypeID = rw["DocTypeID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DocTypeID"]),
                                                               DocTypeText = rw["DocTypeName"] == DBNull.Value ? null : Convert.ToString(rw["DocTypeName"]),
                                                               DocNameID = rw["DocNameID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DocNameID"]),
                                                               DocNameText = rw["DocName"] == DBNull.Value ? null : Convert.ToString(rw["DocName"]),
                                                               DocDescription = rw["Description"] == DBNull.Value ? null : Convert.ToString(rw["Description"]),
                                                               Remarks = rw["Remarks"] == DBNull.Value ? null : Convert.ToString(rw["Remarks"]),
                                                               FileName = rw["DocFileName"] == DBNull.Value ? null : Convert.ToString(rw["DocFileName"]),
                                                               UploadById = rw["CreatedBy"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CreatedBy"]),//
                                                               UploadBy = rw["UploadBy"] == DBNull.Value ? null : Convert.ToString(rw["UploadBy"]),//
                                                               UploadDate = rw["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["CreatedDate"])//
                                                           }).ToList();
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "GetAssigneeDetailsById", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return AssigneeObj;
        }

        public bool AddEditAssignee(Assignee AssigneeObj, int LoginID, out string result)
        {
            try
            {
                return WOSJobOpeningDAL.AddEditAssignee(AssigneeObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "AddEditAssignee", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public List<ChatDetails> GetChatting(int LoginID, Int64 WOSMoveID, string EMPorAssingee = "A")
        {
            List<ChatDetails> ChatDetailsObj = new List<ChatDetails>();
            try
            {
                DataSet ChatDt = WOSJobOpeningDAL.GetChatting(LoginID, WOSMoveID, EMPorAssingee);

                if (ChatDt != null)
                {
                    if (ChatDt.Tables.Count > 0 && ChatDt.Tables[0].Rows.Count > 0)
                    {
                        ChatDetailsObj = (from rw in ChatDt.Tables[0].AsEnumerable()
                                          select new ChatDetails()
                                          {
                                              ChatID = rw["ChatID"] == DBNull.Value ? 0 : Convert.ToInt64(rw["ChatID"]),
                                              WOSMoveID = rw["WOSMoveID"] == DBNull.Value ? 0 : Convert.ToInt64(rw["WOSMoveID"]),
                                              ChatMsg = rw["ChatMsg"] == DBNull.Value ? null : Convert.ToString(rw["ChatMsg"]),
                                              ChatDateTime = rw["ChatDateTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ChatDateTime"]),
                                              ChatBy = rw["ChatBy"] == DBNull.Value ? null : Convert.ToString(rw["ChatBy"])
                                          }).ToList();
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "GetAssigneeDetailsById", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return ChatDetailsObj;
        }

        public bool AddEditChatting(ChatDetails ChatDetailsObj, int LoginID, out string result)
        {
            try
            {
                return WOSJobOpeningDAL.AddEditChatting(ChatDetailsObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "AddEditChatting", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public List<TaskDetails> GetTaskInfo(int LoginID, Int64 WOSMoveID, string EMPorAssingee = "A")
        {
            List<TaskDetails> TaskDetailsObj = new List<TaskDetails>();
            try
            {
                DataSet TaskDt = WOSJobOpeningDAL.GetTaskInfo(LoginID, WOSMoveID, EMPorAssingee);

                if (TaskDt != null)
                {
                    if (TaskDt.Tables.Count > 0 && TaskDt.Tables[0].Rows.Count > 0)
                    {
                        TaskDetailsObj = (from rw in TaskDt.Tables[0].AsEnumerable()
                                          select new TaskDetails()
                                          {
                                              ProgressID = rw["ProgressID"] == DBNull.Value ? 0 : Convert.ToInt64(rw["ProgressID"]),
                                              TaskMasterID = rw["TaskMasterID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["TaskMasterID"]),
                                              WOSMoveID = rw["WOSMoveID"] == DBNull.Value ? 0 : Convert.ToInt64(rw["WOSMoveID"]),
                                              TaskDescription = rw["TaskDescription"] == DBNull.Value ? null : Convert.ToString(rw["TaskDescription"]),
                                              ScheduleDate = rw["ScheduleDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ScheduleDate"]),
                                              ActualDate = rw["ActualDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["ActualDate"])
                                          }).ToList();
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "GetAssigneeDetailsById", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return TaskDetailsObj;
        }

        public bool AddEditTaskInfo(TaskDetails TaskDetailsObj, int LoginID, out string result)
        {
            try
            {
                return WOSJobOpeningDAL.AddEditTaskInfo(TaskDetailsObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "AddEditTaskInfo", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool InsertRequestDocsData(Assignee Assignee, int LoginID, string Url, out string result)
        {
            result = string.Empty;
            try
            {
                return WOSJobOpeningDAL.InsertRequestDocsData(Assignee, LoginID, Url, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "InsertRequestDocsData", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool InsertDocument(WOSJobDocUpload WOSJobDocUploadObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                return WOSJobOpeningDAL.InsertDocument(WOSJobDocUploadObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "InsertDocument", Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public List<WOSSubService> GetCostSheetForJob(Int64? WOSMoveID, int? RevCurrID, int? CostCurrID, int? LoginID)
        {
            IQueryable<WOSSubService> CostSheetForJobList;
            try
            {
                DataSet CostSheetForJobDs = WOSJobOpeningDAL.GetCostSheetForJob(WOSMoveID, RevCurrID, CostCurrID, LoginID);

                if (CostSheetForJobDs != null && CostSheetForJobDs.Tables.Count > 0 && CostSheetForJobDs.Tables[0].Rows.Count > 0)
                {
                    var result = (from rw in CostSheetForJobDs.Tables[0].AsEnumerable()
                                  select new WOSSubService()
                                  {
                                      SubServiceMastID = rw["SubServiceMastID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["SubServiceMastID"]),
                                      ServiceMastID = rw["ServiceMastID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["ServiceMastID"]),
                                      ServiceName = rw["ServiceName"] == DBNull.Value ? null : Convert.ToString(rw["ServiceName"]),
                                      SubServiceName = rw["SubServiceName"] == DBNull.Value ? null : Convert.ToString(rw["SubServiceName"]),
                                      MastCostAmount = rw["MastCostAmt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["MastCostAmt"]),
                                      MastRevenueAmount = rw["MastRevenueAmt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["MastRevenueAmt"]),
                                      BillCostAmount = rw["BillCostAmt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["BillCostAmt"]),
                                      BillRevenueAmount = rw["BillRevenueAmt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["BillRevenueAmt"]),
                                      UnbilledAmount = rw["UnbilledAmt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["UnbilledAmt"]),
                                      IsChecked = rw["IsChecked"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsChecked"]),
                                      SrNo = rw["DispOrder"] == DBNull.Value ? 0 : Convert.ToInt32(rw["DispOrder"])
                                  }).ToList();
                    CostSheetForJobList = result.AsQueryable();

                    return CostSheetForJobList.ToList();
                }
                else
                {
                    return new List<WOSSubService>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "GetCostSheetForJob", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool SaveCostSheet(Entities.WOSCustomer WOSCustomer, int LoginID, out string result)
        {
            try
            {
                return WOSJobOpeningDAL.SaveCostSheet(WOSCustomer, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "SaveCostSheet", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool ApproveCostSheet(Entities.WOSJobOpening WOSJobOpeningObj, bool IsApprove, int LoginID, out string result)
        {
            try
            {
                return WOSJobOpeningDAL.ApproveCostSheet(WOSJobOpeningObj, IsApprove, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "ApproveCostSheet", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public DataSet GetWOSCostSheet(int LoginID, Int64? WOSMoveID, bool IsXlFormat = false)
        {
            try
            {
                return WOSJobOpeningDAL.GetWOSCostSheet(LoginID, WOSMoveID, IsXlFormat);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "GetWOSCostSheet", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public JobDocument GetJobDocumentDetail(int FileID, int LoginID)
        {
            JobDocument JobDocumentObj = new JobDocument();
            try
            {
                DataSet JobDocumentDs = WOSJobOpeningDAL.GetJobDocumentDetail(FileID, LoginID);

                if (JobDocumentDs != null && JobDocumentDs.Tables.Count > 0 && JobDocumentDs.Tables[0].Rows.Count > 0)
                {
                    JobDocumentObj.FileID = Convert.ToInt32(JobDocumentDs.Tables[0].Rows[0]["FileID"]);
                    JobDocumentObj.DocTypeID = Convert.ToInt32(JobDocumentDs.Tables[0].Rows[0]["DocTypeID"]);
                    JobDocumentObj.DocNameID = Convert.ToInt32(JobDocumentDs.Tables[0].Rows[0]["DocNameID"]);
                    JobDocumentObj.FilePath = Convert.ToString(JobDocumentDs.Tables[0].Rows[0]["DocFilePath"]);
                    JobDocumentObj.FileName = Convert.ToString(JobDocumentDs.Tables[0].Rows[0]["DocFileName"]);
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "GetJobDocumentDetail", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return JobDocumentObj;
        }

        public bool SaveWOSHouseDetails(WOSHouseDetails WOSHouseDetailsObj, int LoginID, out string result)
        {
            try
            {
                return WOSJobOpeningDAL.SaveWOSHouseDetails(WOSHouseDetailsObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "SaveWOSHouseDetails", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool CancelWOSJob(Entities.WOSJobOpening WOSJobOpeningObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                return WOSJobOpeningDAL.CancelWOSJob(WOSJobOpeningObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "CancelWOSJob", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool SendLinkToAssignee(Int64 WOSMoveID, string EmailTo, string EmailCc, string EmailBcc, string Url, int LoginID, out string result)
        {
            try
            {
                return WOSJobOpeningDAL.SendLinkToAssignee(WOSMoveID, EmailTo, EmailCc, EmailBcc, Url, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WOSJobOpeningBL", "SendLinkToAssignee", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }
    }
}