using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.WHAssetManagement;
using RELOCBS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;

namespace RELOCBS.BL.WHAssetManagement
{
    public class WHAssetManagementBL
    {
        private WHAssetManagementDAL _WHAssetManagementDAL;
        public WHAssetManagementDAL WHAssetManagementDAL
        {
            get
            {
                if (_WHAssetManagementDAL == null)
                    _WHAssetManagementDAL = new WHAssetManagementDAL();
                return _WHAssetManagementDAL;
            }
        }

        public IEnumerable<WHInOutAssetMaster> GetWHInOutAssetList(int LoginID, DateTime? FromDate, DateTime? ToDate, string JobID, string RefJobID, string FilterName, string FilterValue, string Sort, string SortDir, int Skip, int PageSize, out int TotalCount)
        {
            IQueryable<WHInOutAssetMaster> WHInOutAssetList;
            TotalCount = 0;
            try
            {
                DataSet WHInOutAssetDs = WHAssetManagementDAL.GetWHInOutAssetList(LoginID, FromDate, ToDate, JobID, RefJobID, FilterName, FilterValue);

                if (WHInOutAssetDs != null && WHInOutAssetDs.Tables.Count > 0 && WHInOutAssetDs.Tables[0].Rows.Count > 0)
                {
                    var result = (from rw in WHInOutAssetDs.Tables[0].AsEnumerable()
                                  select new WHInOutAssetMaster()
                                  {
                                      MoveID = rw["MoveID"] == DBNull.Value ? 0 : Convert.ToInt64(rw["MoveID"]),
                                      JobID = rw["JobID"] == DBNull.Value ? null : Convert.ToString(rw["JobID"]),
                                      RefJobID = rw["RefJobID"] == DBNull.Value ? null : Convert.ToString(rw["RefJobID"]),
                                      WareHouse = rw["WareHouse"] == DBNull.Value ? null : Convert.ToString(rw["WareHouse"])
                                  }).ToList();
                    WHInOutAssetList = result.AsQueryable();

                    TotalCount = WHInOutAssetList.Count();
                    WHInOutAssetList = WHInOutAssetList.OrderBy(Sort + " " + SortDir);
                    if (PageSize > 0)
                    {
                        WHInOutAssetList = WHInOutAssetList.Skip(Skip).Take(PageSize);
                    }
                    return WHInOutAssetList.ToList();
                }
                else
                {
                    return new List<WHInOutAssetMaster>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "GetWHInOutAssetList", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public WHAssetMaster GetAssetInwardDetails(int LoginID, Int64? MoveID, Int64? InMastID = null, Int64? OutMasterID = null)
        {
            WHAssetMaster WHAssetMasterObj = new WHAssetMaster();
            try
            {
                DataSet WHAssetMasterDs = WHAssetManagementDAL.GetAssetInwardDetails(LoginID, MoveID, InMastID, OutMasterID);

                if (WHAssetMasterDs != null)
                {
                    if (WHAssetMasterDs.Tables.Count > 0 && WHAssetMasterDs.Tables[0].Rows.Count > 0)
                    {
                        WHAssetMasterObj.WHInAssetMaster = (from rw in WHAssetMasterDs.Tables[0].AsEnumerable()
                                                            select new WHInAssetMaster()
                                                            {
                                                                MoveID = rw["MoveID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["MoveID"]),
                                                                JobID = rw["JobID"] == DBNull.Value ? null : Convert.ToString(rw["JobID"]),
                                                                RefJobID = rw["RefJobID"] == DBNull.Value ? null : Convert.ToString(rw["RefJobID"]),
                                                                WareHouseID = rw["WareHouseID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["WareHouseID"]),
                                                                WareHouse = rw["WareHouse"] == DBNull.Value ? null : Convert.ToString(rw["WareHouse"]),
                                                                ShipperName = rw["ShipperName"] == DBNull.Value ? null : Convert.ToString(rw["ShipperName"]),
                                                                JobOpenedDate = rw["JobOpenedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["JobOpenedDate"])
                                                            }).FirstOrDefault();

                        WHAssetMasterObj.WHInAssetMasterList = (from rw in WHAssetMasterDs.Tables[0].AsEnumerable()
                                                                select new WHInAssetMaster()
                                                                {
                                                                    InMastID = rw["InMastID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["InMastID"]),
                                                                    GateChallanNumber = rw["GateChallanNumber"] == DBNull.Value ? null : Convert.ToString(rw["GateChallanNumber"]),
                                                                    GateChalanDate = rw["GateChalanDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["GateChalanDate"]),
                                                                    IsDirectDel = rw["IsDirectDel"] == DBNull.Value ? (bool?)null : Convert.ToBoolean(rw["IsDirectDel"]),
                                                                    WareHouseID = rw["WareHouseID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["WareHouseID"]),
                                                                    InDateTime = rw["InDateTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["InDateTime"]),
                                                                    NoOfPackaage = rw["NoOfPackaage"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["NoOfPackaage"]),
                                                                    TotalVol = rw["TotalVol"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["TotalVol"]),
                                                                    VolUnitID = rw["VolUnitID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["VolUnitID"]),
                                                                    VolUnit = rw["VolUnit"] == DBNull.Value ? null : Convert.ToString(rw["VolUnit"]),
                                                                    Remarks = rw["Remarks"] == DBNull.Value ? null : Convert.ToString(rw["Remarks"])
                                                                }).ToList();
                    }
                    if (WHAssetMasterDs.Tables.Count > 1 && WHAssetMasterDs.Tables[1].Rows.Count > 0)
                    {
                        WHAssetMasterObj.WHInAssetMaster.WHInAssetDetailsList = (from rw in WHAssetMasterDs.Tables[1].AsEnumerable()
                                                                                 select new WHInAssetDetails()
                                                                                 {
                                                                                     InMastID = rw["InMastID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["InMastID"]),
                                                                                     AssetDetID = rw["AssetDetID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["AssetDetID"]),
                                                                                     GateChallanNumber = rw["GateChallanNumber"] == DBNull.Value ? null : Convert.ToString(rw["GateChallanNumber"]),
                                                                                     GateChalanDate = rw["GateChalanDate"] == DBNull.Value ? null : Convert.ToString(rw["GateChalanDate"]),
                                                                                     BarcodeID = rw["BarcodeID"] == DBNull.Value ? null : Convert.ToString(rw["BarcodeID"]),
                                                                                     BarcodeSeqNo = rw["BarcodeSeqNo"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["BarcodeSeqNo"]),
                                                                                     AssetRefID = rw["AssetRefID"] == DBNull.Value ? null : Convert.ToString(rw["AssetRefID"]),
                                                                                     AssetDescription = rw["AssetDescription"] == DBNull.Value ? null : Convert.ToString(rw["AssetDescription"]),
                                                                                     AssetDimL = rw["AssetDimL"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["AssetDimL"]),
                                                                                     AssetDimB = rw["AssetDimB"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["AssetDimB"]),
                                                                                     AssetDimH = rw["AssetDimH"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["AssetDimH"]),
                                                                                     AssetVol = rw["AssetVol"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["AssetVol"]),
                                                                                     DimentionUnitID = rw["DimentionUnitID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DimentionUnitID"]),
                                                                                     DimentionUnit = rw["DimentionUnit"] == DBNull.Value ? null : Convert.ToString(rw["DimentionUnit"]),
                                                                                     VolumeUnitID = rw["VolumeUnitID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["VolumeUnitID"]),
                                                                                     VolumeUnit = rw["VolumeUnit"] == DBNull.Value ? null : Convert.ToString(rw["VolumeUnit"]),
                                                                                     AssetRemarks = rw["Remarks"] == DBNull.Value ? null : Convert.ToString(rw["Remarks"])
                                                                                 }).ToList();
                    }
                    if (WHAssetMasterDs.Tables.Count > 2 && WHAssetMasterDs.Tables[2].Rows.Count > 0)
                    {
                        //WHAssetMasterObj.WHOutAssetMaster = (from rw in WHAssetMasterDs.Tables[2].AsEnumerable()
                        //                                     select new WHOutAssetMaster()
                        //                                     { }).FirstOrDefault();

                        WHAssetMasterObj.WHOutAssetMasterList = (from rw in WHAssetMasterDs.Tables[2].AsEnumerable()
                                                                 select new WHOutAssetMaster()
                                                                 {
                                                                     OutMasterID = rw["OutMasterID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["OutMasterID"]),
                                                                     GateOutChalanNo = rw["GateOutChalanNo"] == DBNull.Value ? null : Convert.ToString(rw["GateOutChalanNo"]),
                                                                     GateOutChalanDate = rw["GateOutChalanDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["GateOutChalanDate"]),
                                                                     OutDateTime = rw["OutDateTime"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["OutDateTime"]),
                                                                     OutNoPackage = rw["OutNoPackage"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["OutNoPackage"]),
                                                                     OutVolumeUnitID = rw["OutVolumeUnitID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["OutVolumeUnitID"]),
                                                                     OutVolumeUnit = rw["OutVolumeUnit"] == DBNull.Value ? null : Convert.ToString(rw["OutVolumeUnit"]),
                                                                     OutVolume = rw["OutVolume"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["OutVolume"]),
                                                                     OutLocName = rw["OutLocName"] == DBNull.Value ? null : Convert.ToString(rw["OutLocName"]),
                                                                     OutLocAdd1 = rw["OutLocAdd1"] == DBNull.Value ? null : Convert.ToString(rw["OutLocAdd1"]),
                                                                     OutLocAdd2 = rw["OutLocAdd2"] == DBNull.Value ? null : Convert.ToString(rw["OutLocAdd2"]),
                                                                     OutLocCityID = rw["OutLocCityID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["OutLocCityID"]),
                                                                     OutLocCity = rw["OutLocCity"] == DBNull.Value ? null : Convert.ToString(rw["OutLocCity"]),
                                                                     OutLocContactPerson = rw["OutLocContactPerson"] == DBNull.Value ? null : Convert.ToString(rw["OutLocContactPerson"]),
                                                                     OutLocPinCode = rw["OutLocPinCode"] == DBNull.Value ? null : Convert.ToString(rw["OutLocPinCode"]),
                                                                     OutLocPhone = rw["OutLocPhone"] == DBNull.Value ? null : Convert.ToString(rw["OutLocPhone"]),
                                                                     DeliveryProofNo = rw["DeliveryProofNo"] == DBNull.Value ? null : Convert.ToString(rw["DeliveryProofNo"]),
                                                                     IsProofUploaded = rw["IsProofUploaded"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsProofUploaded"]),
                                                                     Remarks = rw["Remarks"] == DBNull.Value ? null : Convert.ToString(rw["Remarks"])
                                                                 }).ToList();
                    }
                    if (WHAssetMasterDs.Tables.Count > 3 && WHAssetMasterDs.Tables[3].Rows.Count > 0)
                    {
                        WHAssetMasterObj.WHOutAssetMaster.WHOutAssetDetailsList = (from rw in WHAssetMasterDs.Tables[3].AsEnumerable()
                                                                                   select new WHOutAssetDetails()
                                                                                   {
                                                                                       OutMasterID = rw["OutMasterID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["OutMasterID"]),
                                                                                       OutDetailID = rw["OutDetailID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["OutDetailID"]),
                                                                                       AssetDetID = rw["AssetDetID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["AssetDetID"]),
                                                                                       GateOutChalanNo = rw["GateOutChalanNo"] == DBNull.Value ? null : Convert.ToString(rw["GateOutChalanNo"]),
                                                                                       GateOutChalanDate = rw["GateOutChalanDate"] == DBNull.Value ? null : Convert.ToString(rw["GateOutChalanDate"]),
                                                                                       BarcodeID = rw["BarcodeID"] == DBNull.Value ? null : Convert.ToString(rw["BarcodeID"]),
                                                                                       BarcodeSeqNo = rw["BarcodeSeqNo"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["BarcodeSeqNo"]),
                                                                                       AssetRefID = rw["AssetRefID"] == DBNull.Value ? null : Convert.ToString(rw["AssetRefID"]),
                                                                                       AssetDescription = rw["AssetDescription"] == DBNull.Value ? null : Convert.ToString(rw["AssetDescription"]),
                                                                                       AssetRemarks = rw["Remarks"] == DBNull.Value ? null : Convert.ToString(rw["Remarks"])
                                                                                   }).ToList();
                    }
                    if (WHAssetMasterDs.Tables.Count > 4 && WHAssetMasterDs.Tables[4].Rows.Count > 0)
                    {
                        WHAssetMasterObj.WHJobDocUploadList = (from rw in WHAssetMasterDs.Tables[4].AsEnumerable()
                                                               select new WHJobDocUpload()
                                                               {
                                                                   FileID = rw["FileID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["FileID"]),
                                                                   DocTypeID = rw["DocTypeID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DocTypeID"]),
                                                                   DocTypeText = rw["DocTypeName"] == DBNull.Value ? null : Convert.ToString(rw["DocTypeName"]),
                                                                   DocNameID = rw["DocNameID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["DocNameID"]),
                                                                   DocNameText = rw["DocName"] == DBNull.Value ? null : Convert.ToString(rw["DocName"]),
                                                                   DocDescription = rw["Description"] == DBNull.Value ? null : Convert.ToString(rw["Description"]),
                                                                   FileName = rw["DocFileName"] == DBNull.Value ? null : Convert.ToString(rw["DocFileName"]),
                                                                   UploadById = rw["CreatedBy"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["CreatedBy"]),
                                                                   UploadBy = rw["UploadBy"] == DBNull.Value ? null : Convert.ToString(rw["UploadBy"]),
                                                                   UploadDate = rw["CreatedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["CreatedDate"])
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
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "GetAssetInwardDetails", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return WHAssetMasterObj;
        }

        public bool SaveAssetInwardDetails(WHAssetMaster WHAssetMasterObj, int LoginID, out string result)
        {
            try
            {
                return WHAssetManagementDAL.SaveAssetInwardDetails(WHAssetMasterObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "SaveAssetInwardDetails", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool SaveAssetOutwardDetails(WHAssetMaster WHAssetMasterObj, int LoginID, out string result)
        {
            try
            {
                return WHAssetManagementDAL.SaveAssetOutwardDetails(WHAssetMasterObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "SaveAssetOutwardDetails", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public bool InsertDocument(WHJobDocUpload WHJobDocUploadObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                return WHAssetManagementDAL.InsertDocument(WHJobDocUploadObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "InsertDocument", Properties.Resources.UnExpectedErrorAtBL, ex);
            }

        }

        public bool SendLinkWHAssetReport(Int64 MoveID, string EmailTo, string EmailCc, string EmailBcc, string Url, int LoginID, out string result)
        {
            try
            {
                return WHAssetManagementDAL.SendLinkWHAssetReport(MoveID, EmailTo, EmailCc, EmailBcc, Url, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "SendLinkWHAssetReport", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public IEnumerable<WHInOutAssetMaster> GetWHInOutAssetReport(int LoginID, string ReportName, Int64? MoveID, string RefJobID, string Sort, string SortDir, int Skip, int PageSize, out int TotalCount)
        {
            IQueryable<WHInOutAssetMaster> WHInOutAssetList;
            TotalCount = 0;
            try
            {
                DataSet WHInOutAssetDs = WHAssetManagementDAL.GetWHInOutAssetReport(LoginID, ReportName, MoveID, RefJobID);

                if (WHInOutAssetDs != null && WHInOutAssetDs.Tables.Count > 0 && WHInOutAssetDs.Tables[0].Rows.Count > 0)
                {
                    var result = (from rw in WHInOutAssetDs.Tables[0].AsEnumerable()
                                  select new WHInOutAssetMaster()
                                  {
                                      RefJobID = rw["Ref Job ID"] == DBNull.Value ? null : Convert.ToString(rw["Ref Job ID"]),
                                      WareHouse = rw["WareHouse Name"] == DBNull.Value ? null : Convert.ToString(rw["WareHouse Name"]),
                                      InDateTime = rw["Inward Date Time"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["Inward Date Time"]),
                                      BarcodeSeqNo = rw["Box No"] == DBNull.Value ? null : Convert.ToString(rw["Box No"]),
                                      InAssetRefID = rw["In Asset Ref ID"] == DBNull.Value ? null : Convert.ToString(rw["In Asset Ref ID"]),
                                      InAssetDescription = rw["In Asset Description"] == DBNull.Value ? null : Convert.ToString(rw["In Asset Description"]),
                                      InAssetRemarks = rw["In Asset Remarks"] == DBNull.Value ? null : Convert.ToString(rw["In Asset Remarks"]),
                                      OutDateTime = rw["Outward Date Time"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["Outward Date Time"]),
                                      OutLocAdd1 = rw["Out Loc Add 1"] == DBNull.Value ? null : Convert.ToString(rw["Out Loc Add 1"]),
                                      OutLocAdd2 = rw["Out Loc Add 2"] == DBNull.Value ? null : Convert.ToString(rw["Out Loc Add 2"]),
                                      OutLocCity = rw["Out Loc City Name"] == DBNull.Value ? null : Convert.ToString(rw["Out Loc City Name"]),
                                      OutLocContactPerson = rw["Out Loc Contact Person"] == DBNull.Value ? null : Convert.ToString(rw["Out Loc Contact Person"]),
                                      OutAssetRemarks = rw["Out Asset Remarks"] == DBNull.Value ? null : Convert.ToString(rw["Out Asset Remarks"])
                                  }).ToList();

                    WHInOutAssetList = result.AsQueryable();
                    TotalCount = WHInOutAssetList.Count();

                    WHInOutAssetList = WHInOutAssetList.OrderBy(Sort + " " + SortDir);
                    if (PageSize > 0)
                    {
                        WHInOutAssetList = WHInOutAssetList.Skip(Skip).Take(PageSize);
                    }

                    return WHInOutAssetList.ToList();
                }
                else
                {
                    return new List<WHInOutAssetMaster>();
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "GetWHInOutAssetReport", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public WHInAssetDetails GetDetailsByAssetDetID(int LoginID, Int64? AssetDetID)
        {
            WHInAssetDetails WHInAssetDetailsObj = new WHInAssetDetails();
            try
            {
                DataSet AssetDetailsDs = WHAssetManagementDAL.GetDetailsByAssetDetID(LoginID, AssetDetID);

                if (AssetDetailsDs != null)
                {
                    if (AssetDetailsDs.Tables.Count > 0 && AssetDetailsDs.Tables[0].Rows.Count > 0)
                    {
                        WHInAssetDetailsObj = (from rw in AssetDetailsDs.Tables[0].AsEnumerable()
                                               select new WHInAssetDetails()
                                               {
                                                   AssetDetID = rw["AssetDetID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["AssetDetID"]),
                                                   BarcodeID = rw["BarcodeID"] == DBNull.Value ? null : Convert.ToString(rw["BarcodeID"]),
                                                   BarcodeSeqNo = rw["BarcodeSeqNo"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["BarcodeSeqNo"]),
                                                   AssetRefID = rw["AssetRefID"] == DBNull.Value ? null : Convert.ToString(rw["AssetRefID"]),
                                                   AssetDescription = rw["AssetDescription"] == DBNull.Value ? null : Convert.ToString(rw["AssetDescription"]),
                                                   AssetRemarks = rw["Remarks"] == DBNull.Value ? null : Convert.ToString(rw["Remarks"])
                                               }).FirstOrDefault();
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "GetAssetLiftVanMapping", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return WHInAssetDetailsObj;
        }

        public WHAssetMaster GetAssetLiftVanMapping(int LoginID, Int64? LiftVanID, Int64? MoveID)
        {
            WHAssetMaster WHAssetMasterObj = new WHAssetMaster();
            try
            {
                DataSet AssetLiftVanMapDs = WHAssetManagementDAL.GetAssetLiftVanMapping(LoginID, LiftVanID, MoveID);

                if (AssetLiftVanMapDs != null)
                {
                    if (AssetLiftVanMapDs.Tables.Count > 0 && AssetLiftVanMapDs.Tables[0].Rows.Count > 0)
                    {
                        WHAssetMasterObj.WHLocationMap.LiftVanDetailsList = (from rw in AssetLiftVanMapDs.Tables[0].AsEnumerable()
                                                                             select new LiftVanDetails()
                                                                             {
                                                                                 AssetDetID = rw["ID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["ID"]),
                                                                                 AssetDesc = rw["NAME"] == DBNull.Value ? null : Convert.ToString(rw["NAME"])
                                                                             }).ToList();
                    }
                    if (AssetLiftVanMapDs.Tables.Count > 1 && AssetLiftVanMapDs.Tables[1].Rows.Count > 0)
                    {
                        WHAssetMasterObj.WHLocationMap.DefLiftVanDetailsList = (from rw in AssetLiftVanMapDs.Tables[1].AsEnumerable()
                                                                                select new LiftVanDetails()
                                                                                {
                                                                                    AssetDetID = rw["ID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["ID"]),
                                                                                    AssetDesc = rw["NAME"] == DBNull.Value ? null : Convert.ToString(rw["NAME"])
                                                                                }).ToList();
                    }
                    if (AssetLiftVanMapDs.Tables.Count > 2 && AssetLiftVanMapDs.Tables[2].Rows.Count > 0)
                    {
                        WHAssetMasterObj.WHLocationMap.OtherLiftVanDetailsList = (from rw in AssetLiftVanMapDs.Tables[2].AsEnumerable()
                                                                                  select new WHInAssetDetails()
                                                                                  {
                                                                                      InMastID = rw["InMastID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["InMastID"]),
                                                                                      AssetDetID = rw["AssetDetID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["AssetDetID"]),
                                                                                      JobID = rw["JobID"] == DBNull.Value ? null : Convert.ToString(rw["JobID"]),
                                                                                      BarcodeID = rw["BarcodeID"] == DBNull.Value ? null : Convert.ToString(rw["BarcodeID"]),
                                                                                      BarcodeSeqNo = rw["BarcodeSeqNo"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["BarcodeSeqNo"]),
                                                                                      AssetRefID = rw["AssetRefID"] == DBNull.Value ? null : Convert.ToString(rw["AssetRefID"]),
                                                                                      AssetDescription = rw["AssetDescription"] == DBNull.Value ? null : Convert.ToString(rw["AssetDescription"]),
                                                                                      AssetRemarks = rw["Remarks"] == DBNull.Value ? null : Convert.ToString(rw["Remarks"])
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
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "GetAssetLiftVanMapping", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return WHAssetMasterObj;
        }

        public bool SaveAssetLiftVanMapping(WHAssetMaster WHAssetMasterObj, int LoginID, out string result)
        {
            try
            {
                return WHAssetManagementDAL.SaveAssetLiftVanMapping(WHAssetMasterObj, LoginID, out result);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "SaveAssetLiftVanMapping", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
        }

        public DataSet GetBingoChart(int LoginID, Int64? MoveID)
        {
            DataSet BingoChartDs = new DataSet();
            try
            {
                BingoChartDs = WHAssetManagementDAL.GetBingoChart(LoginID, MoveID);
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "GetBingoChart", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return BingoChartDs;
        }

        public bool ValidateBoxNo(int LoginID, Int64? MoveID, Int64? BoxNo)
        {
            bool result = false;
            try
            {
                DataSet ValidateBoxNoDs = WHAssetManagementDAL.ValidateBoxNo(LoginID, MoveID, BoxNo);
                if (ValidateBoxNoDs != null)
                {
                    if (ValidateBoxNoDs.Tables.Count > 0 && ValidateBoxNoDs.Tables[0].Rows.Count > 0)
                    {
                        DataRow rw = ValidateBoxNoDs.Tables[0].Rows[0];
                        result = rw["IsValid"] == DBNull.Value ? false : Convert.ToBoolean(rw["IsValid"]);
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "ValidateBoxNo", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return result;
        }

        public WHOutAssetMaster GetInOutWHDeliveryAddress(int LoginID, Int64? MoveID, string AddressType)
        {
            WHOutAssetMaster WHOutAssetMasterObj = new WHOutAssetMaster();
            try
            {
                DataSet AddressDs = WHAssetManagementDAL.GetInOutWHDeliveryAddress(LoginID, MoveID, AddressType);

                if (AddressDs != null)
                {
                    if (AddressDs.Tables.Count > 0 && AddressDs.Tables[0].Rows.Count > 0)
                    {
                        WHOutAssetMasterObj = (from rw in AddressDs.Tables[0].AsEnumerable()
                                               select new WHOutAssetMaster()
                                               {
                                                   OutLocName = rw["OutLocName"] == DBNull.Value ? null : Convert.ToString(rw["OutLocName"]),
                                                   OutLocAdd1 = rw["OutLocAdd1"] == DBNull.Value ? null : Convert.ToString(rw["OutLocAdd1"]),
                                                   OutLocAdd2 = rw["OutLocAdd2"] == DBNull.Value ? null : Convert.ToString(rw["OutLocAdd2"]),
                                                   OutLocCityID = rw["OutLocCityID"] == DBNull.Value ? (int?)null : Convert.ToInt32(rw["OutLocCityID"]),
                                                   OutLocCity = rw["OutLocCity"] == DBNull.Value ? null : Convert.ToString(rw["OutLocCity"]),
                                                   OutLocPinCode = rw["OutLocPinCode"] == DBNull.Value ? null : Convert.ToString(rw["OutLocPinCode"]),
                                                   OutLocPhone = rw["OutLocPhone"] == DBNull.Value ? null : Convert.ToString(rw["OutLocPhone"]),
                                               }).FirstOrDefault();
                    }
                }
            }
            catch (DataAccessException ex)
            {
                throw new BussinessLogicException(Properties.Resources.UnExpectedErrorAtBL);
            }
            catch (Exception ex)
            {
                throw new BussinessLogicException(Convert.ToString(LoginID), "WHAssetManagementBL", "GetInOutWHDeliveryAddress", Properties.Resources.UnExpectedErrorAtBL, ex);
            }
            return WHOutAssetMasterObj;
        }
    }
}