using Newtonsoft.Json;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.DAL.Repository;
using RELOCBS.Entities;
using RELOCBS.Utility;
using System;
using System.Data;
using System.IO;
using System.Linq;

namespace RELOCBS.DAL.Survey
{
    public class SurveyDAL : Repository<SurveyData>, IDisposable
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

        public bool Insert(SurveyViewModel survey, out string result, DateTime? SchSurveydate)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {


                        //XmlDocument doc = new XmlDocument();
                        //doc.LoadXml(survey.ServiceListHidden);
                        //string jsonText = JsonConvert.SerializeXmlNode(doc);

                        // To convert JSON text contained in string json into an XML node
                        //XmlDocument doc = JsonConvert.DeserializeXmlNode(json);


                        //XmlDocument xml = new XmlDocument();
                        //XmlElement root = xml.CreateElement("SurveyDetails");
                        //xml.AppendChild(root);


                        //if (survey.ServiceDetailList!=null)
                        //{
                        //    foreach (var item in survey.ServiceDetailList)
                        //    {

                        //        XmlElement child = xml.CreateElement("SurveyDetail");
                        //        //XmlElement child1 = 
                        //        child.AppendChild(xml.CreateElement("SurveyID")).InnerText = Convert.ToString((survey.SurveyId == null || survey.SurveyId <= 0) ? null : survey.SurveyId);
                        //        child.AppendChild(xml.CreateElement("SurveyerID")).InnerText = Convert.ToString(survey.SurveyConductedById);
                        //        child.AppendChild(xml.CreateElement("SurveyDate")).InnerText = Convert.ToString(survey.SurveyDate);
                        //        child.AppendChild(xml.CreateElement("SurveyTime")).InnerText = Convert.ToString(survey.SurveyDateTime);
                        //        child.AppendChild(xml.CreateElement("SurveyDetailsID")).InnerText = Convert.ToString(item.SurveyDetailsID);
                        //        child.AppendChild(xml.CreateElement("RateCompID")).InnerText = Convert.ToString(item.RateCompID);
                        //        child.AppendChild(xml.CreateElement("CostHeadID")).InnerText = Convert.ToString(item.CostHeadID);
                        //        //XmlElement child2 = (XmlElement)
                        //        child.AppendChild(xml.CreateElement("RemarksOnCostHead")).InnerText = Convert.ToString(item.RemarksOnCostHead);
                        //        root.AppendChild(child);
                        //    }

                        //}
                        //else
                        //{
                        //    XmlElement child = xml.CreateElement("CostHeadwiseDetail");
                        //    //XmlElement child2 = xml.CreateElement("child");
                        //    child.AppendChild(xml.CreateElement("CostHeadID")).InnerText = "0";
                        //    //XmlElement child2 = (XmlElement)
                        //    child.AppendChild(xml.CreateElement("NetAmt")).InnerText = Convert.ToString(SaveRate.Rate);
                        //    //child1.AppendChild(child2);
                        //    root.AppendChild(child);
                        //}

                        //JObject jsonObject = JObject.Parse(survey.ServiceListHidden);
                        //JObject SurveyID = new JObject();
                        //SurveyID["SurveyID"] = survey.SurveyId;
                        //JObject SurveyerID = new JObject();
                        //SurveyerID["SurveyerID"] = survey.SurveyId;
                        //jsonObject["SurveyDetail"].FirstOrDefault().AddAfterSelf(SurveyID);
                        //jsonObject["SurveyDetail"].FirstOrDefault().AddAfterSelf(SurveyerID);
                        string ServiceHeadXml = null;
                        if (survey.ServiceListHidden != null)
                        {
                            System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(survey.ServiceListHidden, "SurveyDetails");
                            //xml.OuterXml;
                            ServiceHeadXml = node.ToString();

                        }


                        string CostHeadXml = null;
                        if (survey.CostListHidden != null)
                        {
                            System.Xml.Linq.XNode node1 = JsonConvert.DeserializeXNode(survey.CostListHidden, "CostHeadwiseDetails");
                            CostHeadXml = node1.ToString();

                        }


                        conn.AddCommand("[Survey].[AddEditSurvey]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AddEditDelete", SqlDbType.VarChar, 1, ParameterDirection.Input, ((survey.SurveyId != null && survey.SurveyId > 0) ? "E" : "I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_surveyID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, ((survey.SurveyId != null && survey.SurveyId > 0) ? survey.SurveyId : -1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqDetailID", SqlDbType.BigInt, 0, ParameterDirection.Input, survey.EnquiryDetail.EnqDetailID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyerID", SqlDbType.Int, 0, ParameterDirection.Input, survey.SurveyConductedById);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.SurveyDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveryTime", SqlDbType.Time, 7, ParameterDirection.Input, survey.SurveyDateTime);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.PackDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfDays", SqlDbType.Int, 0, ParameterDirection.Input, (survey.NoOfDays));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoadDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.LoadDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QuoteSubmissionDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.QuotationSubmissionDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ReqDeliveryDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.DeliveryDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsurBy", SqlDbType.VarChar, 50, ParameterDirection.Input, (survey.InsuredById));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ApproxInsuAmt", SqlDbType.BigInt, 0, ParameterDirection.Input, (survey.InsuredAmount));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsCurrrencyID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.InsCurrencyID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Competetion", SqlDbType.Bit, 0, ParameterDirection.Input, (survey.IsCompition));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompetitorId", SqlDbType.Int, 0, ParameterDirection.Input, (0));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Competitortext", SqlDbType.VarChar, 100, ParameterDirection.Input, (survey.CompititorName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateAvailable", SqlDbType.Bit, 0, ParameterDirection.Input, (survey.RateAvailable));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AvailableRate", SqlDbType.BigInt, 0, ParameterDirection.Input, (survey.AvailableRate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCurrencyID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.RateCurrencyID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MonitoryEntitlement", SqlDbType.BigInt, 0, ParameterDirection.Input, (survey.MonitoryEntitlement));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MonitoryEntCurrID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.MonitoryEntCurrID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(survey.SurveyRemarks));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Isactive", SqlDbType.Bit, 0, ParameterDirection.Input, true);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VolumeUnitID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_VolumeUnitID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TobePackedVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Volume_TobePackedValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NetVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Volume_NetValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GrossVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Volume_GrossValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_WeightUnitID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NetWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Weight_NetValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GrossWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Weight_GrossValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ACWTWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Weight_ACWTValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LooseCased", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.SurveyLooseCased));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DensityFact", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.SurveyDensityFact));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LCLorFCL", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.SurveyLCLorFCL));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContainerSizeID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_ContainerID));

                        //Entitlement
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntVolumeUnitID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.EnquiryDetail.VolumeUnitID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntTobePackedVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.VolumeToPack));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntNetVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.VolumeNet));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntGrossVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.VolumeGross));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntWeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.EnquiryDetail.WtUnitid));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntNetWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.WtNet));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntGrossWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.WtGross));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntACWTWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.WtACWT));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntLooseCased", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.EnquiryDetail.LooseCased));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntDensityFact", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.DensiyFactor));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntLCLorFCL", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.EnquiryDetail.LCLFCL));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntContainerSizeID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.EnquiryDetail.ContainerTypeID));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAdd", SqlDbType.VarChar, 10000, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgAdd));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAdd2", SqlDbType.VarChar, 10000, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgAdd2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCityID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.OrgCityID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPhone", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgEmailID", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgEmail));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPin", SqlDbType.VarChar, 15, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgPin));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAdd", SqlDbType.VarChar, 10000, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.DestAdd));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAdd2", SqlDbType.VarChar, 10000, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.DestAdd2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestCityID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.DestCityID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPhone", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.DestPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Email", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.Email));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPin", SqlDbType.VarChar, 15, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.DestPin));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShipCategory", SqlDbType.Int, 0, ParameterDirection.Input, survey.ShipCategoryID);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyDetailsXML", SqlDbType.Xml, -1, ParameterDirection.Input, ServiceHeadXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, -1, ParameterDirection.Input, CostHeadXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, survey.MoveId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RoadKMS", SqlDbType.Float, 0, ParameterDirection.Input, survey.RoadKMS);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_StageRemark", SqlDbType.VarChar, -1, ParameterDirection.Input, survey.StageRemark);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SchSurveyDate", SqlDbType.DateTime, 0, ParameterDirection.Input, SchSurveydate);


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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Completed(SurveyViewModel survey, out string result)
        {
            result = string.Empty;

            try
            {

                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(survey.ServiceListHidden, "SurveyDetails");

                        string CostHeadXml = null;
                        if (survey.CostListHidden != null)
                        {
                            System.Xml.Linq.XNode node1 = JsonConvert.DeserializeXNode(survey.CostListHidden, "CostHeadwiseDetails");
                            CostHeadXml = node1.ToString();

                        }
                        //xml.OuterXml;
                        string ServiceHeadXml = node.ToString();

                        conn.AddCommand("[Survey].[UpdateSurveyCompleted]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AddEditDelete", SqlDbType.VarChar, 1, ParameterDirection.Input, ((survey.SurveyId != null && survey.SurveyId > 0) ? "E" : "I"));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_surveyID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, ((survey.SurveyId != null && survey.SurveyId > 0) ? survey.SurveyId : -1));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqDetailID", SqlDbType.BigInt, 0, ParameterDirection.Input, survey.EnquiryDetail.EnqDetailID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyerID", SqlDbType.Int, 0, ParameterDirection.Input, survey.SurveyConductedById);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.SurveyDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveryTime", SqlDbType.Time, 7, ParameterDirection.Input, survey.SurveyDateTime.Value);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.SurveyDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfDays", SqlDbType.Int, 0, ParameterDirection.Input, (survey.NoOfDays));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoadDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.LoadDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QuoteSubmissionDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.QuotationSubmissionDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ReqDeliveryDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.DeliveryDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsurBy", SqlDbType.VarChar, 50, ParameterDirection.Input, (survey.InsuredById));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ApproxInsuAmt", SqlDbType.BigInt, 0, ParameterDirection.Input, (survey.InsuredAmount));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsCurrrencyID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.InsCurrencyID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Competetion", SqlDbType.Bit, 0, ParameterDirection.Input, (survey.IsCompition));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompetitorId", SqlDbType.Int, 0, ParameterDirection.Input, (0));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Competitortext", SqlDbType.VarChar, 0, ParameterDirection.Input, (survey.CompititorName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateAvailable", SqlDbType.Bit, 0, ParameterDirection.Input, (survey.RateAvailable));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AvailableRate", SqlDbType.BigInt, 0, ParameterDirection.Input, (survey.AvailableRate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCurrencyID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.RateCurrencyID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MonitoryEntitlement", SqlDbType.BigInt, 0, ParameterDirection.Input, (survey.MonitoryEntitlement));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MonitoryEntCurrID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.MonitoryEntCurrID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(survey.SurveyRemarks));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Isactive", SqlDbType.Bit, 0, ParameterDirection.Input, true);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VolumeUnitID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_VolumeUnitID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TobePackedVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Volume_TobePackedValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NetVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Volume_NetValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GrossVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Volume_GrossValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_WeightUnitID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NetWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Weight_NetValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GrossWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Weight_GrossValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ACWTWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Weight_ACWTValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LooseCased", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.SurveyLooseCased));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DensityFact", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.SurveyDensityFact));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LCLorFCL", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.SurveyLCLorFCL));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContainerSizeID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_ContainerID));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntVolumeUnitID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.EnquiryDetail.VolumeUnitID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntTobePackedVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.VolumeToPack));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntNetVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.VolumeNet));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntGrossVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.VolumeGross));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntWeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.EnquiryDetail.WtUnitid));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntNetWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.WtNet));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntGrossWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.WtGross));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntACWTWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.WtACWT));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntLooseCased", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.EnquiryDetail.LooseCased));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntDensityFact", SqlDbType.Float, 0, ParameterDirection.Input, (survey.EnquiryDetail.DensiyFactor));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntLCLorFCL", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.EnquiryDetail.LCLFCL));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EntContainerSizeID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.EnquiryDetail.ContainerTypeID));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAdd", SqlDbType.VarChar, 10000, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgAdd));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAdd2", SqlDbType.VarChar, 10000, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgAdd2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCityID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.OrgCityID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPhone", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgEmailID", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgEmail));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPin", SqlDbType.VarChar, 15, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgPin));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAdd", SqlDbType.VarChar, 10000, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.DestAdd));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAdd2", SqlDbType.VarChar, 10000, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.DestAdd2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestCityID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.DestCityID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPhone", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.DestPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Email", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.Email));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPin", SqlDbType.VarChar, 15, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.DestPin));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyDetailsXML", SqlDbType.Xml, -1, ParameterDirection.Input, ServiceHeadXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, -1, ParameterDirection.Input, CostHeadXml);

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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyDAL", "Insert", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            //return true;
        }

        public bool Update(SurveyViewModel survey, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(survey.ServiceListHidden, "SurveyDetails");

                        string CostHeadXml = null;
                        if (survey.CostListHidden != null)
                        {
                            System.Xml.Linq.XNode node1 = JsonConvert.DeserializeXNode(survey.CostListHidden, "CostHeadwiseDetails");
                            CostHeadXml = node1.ToString();

                        }
                        //xml.OuterXml;
                        string ServiceHeadXml = node.ToString();

                        conn.AddCommand("[Survey].[AddEditSurvey]", QueryType.Procedure);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AddEditDelete", SqlDbType.VarChar, 1, ParameterDirection.Input, "E");
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_surveyID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, (survey.SurveyId <= 0 ? -1 : survey.SurveyId));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqDetailID", SqlDbType.BigInt, 0, ParameterDirection.Input, survey.EnquiryDetail.EnqDetailID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyerID", SqlDbType.Int, 0, ParameterDirection.Input, survey.SurveyConductedById);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.SurveyDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveryTime", SqlDbType.Time, 7, ParameterDirection.Input, survey.SurveyDateTime);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PackDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.SurveyDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfDays", SqlDbType.Int, 0, ParameterDirection.Input, (survey.NoOfDays));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoadDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.LoadDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_QuoteSubmissionDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.QuotationSubmissionDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ReqDeliveryDate", SqlDbType.DateTime, 0, ParameterDirection.Input, (survey.DeliveryDate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsurBy", SqlDbType.VarChar, 50, ParameterDirection.Input, (survey.InsuredById));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ApproxInsuAmt", SqlDbType.BigInt, 0, ParameterDirection.Input, (survey.InsuredAmount));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InsCurrrencyID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.InsCurrencyID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Competetion", SqlDbType.Bit, 0, ParameterDirection.Input, (survey.IsCompition));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompetitorId", SqlDbType.Int, 0, ParameterDirection.Input, (0));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Competitortext", SqlDbType.VarChar, 100, ParameterDirection.Input, (survey.CompititorName));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateAvailable", SqlDbType.Bit, 0, ParameterDirection.Input, (survey.RateAvailable));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AvailableRate", SqlDbType.BigInt, 0, ParameterDirection.Input, (survey.AvailableRate));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RateCurrencyID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.RateCurrencyID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MonitoryEntitlement", SqlDbType.BigInt, 0, ParameterDirection.Input, (survey.MonitoryEntitlement));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MonitoryEntCurrID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.MonitoryEntCurrID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.VarChar, 500, ParameterDirection.Input, CSubs.PSafeValue(survey.SurveyRemarks));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Isactive", SqlDbType.Bit, 0, ParameterDirection.Input, true);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VolumeUnitID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_VolumeUnitID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TobePackedVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Volume_TobePackedValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NetVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Volume_NetValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GrossVol", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Volume_GrossValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WeightUnitID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_WeightUnitID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NetWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Weight_NetValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GrossWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Weight_GrossValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ACWTWt", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_Weight_ACWTValue));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LooseCased", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.SurveyLooseCased));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DensityFact", SqlDbType.Float, 0, ParameterDirection.Input, (survey.shipmentDetail.SurveyDensityFact));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LCLorFCL", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.SurveyLCLorFCL));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ContainerSizeID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.Survey_ContainerID));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAdd", SqlDbType.VarChar, 10000, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgAdd));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgAdd2", SqlDbType.VarChar, 10000, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgAdd2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgCityID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.OrgCityID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPhone", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgEmailID", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgEmail));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OrgPin", SqlDbType.VarChar, 15, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.OrgPin));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAdd", SqlDbType.VarChar, 10000, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.DestAdd));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestAdd2", SqlDbType.VarChar, 10000, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.DestAdd2));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestCityID", SqlDbType.Int, 0, ParameterDirection.Input, (survey.shipmentDetail.DestCityID));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPhone", SqlDbType.VarChar, 30, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.DestPhone));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Email", SqlDbType.VarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.Email));
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestPin", SqlDbType.VarChar, 15, ParameterDirection.Input, CSubs.PSafeValue(survey.shipmentDetail.DestPin));

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyDetailsXML", SqlDbType.Xml, -1, ParameterDirection.Input, ServiceHeadXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostHeadWiseInfo", SqlDbType.Xml, -1, ParameterDirection.Input, CostHeadXml);

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
                                return true;
                            }
                            else
                            {
                                return false;
                            }

                        }
                        else
                            throw new ArgumentException(conn.ErrorMessage);

                    }
                    else
                        throw new ArgumentException(conn.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyDAL", "Update", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            // return true;
        }

        public bool DeleteById(int id, out string result)
        {
            result = string.Empty;

            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {

                        conn.AddCommand("Comm.surveyDelete", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_surveyID", SqlDbType.Int, 0, ParameterDirection.Input, id);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MESSAGE", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {

                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_MESSAGE"));

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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyDAL", "DeleteById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

        }

        public DataSet GetDetailById(int LoginID, int? EnqDetailID, int? surveyID = -1)
        {
            DataSet surveyDetailDt = new DataSet();

            try
            {
                string query = string.Format("EXEC [Survey].[GetSurveyDetailsForDisplay] @SP_EnqDetailID={0}, @SP_surveyID={1},@SP_LoginID={2}",
                 CSubs.QSafeValue(Convert.ToString(EnqDetailID)), CSubs.QSafeValue(Convert.ToString(surveyID)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                surveyDetailDt = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "surveyDAL", "GetDetailById", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return surveyDetailDt;

        }

        public IQueryable<SurveyData> GetsurveyList(DateTime? FromDate, DateTime? Todate, string Shipper, string searchType, string search, int LoggedinUserID, int? EnqID = -1, int? CompId = -1, int? surveyID = 1, int? RMCBuss = 0)
        {
            try
            {
                string query = string.Format("exec [Survey].[GetSurveyForGrid]  @SP_FromDate={0},@SP_ToDate={1},@SP_EnqID={2},@SP_CompId={3},@SP_surveyID={4},@SP_SearchType={5},@SP_SearchNo={6},@SP_LoginID={7},@SP_IsRMCBuss={8},@SP_Shipper={9}",
                CSubs.QSafeValue(FromDate == null ? null : ((DateTime)FromDate).ToString("dd-MMM-yyyy")),
                CSubs.QSafeValue(Todate == null ? null : ((DateTime)Todate).ToString("dd-MMM-yyyy")),
                CSubs.QSafeValue(Convert.ToString(EnqID)),
                CSubs.QSafeValue(Convert.ToString(CompId)),
                CSubs.QSafeValue(Convert.ToString(surveyID)),
                CSubs.QSafeValue(searchType),
                CSubs.QSafeValue(search, EValueType.INT),
                Convert.ToString(LoggedinUserID),
                Convert.ToString(RMCBuss),
                CSubs.QSafeValue(Shipper)
                );

                DataTable dataTable = CSubs.GetDataTable(query);

                IQueryable<SurveyData> data = null;

                if (dataTable != null)
                {
                    var result = (from rw in dataTable.AsEnumerable()
                                  select new SurveyData()
                                  {
                                      SurveyId = rw["surveyID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["surveyID"]),
                                      EnqId = Convert.ToInt64(rw["EnqID"]),
                                      EnqNo = Convert.ToString(rw["EnqNo"]),
                                      EnqDetailId = Convert.ToInt64(rw["EnqDetailID"]),
                                      EnqshpID = Convert.ToInt64(rw["EnqshpID"]),
                                      ServiceLine = Convert.ToString(rw["ServiceLine"]),
                                      GoodsDesc = Convert.ToString(rw["GoodsDesc"]),
                                      SurveyDate = rw["SurveyDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["SurveyDate"]),
                                      SurveyDateTime = rw["SurveryTime"] == DBNull.Value ? (TimeSpan?)null : Convert.ToDateTime(rw["SurveryTime"]).TimeOfDay,
                                      SurveyRemark = Convert.ToString(rw["SurveyRemark"]),
                                      ShipperName = Convert.ToString(rw["ShipperName"]),
                                      Origin = Convert.ToString(rw["Origin"]),
                                      Destination = Convert.ToString(rw["Destination"]),
                                      BussLineName = Convert.ToString(rw["BussLineName"]),
                                      EnqDate = Convert.ToDateTime(rw["EnqDate"]),
                                      EnqReceivedbyName = Convert.ToString(rw["EnqReceivedbyName"]),
                                      AgentName = Convert.ToString(rw["AgentName"]),
                                      AccountName = Convert.ToString(rw["AccountName"]),
                                      EnqReceivedDate = rw["EnqReceivedDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["EnqReceivedDate"]),
                                      Status = Convert.ToString(rw["Status"]),
                                      EnqRemark = Convert.ToString(rw["EnqRemark"]),
                                      RevenueBrName = Convert.ToString(rw["RevenueBrName"]),
                                      Mode = Convert.ToString(rw["Mode"]),
                                      SurveyConductedByName = Convert.ToString(rw["SurveyByName"]),
                                      CompletedStatus = Convert.ToString(rw["CompletedStatus"]),
                                      CompletedBy = Convert.ToString(rw["CompletedBy"]),
                                      //CompletedDate = rw["CompletedDate"]== DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rw["CompletedDate"]),
                                      IsEditVisible = LoggedinUserID == (rw["SurveyByID"] == DBNull.Value ? 0 : Convert.ToInt32(rw["SurveyByID"])) ? true : false,
                                      ApproveStatus = Convert.ToString(rw["ApproveStatus"])
                                  }).ToList();


                    data = result.AsQueryable<SurveyData>();
                }



                return data;

            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "surveyDAL", "GetsurveyList", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            //return null;

        }

        public SurveyViewModel GetSurveyCostHeads(int SurveyID, int LoggedinUserID)
        {

            SurveyViewModel survey = new SurveyViewModel();

            try
            {
                string query = string.Format("exec [Survey].[GetSurveyCostheadsForGrid] @SP_SurveyID={0},@SP_LoginID={1}",
                CSubs.QSafeValue(Convert.ToString(SurveyID)),
                Convert.ToString(LoggedinUserID)
                );

                DataSet dataSet = CSubs.GetDataSet(query);

                survey.shipmentDetail = (from rw in dataSet.Tables[0].AsEnumerable()
                                         select new ShipmentDetail()
                                         {
                                             SurveyID = rw["surveyID"] == DBNull.Value ? (Int64?)null : Convert.ToInt64(rw["surveyID"]),
                                             SurveyLooseCased = Convert.ToString(rw["SurveyLooseCased"]),
                                             SurveyDensityFact = rw["SurveyDensityFact"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyDensityFact"]),
                                             SurveyLCLorFCL = Convert.ToString(rw["SurveyLCLorFCL"]),
                                             Survey_VolumeUnitID = rw["SurveyVolumeUnitID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["SurveyVolumeUnitID"]),
                                             DestAdd = Convert.ToString(rw["DestAdd"]),
                                             DestCityID = rw["DestCityID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["DestCityID"]),
                                             DestPhone = Convert.ToString(rw["DestPhone"]),
                                             Email = Convert.ToString(rw["Email"]),
                                             OrgAdd = Convert.ToString(rw["OrgAdd"]),
                                             OrgCityID = rw["OrgCityID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["OrgCityID"]),
                                             OrgPhone = Convert.ToString(rw["OrgPhone"]),
                                             Remarks = "",
                                             Survey_ContainerID = rw["SurveyContainerSizeID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["SurveyContainerSizeID"]),
                                             Survey_Volume_GrossValue = rw["SurveyGrossVol"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyGrossVol"]),
                                             Survey_Volume_NetValue = rw["SurveyNetVol"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyNetVol"]),
                                             Survey_Volume_TobePackedValue = rw["SurveyTobePackedVol"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyTobePackedVol"]),
                                             Survey_WeightUnitID = rw["SurveyWeightUnitID"] == DBNull.Value ? (Int32?)null : Convert.ToInt32(rw["SurveyWeightUnitID"]),
                                             Survey_Weight_ACWTValue = rw["SurveyACWTWt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyACWTWt"]),
                                             Survey_Weight_GrossValue = rw["SurveyGrossWt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyGrossWt"]),
                                             Survey_Weight_NetValue = rw["SurveyNetWt"] == DBNull.Value ? (decimal?)null : Convert.ToDecimal(rw["SurveyNetWt"]),
                                         }).First();

                if (dataSet.Tables[1] != null && dataSet.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow item in dataSet.Tables[1].Rows)
                    {
                        ServiceDetail service = new ServiceDetail();
                        service.SurveyDetailsID = Convert.ToInt32(item["SurveyDetailsID"]);
                        service.CostHeadID = Convert.ToInt32(item["CostHeadID"]);
                        service.CostHeadName = Convert.ToString(item["CostHeadName"]);
                        service.RateCompID = Convert.ToInt32(item["RateCompID"]);
                        service.RateCompName = Convert.ToString(item["RateComponentName"]);
                        service.RemarksOnCostHead = Convert.ToString(item["RemarksOnCostHead"]);
                        survey.ServiceDetailList.Add(service);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoggedinUserID), "surveyDAL", "GetSurveyCostHeads", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return survey;
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public DataTable GetEnquiryDetailAddress(int LoginID, Int64 EnqDetailID, bool IsOrigin, bool IsDest)
        {
            try
            {

                string query = string.Format("exec [Survey].[GetAddressFromEnq] @SP_EnqDetailID={0},@SP_IsOrgAdd={1},@SP_IsDestAdd={2},@SP_LoginID={3}",
                CSubs.QSafeValue(Convert.ToString(EnqDetailID)),
                CSubs.QSafeValue(Convert.ToString(IsOrigin)),
                CSubs.QSafeValue(Convert.ToString(IsDest)),
                Convert.ToString(LoginID)
                );

                return CSubs.GetDataTable(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "surveyDAL", "GetEnquiryDetailAddress", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetCopyEnqSurvey(int LoginID, Int64 EnqDetailID, Int64 CopyEnqDetailID)
        {
            DataSet surveyDetailDt = new DataSet();

            try
            {
                string query = string.Format("EXEC [Survey].[GetCopySurveyDetailDisplay] @SP_EnqDetailID={0}, @SP_CopyEnqDetailID={1},@SP_LoginID={2}",
                 CSubs.QSafeValue(Convert.ToString(EnqDetailID)), CSubs.QSafeValue(Convert.ToString(CopyEnqDetailID)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                surveyDetailDt = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "surveyDAL", "GetCopyEnqSurvey", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return surveyDetailDt;
        }

        public JobDocument GetVoxmeReport(int LoginID, Int64 EnqID, string EnqNo)
        {
            JobDocument document = new JobDocument();

            DataTable dt = new DataTable();
            string FileName = "";
            string saved = string.Empty;
            try
            {
                string query = string.Format("[Survey].[GetVoxmiReportFileDownload] @sp_enqID={0},@SP_EnqNo={1},@SP_LoginID={2}", EnqID, CSubs.QSafeValue(EnqNo), LoginID);
                dt = CSubs.GetDataTable(query);

                if (dt != null && dt.Rows.Count > 0)
                {
                    document.FileName = dt.Rows[0]["DocFileName"].ToString();
                    document.FilePath = dt.Rows[0]["DocFilePath"].ToString();
                    saved = dt.Rows[0]["Saved"].ToString();

                    if (saved == "N")
                    {
                        string FilePath = System.Configuration.ConfigurationManager.AppSettings["JobDMS"].ToString();
                        FileName = document.FileName;
                        string Extension = Path.GetExtension(FileName);
                        string result = string.Empty;
                        string VoxmiServerPath = System.Configuration.ConfigurationManager.AppSettings["VoxmeFolder"].ToString();
                        VoxmiServerPath = Path.Combine(VoxmiServerPath, FileName);

                        FileInfo fi = new FileInfo(VoxmiServerPath);
                        bool exists = fi.Exists;

                        if (exists)
                        {

                            using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, System.Data.IsolationLevel.ReadCommitted))
                            {
                                if (conn.Connect())
                                {

                                    conn.AddCommand("[Survey].[AddVoxmiReportDocument]", QueryType.Procedure);
                                    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Description", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue("Voxme Report"));
                                    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EnqID", SqlDbType.BigInt, 0, ParameterDirection.Input, EnqID);
                                    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput, CSubs.PSafeValue(FilePath));
                                    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileName", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(FileName));
                                    conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Ext", SqlDbType.NVarChar, 50, ParameterDirection.Input, CSubs.PSafeValue(Extension));
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
                                            FilePath = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_FilePath"));

                                            try
                                            {
                                                // Create the file and clean up handles.
                                                //using (FileStream fs = File.Create(VoxmiServerPath)) { }
                                                // Ensure that the target does not exist.
                                                //File.Delete(FilePath);
                                                // Copy the file.
                                                //File.Copy(VoxmiServerPath, FilePath);
                                                System.IO.File.Copy(VoxmiServerPath, FilePath, true);
                                                conn.CommitTransaction();

                                                document.FileName = FileName;
                                                document.FilePath = FilePath;

                                                return document;

                                            }
                                            catch (Exception ex)
                                            {
                                                conn.RollbackTransaction();
                                                CSubs.LogError(new SurveyDAL(), "VoxmeSaveLocal", ex.ToString());
                                                throw new Exception("Unable to save File");
                                            }

                                        }
                                        else
                                        {
                                            conn.RollbackTransaction();
                                            throw new Exception("File not found");

                                        }
                                    }
                                    else
                                    {
                                        conn.RollbackTransaction();
                                        throw new Exception(conn.ErrorMessage);
                                    }

                                }
                                else
                                    throw new Exception(conn.ErrorMessage);
                            }

                        }
                        else
                        {

                            document.FilePath = VoxmiServerPath;
                            CSubs.LogError(new SurveyDAL(), "VoxmeSaveLocal", VoxmiServerPath + " : not able access file");
                        }



                    }


                    return document;
                }


            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "surveyDAL", "GetVoxmeReport", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }

            return document;

        }

        public DataSet GetCommItemDetails(int LoginID, Int64? surveyID)
        {
            DataSet CommItemDetailsDs = new DataSet();
            try
            {
                string query = string.Format("EXEC [Survey].[GetCoomerSurItems] @SP_surveyID={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(surveyID)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                CommItemDetailsDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "surveyDAL", "GetCommItemDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return CommItemDetailsDs;
        }

        public bool SaveCommItemDetails(SurveyViewModel survey, out string result)
        {
            result = string.Empty;
            try
            {
                using (App_Code.CDALSQL conn = new App_Code.CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string SurveyItemDetailXml = null;
                        if (!string.IsNullOrEmpty(survey.SurveyCommItemListHidden))
                        {
                            System.Xml.Linq.XNode node = JsonConvert.DeserializeXNode(survey.SurveyCommItemListHidden, "SurveyItemDetails");
                            SurveyItemDetailXml = node.ToString();
                        }

                        conn.AddCommand("[Survey].[AddEditCoomerSurItems]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, UserSession.GetUserSession().LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_surveyID", SqlDbType.BigInt, 0, ParameterDirection.Input, survey.SurveyId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SurveyItemDetails", SqlDbType.Xml, 0, ParameterDirection.Input, SurveyItemDetailXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.NVarChar, 500, ParameterDirection.Output);
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
                throw new DataAccessException(Convert.ToString(UserSession.GetUserSession().LoginID), "surveyDAL", "SaveCommItemDetails", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}