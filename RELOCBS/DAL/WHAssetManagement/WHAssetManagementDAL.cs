using Newtonsoft.Json;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace RELOCBS.DAL.WHAssetManagement
{
    public class WHAssetManagementDAL
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

        public DataSet GetWHInOutAssetList(int LoginID, DateTime? FromDate, DateTime? ToDate, string JobID, string RefJobID, string FilterName, string FilterValue)
        {
            DataSet WHInOutAssetDs = null;
            try
            {
                string query = string.Format("EXEC [Warehouse].[GetWHInOutAssetForGrid] @SP_FromDate={0}, @SP_ToDate={1}, @SP_LoginID={2}, @SP_JobID={3}, @SP_RefJobID={4}, @SP_FilterName={5}, @SP_FilterValue={6}",
                    CSubs.QSafeValue(Convert.ToString(FromDate)), CSubs.QSafeValue(Convert.ToString(ToDate)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                    CSubs.QSafeValue(JobID), CSubs.QSafeValue(RefJobID), CSubs.QSafeValue(FilterName), CSubs.QSafeValue(FilterValue));

                WHInOutAssetDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "GetWHInOutAssetList", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return WHInOutAssetDs;
        }

        public DataSet GetAssetInwardDetails(int LoginID, Int64? MoveID, Int64? InMastID = null, Int64? OutMasterID = null)
        {
            DataSet AssetInwardDetailsDs = new DataSet();
            try
            {
                string query = string.Format("[Warehouse].[GetAssetInwardDetails] @SP_LoginID={0}, @SP_MoveID={1}, @SP_InMastID={2}, @SP_OutMasterID={3}",
                    CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(InMastID)), CSubs.QSafeValue(Convert.ToString(OutMasterID)));

                AssetInwardDetailsDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "GetAssetInwardDetails", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return AssetInwardDetailsDs;
        }

        public bool SaveAssetInwardDetails(WHAssetMaster WHAssetMasterObj, int LoginID, out string result)
        {
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string AssetDetailsXmlString = string.Empty;
                        if (!string.IsNullOrEmpty(WHAssetMasterObj.WHInAssetDetailsListHidden))
                        {
                            XNode node = JsonConvert.DeserializeXNode(WHAssetMasterObj.WHInAssetDetailsListHidden, "AssetDetails");
                            AssetDetailsXmlString = node.ToString();
                        }

                        conn.AddCommand("[Warehouse].[AddEditAssetInwardDetails]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InMastID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, WHAssetMasterObj.WHInAssetMaster.InMastID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, WHAssetMasterObj.WHInAssetMaster.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RefJobID", SqlDbType.VarChar, 50, ParameterDirection.Input, WHAssetMasterObj.WHInAssetMaster.RefJobID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_isdirectDel", SqlDbType.Bit, 0, ParameterDirection.Input, WHAssetMasterObj.WHInAssetMaster.IsDirectDel);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WareHouseID", SqlDbType.Int, 0, ParameterDirection.Input, WHAssetMasterObj.WHInAssetMaster.WareHouseID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IndateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, WHAssetMasterObj.WHInAssetMaster.InDateTime);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfPackaage", SqlDbType.Int, 0, ParameterDirection.Input, WHAssetMasterObj.WHInAssetMaster.NoOfPackaage);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Totalvol", SqlDbType.Float, 0, ParameterDirection.Input, WHAssetMasterObj.WHInAssetMaster.TotalVol);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VolUnitID", SqlDbType.Int, 0, ParameterDirection.Input, WHAssetMasterObj.WHInAssetMaster.VolUnitID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Isactive", SqlDbType.Bit, 0, ParameterDirection.Input, WHAssetMasterObj.WHInAssetMaster.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 500, ParameterDirection.Input, WHAssetMasterObj.WHInAssetMaster.Remarks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AssetDetails", SqlDbType.Xml, 0, ParameterDirection.Input, AssetDetailsXmlString);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.VarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                WHAssetMasterObj.WHInAssetMaster.InMastID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_InMastID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "SaveAssetInwardDetails", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool SaveAssetOutwardDetails(WHAssetMaster WHAssetMasterObj, int LoginID, out string result)
        {
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string AssetDetailsXmlString = string.Empty;
                        if (!string.IsNullOrEmpty(WHAssetMasterObj.WHOutAssetDetailsListHidden))
                        {
                            XNode node = JsonConvert.DeserializeXNode(WHAssetMasterObj.WHOutAssetDetailsListHidden, "AssetDetails");
                            AssetDetailsXmlString = node.ToString();
                        }

                        conn.AddCommand("[Warehouse].[AddEditAssetOutwardDetails]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMasterID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, WHAssetMasterObj.WHOutAssetMaster.OutMasterID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, WHAssetMasterObj.WHInAssetMaster.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutDateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.OutDateTime);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutNoPackage", SqlDbType.Int, 0, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.OutNoPackage);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutVolumeUnitID", SqlDbType.Int, 0, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.OutVolumeUnitID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutVolume", SqlDbType.Float, 0, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.OutVolume);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AddressType", SqlDbType.VarChar, 20, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.AddressType);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutLocName", SqlDbType.VarChar, 100, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.OutLocName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutLocAdd1", SqlDbType.VarChar, 500, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.OutLocAdd1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutLocAdd2", SqlDbType.VarChar, 500, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.OutLocAdd2);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutLocCityID", SqlDbType.Int, 0, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.OutLocCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutLocPinCode", SqlDbType.VarChar, 10, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.OutLocPinCode);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutLocPhone", SqlDbType.VarChar, 10, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.OutLocPhone);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutLocContactPerson", SqlDbType.VarChar, 50, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.OutLocContactPerson);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DeliveryProofNo", SqlDbType.VarChar, 20, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.DeliveryProofNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsProofUploaded", SqlDbType.Bit, 0, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.IsProofUploaded);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Isactive", SqlDbType.Bit, 0, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 500, ParameterDirection.Input, WHAssetMasterObj.WHOutAssetMaster.Remarks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AssetDetails", SqlDbType.Xml, 0, ParameterDirection.Input, AssetDetailsXmlString);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.VarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                WHAssetMasterObj.WHOutAssetMaster.OutMasterID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMasterID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "SaveAssetOutwardDetails", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertDocument(WHJobDocUpload WHJobDocUploadObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                string FileName = string.Empty;
                string Extension = string.Empty;
                string FilePath = System.Configuration.ConfigurationManager.AppSettings["WarehouseDMS"].ToString();
                int FileUploadCount = 0;

                if (WHJobDocUploadObj.File != null)
                {
                    using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, IsolationLevel.ReadCommitted))
                    {
                        if (conn.Connect())
                        {
                            conn.AddCommand("[Warehouse].[AddEditMoveDocFileUpload]", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, WHJobDocUploadObj.MoveID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocTypeID", SqlDbType.Int, 0, ParameterDirection.Input, WHJobDocUploadObj.DocTypeID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocNameID", SqlDbType.Int, 0, ParameterDirection.Input, WHJobDocUploadObj.DocNameID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Description", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(WHJobDocUploadObj.DocDescription));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IdentificationID", SqlDbType.BigInt, 0, ParameterDirection.Input, WHJobDocUploadObj.ID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileName", SqlDbType.NVarChar, 250, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Ext", SqlDbType.NVarChar, 50, ParameterDirection.Input);

                            foreach (var file in WHJobDocUploadObj.File)
                            {
                                if (file != null && file.ContentLength > 0)
                                {
                                    FilePath = System.Configuration.ConfigurationManager.AppSettings["JobDMS"].ToString();
                                    FileName = file.FileName;
                                    Extension = Path.GetExtension(file.FileName);
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath", CSubs.PSafeValue(FilePath));
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_FileName", CSubs.PSafeValue(FileName));
                                    conn.SetParameterValue(ParameterOF.PROCEDURE, "@SP_Ext", CSubs.PSafeValue(Extension));
                                    conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                                    if (!conn.IsError)
                                    {
                                        int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                                        result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                                        if (ReturnStatus == 0)
                                        {
                                            FilePath = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath"));
                                            try
                                            {
                                                file.SaveAs(FilePath);
                                                conn.CommitTransaction();
                                                FileUploadCount++;
                                            }
                                            catch (Exception ex)
                                            {
                                                throw new Exception("Unable to save File");
                                            }
                                        }
                                        else
                                        {
                                            conn.RollbackTransaction();
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        conn.RollbackTransaction();
                                        throw new Exception(conn.ErrorMessage);
                                    }
                                }
                            }
                        }
                        else
                            throw new Exception(conn.ErrorMessage);
                    }

                    if (FileUploadCount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        result = "Unable to save files";
                        return false;
                    }
                }
                else
                    throw new Exception("File Not Found");
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "InsertDocument", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool SendLinkWHAssetReport(Int64 MoveID, string EmailTo, string EmailCc, string EmailBcc, string Url, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[Warehouse].[AddEditSendLinkWHAssetReport]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailTo", SqlDbType.VarChar, 1000, ParameterDirection.Input, EmailTo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailCc", SqlDbType.VarChar, 1000, ParameterDirection.Input, EmailCc);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailBcc", SqlDbType.VarChar, 1000, ParameterDirection.Input, EmailBcc);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Url", SqlDbType.NVarChar, 1000, ParameterDirection.Input, Url);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.Int, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.VarChar, 100, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "SendLinkWHAssetReport", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetWHInOutAssetReport(int LoginID, string ReportName, Int64? MoveID, string RefJobID)
        {
            DataSet WHInOutAssetDs = new DataSet();
            try
            {
                string query = string.Format("[Warehouse].[AssetsReports] @SP_ReportName={0}, @SP_MoveID={1}, @SP_RefJobID={2}",
                    CSubs.QSafeValue(ReportName), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(RefJobID));

                WHInOutAssetDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "GetWHInOutAssetReport", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return WHInOutAssetDs;
        }

        public DataSet GetDetailsByAssetDetID(int LoginID, Int64? AssetDetID)
        {
            DataSet AssetDetailsDs = new DataSet();
            try
            {
                string query = string.Format("[Warehouse].[GetDetailsByAssetDetID] @SP_LoginID={0}, @SP_AssetDetID={1}",
                    CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(AssetDetID)));

                AssetDetailsDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "GetDetailsByAssetDetID", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return AssetDetailsDs;
        }

        public DataSet GetAssetLiftVanMapping(int LoginID, Int64? LiftVanID, Int64? MoveID)
        {
            DataSet AssetLiftVanMapDs = new DataSet();
            try
            {
                string query = string.Format("[Warehouse].[GetAssetLiftVanMapping] @SP_LoginID={0}, @SP_LiftVanID={1}, @SP_MoveID={2}",
                    CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(LiftVanID)), CSubs.QSafeValue(Convert.ToString(MoveID)));

                AssetLiftVanMapDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "GetAssetLiftVanMapping", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return AssetLiftVanMapDs;
        }

        public bool SaveAssetLiftVanMapping(WHAssetMaster WHAssetMasterObj, int LoginID, out string result)
        {
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string AssetLiftVanDetails = string.Empty;
                        if (WHAssetMasterObj.WHLocationMap.AssetList != null)
                        {
                            var AssetList = new XElement("AssetLiftVanDetails", WHAssetMasterObj.WHLocationMap.AssetList.Select(x => new XElement("AssetLiftVanDetail", new XElement("AssetDetID", x))));
                            AssetLiftVanDetails = AssetList.ToString();
                        }

                        conn.AddCommand("[Warehouse].[AddEditAssetLiftVanMapping]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, WHAssetMasterObj.WHInAssetMaster.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LiftVanID", SqlDbType.BigInt, 0, ParameterDirection.Input, WHAssetMasterObj.WHLocationMap.LiftVanID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AssetLiftVanDetails", SqlDbType.Xml, 0, ParameterDirection.Input, AssetLiftVanDetails);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.NVarChar, -1, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

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
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "SaveAssetLiftVanMapping", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetBingoChart(int LoginID, Int64? MoveID)
        {
            DataSet BingoChartDs = new DataSet();
            try
            {
                string query = string.Format("[Warehouse].[GetBingoChart] @SP_LoginID={0},@SP_MoveID={1}",
                    CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(MoveID)));

                BingoChartDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "GetBingoChart", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return BingoChartDs;
        }

        public DataSet ValidateBoxNo(int LoginID, Int64? MoveID, Int64? BoxNo)
        {
            DataSet ValidateBoxNoDs = new DataSet();
            try
            {
                string query = string.Format("[Warehouse].[ValidateBoxNo] @SP_LoginID={0}, @SP_MoveID={1}, @SP_BoxNo={2}",
                    CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(BoxNo)));

                ValidateBoxNoDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "ValidateBoxNo", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return ValidateBoxNoDs;
        }

        public DataSet GetInOutWHDeliveryAddress(int LoginID, Int64? MoveID, string AddressType)
        {
            DataSet AddressDs = new DataSet();
            try
            {
                string query = string.Format("[Warehouse].[GetInOutWHDeliveryAddress] @SP_LoginID={0}, @SP_MoveID={1}, @SP_AddressType={2}",
                    CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(AddressType));

                AddressDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WHAssetManagementDAL", "GetInOutWHDeliveryAddress", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return AddressDs;
        }
    }
}