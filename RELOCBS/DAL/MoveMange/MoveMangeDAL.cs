using Newtonsoft.Json;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace RELOCBS.DAL.MoveMange
{
    public class MoveMangeDAL
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

        public DataSet GetCostSheet(int LoginID, Int64? MoveID)
        {
            try
            {
                DataSet dsCostSheet = CSubs.GetDataSet(string.Format("[MoveMan].[GetCostSheet] @SP_MoveID={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(LoginID))));

                if (dsCostSheet != null && dsCostSheet.Tables.Count >= 2)
                {
                    if (dsCostSheet.Tables[0] != null && dsCostSheet.Tables[0].Columns.Contains("ComponentID"))
                        dsCostSheet.Tables[0].Columns.Remove("ComponentID");


                    if (dsCostSheet.Tables[0] != null && dsCostSheet.Tables[0].Columns.Contains("CostHeadID"))
                        dsCostSheet.Tables[0].Columns.Remove("CostHeadID");
                }

                return dsCostSheet;
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetCostSheet", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertMove(MoveManageViewModel SaveRate, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                //using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                //{
                //    if (conn.Connect())
                //    {


                //        System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SaveRate.MoveInstructionMst.HFVMoveInstructionList, "CostHeadwiseDetails");
                //        string QuotingHeadXml = node.ToString();

                //        conn.AddCommand("[MoveMan].[AddEditServiceOrderDetails]", QueryType.Procedure);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                //        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.MoveID <=0 ? -1 : SaveRate.MoveID);

                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.SurveyID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCityID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.FromLocationID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestCityID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.ToLocationID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPortID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.ExitPointID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPortID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.EntryPointID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ModeID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.ModeID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.RMCID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BussLineID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.BusinessLineID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.GoodsDescriptionID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCurrId", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.RateCurrencyID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.CompanyID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.WeightUnitID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightFm", SqlDbType.Float, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.WeightUnitFrom);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightTo", SqlDbType.Float, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.WeightUnitTo);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.RateCompRateWtID);
                //        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompanyRatewtID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.RateCompRateWtID);

                //        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_ShipingLineID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveInstructionMst.ShipingLineID);
                //        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, -1, ParameterDirection.Input, (QuotingHeadXml));
                //        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                //        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                //        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                //        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                //        if (!conn.IsError)
                //        {
                //            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                //            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                //            if (ReturnStatus == 0)
                //            {
                //                SaveRate.MoveID=Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
                //                return true;
                //            }
                //            else
                //            {
                //                return false;
                //            }
                //        }
                //        else
                //            throw new Exception(conn.ErrorMessage);

                //    }
                //    else
                //        throw new Exception(conn.ErrorMessage);
                //}
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertSurvey(MoveManageViewModel SavePacking, int LoginID, out string result)
        {
            result = String.Empty;
            System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SavePacking.SurveySOList.HFSOList, "CostHeadwiseDetails");
            string QuotingHeadXml = node.ToString();
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    //if (conn.Connect())
                    //{

                    //    var PackingSOXML = SavePacking.PackingSOList != null ? new XElement("CostHeadwiseDetails", SavePacking.PackingSOList.Select(x => new XElement("CostHeadwiseDetail", x))) : new XElement("CostHeadwiseDetails");
                    //    var PackingCostXML = SavePacking.PackingSOList != null ? new XElement("CostHeadwiseDetails", SavePacking.PackingCostList.Select(x => new XElement("CostHeadwiseDetail", x))) : new XElement("CostHeadwiseDetails");
                    //    //System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SavePacking.PackingSOList, "CostHeadwiseDetails");
                    //    //string QuotingHeadXml = node.ToString();

                    //    conn.AddCommand("[MoveMan].[AddEditOrgInfo]", QueryType.Procedure);
                    //    //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingDetail.MoveID <= 0 ? -1 : SavePacking.PackingDetail.MoveID);

                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAgentID ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingDetail.OrgAgentID);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FrtAgentID ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingDetail.FrtAgentID);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAgentID ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingDetail.DestAgentID);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ExitPortID ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingDetail.ExitPortID);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntryPortID ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingDetail.EntryPortID);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackDate ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.Packdate);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoadDate ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.Loaddate);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VolumeUnitID ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.VolumeUnitID);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TobePackedVol ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.TobePackedVol);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NetVol ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.NetVol);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GrossVol ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.GrossVol);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightUnitID ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.WeightUnitID);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NetWt ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.NetWt);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GrossWt ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.GrossWt);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ACWTWt ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.ACWTWt);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LooseCased ", SqlDbType.BigInt, 0, ParameterDirection.Input, null);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DensityFact ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.DensityFact);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LCLorFCL ", SqlDbType.BigInt, 0, ParameterDirection.Input, null);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContainerSizeID ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.ContainerSizeID);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfPacks ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.NoOfPacks);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_noOfCrates ", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingReport.NoOfCrates);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo ", SqlDbType.BigInt, 0, ParameterDirection.Input, PackingCostXML.HasElements ? Convert.ToString(PackingCostXML) : null);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseServiceOrd ", SqlDbType.BigInt, 0, ParameterDirection.Input, PackingSOXML.HasElements ? Convert.ToString(PackingSOXML) : null);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID ", SqlDbType.BigInt, 0, ParameterDirection.Input, LoginID);



                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                    //    conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                    //    conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                    //    if (!conn.IsError)
                    //    {
                    //        int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                    //        result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                    //        if (ReturnStatus == 0)
                    //        {
                    //            //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
                    //            return true;
                    //        }
                    //        else
                    //        {
                    //            return false;
                    //        }
                    //    }
                    //    else
                    //        throw new Exception(conn.ErrorMessage);

                    //}
                    //else
                    //    throw new Exception(conn.ErrorMessage);
                }
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertPacking", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertPacking(MoveManageViewModel SavePacking, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string QuotingHeadXml1 = string.Empty;
                        string QuotingHeadXml = string.Empty;
                        /*var PackingSOXML = SavePacking.PackingSOList != null ? new XElement("CostHeadwiseDetails", SavePacking.PackingSOList.Select(x => new XElement("CostHeadwiseDetail", x))) : new XElement("CostHeadwiseDetails");
                        var PackingCostXML = SavePacking.PackingSOList != null ? new XElement("CostHeadwiseDetails", SavePacking.PackingCostList.Select(x => new XElement("CostHeadwiseDetail", x))) : new XElement("CostHeadwiseDetails");*/

                        if (SavePacking.PackingCostList.HFCostList != null && SavePacking.PackingCostList.HFCostList.Length > 0)
                        {
                            System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SavePacking.PackingCostList.HFCostList, "CostHeadwiseDetails");
                            QuotingHeadXml1 = node.ToString();
                        }
                        if (SavePacking.PackingSOList.HFSOList != null && SavePacking.PackingSOList.HFSOList.Length > 0)
                        {
                            System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SavePacking.PackingSOList.HFSOList, "CostHeadwiseDetails");
                            QuotingHeadXml = node.ToString();
                        }

                        conn.AddCommand("[MoveMan].[AddEditOrgInfo]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.MoveID <= 0 ? -1 : SavePacking.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAgentID", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingDetail.OrgAgentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FrtAgentID", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.MoveJob.ModeID == 3 && SavePacking.IsDTD ? SavePacking.PackingDetail.OrgAgentID : SavePacking.PackingDetail.FrtAgentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAgentID", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.MoveJob.ModeID == 3 && SavePacking.IsDTD ? SavePacking.PackingDetail.OrgAgentID : SavePacking.PackingDetail.DestAgentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ExitPortID", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingDetail.ExitPortID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntryPortID", SqlDbType.BigInt, 0, ParameterDirection.Input, SavePacking.PackingDetail.EntryPortID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SavePacking.PackingReport.Packdate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SchPackDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SavePacking.PackingReport.ScheduledPackDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoadDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SavePacking.PackingReport.Loaddate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VolumeUnitID", SqlDbType.Int, 0, ParameterDirection.Input, SavePacking.PackingReport.VolumeUnitID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TobePackedVol", SqlDbType.Float, 0, ParameterDirection.Input, SavePacking.PackingReport.TobePackedVol);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NetVol", SqlDbType.Float, 0, ParameterDirection.Input, SavePacking.PackingReport.NetVol);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GrossVol", SqlDbType.Float, 0, ParameterDirection.Input, SavePacking.PackingReport.GrossVol);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, SavePacking.PackingReport.WeightUnitID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NetWt", SqlDbType.Float, 0, ParameterDirection.Input, SavePacking.PackingReport.NetWt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GrossWt", SqlDbType.Float, 0, ParameterDirection.Input, SavePacking.PackingReport.GrossWt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ACWTWt", SqlDbType.Float, 0, ParameterDirection.Input, SavePacking.PackingReport.ACWTWt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LooseCased", SqlDbType.VarChar, 8, ParameterDirection.Input, SavePacking.PackingReport.LooseCased);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DensityFact", SqlDbType.Float, 0, ParameterDirection.Input, SavePacking.PackingReport.DensityFact);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LCLorFCL", SqlDbType.VarChar, 3, ParameterDirection.Input, SavePacking.PackingReport.LCLorFCL);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContainerSizeID", SqlDbType.Int, 0, ParameterDirection.Input, SavePacking.PackingReport.ContainerSizeID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfPacks", SqlDbType.Int, 0, ParameterDirection.Input, SavePacking.PackingReport.NoOfPacks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_noOfCrates", SqlDbType.Int, 0, ParameterDirection.Input, SavePacking.PackingReport.NoOfCrates);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgStgStartDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SavePacking.PackingReport.OrgStgStartDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgStgEndDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SavePacking.PackingReport.OrgStgEndDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, 0, ParameterDirection.Input, QuotingHeadXml1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseServiceOrd", SqlDbType.Xml, 0, ParameterDirection.Input, QuotingHeadXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.BigInt, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RoadKMS", SqlDbType.Float, 0, ParameterDirection.Input, SavePacking.PackingReport.RoadKMS);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsGPApproved", SqlDbType.Bit, 0, ParameterDirection.Input, SavePacking.IsGPApproved);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StageRemark", SqlDbType.VarChar, -1, ParameterDirection.Input, SavePacking.PackingReport.StageRemarks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertPacking", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertFreight(MoveManageViewModel SaveFreight, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string QuotingHeadXml1 = string.Empty;
                        string QuotingHeadXml = string.Empty;
                        string TransitWtVolInfoXML = string.Empty;
                        string InvDetailsXml = string.Empty;
                        string JobIDDetailsXml = string.Empty;
                        if (SaveFreight.FreightCostList.HFCostList != null && SaveFreight.FreightCostList.HFCostList.Length > 0)
                        {
                            System.Xml.Linq.XNode node1 = JsonConvert.DeserializeXNode(SaveFreight.FreightCostList.HFCostList, "CostHeadwiseDetails");
                            QuotingHeadXml1 = node1.ToString();
                        }
                        if (SaveFreight.FreightSOList.HFSOList != null && SaveFreight.FreightSOList.HFSOList.Length > 0)
                        {

                            System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SaveFreight.FreightSOList.HFSOList, "CostHeadwiseDetails");
                            QuotingHeadXml = node.ToString();
                        }

                        XmlDocument node2 = JsonConvert.DeserializeXmlNode(SaveFreight.FreightReport.HFTransitList, "TransitInfos");
                        XDocument xdoc = XDocument.Load(new XmlNodeReader(node2));
                        string QuotingHeadXml2 = xdoc.ToString();
                        if (SaveFreight.FreightReport.HFTransitWtVolList != null && SaveFreight.FreightReport.HFTransitWtVolList.Length > 0)
                        {
                            XmlDocument node3 = JsonConvert.DeserializeXmlNode(SaveFreight.FreightReport.HFTransitWtVolList, "TransitInfos");
                            XDocument xdoc3 = XDocument.Load(new XmlNodeReader(node3));
                            TransitWtVolInfoXML = xdoc3.ToString();
                        }

                        if (SaveFreight.FreightReport.HFTransitInvoiceList != null && SaveFreight.FreightReport.HFTransitInvoiceList.Length > 0)
                        {

                            XmlDocument node4 = JsonConvert.DeserializeXmlNode(SaveFreight.FreightReport.HFTransitInvoiceList, "InvDetails");
                            XDocument xdoc4 = XDocument.Load(new XmlNodeReader(node4));
                            //System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SaveFreight.FreightReport.HFTransitInvoiceList, "InvDetails");
                            InvDetailsXml = xdoc4.ToString();
                        }
                        if (SaveFreight.FreightReport.HFTransitInvJobList != null && SaveFreight.FreightReport.HFTransitInvJobList.Length > 0)
                        {
                            XmlDocument node5 = JsonConvert.DeserializeXmlNode(SaveFreight.FreightReport.HFTransitInvJobList, "JobIDDetails");
                            XDocument xdoc5 = XDocument.Load(new XmlNodeReader(node5));
                            //System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SaveFreight.FreightReport.HFTransitInvJobList, "JobIDDetails");
                            JobIDDetailsXml = xdoc5.ToString();
                        }
                        conn.AddCommand("[MoveMan].[AddEditFrtInfo]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveFreight.MoveID <= 0 ? -1 : SaveFreight.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAgentID", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightDetail.OrgAgentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FrtAgentID", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.MoveJob.ModeID == 3 && SaveFreight.IsDTD ? SaveFreight.FreightDetail.OrgAgentID : SaveFreight.FreightDetail.FrtAgentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAgentID", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.MoveJob.ModeID == 3 && SaveFreight.IsDTD ? SaveFreight.FreightDetail.OrgAgentID : SaveFreight.FreightDetail.DestAgentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransitAgentID", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.TransitAgent);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ExitPortID", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightDetail.ExitPortID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntryPortID", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightDetail.EntryPortID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PortLoadID", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightReport.PortLoad);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PortDischargeID", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightReport.PortDischarge);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipmentCartedOn", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveFreight.FreightReport.ShipmentCartedOn);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CustomeClearedOn", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveFreight.FreightReport.CustomeClearedOn);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SB_GivenOn", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveFreight.FreightReport.SB_GivenOn);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BLSentToAgentOn", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveFreight.FreightReport.BLSentToAgentOn);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContainerNo", SqlDbType.VarChar, 30, ParameterDirection.Input, SaveFreight.FreightReport.ContainerNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SealNo", SqlDbType.VarChar, 30, ParameterDirection.Input, SaveFreight.FreightReport.SealNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillNo", SqlDbType.VarChar, 30, ParameterDirection.Input, SaveFreight.FreightReport.Bill_No);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BL_ReleasedOn", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveFreight.FreightReport.BLReleaseOn);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipDespDateSDs", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveFreight.FreightReport.SD);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipDespDateOPs", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveFreight.FreightReport.OPS);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SchAgent", SqlDbType.VarChar, 50, ParameterDirection.Input, SaveFreight.FreightReport.SSAgent);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FsOrDs", SqlDbType.VarChar, 5, ParameterDirection.Input, SaveFreight.FreightReport.FS_DS);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ISFNumber", SqlDbType.VarChar, 20, ParameterDirection.Input, SaveFreight.FreightReport.ISF_Ref);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LCLorFCL", SqlDbType.VarChar, 5, ParameterDirection.Input, SaveFreight.FreightReport.LCL_FCL);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Airlines", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightReport.AirLine);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Currier", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightReport.Courier);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TruckNo", SqlDbType.VarChar, 20, ParameterDirection.Input, SaveFreight.FreightReport.TruckNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehTypeID", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightReport.VehicleTypeId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TotCap", SqlDbType.Float, 0, ParameterDirection.Input, SaveFreight.FreightReport.TotalCapacity);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfPacks", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightReport.NoOfPacks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransitShipment", SqlDbType.Bit, 0, ParameterDirection.Input, SaveFreight.FreightReport.TransitShipment);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, 0, ParameterDirection.Input, QuotingHeadXml1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseServiceOrd", SqlDbType.Xml, 0, ParameterDirection.Input, QuotingHeadXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransitInfo", SqlDbType.Xml, 0, ParameterDirection.Input, QuotingHeadXml2);

                        //new changes
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_HFTransitList", SqlDbType.Xml, 0, ParameterDirection.Input, TransitWtVolInfoXML);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsISF", SqlDbType.Bit, 0, ParameterDirection.Input, SaveFreight.FreightReport.IsISF);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsShipDespDateSDs", SqlDbType.Bit, 0, ParameterDirection.Input, SaveFreight.FreightReport.IsSD);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsBLSentToAgent", SqlDbType.Bit, 0, ParameterDirection.Input, SaveFreight.FreightReport.IsBLSentToAgent);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsDirectCarting", SqlDbType.Bit, 0, ParameterDirection.Input, SaveFreight.FreightReport.IsDirectCarting);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ForwardingBrID", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightReport.ForwardingBr);

                        ////new changes
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FCL20", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightReport.FCL_20);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FCL40", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightReport.FCL_40);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FCLHC40", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightReport.FCLHC_40);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_THCPrepaid", SqlDbType.Bit, 0, ParameterDirection.Input, SaveFreight.FreightReport.THCPrepaid);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_THCCollect", SqlDbType.Bit, 0, ParameterDirection.Input, SaveFreight.FreightReport.THCCollect);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SSLAgentID", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightReport.SSLAgentId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CarrierID", SqlDbType.Int, 0, ParameterDirection.Input, SaveFreight.FreightReport.SSLCarrierId);

                        ///new chagnes 
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransInvMasterID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveFreight.FreightReport.TransInvMasterID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Frt_TranshipmentInvDetails", SqlDbType.Xml, -1, ParameterDirection.Input, InvDetailsXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Frt_TranshipInv_Job_Mapping", SqlDbType.Xml, -1, ParameterDirection.Input, JobIDDetailsXml);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID ", SqlDbType.BigInt, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SchDeliveryDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveFreight.FreightReport.DeliveryDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertFreight", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertDelivery(MoveManageViewModel SaveDelivery, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string QuotingHeadXml1 = string.Empty;
                        string QuotingHeadXml = string.Empty;
                        /*var DeliverySOXML = SaveDelivery.DeliverySOList != null ? new XElement("CostHeadwiseDetails", SaveDelivery.DeliverySOList.Select(x => new XElement("CostHeadwiseDetail", x))) : new XElement("CostHeadwiseDetails");
                        var DeliveryCostXML = SaveDelivery.DeliverySOList != null ? new XElement("CostHeadwiseDetails", SaveDelivery.DeliveryCostList.Select(x => new XElement("CostHeadwiseDetail", x))) : new XElement("CostHeadwiseDetails");*/




                        if (SaveDelivery.DeliveryCostList.HFCostList != null && SaveDelivery.DeliveryCostList.HFCostList.Length > 0)
                        {
                            System.Xml.Linq.XNode node1 = JsonConvert.DeserializeXNode(SaveDelivery.DeliveryCostList.HFCostList, "CostHeadwiseDetails");
                            QuotingHeadXml1 = node1.ToString();

                        }
                        if (SaveDelivery.DeliverySOList.HFSOList != null && SaveDelivery.DeliverySOList.HFSOList.Length > 0)
                        {
                            System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SaveDelivery.DeliverySOList.HFSOList, "CostHeadwiseDetails");
                            QuotingHeadXml = node.ToString();
                        }

                        conn.AddCommand("[MoveMan].[AddEditDestInfo]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveDelivery.MoveID <= 0 ? -1 : SaveDelivery.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAgentID", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.DeliveryDetail.OrgAgentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FrtAgentID", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.MoveJob.ModeID == 3 && SaveDelivery.IsDTD ? SaveDelivery.DeliveryDetail.OrgAgentID : SaveDelivery.DeliveryDetail.FrtAgentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAgentID", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.MoveJob.ModeID == 3 && SaveDelivery.IsDTD ? SaveDelivery.DeliveryDetail.OrgAgentID : SaveDelivery.DeliveryDetail.DestAgentID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ExitPortID", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.DeliveryDetail.ExitPortID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntryPortID", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.DeliveryDetail.EntryPortID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DeliveryDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.DeliveryDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VolumeUnitID", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.VolumeUnitID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TobePackedVol", SqlDbType.Float, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.TobePackedVol);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NetVol", SqlDbType.Float, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.NetVol);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GrossVol", SqlDbType.Float, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.GrossVol);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.WeightUnitID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NetWt", SqlDbType.Float, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.NetWt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GrossWt", SqlDbType.Float, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.GrossWt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ACWTWt", SqlDbType.Float, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.ACWTWt);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LooseCased", SqlDbType.VarChar, 8, ParameterDirection.Input, SaveDelivery.DeliveryReport.LooseCased);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DensityFact", SqlDbType.Float, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.DensityFact);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LCLorFCL", SqlDbType.VarChar, 3, ParameterDirection.Input, SaveDelivery.DeliveryReport.LCLorFCL);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContainerSizeID", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.ContainerSizeID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfPacks", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.NoOfPacks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_noOfCrates", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.NoOfCrates);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsClaim", SqlDbType.Bit, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.IsClaim);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsServiceEvaluation", SqlDbType.Bit, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.ServiceEvaluation);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceEvaluationScore", SqlDbType.Float, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.ServiceEvaluationScore);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceEvaluationRemark", SqlDbType.VarChar, 100, ParameterDirection.Input, SaveDelivery.DeliveryReport.ServiceEvaluationRemarks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PassportNo", SqlDbType.VarChar, 50, ParameterDirection.Input, SaveDelivery.DeliveryReport.PassportNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestStgStartDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.DestStgStartDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestStgEndDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.DestStgEndDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, 0, ParameterDirection.Input, QuotingHeadXml1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseServiceOrd", SqlDbType.Xml, 0, ParameterDirection.Input, QuotingHeadXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RoadKMS", SqlDbType.Float, 0, ParameterDirection.Input, SaveDelivery.DeliveryReport.RoadKMS);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsGPApproved", SqlDbType.Bit, 0, ParameterDirection.Input, SaveDelivery.IsGPApproved);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StageRemark", SqlDbType.VarChar, -1, ParameterDirection.Input, SaveDelivery.DeliveryReport.StageRemarks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertDelivery", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool ApproveSurvey(MoveManageViewModel SaveDelivery, bool IsApprove, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[MoveMan].[ApproveSurveyCostSheet]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveDelivery.MoveID <= 0 ? -1 : SaveDelivery.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRemoveApproval", SqlDbType.Bit, 0, ParameterDirection.Input, !IsApprove);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSenttoApproval", SqlDbType.Bit, 0, ParameterDirection.Input, SaveDelivery.SurveyReport.IsCSSenttoApprove);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ApprovalUser", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.SurveyReport.CSSenttoApproveUser);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "ApproveSurvey", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool ApprovePacking(MoveManageViewModel SaveDelivery, bool IsApprove, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[MoveMan].[ApprovePackingCostSheet]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveDelivery.MoveID <= 0 ? -1 : SaveDelivery.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRemoveApproval", SqlDbType.Bit, 0, ParameterDirection.Input, !IsApprove);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSenttoApproval", SqlDbType.Bit, 0, ParameterDirection.Input, SaveDelivery.PackingReport.IsCSSenttoApprove);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ApprovalUser", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.PackingReport.CSSenttoApproveUser);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "ApproveDelivery", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool ApproveDelivery(MoveManageViewModel SaveDelivery, bool IsApprove, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[MoveMan].[ApproveCostSheet]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveDelivery.MoveID <= 0 ? -1 : SaveDelivery.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRemoveApproval", SqlDbType.Bit, 0, ParameterDirection.Input, !IsApprove);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSenttoApproval", SqlDbType.Bit, 0, ParameterDirection.Input, SaveDelivery.IsCSSenttoApprove);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ApprovalUser", SqlDbType.Int, 0, ParameterDirection.Input, SaveDelivery.CSSenttoApproveUser);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "ApproveDelivery", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertMoveCost(MoveManageViewModel SaveRate, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SaveRate.MoveCostMst.HFVMoveCostHeadList, "CostHeadwiseDetails");
                        string QuotingHeadXml = node.ToString();
                        conn.AddCommand("[MoveMan].[AddEditMoveCost]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.MoveID <= 0 ? -1 : SaveRate.MoveID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.SurveyID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCityID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveCostMst.FromLocationID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestCityID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveCostMst.ToLocationID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPortID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveCostMst.FromLocationID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPortID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveCostMst.ToLocationID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ModeID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveCostMst.ModeID);

                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveCostMst.RMCID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BussLineID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveCostMst.BusinessLineID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveCostMst.GoodsDescriptionID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCurrId", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveCostMst.RateCurrencyID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.CompanyID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveCostMst.WeightUnitID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightFm", SqlDbType.Float, 0, ParameterDirection.Input, SaveRate.MoveCostMst.WeightUnitFrom);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightTo", SqlDbType.Float, 0, ParameterDirection.Input, SaveRate.MoveCostMst.WeightUnitTo);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.RateCompRateWtID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompanyRatewtID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.RateCompRateWtID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_ShipingLineID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveCostMst.ShipingLineID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, -1, ParameterDirection.Input, (QuotingHeadXml));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertMoveCost", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertJobOpening(MoveManageViewModel SaveRate, int LoginID, XElement charges, out string result)
        {
            result = String.Empty;

            try
            {
                System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(SaveRate.MoveJob.HFVMoveRateCompList, "CostHeadwiseDetails");
                string QuotingHeadXml = node.ToString();
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        //int Nationality = string.IsNullOrEmpty(SaveRate.MoveJob.Shipper.Nationality) ? 0 : Convert.ToInt32(SaveRate.MoveJob.Shipper.Nationality);
                        conn.AddCommand("[MoveMan].[AddEditNewJobDetails]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, SaveRate.MoveID <= 0 ? -1 : SaveRate.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.SurveyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AccountID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.AccountId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClientId", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.ClientId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperTitle", SqlDbType.VarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.Shipper.Title));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperFName", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.Shipper.ShipperFName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperLName", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.Shipper.ShipperLName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 200, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.Shipper.Address1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 200, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.Shipper.Address2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Email", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.Shipper.Email));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AddressCityId", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.Shipper.AddressCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Pin", SqlDbType.VarChar, 10, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.Shipper.PIN));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone1", SqlDbType.VarChar, 15, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.Shipper.Phone1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Phone2", SqlDbType.VarChar, 15, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.Shipper.Phone2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipCategoryID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.Shipper.ShipCategoryID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperDOB", SqlDbType.Date, 0, ParameterDirection.Input, SaveRate.MoveJob.Shipper.DOB);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperDesig", SqlDbType.VarChar, 25, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.Shipper.Designation));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperNationality", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.Shipper.Nationality));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCityID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.FromLocationID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestCityID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.ToLocationID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPortID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.ExitPointID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPortID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.EntryPointID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ModeID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.ModeID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_ShipingLineID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.ShipingLineID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_RMCID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.RMCID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BussLineID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.BusinessLineID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GoodsDescID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.GoodsDescriptionID);
                        ///Not used variable in sp
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCurrId", SqlDbType.Int, 0, ParameterDirection.Input, 1);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.WeightUnitID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightFm", SqlDbType.Float, 0, ParameterDirection.Input, SaveRate.MoveJob.WeightUnitFrom);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightTo", SqlDbType.Float, 0, ParameterDirection.Input, SaveRate.MoveJob.WeightUnitTo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, 0, ParameterDirection.Input, QuotingHeadXml);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        ///Not used variable in sp
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompRateID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.RateCompRateWtID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCompanyRatewtID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.RateCompRateWtID);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RMCFileNo", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.RMCFileNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WorkOrderNo", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.WKNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobAssignTo", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.MoveCoordinatorID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobAssistTo", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.AssistingMoveCoordinatorID);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAdd1", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.OrgAdd));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAdd2", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.OrgAdd2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AddOrgCityID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.OrgCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPhone", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.OrgPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgEmail", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.OrgEmail));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPin", SqlDbType.VarChar, 15, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.OrgPin));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAdd1", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.DestAdd));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAdd2", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.DestAdd2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AddDestCityID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.DestCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPhone", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.DestPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestEmail", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.DestEmail));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPin", SqlDbType.VarChar, 15, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.MoveJob.DestPin));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContainerTypeID ", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.ContainerTypeID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GenerateJobNo", SqlDbType.Bit, 0, ParameterDirection.Input, string.IsNullOrEmpty(SaveRate.JobNo) ? true : false);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FixedCostWithID", SqlDbType.Xml, 0, ParameterDirection.Input, charges.HasElements ? Convert.ToString(charges) : null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OldCBSJobNo", SqlDbType.VarChar, 20, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.OldJobNo));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsurBy", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.InsurBy);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_HoSdEmpID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.HoSdEmpID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BrSdEmpID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.BrSdEmpID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestBrSdEmpID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.DestBrSdEmpID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRmcBuss", SqlDbType.Bit, 0, ParameterDirection.Input, SaveRate.RMCBuss);
                        ////For Private Client below fields need to be Compulsary for Instruction sheet creation (Cheque_No,Cheque_Amt)
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChequeNo", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.Cheque_No));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChequeAmt", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.Cheque_Amt));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChequeRemark", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.Cheque_Remark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TentativeMoveDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SaveRate.MoveJob.TentativeMoveDate);
                        //Insurance Details
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsContactPerson", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.insuranceDetail.ContactPerson));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsContactNumber", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.insuranceDetail.ContactNumber));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsEmailID", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.insuranceDetail.EmailID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsFinancePerson", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.insuranceDetail.FinancePerson));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsuranceValueAmount", SqlDbType.Decimal, 0, ParameterDirection.Input, SaveRate.insuranceDetail.InsuranceValueAmount);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsuranceValueCurrency", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.insuranceDetail.InsuranceValueCurrency);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsuranceBreakdown", SqlDbType.Bit, 0, ParameterDirection.Input, SaveRate.insuranceDetail.InsuranceBreakdown);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BreakdownInsuranceDMS", SqlDbType.Bit, 0, ParameterDirection.Input, SaveRate.insuranceDetail.BreakdownInsurance);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FinancePerson", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.MoveJob.FinancePerson);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgWarehouse", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.OrgWarehouse);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestWarehouse", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.DestWarehouse);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsOutSourced", SqlDbType.Bit, 0, ParameterDirection.Input, SaveRate.IsOutSourced);

                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertJobOpening", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


        }

        public IQueryable<ExportSunJob> GetForSunGrid(int LoginID, DateTime? FromDate, DateTime? ToDate, bool IsWOS = false)
        {

            try
            {
                /*if (RELOCBS.Common.CommonService.RMCBuss == UserSession.GetUserSession().BussinessLine)
                {

                }*/


                string query = string.Format("exec [MoveMan].[GetJobForSun]  @SP_LoginID={0},@SP_FromDate={1},@SP_ToDate={2},@SP_CompId={3},@SP_IsWOS={4}",
                Convert.ToString(LoginID), CSubs.QSafeValue(Convert.ToString(FromDate)), CSubs.QSafeValue(Convert.ToString(ToDate)),
                CSubs.QSafeValue(UserSession.GetUserSession().CompanyID.ToString()), CSubs.QSafeValue(Convert.ToString(IsWOS)));

                DataTable dataTable = CSubs.GetDataTable(query);
                var result = new List<ExportSunJob>();
                if (dataTable != null)
                {
                    result = (from rw in dataTable.AsEnumerable()
                              select new ExportSunJob()
                              {
                                  Layout = Convert.ToString(rw["Layout"]),
                                  AnalysisDimensionID = Convert.ToString(rw["Analysis Dimension ID"]),
                                  AnalysisDimension = Convert.ToString(rw["Analysis Dimension"]),
                                  AnalysisCode = Convert.ToString(rw["Analysis Code"]),
                                  Description = Convert.ToString(rw["Description"]),
                                  LookupCode = Convert.ToString(rw["Lookup Code"]),
                                  Budgetcheck = Convert.ToString(rw["Budget check"]),
                                  Budgetstop = Convert.ToString(rw["Budget stop"]),
                                  Prohibitposting = Convert.ToString(rw["Prohibit posting"]),
                                  Navigationmethod = Convert.ToString(rw["Navigation method"]),
                                  Combinedbudgetcheck = Convert.ToString(rw["Combined budget check"]),
                                  DAG = Convert.ToString(rw["DAG"]),
                                  OriginCity = Convert.ToString(rw["Origin City"]),
                                  CorporateGroup = Convert.ToString(rw["Corporate Group"]),
                                  Agentrefno = Convert.ToString(rw["Agent ref no"]),
                                  Mode = Convert.ToString(rw["Mode"]),
                                  Jobdate = Convert.ToString(rw["Job date"]),
                                  Controller = Convert.ToString(rw["Controller"]),
                                  DestinationCity = Convert.ToString(rw["Destination City"]),
                              }).ToList();
                }


                IQueryable<ExportSunJob> data = result.AsQueryable<ExportSunJob>();

                return data;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public IQueryable<MoveManageGrid> GetForGrid(int LoginID, string search, string searchtype, string Shipper, bool RMCBuss)
        {

            try
            {
                string query = string.Format("exec [MoveMan].[GetMoveSurveyForGrid]  @SP_LoginID={0},@SP_FilterValue={1},@SP_FilterName={2},@SP_IsRMCBuss={3},@SP_Shipper={4},@SP_CompId={5}",
                Convert.ToString(LoginID), CSubs.QSafeValue(search), CSubs.QSafeValue(searchtype), RMCBuss, CSubs.QSafeValue(Shipper), CSubs.QSafeValue(UserSession.GetUserSession().CompanyID.ToString()));

                DataTable dataTable = CSubs.GetDataTable(query);
                var result = new List<MoveManageGrid>();
                if (dataTable != null)
                {
                    result = (from rw in dataTable.AsEnumerable()
                              select new MoveManageGrid()
                              {
                                  SurveyId = Convert.ToInt64(rw["SurveyID"]),
                                  JobNo = Convert.ToString(rw["JobId"]),
                                  //ratecompanyratewtid = Convert.ToInt64(rw["ratecompanyratewtid"]),
                                  /*FromCity = Convert.ToString(rw["FromCity"]),
                                  ToCity = Convert.ToString(rw["ToCity"]),
                                  EntryPort = Convert.ToString(rw["EntryPort"]),
                                  Exitport = Convert.ToString(rw["Exitport"]),
                                  TotEstimate = Convert.ToInt64(rw["TotEstimate"]),
                                  WeightFrom = Convert.ToInt64(rw["WeightFrom"]),
                                  WeightTo = Convert.ToInt64(rw["WeightTo"]),*/
                                  BussLineName = Convert.ToString(rw["BussLineName"]),
                                  IsRMCBuss = RMCBuss,
                                  ShipperName = Convert.ToString(rw["ShipperName"]),
                                  SurveyDate = rw["SurveyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["SurveyDate"]),
                                  SurveyDateTime = rw["SurveryTime"] == DBNull.Value ? (TimeSpan?)null : (TimeSpan?)rw["SurveryTime"],
                                  EnqDate = rw["EnqDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EnqDate"]),
                                  EnqDetailId = Convert.ToInt64(rw["EnqDetailID"]),
                                  EnqId = Convert.ToInt64(rw["EnqID"]),
                                  MoveId = Convert.ToInt64(rw["MoveID"]),
                                  BillCollId = Convert.ToInt64(rw["BillCollId"]),
                                  AccountName = Convert.ToString(rw["AccountName"]),
                                  AgentName = Convert.ToString(rw["AgentName"]),
                                  Mode = Convert.ToString(rw["Mode"]),
                                  JobOpenedDate = rw["JobOpenedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["JobOpenedDate"]),
                                  ServiceLine = Convert.ToString(rw["ServiceLine"]),
                                  EnqNo = Convert.ToString(rw["EnqNo"]),
                                  EnqSeqID = Convert.ToInt32(rw["EnqDetSequenceID"]),
                                  JobStatus = Convert.ToString(rw["JobStatus"]),
                              }).ToList();
                }


                IQueryable<MoveManageGrid> data = result.AsQueryable<MoveManageGrid>();

                return data;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetForGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetDetailById(int LoginID, int? SurveyID, int? MoveID, int RMCBuss, bool IsSurveyGetCost, bool IsPackingGetCost, bool IsDeliveryGetCost)
        {
            DataSet Dtobj = new DataSet();
            try
            {
                string query = string.Format("exec [MoveMan].[GetMoveDetailsForDisplay] @SP_SurveyID={0},@SP_MoveID={1},@SP_LoginID={2},@SP_IsRMCBuss={3},@SP_IsSurveyGetCost={4}, @SP_IsPackGetCost={5}, @SP_IsDeliveryGetCost={6}",
                 CSubs.QSafeValue(Convert.ToString(SurveyID)), CSubs.QSafeValue(Convert.ToString(MoveID))
                 , CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(RMCBuss))
                 , CSubs.QSafeValue(Convert.ToString(IsSurveyGetCost)), CSubs.QSafeValue(Convert.ToString(IsPackingGetCost))
                 , CSubs.QSafeValue(Convert.ToString(IsDeliveryGetCost)));

                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


            return Dtobj;
        }

        public DataTable GetAgentGridCombo(int RateCompID, int SurveyID)
        {

            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("exec [MoveMan].[ForCombo_AgentListWithRate] @SP_RateCompID={0},@SP_Loginid={1},@SP_SurveyID={2},@SP_CompID={3}",
                 CSubs.QSafeValue(Convert.ToString(RateCompID)), CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().LoginID))
                 , CSubs.QSafeValue(Convert.ToString(SurveyID))
                 , CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID)));

                Dtobj = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveMangeDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


            return Dtobj;
        }

        public DataTable GetCost(int AgentId, int CityId, int RMCId, int GoodsDescId, DateTime JobDate, decimal ConversionRate, int CostHeadId)
        {
            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("exec [Comm].[GETCityCostForServices] @SP_AgentID={0},@SP_CityID={1},@SP_RMCId={2}," +
                    "@SP_CompanyID={3},@SP_GoodsDescID={4},@SP_CostHeadID={5},@SP_isActive={6},@SP_JobDate={7}," +
                    "@SP_ConversionRate={8},@SP_LoginID={9}",
                 CSubs.QSafeValue(Convert.ToString(AgentId)), CSubs.QSafeValue(Convert.ToString(CityId))
                 , CSubs.QSafeValue(Convert.ToString(RMCId)), CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID))
                 , CSubs.QSafeValue(Convert.ToString(GoodsDescId)), CSubs.QSafeValue(Convert.ToString(CostHeadId))
                 , 1, CSubs.QSafeValue(Convert.ToString(JobDate)), CSubs.QSafeValue(Convert.ToString(ConversionRate))
                 , CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().LoginID)));

                Dtobj = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveMangeDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }


            return Dtobj;
        }

        public bool InsertDocument(MoveManageViewModel SaveRate, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                string FileName = string.Empty;
                string Extension = string.Empty;
                string FilePath = System.Configuration.ConfigurationManager.AppSettings["JobDMS"].ToString();
                int FileUploadCount = 0;

                if (SaveRate.jobDocUpload.file != null)
                {
                    using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, System.Data.IsolationLevel.ReadCommitted))
                    {
                        if (conn.Connect())
                        {

                            conn.AddCommand("[MoveMan].[AddEditMoveDocFileUpload]", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.MoveID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocTypeID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.jobDocUpload.DocTypeID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocNameID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.jobDocUpload.DocNameID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Description", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.jobDocUpload.DocDescription));

                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IdentificationID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.jobDocUpload.ID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileName", SqlDbType.NVarChar, 250, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Ext", SqlDbType.NVarChar, 50, ParameterDirection.Input);

                            foreach (var file in SaveRate.jobDocUpload.file)
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
                                            //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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

                        result = "Uable to save files";
                        return false;
                    }


                }
                else
                    throw new Exception("File Not Found");


            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }


        public bool InsertGCCDocument(MoveManageViewModel SaveRate, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                string FileName = string.Empty;
                string Extension = string.Empty;
                string FilePath = System.Configuration.ConfigurationManager.AppSettings["GCCDMS"].ToString();
                int FileUploadCount = 0;

                if (SaveRate.jobDocUpload.file != null)
                {
                    using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, System.Data.IsolationLevel.ReadCommitted))
                    {
                        if (conn.Connect())
                        {

                            conn.AddCommand("[MoveMan].[AddEditGCCInsuranceDetail]", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.MoveID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocTypeID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.jobDocUpload.DocTypeID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocNameID", SqlDbType.Int, 0, ParameterDirection.Input, SaveRate.jobDocUpload.DocNameID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Description", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(SaveRate.jobDocUpload.DocDescription));

                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IdentificationID", SqlDbType.BigInt, 0, ParameterDirection.Input, SaveRate.jobDocUpload.ID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileName", SqlDbType.NVarChar, 250, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 100, ParameterDirection.Input, SaveRate.jobDocUpload.Remarks);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PolicyNo", SqlDbType.VarChar, 100, ParameterDirection.Input, SaveRate.jobDocUpload.PolicyNo);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Ext", SqlDbType.NVarChar, 50, ParameterDirection.Input);

                            foreach (var file in SaveRate.jobDocUpload.file)
                            {
                                if (file != null && file.ContentLength > 0)
                                {
                                    FilePath = System.Configuration.ConfigurationManager.AppSettings["GCCDMS"].ToString();
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
                                            //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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

                        result = "Uable to save files";
                        return false;
                    }


                }
                else
                    throw new Exception("File Not Found");


            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertGCCDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataSet GetEmailNDdocuments(int MoveId)
        {
            DataSet Dtobj = new DataSet();

            try
            {
                string query = string.Format("exec [MoveMan].[GetJobEmail_ND_Documents] @SP_MoveID={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(MoveId))
                 , CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().LoginID)));

                Dtobj = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveMangeDAL", "GetEmailNDdocuments", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;

        }

        public JobDocument GetDownloadFile(int FileID, int LoginID)
        {
            JobDocument job = new JobDocument();
            try
            {
                DataTable dt = CSubs.GetDataTable(string.Format("exec [MoveMan].[GetJobDocumentDetail] @SP_Fileid={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(FileID)), CSubs.QSafeValue(Convert.ToString(LoginID))));

                if (dt != null && dt.Rows.Count > 0)
                {
                    job.FileID = Convert.ToInt32(dt.Rows[0]["FileID"]);
                    job.DocTypeID = Convert.ToInt32(dt.Rows[0]["DocTypeID"]);
                    job.DocNameID = Convert.ToInt32(dt.Rows[0]["DocNameID"]);
                    job.FilePath = Convert.ToString(dt.Rows[0]["DocFilePath"]);
                    job.FileName = Convert.ToString(dt.Rows[0]["DocFileName"]);
                }

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetDownloadFile", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return job;

        }

        public DataTable GetEmailTransactionDetail(int ActivityID = 0, int MailTransactionID = 0, int LoginID = 0)
        {
            DataTable dt = new DataTable();
            try
            {
                dt = CSubs.GetDataTable(string.Format("exec [MoveMan].[GetMailTransactionDetail] @SP_ActivityID={0},@SP_MailTransactionID={1}", CSubs.QSafeValue(Convert.ToString(ActivityID)), CSubs.QSafeValue(Convert.ToString(MailTransactionID))));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetEmailTransactionDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return dt;

        }

        public bool InsertEmailData(MoveManageViewModel SaveEmail, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[MoveMan].[AddEditEmailInfo]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.Input, SaveEmail.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActivityID", SqlDbType.Int, 0, ParameterDirection.Input, SaveEmail.MoveEmail.ActivityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailTo", SqlDbType.VarChar, 100, ParameterDirection.Input, SaveEmail.MoveEmail.EmailTo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailCC", SqlDbType.VarChar, 100, ParameterDirection.Input, SaveEmail.MoveEmail.EmailCC);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailBCC", SqlDbType.VarChar, 100, ParameterDirection.Input, SaveEmail.MoveEmail.EmailBCC);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailSubject", SqlDbType.VarChar, 100, ParameterDirection.Input, SaveEmail.MoveEmail.Subject);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailBody", SqlDbType.VarChar, -1, ParameterDirection.Input, SaveEmail.MoveEmail.Body);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SentBy", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SentOn", SqlDbType.DateTime, 0, ParameterDirection.Input, DateTime.Now);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertEmailData", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertFollowUpDetials(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[MoveMan].[AddEditFollowupInfo]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.Input, SaveData.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FollowupDate", SqlDbType.Date, 0, ParameterDirection.Input, SaveData.FollowUp.FollowUpDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FollowupRemarks", SqlDbType.VarChar, 1000, ParameterDirection.Input, SaveData.FollowUp.FollowUpRemark);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertFollowUpDetials", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertCloseJobDetials(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[MoveMan].[CloseJobInfo]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.Input, SaveData.MoveID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FollowupDate", SqlDbType.Date, 0, ParameterDirection.Input, SaveData.FollowUp.FollowUpDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CloseJobRemarks", SqlDbType.VarChar, 1000, ParameterDirection.Input, SaveData.CloseJobRemark);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertCloseJobDetials", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }


        public bool InsertInsuranceBySD(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[Ins].[AddEditInsBySD]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.Input, SaveData.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackDate", SqlDbType.Date, 0, ParameterDirection.Input, SaveData.Insurance.InsPackDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsuranceValue", SqlDbType.Float, 0, ParameterDirection.Input, SaveData.Insurance.InsuranceValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CurrID", SqlDbType.Int, 0, ParameterDirection.Input, SaveData.Insurance.InsuranceCurrID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PremiumRate", SqlDbType.Float, 0, ParameterDirection.Input, SaveData.Insurance.PremiumRate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IDV_value_Car", SqlDbType.Float, 0, ParameterDirection.Input, SaveData.Insurance.IDVCarValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehMakeModel", SqlDbType.VarChar, 50, ParameterDirection.Input, SaveData.Insurance.VehMakeModel);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSendForInsurance", SqlDbType.Bit, 0, ParameterDirection.Input, SaveData.Insurance.IsSendForInsurance);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@OutMsg", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertFollowUpDetials", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool CancelJob(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[MoveMan].[AddEditCancelJob]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.Input, SaveData.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CancelRemarks", SqlDbType.VarChar, 1000, ParameterDirection.Input, SaveData.JobCancel.CancelRemark);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "CancelJob", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool ApproveAdvanceCaution(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[MoveMan].[ApproveAdvanceCautionJob]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.Input, SaveData.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "ApproveAdvanceCaution", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool DeleteDocument(int ID, int LoginID, out string result)
        {
            result = String.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[MoveMan].[DeleteDocFileUpload]", QueryType.Procedure);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@Sp_EditDeleteInsert", SqlDbType.VarChar, 1, ParameterDirection.Input, CSubs.PSafeValue("I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ID", SqlDbType.BigInt, 0, ParameterDirection.Input, ID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@OutMsg", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "DeleteDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetDocumentGrid(Int64 ID, String DocFromType, int DocTypeID = -1, int DocNameID = -1, string DocDescription = "")
        {
            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("exec [MoveMan].[GetDocumentGrid] @SP_ID={0},@SP_LoginID={1},@SP_DocTypeID={2},@SP_DocFromType={3},@SP_DocNameID={4},@SP_DocDescription={5}",
                    CSubs.QSafeValue(Convert.ToString(ID))
                 , CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().LoginID))
                 , CSubs.QSafeValue(Convert.ToString(DocTypeID))
                 , CSubs.QSafeValue(DocFromType)
                 , CSubs.QSafeValue(Convert.ToString(DocNameID))
                 , CSubs.QSafeValue(DocDescription)
                 );

                Dtobj = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveMangeDAL", "GetDocumentGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;

        }

        public bool InsertVendorEvaluation(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string QuestionXml = string.Empty;
                        List<JobVendorEvalQuestion> Questions = new List<JobVendorEvalQuestion>();
                        Questions.AddRange(SaveData.vendorEvaluation.OrgEvalQuestions);
                        Questions.AddRange(SaveData.vendorEvaluation.DestEvalQuestions);
                        QuestionXml = Convert.ToString(new XElement("Questions", from emp in Questions
                                                                                 select new XElement("Question",
                                                                            new XElement("QustID", emp.QuestionID),
                                                                            new XElement("Ans", emp.Answer),
                                                                            new XElement("AnsOptionID", emp.AnswerOptionID),
                                                                            new XElement("AnsType", emp.AnswerType),
                                                                            new XElement("AnsText", emp.AnswerText)
                                                                        )));

                        conn.AddCommand("[MoveMan].[AddEditVendorEvaluationForJob]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.Input, SaveData.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QuestionsXml", SqlDbType.Xml, -1, ParameterDirection.Input, QuestionXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgRemark", SqlDbType.NVarChar, 3500, ParameterDirection.Input, CSubs.PSafeValue(SaveData.vendorEvaluation.OrgRemark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestRemark", SqlDbType.NVarChar, 3500, ParameterDirection.Input, CSubs.PSafeValue(SaveData.vendorEvaluation.DestRemark));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgActionable", SqlDbType.NVarChar, 3500, ParameterDirection.Input, CSubs.PSafeValue(SaveData.vendorEvaluation.OrgActionable));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestActionable", SqlDbType.NVarChar, 3500, ParameterDirection.Input, CSubs.PSafeValue(SaveData.vendorEvaluation.DestActionable));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@OutMsg", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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
                return true;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertVendorEvaluation", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public JobVendorEvaluation GetVendorEvaluation(Int64 MoveId)
        {
            JobVendorEvaluation evaluation = new JobVendorEvaluation();
            evaluation.MoveID = MoveId;
            int LoginID = UserSession.GetUserSession().LoginID;

            try
            {
                int CompanyID = UserSession.GetUserSession().CompanyID;
                bool RMCBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";

                DataSet ds = CSubs.GetDataSet(string.Format("exec [MoveMan].[GetVendorEvaluationForJob]  @SP_MoveId={0},@SP_LoginID={1},@SP_CompID={2},@SP_IsRMCBuss={3}"
                    , CSubs.QSafeValue(Convert.ToString(MoveId))
                    , CSubs.QSafeValue(Convert.ToString(LoginID))
                    , CSubs.QSafeValue(Convert.ToString(CompanyID))
                    , CSubs.QSafeValue(Convert.ToString(RMCBuss))
                    ));

                if (ds != null && ds.Tables.Count > 0)
                {

                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        ////Origin
                        evaluation.OrgEvalQuestions = (from rw in ds.Tables[0].AsEnumerable()
                                                       let MoveCompID = rw.Field<int>("MoveCompID")
                                                       where MoveCompID == 1
                                                       select new JobVendorEvalQuestion()
                                                       {
                                                           QuestionID = Convert.ToInt32(rw["VendorEvolQstmasterID"]),
                                                           RateCompID = Convert.ToInt32(rw["MoveCompID"]),
                                                           QuestionDetail = Convert.ToString(rw["QuestionDet"]),
                                                           IsRMCBuss = RMCBuss,
                                                           Order = Convert.ToInt32(rw["QuestionOrder"]),
                                                           Answer = !string.IsNullOrWhiteSpace(Convert.ToString(rw["AnsForQuestion"])) ? Convert.ToBoolean(rw["AnsForQuestion"]) : (bool?)null,
                                                           Weightage = Convert.ToInt32(rw["Weightage"]),
                                                           AnswerOptionID = !string.IsNullOrWhiteSpace(Convert.ToString(rw["AnsOptionID"])) ? Convert.ToInt32(rw["AnsOptionID"]) : (int?)null,
                                                           AnswerType = Convert.ToString(rw["AnswerType"]),
                                                           AnswerText = Convert.ToString(rw["AnsText"]),
                                                           options = GetVendorEValQuestionOptions()
                                                       }).ToList();

                        //evaluation.OrgSelectedQuestions= evaluation.OrgEvalQuestions.Where(m=>m.Answer==true).Select(c => c.QuestionID).ToList();

                        ////Destination
                        evaluation.DestEvalQuestions = (from rw in ds.Tables[0].AsEnumerable()
                                                        let MoveCompID = rw.Field<int>("MoveCompID")
                                                        where MoveCompID == 3
                                                        select new JobVendorEvalQuestion()
                                                        {
                                                            QuestionID = Convert.ToInt32(rw["VendorEvolQstmasterID"]),
                                                            RateCompID = Convert.ToInt32(rw["MoveCompID"]),
                                                            QuestionDetail = Convert.ToString(rw["QuestionDet"]),
                                                            IsRMCBuss = RMCBuss,
                                                            Order = Convert.ToInt32(rw["QuestionOrder"]),
                                                            Answer = !string.IsNullOrWhiteSpace(Convert.ToString(rw["AnsForQuestion"])) ? Convert.ToBoolean(rw["AnsForQuestion"]) : (bool?)null,
                                                            Weightage = Convert.ToInt32(rw["Weightage"]),
                                                            AnswerOptionID = !string.IsNullOrWhiteSpace(Convert.ToString(rw["AnsOptionID"])) ? Convert.ToInt32(rw["AnsOptionID"]) : (int?)null,
                                                            AnswerType = Convert.ToString(rw["AnswerType"]),
                                                            AnswerText = Convert.ToString(rw["AnsText"]),
                                                            options = GetVendorEValQuestionOptions()
                                                        }).ToList();

                        //evaluation.DestSelectedQuestions = evaluation.DestEvalQuestions.Where(m => m.Answer == true).Select(c => c.QuestionID).ToList();
                    }


                    if (ds.Tables.Count > 1 && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        evaluation.OrgRemark = Convert.ToString(ds.Tables[1].Rows[0]["OrgRemarks"]);
                        evaluation.OrgActionable = Convert.ToString(ds.Tables[1].Rows[0]["OrgActionable"]);
                        evaluation.DestRemark = Convert.ToString(ds.Tables[1].Rows[0]["DestRemarks"]);
                        evaluation.DestActionable = Convert.ToString(ds.Tables[1].Rows[0]["DestActionable"]);
                        evaluation.MoveID = Convert.ToInt64(ds.Tables[1].Rows[0]["MoveID"]);
                        evaluation.IsFeedbackEntered = true;
                    }
                    else
                    {
                        evaluation.IsFeedbackEntered = false;
                    }

                    if (ds.Tables.Count > 2 && ds.Tables[2] != null && ds.Tables[2].Rows.Count > 0)
                    {
                        evaluation.ShowActionable = Convert.ToString(ds.Tables[2].Rows[0]["ShowActionable"]);
                    }


                }

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetVendorEvaluation", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return evaluation;
        }

        public JobLabel GetJobLable(Int64 MoveId, int LoginID)
        {
            JobLabel label = new JobLabel();


            try
            {
                //int CompanyID = UserSession.GetUserSession().CompanyID;
                //bool RMCBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";

                DataTable dt = CSubs.GetDataTable(string.Format("exec [MoveMan].[GetJobLabel]  @SP_MoveID={0},@SP_LoginID={1}"
                    , CSubs.QSafeValue(Convert.ToString(MoveId))
                    , CSubs.QSafeValue(Convert.ToString(LoginID))
                    ));

                if (dt != null && dt.Rows.Count > 0)
                {
                    label = (from rw in dt.AsEnumerable()
                             select new JobLabel()
                             {
                                 JobNo = Convert.ToString(rw["JobNo"]),
                                 CLIENT = Convert.ToString(rw["Client"]),
                                 SHIPPER = Convert.ToString(rw["Shipper"]),
                                 CORPORATE = Convert.ToString(rw["Corporate"]),
                                 ORIGIN = Convert.ToString(rw["Origin"]),
                                 DESTINATION = Convert.ToString(rw["Destination"]),
                                 ORIGIN_Agent = Convert.ToString(rw["OA"]),
                                 DESTINATION_Agent = Convert.ToString(rw["DA"]),
                                 Shipper_Address = Convert.ToString(rw["Shipper_Address"]),
                                 Shipper_Phone1 = Convert.ToString(rw["Shipper_Phone1"]),
                                 Shipper_Phone2 = Convert.ToString(rw["Shipper_Phone2"]),
                                 Shipper_Fax = Convert.ToString(rw["Shipper_Fax"]),
                                 Shipper_Email = Convert.ToString(rw["Shipper_Email"]),
                                 Client_Address = Convert.ToString(rw["Client_Address"]),
                                 Client_Phone1 = Convert.ToString(rw["Client_Phone1"]),
                                 Client_Phone2 = Convert.ToString(rw["Client_Phone2"]),
                                 Client_Fax = Convert.ToString(rw["Client_Fax"]),
                                 BusinessLine = Convert.ToString(rw["BusinessLine"]),
                                 Mode = Convert.ToString(rw["Mode"]),
                                 Insurance = Convert.ToString(rw["Insurance"]),
                                 MoveCordinator = Convert.ToString(rw["MoveCordinator"]),
                                 ShipmentType = Convert.ToString(rw["ShipmentType"]),

                             }).FirstOrDefault();


                }

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "JobLabel", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return label;
        }

        public DataTable GetJobReportParams(Int64 MoveID, Int64 SurveyID, int ReportID, int LoginID)
        {
            try
            {
                return CSubs.GetDataTable(string.Format("exec [Report].[GetReportsParamsForJob]  @SP_MoveID={0},@SP_SurveyID={1},@SP_ReportID={2},@SP_LoginID={3}"
                    , CSubs.QSafeValue(Convert.ToString(MoveID))
                    , CSubs.QSafeValue(Convert.ToString(SurveyID))
                    , CSubs.QSafeValue(Convert.ToString(ReportID))
                    , CSubs.QSafeValue(Convert.ToString(LoginID))
                    ));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetJobReportParams", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataTable GetTrasshipmentVessel(Int64 MoveID, int LoginID)
        {
            try
            {
                return CSubs.GetDataTable(string.Format("exec [MoveMan].[GetFrtInfo_TransshipmentVesselGrid] @SP_MoveID={0},@SP_LoginID={1}"
                    , CSubs.QSafeValue(Convert.ToString(MoveID))
                    , CSubs.QSafeValue(Convert.ToString(LoginID))
                    ));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetTrasshipmentVessel", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public TranShipmentWtVol getGetJobWtDetail(Int64 MoveId, int LoginID)
        {
            TranShipmentWtVol data = new TranShipmentWtVol();
            try
            {
                //int CompanyID = UserSession.GetUserSession().CompanyID;
                //bool RMCBuss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";

                DataTable dt = CSubs.GetDataTable(string.Format("exec [MoveMan].[GetPackageWtDetForFrtModule]  @SP_MoveID={0},@SP_LoginID={1}"
                    , CSubs.QSafeValue(Convert.ToString(MoveId))
                    , CSubs.QSafeValue(Convert.ToString(LoginID))
                    ));

                if (dt != null && dt.Rows.Count > 0)
                {
                    data = (from rw in dt.AsEnumerable()
                            select new TranShipmentWtVol()
                            {
                                MoveID = Convert.ToInt64(rw["MoveID"]),
                                JobNo = Convert.ToString(rw["JobID"]),
                                Shipper = Convert.ToString(rw["ShipperName"]),
                                WtVolUnitId = string.IsNullOrEmpty(Convert.ToString(rw["WtUnitID"])) ? (int?)null : Convert.ToInt32(rw["WtUnitID"]),
                                WtVolUnit = Convert.ToString(rw["WtUnitName"]),
                                WtVol = string.IsNullOrEmpty(Convert.ToString(rw["WtValue"])) ? (decimal?)null : Convert.ToDecimal(rw["WtValue"]),
                                VolUnitId = string.IsNullOrEmpty(Convert.ToString(rw["VolUnitid"])) ? (int?)null : Convert.ToInt32(rw["VolUnitid"]),
                                VolUnit = Convert.ToString(rw["VolUnitNmae"]),
                                Vol = string.IsNullOrEmpty(Convert.ToString(rw["VolValue"])) ? (decimal?)null : Convert.ToDecimal(rw["VolValue"]),
                                NoOfPacks = string.IsNullOrEmpty(Convert.ToString(rw["NoOfPacks"])) ? (int?)null : Convert.ToInt32(rw["NoOfPacks"]),
                            }).FirstOrDefault();


                }

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "getGetJobWtDetail", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return data;
        }

        public DataTable CheckCreditPrivateClient(Int64 MoveID, int LoginID, bool IsWIS = false)
        {
            try
            {
                return CSubs.GetDataTable(string.Format("exec [Moveman].[CheckCreditPrivateClient] @SP_MoveID={0},@SP_LoginID={1},@SP_IsFromWIS={2}"
                    , CSubs.QSafeValue(Convert.ToString(MoveID))
                    , CSubs.QSafeValue(Convert.ToString(LoginID))
                    , CSubs.QSafeValue(Convert.ToString(IsWIS))
                    ));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "CheckCreditPrivateClient", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }




        public DataTable GetMailFormat(Int64 ActivityID, int LoginID, Int64? MoveID, Int64? TransID)
        {
            try
            {
                return CSubs.GetDataTable(string.Format("exec [MailDet].[GetFormatForDisplay] @SP_ActivityTypeID={0},@SP_MoveID={1},@SP_TransID={2}",
                    CSubs.QSafeValue(Convert.ToString(ActivityID)),
                    CSubs.QSafeValue(Convert.ToString(MoveID)),
                    CSubs.QSafeValue(Convert.ToString(TransID))));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetMailFormat", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataTable GetMailActivityHistory(int LoginID, Int64? MoveId, Int64? ActivityID)
        {
            try
            {
                return CSubs.GetDataTable(string.Format("exec [MailDet].[GetMailActivityHistory] @SP_MoveID={0},@SP_ActivityID={1}", CSubs.QSafeValue(Convert.ToString(MoveId)), CSubs.QSafeValue(Convert.ToString(ActivityID))));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetMailActivityHistory", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertACODetails(MoveManageViewModel SaveData, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[MoveMan].[AddEditACODetails]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ACODetailsId", SqlDbType.Int, 0, ParameterDirection.Input, SaveData.ACODetails.ACODetailsId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.Input, SaveData.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobStatusSDId", SqlDbType.Int, 0, ParameterDirection.Input, SaveData.ACODetails.JobStatusSDId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillingStatusId", SqlDbType.Int, 0, ParameterDirection.Input, SaveData.ACODetails.BillingStatusId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 300, ParameterDirection.Input, SaveData.ACODetails.Remarks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 8000, ParameterDirection.Output);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_ACODetailsId", SqlDbType.Int, 0, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                SaveData.ACODetails.ACODetailsId = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_ACODetailsId"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertFollowUpDetials", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataTable GetACODetails(int LoginID, Int64 MoveID)
        {
            DataTable Dtobj = new DataTable();
            try
            {
                string query = string.Format("exec [MoveMan].[GetACODetailsForDisplay] @SP_LoginID={0},@SP_MoveID={1}",
                    CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(MoveID)));

                Dtobj = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveMangeDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return Dtobj;
        }

        public IEnumerable<SelectListItem> GetVendorEValQuestionOptions()
        {
            return CSubs.BindDropdown(string.Format("Exec [MoveMan].[ForCombo_VendorEvalAnsOptions] @SP_Type='ALLACTIVE',@SP_Loginid={0}", UserSession.GetUserSession().LoginID));
        }

        public bool InsertRequestDocsData(MoveManageViewModel ViewData, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string DocNameIDListXml = string.Empty;
                        if (!string.IsNullOrEmpty(ViewData.RequestDocsUpload.DocNameIDListHidden))
                        {
                            XNode node = JsonConvert.DeserializeXNode(ViewData.RequestDocsUpload.DocNameIDListHidden, "DocNameIDLists");
                            DocNameIDListXml = node.ToString();
                        }

                        conn.AddCommand("[MoveMan].[AddEditRequestDocs]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.Int, 0, ParameterDirection.InputOutput, ViewData.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobDocTypeId", SqlDbType.Int, 0, ParameterDirection.Input, ViewData.jobDocUpload.JobDocTypeId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocNameIDListXml", SqlDbType.Xml, -1, ParameterDirection.Input, DocNameIDListXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.NVarChar, 500, ParameterDirection.Input, ViewData.jobDocUpload.Remarks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RequestDocsGroupID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, ViewData.RequestDocsGroupID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                ViewData.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
                                ViewData.RequestDocsGroupID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_RequestDocsGroupID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertRequestDocsData", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetRequestDocsDetails(Int64 MoveID, Int64 RequestDocsGroupID)
        {
            DataSet ds = new DataSet();
            try
            {
                string query = string.Format("exec [MoveMan].[GetRequestDocsDetails] @SP_MoveID={0},@SP_RequestDocsGroupID={1}",
                 CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(RequestDocsGroupID)));

                ds = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(null, "MoveMangeDAL", "GetRequestDocsDetails", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return ds;
        }

        public DataSet GetShipperFeedbackTemplate(int LoginID, Int64? MoveID, int? SFTemplateID, Int64? ShipperFeedbackID, bool IsLoggedInUser)
        {
            DataSet SFQuestionsDs = new DataSet();
            try
            {
                SFQuestionsDs = CSubs.GetDataSet(string.Format("[feedback].[GetShipperFeedbackTemplate] @SP_LoginID={0},@SP_MoveID={1},@SP_SFTemplateID={2},@SP_ShipperFeedbackID={3},@SP_IsLoggedInUser={4}",
                    CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(SFTemplateID)),
                    CSubs.QSafeValue(Convert.ToString(ShipperFeedbackID)), CSubs.QSafeValue(Convert.ToString(IsLoggedInUser))));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "GetShipperFeedbackTemplate", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return SFQuestionsDs;
        }

        public bool AddEditShipperFeedback(MoveManageViewModel ViewData, int LoginID, int CompanyID, bool IsSubmitFeedback, string Url, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string SFQuestionIDListXml = string.Empty;
                        if (!string.IsNullOrEmpty(ViewData.ShipperFeedback.SFQuestionIDListHidden))
                        {
                            XNode node = JsonConvert.DeserializeXNode(ViewData.ShipperFeedback.SFQuestionIDListHidden, "SFQuestionIDLists");
                            SFQuestionIDListXml = node.ToString();
                        }

                        conn.AddCommand("[feedback].[AddEditShipperFeedback]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, ViewData.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipperFeedbackID", SqlDbType.Int, 0, ParameterDirection.InputOutput, ViewData.ShipperFeedback.ShipperFeedbackID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailTo", SqlDbType.VarChar, 200, ParameterDirection.Input, ViewData.ShipperFeedback.EmailTo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SFTemplateID", SqlDbType.Int, 0, ParameterDirection.Input, ViewData.ShipperFeedback.SFTemplateID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSubmitFeedback", SqlDbType.Bit, 0, ParameterDirection.Input, IsSubmitFeedback);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SFQuestionIDList", SqlDbType.Xml, -1, ParameterDirection.Input, SFQuestionIDListXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Url", SqlDbType.NVarChar, 1000, ParameterDirection.Input, Url);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.VarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                ViewData.ShipperFeedback.ShipperFeedbackID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_ShipperFeedbackID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "AddEditShipperFeedback", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertAgentInvDocument(AgentInvoice model, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                string FileName = string.Empty;
                string Extension = string.Empty;
                string FilePath = System.Configuration.ConfigurationManager.AppSettings["JobDMS"].ToString();
                int FileUploadCount = 0;

                if (model.file != null)
                {
                    using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, System.Data.IsolationLevel.ReadCommitted))
                    {
                        if (conn.Connect())
                        {

                            conn.AddCommand("[MoveMan].[AddEditVendorInvDMSDetail]", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.ID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocTypeID", SqlDbType.Int, 0, ParameterDirection.Input, model.DocTypeID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocNameID", SqlDbType.Int, 0, ParameterDirection.Input, model.DocNameID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Description", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(model.DocDescription));

                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IdentificationID", SqlDbType.BigInt, 0, ParameterDirection.Input, model.ID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileName", SqlDbType.NVarChar, 250, ParameterDirection.Input);

                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AgentID", SqlDbType.Int, 0, ParameterDirection.Input, model.InvDmsAgentId);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InvRefNo", SqlDbType.VarChar, 100, ParameterDirection.Input, CSubs.PSafeValue(model.InvoiceNo));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Amount", SqlDbType.Float, 0, ParameterDirection.Input, model.Amount);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Ext", SqlDbType.NVarChar, 50, ParameterDirection.Input);

                            foreach (var file in model.file)
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
                                            //SaveRate.MoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MoveID"));
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

                        result = "Uable to save files";
                        return false;
                    }


                }
                else
                    throw new Exception("File Not Found");


            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertAgentInvDocument", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }


        public DataTable GetAgentInvDocumentGrid(Int64 ID, String DocFromType, int DocTypeID = -1, int DocNameID = -1, string DocDescription = "")
        {
            DataTable Dtobj = new DataTable();

            try
            {
                string query = string.Format("exec [MoveMan].[GetAgentInvDocumentGrid] @SP_ID={0},@SP_LoginID={1},@SP_DocTypeID={2},@SP_DocFromType={3},@SP_DocNameID={4},@SP_DocDescription={5}",
                    CSubs.QSafeValue(Convert.ToString(ID))
                 , CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().LoginID))
                 , CSubs.QSafeValue(Convert.ToString(DocTypeID))
                 , CSubs.QSafeValue(DocFromType)
                 , CSubs.QSafeValue(Convert.ToString(DocNameID))
                 , CSubs.QSafeValue(DocDescription)
                 );

                Dtobj = CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveMangeDAL", "GetDocumentGrid", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return Dtobj;

        }

        public bool InsertGPApproval(GPApproval ViewData, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[MoveMan].[AddEditGPApproval]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, ViewData.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsProceedForApproval", SqlDbType.Bit, 0, ParameterDirection.Input, false);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSendForApproval", SqlDbType.Bit, 0, ParameterDirection.Input, ViewData.IsGPSendtoApproval);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsApprove", SqlDbType.Bit, 0, ParameterDirection.Input, ViewData.IsGPApprove);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSendToSD", SqlDbType.Bit, 0, ParameterDirection.Input, ViewData.IsGPSendtoSD);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.VarChar, -1, ParameterDirection.Input, ViewData.GPApprovalRemark);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ApprovalUserID", SqlDbType.Int, -1, ParameterDirection.Input, ViewData.GPSendForApprovalUser);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GPPercent", SqlDbType.Decimal, -1, ParameterDirection.Input, ViewData.GPPercent);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GPAmount", SqlDbType.Decimal, -1, ParameterDirection.Input, ViewData.GPAmount);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GPMasterID", SqlDbType.Int, -1, ParameterDirection.Input, ViewData.GPMasterID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.VarChar, 500, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertGPApproval", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertGPApproval(MoveManageViewModel ViewData, int LoginID, string stage, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("[MoveMan].[AddEditGPApproval]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, ViewData.MoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsProceedForApproval", SqlDbType.Bit, 0, ParameterDirection.Input, true);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSendForApproval", SqlDbType.Bit, 0, ParameterDirection.Input, false);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsApprove", SqlDbType.Bit, 0, ParameterDirection.Input, false);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSendToSD", SqlDbType.Bit, 0, ParameterDirection.Input, false);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ApprovalUserID", SqlDbType.Int, -1, ParameterDirection.Input, ViewData.GPSendForApprovalUser);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GPPercent", SqlDbType.Decimal, -1, ParameterDirection.Input, ViewData.GPPercent);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GPAmount", SqlDbType.Decimal, -1, ParameterDirection.Input, ViewData.GPAmount);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GPMasterID", SqlDbType.Int, -1, ParameterDirection.Input, ViewData.GPMasterID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remark", SqlDbType.VarChar, -1, ParameterDirection.Input, null);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Stage", SqlDbType.VarChar, 20, ParameterDirection.Input, stage);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.VarChar, 500, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "InsertGPApproval", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }


        public DataTable GetGPAmount(decimal RevAmt, decimal GPPercent, int MoveID, string BaseCurr)
        {
            DataTable GPPEecentDs = new DataTable();
            try
            {
                GPPEecentDs = CSubs.GetDataTable(string.Format("[MoveMan].[GetGPAmount] @SP_RevAmount={0},@SP_MoveID={1},@SP_GPPercent={2},@SP_BaseCurr={3}",
                    CSubs.QSafeValue(Convert.ToString(RevAmt)), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(GPPercent)), CSubs.QSafeValue(Convert.ToString(BaseCurr))));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveMangeDAL", "GetGPAmount", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return GPPEecentDs;
        }

        public DataTable GetPrevGPAmount(int MoveID)
        {
            DataTable GPPEecentDs = new DataTable();
            try
            {
                GPPEecentDs = CSubs.GetDataTable(string.Format("[MoveMan].[GetPrevGPAmount] @SP_MoveID={0}", CSubs.QSafeValue(Convert.ToString(MoveID))));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveMangeDAL", "GetPrevGPAmount", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return GPPEecentDs;
        }

        public DataTable GetGDPRNationality(String Nationality)
        {
            DataTable GDPRNationalityDs = new DataTable();
            try
            {
                GDPRNationalityDs = CSubs.GetDataTable(string.Format("[MoveMan].[GetGDPRNationality] @SP_Nationality={0}",
                    CSubs.QSafeValue(Convert.ToString(Nationality))));
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "MoveMangeDAL", "GetGDPRNationality", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return GDPRNationalityDs;
        }

        public bool UnlockSTGDate(MoveManageViewModel ViewData, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[MoveMan].[UnlockSTGDate]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, ViewData.MoveID);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsUnlockSTGDate", SqlDbType.Bit, 0, ParameterDirection.Input, true);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.VarChar, 500, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(LoginID), "MoveMangeDAL", "UnlockSTGDate", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

    }
}