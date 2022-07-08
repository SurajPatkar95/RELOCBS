using Newtonsoft.Json;
using RELOCBS.App_Code;
using RELOCBS.Common.ExceptionHandling;
using RELOCBS.Entities;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace RELOCBS.DAL.WOSJobOpening
{
    public class WOSJobOpeningDAL
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

        public DataSet GetWOSJobOpeningList(DateTime? FromDate, DateTime? ToDate, int? LoginID, int? CompId, bool? IsJobDate, bool? IsRMCBuss, string AssigneeName,
            string SearchType, string Search)
        {
            DataSet WOSJobOpeningDs = null;
            try
            {
                string query = string.Format("EXEC [WOS].[GetMoveForGrid] @SP_FromDate={0}, @SP_ToDate={1}, @SP_LoginID={2}, @SP_CompId={3}, @SP_IsJobDate={4}, " +
                    "@Sp_IsRMCBuss={5}, @SP_AssigneeName={6}, @SP_FilterName={7}, @SP_FilterValue={8}",
                    CSubs.QSafeValue(Convert.ToString(FromDate)), CSubs.QSafeValue(Convert.ToString(ToDate)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                    CSubs.QSafeValue(Convert.ToString(CompId)), CSubs.QSafeValue(Convert.ToString(IsJobDate)), CSubs.QSafeValue(Convert.ToString(IsRMCBuss)),
                    CSubs.QSafeValue(Convert.ToString(AssigneeName)), CSubs.QSafeValue(Convert.ToString(SearchType)), CSubs.QSafeValue(Convert.ToString(Search)));

                WOSJobOpeningDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "GetWOSJobOpeningList", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return WOSJobOpeningDs;
        }

        public DataSet GetWOSJobDetailsById(Int64 WOSMoveID, int LoginID)
        {
            DataSet WOSJobOpeningDs = new DataSet();
            try
            {
                string query = string.Format("EXEC [WOS].[GetJobDetails] @SP_WOSMoveID={0},@SP_LoginID={1}",
                    CSubs.QSafeValue(Convert.ToString(WOSMoveID)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                WOSJobOpeningDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "GetWOSJobDetailsById", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return WOSJobOpeningDs;
        }

        public bool SaveWOSJobDetails(Entities.WOSJobOpening WOSJobOpening, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[CreateJob]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WOSMoveID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, WOSJobOpening.WOSMoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRMCBus", SqlDbType.Bit, 0, ParameterDirection.Input, WOSJobOpening.IsRMCBus);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AssigneeFName", SqlDbType.VarChar, 100, ParameterDirection.Input, WOSJobOpening.AssigneeFName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AssigneeLName", SqlDbType.VarChar, 100, ParameterDirection.Input, WOSJobOpening.AssigneeLName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FromCityID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSJobOpening.FromCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ToCityID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSJobOpening.ToCityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileNo", SqlDbType.VarChar, 100, ParameterDirection.Input, WOSJobOpening.FileNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WONumber", SqlDbType.VarChar, 100, ParameterDirection.Input, WOSJobOpening.WONumber);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OldJobNo", SqlDbType.VarChar, 50, ParameterDirection.Input, WOSJobOpening.OldJobNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TentativeBillingMonth", SqlDbType.DateTime, 0, ParameterDirection.Input, WOSJobOpening.TentativeBillingMonth);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ClientID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSJobOpening.ClientID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AccountID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSJobOpening.AccountID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Isactive", SqlDbType.Bit, 0, ParameterDirection.Input, WOSJobOpening.IsActive);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CompanyID", SqlDbType.Int, 0, ParameterDirection.Input, WOSJobOpening.CompanyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceLineID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSJobOpening.ServiceLineID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RevBranchID", SqlDbType.Int, 0, ParameterDirection.Input, WOSJobOpening.RevBranchID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobBrID", SqlDbType.Int, 0, ParameterDirection.Input, WOSJobOpening.JobBrID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SDId", SqlDbType.Int, 0, ParameterDirection.Input, WOSJobOpening.SDId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SRId", SqlDbType.Int, 0, ParameterDirection.Input, WOSJobOpening.SRId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_KAM", SqlDbType.Int, 0, ParameterDirection.Input, WOSJobOpening.KAM);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobRemarks", SqlDbType.VarChar, 1000, ParameterDirection.Input, WOSJobOpening.JobRemarks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.Int, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.VarChar, 100, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                WOSJobOpening.WOSMoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_WOSMoveID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "SaveWOSJobDetails", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetAssigneeDetailsById(int LoginID, Int64 WOSMoveID)
        {
            DataSet AssigneeDt = new DataSet();
            try
            {
                string query = string.Format("EXEC [WOS].[GetTransfereeDetailsForDisplay] @SP_LoginID={0}, @SP_WOSMoveID={1}", LoginID, WOSMoveID);
                AssigneeDt = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "GetAssigneeDetailsById", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return AssigneeDt;
        }

        public bool AddEditAssignee(Assignee AssigneeObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        XNode node = JsonConvert.DeserializeXNode(AssigneeObj.ChildDetailsListHidden, "ChildDetails");
                        string ChildDetailsXml = node.ToString();

                        conn.AddCommand("[WOS].[AddEditTransferee]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WOSMoveID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, AssigneeObj.WOSMoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransfereeId", SqlDbType.BigInt, 0, ParameterDirection.Input, AssigneeObj.TransfereeId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LastName", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.LastName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FirstName", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.FirstName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MiddleName", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.MiddleName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaidenName", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.MaidenName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Title", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.AssigneeTitleId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransfereeRef", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.AssigneeRef);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GenderId", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.GenderId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSmoker", SqlDbType.Bit, 0, ParameterDirection.Input, AssigneeObj.IsSmoker);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BirthDate", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.BirthDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Languages", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.Languages);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Religion", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.Religion);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailId", SqlDbType.NVarChar, 300, ParameterDirection.Input, AssigneeObj.EmailId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address1", SqlDbType.VarChar, 500, ParameterDirection.Input, AssigneeObj.Address1);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Address2", SqlDbType.VarChar, 500, ParameterDirection.Input, AssigneeObj.Address2);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CityID", SqlDbType.BigInt, 0, ParameterDirection.Input, AssigneeObj.CityID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PINCode", SqlDbType.VarChar, 25, ParameterDirection.Input, AssigneeObj.PINCode);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PassportNo", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.PassportNo);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PassportIssuedBy", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.PassportIssuedBy);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PassportIssueDate", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.PassportIssueDate);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PassportExpiryDate", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.PassportExpiryDate);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryId_Nationality", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.CountryId_Nationality);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryId_OtherNationality", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.CountryId_OtherNationality);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PlaceOfBirth", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.PlaceOfBirth);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryId_CountryOfBirth", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.CountryId_CountryOfBirth);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DrivingLicenseNumber", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.DrivingLicenseNumber);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryId_DrivingLicenseIssuedBy", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.CountryId_DrivingLicenseIssuedBy);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DrivingLicenseIssueDate", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.DrivingLicenseIssueDate);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DrivingLicenseExpiryDate", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.DrivingLicenseExpiryDate);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IDCardNo", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.IDCardNo);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IDCardIssuedBy", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.IDCardIssuedBy);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IDCardIssueDate", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.IDCardIssueDate);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IDCardExpiryDate", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.IDCardExpiryDate);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VAT", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.VAT);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SSN", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.SSN);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TransfereeWorkReference", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.TransfereeWorkReference);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LastName_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.LastName_Partner);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FirstName_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.FirstName_Partner);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MiddleName_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.MiddleName_Partner);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaidenName_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.MaidenName_Partner);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Title_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.Title_Partner);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EmailId_Partner", SqlDbType.NVarChar, 300, ParameterDirection.Input, AssigneeObj.EmailId_Partner);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_GenderId_Partner", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.GenderId_Partner);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSmoker_Partner", SqlDbType.Bit, 0, ParameterDirection.Input, AssigneeObj.IsSmoker_Partner);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BirthDate_Partner", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.BirthDate_Partner);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Languages_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.Languages_Partner);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Religion_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.Religion_Partner);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Occupation_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.Occupation_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PassportNo_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.PassportNo_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PassportIssuedBy_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.PassportIssuedBy_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PassportIssueDate_Partner", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.PassportIssueDate_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PassportExpiryDate_Partner", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.PassportExpiryDate_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryId_Nationality_Partner", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.CountryId_Nationality_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryId_OtherNationality_Partner", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.CountryId_OtherNationality_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PlaceOfBirth_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.PlaceOfBirth_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryId_CountryOfBirth_Partner", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.CountryId_CountryOfBirth_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DrivingLicenseNumber_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.DrivingLicenseNumber_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryId_DrivingLicenseIssuedBy_Partner", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.CountryId_DrivingLicenseIssuedBy_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DrivingLicenseIssueDate_Partner", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.DrivingLicenseIssueDate_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DrivingLicenseExpiryDate_Partner", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.DrivingLicenseExpiryDate_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IDCardNo_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.IDCardNo_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IDCardIssuedBy_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.IDCardIssuedBy_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IDCardIssueDate_Partner", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.IDCardIssueDate_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IDCardExpiryDate_Partner", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.IDCardExpiryDate_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SSN_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.SSN_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VAT_Partner", SqlDbType.NVarChar, 125, ParameterDirection.Input, AssigneeObj.VAT_Partner);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PrivacyAgreementAcceptanceDate", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.PrivacyAgreementAcceptanceDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaritalStatusId", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.MaritalStatusId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MarriageDate", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.MarriageDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsMarriageCertAvailable", SqlDbType.Bit, 0, ParameterDirection.Input, AssigneeObj.IsMarriageCertAvailable);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsPetsMoving", SqlDbType.Bit, 0, ParameterDirection.Input, AssigneeObj.IsPetsMoving);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PetsDescription", SqlDbType.NVarChar, 300, ParameterDirection.Input, AssigneeObj.PetsDescription);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FamilyDetails", SqlDbType.NVarChar, 300, ParameterDirection.Input, AssigneeObj.FamilyDetails);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsMovingTogether", SqlDbType.Bit, 0, ParameterDirection.Input, AssigneeObj.IsMovingTogether);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NextOfKinDetails", SqlDbType.NVarChar, 300, ParameterDirection.Input, AssigneeObj.NextOfKinDetails);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfChildren", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.NoOfChildren);

                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SchoolTypeId", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.SchoolTypeId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfBedrooms", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.NoOfBedrooms);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfBathrooms", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.NoOfBathrooms);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfLivingRooms", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.NoOfLivingRooms);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfDiningRooms", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.NoOfDiningRooms);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LocationId", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.LocationId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_MaxCommutingTime", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.MaxCommutingTime);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CountryId_DrivingLicenseIssuedBy", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.CountryId_DrivingLicenseIssuedBy);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_VehiclesImportLeasePurchase", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.VehiclesImportLeasePurchase);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_NoOfVehicles", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.NoOfVehicles);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TypeOfVehicles", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.TypeOfVehicles);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RentOrPurchase", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.RentOrPurchase);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_HouseOrApartment", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.HouseOrApartment);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsHouse", SqlDbType.Bit, 0, ParameterDirection.Input, AssigneeObj.IsHouse);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsApartment", SqlDbType.Bit, 0, ParameterDirection.Input, AssigneeObj.IsApartment);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OtherHousingPreference", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.OtherHousingPreference);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsAllowanceIncludesUtilities", SqlDbType.Bit, 0, ParameterDirection.Input, AssigneeObj.IsAllowanceIncludesUtilities);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FurnishedOrUnfurnished", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.FurnishedOrUnfurnished);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsAllowanceIncludesFurniture", SqlDbType.Bit, 0, ParameterDirection.Input, AssigneeObj.IsAllowanceIncludesFurniture);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsPreferredCommunities", SqlDbType.Bit, 0, ParameterDirection.Input, AssigneeObj.IsPreferredCommunities);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ByCarOrPublicTransport", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.ByCarOrPublicTransport);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsDriversLicenseAvailable", SqlDbType.Bit, 0, ParameterDirection.Input, AssigneeObj.IsDriversLicenseAvailable);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SpecificSchoolDetails", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.SpecificSchoolDetails);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SpecialSchoolRequirements", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.SpecialSchoolRequirements);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SpecialHealthProblem", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.SpecialHealthProblem);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DestinationServicesDates", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.DestinationServicesDates);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EssentialFeatures", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.EssentialFeatures);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_HotelName", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.HotelName);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_HotelContactNo", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.HotelContactNo);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CurrencyId", SqlDbType.Int, 0, ParameterDirection.Input, AssigneeObj.CurrencyId);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_PreferredCommunitiesDetails", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.PreferredCommunitiesDetails);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WithinEasyReachOfTheProperty", SqlDbType.NVarChar, 500, ParameterDirection.Input, AssigneeObj.WithinEasyReachOfTheProperty);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_HomeFindingVisitDate", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.HomeFindingVisitDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ArrivalDateTime", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.ArrivalDate);
                        //conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ArrivalTime", SqlDbType.DateTime, 0, ParameterDirection.Input, AssigneeObj.ArrivalTime);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AllowancePerMonth", SqlDbType.Float, 0, ParameterDirection.Input, AssigneeObj.AllowancePerMonth);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AllowancePerMonthWithUtilities", SqlDbType.Float, 0, ParameterDirection.Input, AssigneeObj.AllowancePerMonthWithUtilities);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AllowancePerMonthWithFurniture", SqlDbType.Float, 0, ParameterDirection.Input, AssigneeObj.AllowancePerMonthWithFurniture);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChildDetailsXml", SqlDbType.Xml, 0, ParameterDirection.Input, ChildDetailsXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsFinalSubmit", SqlDbType.Bit, 0, ParameterDirection.Input, AssigneeObj.IsFinalSubmit);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.Int, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                AssigneeObj.WOSMoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_WOSMoveID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "AddEditAssignee", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetChatting(int LoginID, Int64 WOSMoveID, string EMPorAssingee = "A")
        {
            DataSet ChatDt = new DataSet();
            try
            {
                string query = string.Format("EXEC [WOS].[GetChatting] @SP_WOSMoveID={0}, @SP_EMPorAssingee={1}, @SP_LoginID={2}", WOSMoveID, EMPorAssingee, LoginID);
                ChatDt = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "GetChatting", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return ChatDt;
        }

        public bool AddEditChatting(ChatDetails ConversationObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[AddEditChatting]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WOSMoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, ConversationObj.WOSMoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_EMPorAssingee", SqlDbType.VarChar, 1, ParameterDirection.Input, ConversationObj.ChatBy);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ChatMsg", SqlDbType.VarChar, 2000, ParameterDirection.Input, ConversationObj.ChatMsg);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.Int, 0, ParameterDirection.ReturnValue);
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "AddEditChatting", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetTaskInfo(int LoginID, Int64 WOSMoveID, string EMPorAssingee = "A")
        {
            DataSet TaskDt = new DataSet();
            try
            {
                string query = string.Format("EXEC [WOS].[GetTaskInfo] @SP_WOSMoveID={0}, @SP_EmpOrAssignee={1}", WOSMoveID, EMPorAssingee);
                TaskDt = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "GetTaskInfo", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return TaskDt;
        }

        public bool AddEditTaskInfo(TaskDetails TaskDetailsObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[AddEditTaskInfo]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WOSMoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, TaskDetailsObj.WOSMoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TaskID", SqlDbType.Int, 0, ParameterDirection.Input, TaskDetailsObj.TaskMasterID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TaskRemarks", SqlDbType.VarChar, 2000, ParameterDirection.Input, TaskDetailsObj.Remarks);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TaskSchDate", SqlDbType.DateTime, 0, ParameterDirection.Input, TaskDetailsObj.ScheduleDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_TaskActDate", SqlDbType.DateTime, 0, ParameterDirection.Input, TaskDetailsObj.ActualDate);

                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.Int, 0, ParameterDirection.ReturnValue);
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "AddEditTaskInfo", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertRequestDocsData(Assignee Assignee, int LoginID, string Url, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        string DocNameIDListXml = string.Empty;
                        if (!string.IsNullOrEmpty(Assignee.WOSJobDocUpload.DocNameIDListHidden))
                        {
                            XNode node = JsonConvert.DeserializeXNode(Assignee.WOSJobDocUpload.DocNameIDListHidden, "DocNameIDLists");
                            DocNameIDListXml = node.ToString();
                        }

                        conn.AddCommand("[WOS].[AddEditRequestDocs]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WOSMoveID", SqlDbType.Int, 0, ParameterDirection.InputOutput, Assignee.WOSMoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Url", SqlDbType.NVarChar, 1000, ParameterDirection.InputOutput, Url);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocNameIDListXml", SqlDbType.Xml, -1, ParameterDirection.Input, DocNameIDListXml);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@Out_Message"));

                            if (ReturnStatus == 0)
                            {
                                Assignee.WOSMoveID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_WOSMoveID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "InsertRequestDocsData", RELOCBS.Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool InsertDocument(WOSJobDocUpload WOSJobDocUploadObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                string FileName = string.Empty;
                string Extension = string.Empty;
                string FilePath = System.Configuration.ConfigurationManager.AppSettings["JobDMS"].ToString();
                int FileUploadCount = 0;

                if (WOSJobDocUploadObj.file != null)
                {
                    using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString(), true, IsolationLevel.ReadCommitted))
                    {
                        if (conn.Connect())
                        {
                            conn.AddCommand("[WOS].[AddEditMoveDocFileUpload]", QueryType.Procedure);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WOSMoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSJobDocUploadObj.WOSMoveID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocTypeID", SqlDbType.Int, 0, ParameterDirection.Input, WOSJobDocUploadObj.DocTypeID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_DocNameID", SqlDbType.Int, 0, ParameterDirection.Input, WOSJobDocUploadObj.DocNameID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Description", SqlDbType.NVarChar, 250, ParameterDirection.Input, CSubs.PSafeValue(WOSJobDocUploadObj.DocDescription));
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsShowToAssignee", SqlDbType.Bit, 0, ParameterDirection.Input, WOSJobDocUploadObj.IsShowToAssignee);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobDocTypeId", SqlDbType.Int, 0, ParameterDirection.Input, WOSJobDocUploadObj.JobDocTypeId);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Remarks", SqlDbType.NVarChar, 250, ParameterDirection.Input, WOSJobDocUploadObj.Remarks);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LOGINID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.SmallInt, 0, ParameterDirection.ReturnValue);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@Out_Message", SqlDbType.NVarChar, 500, ParameterDirection.Output);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IdentificationID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSJobDocUploadObj.ID);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FilePath", SqlDbType.NVarChar, 500, ParameterDirection.InputOutput);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FileName", SqlDbType.NVarChar, 250, ParameterDirection.Input);
                            conn.AddParameter(ParameterOF.PROCEDURE, "@SP_Ext", SqlDbType.NVarChar, 50, ParameterDirection.Input);

                            foreach (var file in WOSJobDocUploadObj.file)
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "InsertDocument", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetCostSheetForJob(Int64? WOSMoveID, int? RevCurrID, int? CostCurrID, int? LoginID)
        {
            DataSet CostSheetForJobDs = null;
            try
            {
                string query = string.Format("EXEC [WOS].[GetCostSheetForJob] @SP_WOSMoveID={0}, @SP_RevCurrID={1}, @SP_CostCurrID={2}, @SP_LoginID={3}",
                    CSubs.QSafeValue(Convert.ToString(WOSMoveID)), CSubs.QSafeValue(Convert.ToString(RevCurrID)), CSubs.QSafeValue(Convert.ToString(CostCurrID)),
                    CSubs.QSafeValue(Convert.ToString(LoginID)));

                CostSheetForJobDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "GetCostSheetForJob", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return CostSheetForJobDs;
        }

        public bool SaveCostSheet(Entities.WOSCustomer WOSCustomerObj, int LoginID, out string result)
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
                            new XElement("RevenueAmount", m.BillRevenueAmount),
                            new XElement("CostAmount", m.BillCostAmount),
                            new XElement("IsChecked", m.IsChecked),
                            new XElement("ToBill", m.ToBill)

                            ));
                        string ServiceListXmlString = ServiceLists.HasElements ? Convert.ToString(ServiceLists) : null;

                        conn.AddCommand("[WOS].[AddEditCostSheet]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobCostMasterID", SqlDbType.BigInt, 0, ParameterDirection.InputOutput, WOSCustomerObj.JobCostMasterID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WOSMoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSCustomerObj.WOSMoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RevnueCurrID", SqlDbType.Int, 0, ParameterDirection.Input, WOSCustomerObj.RevenueCurrencyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostCurrID", SqlDbType.Int, 0, ParameterDirection.Input, WOSCustomerObj.CostCurrencyID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_RevToCostCurrConverRate", SqlDbType.Float, 0, ParameterDirection.Input, WOSCustomerObj.RevToCostCurrConverRate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_CostToRevCurrConverRate", SqlDbType.Float, 0, ParameterDirection.Input, WOSCustomerObj.CostToRevCurrConverRate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AuditFee", SqlDbType.Float, 0, ParameterDirection.Input, WOSCustomerObj.AuditFee);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_AdminFee", SqlDbType.Float, 0, ParameterDirection.Input, WOSCustomerObj.AdminFee);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_BillCommissionPercent", SqlDbType.Float, 0, ParameterDirection.Input, WOSCustomerObj.BillCommissionPercent);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ServiceList", SqlDbType.Xml, 0, ParameterDirection.Input, ServiceListXmlString);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.Int, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.VarChar, 100, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                WOSCustomerObj.JobCostMasterID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_JobCostMasterID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "SaveCostSheet", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool ApproveCostSheet(Entities.WOSJobOpening WOSJobOpeningObj, bool IsApprove, int LoginID, out string result)
        {
            result = String.Empty;

            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[ApproveCostSheet]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WOSMoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSJobOpeningObj.WOSMoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsRemoveApproval", SqlDbType.Bit, 0, ParameterDirection.Input, !IsApprove);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsSenttoApproval", SqlDbType.Bit, 0, ParameterDirection.Input, WOSJobOpeningObj.IsCSSentToApprove);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ApprovalUser", SqlDbType.Int, 0, ParameterDirection.Input, WOSJobOpeningObj.CSSentToApproveUser);
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "ApproveCostSheet", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public DataSet GetWOSCostSheet(int LoginID, Int64? WOSMoveID, bool IsXlFormat)
        {
            DataSet CostSheetDs = null;
            try
            {
                string query = string.Format("[WOS].[GetWOSCostSheet] @SP_LoginID={0}, @SP_WOSMoveID={1}, @SP_IsXlFormat={2}", CSubs.QSafeValue(Convert.ToString(LoginID)),
                    CSubs.QSafeValue(Convert.ToString(WOSMoveID)), CSubs.QSafeValue(Convert.ToString(IsXlFormat)));

                CostSheetDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "GetWOSCostSheet", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return CostSheetDs;
        }

        public DataSet GetJobDocumentDetail(int FileID, int LoginID)
        {
            DataSet JobDocumentDs = null;
            try
            {
                string query = string.Format("exec [WOS].[GetJobDocumentDetail] @SP_FileID={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(FileID)), CSubs.QSafeValue(Convert.ToString(LoginID)));
                JobDocumentDs = CSubs.GetDataSet(query);
            }
            catch (Exception ex)
            {
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "GetJobDocumentDetail", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
            return JobDocumentDs;
        }

        public bool SaveWOSHouseDetails(WOSHouseDetails WOSHouseDetailsObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[AddEditHouseJobTransaction]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WOSMoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSHouseDetailsObj.WOSMoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_HouseMasterID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSHouseDetailsObj.HouseMasterID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_HouseJobTransID", SqlDbType.BigInt, 0, ParameterDirection.Output, WOSHouseDetailsObj.HouseJobTransID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LeaseSignedDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.LeaseSignedDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LeaseRenewedUntilDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.LeaseRenewedUntilDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LeaseExpiryDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.LeaseExpiryDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsFixedPeriod", SqlDbType.Bit, 0, ParameterDirection.Input, WOSHouseDetailsObj.IsFixedPeriod);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_FinalArrDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.FinalArrDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_HHGArrDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.HHGArrDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WorkStartDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.WorkStartDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OfficialHandoverPropertyDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.OfficialHandoverPropertyDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InspectionDateIn", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.InspectionDateIn);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ShortTermInspDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.ShortTermInspDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_SettlingDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.SettlingDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ExpMoveOutDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.ExpMoveOutDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ActMoveOutDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.ActMoveOutDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InspectionDateOut", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.InspectionDateOut);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_ProperyReleaseByDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.ProperyReleaseByDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OfficialHandbackPropertyDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.OfficialHandbackPropertyDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_InactiveDate", SqlDbType.Date, 0, ParameterDirection.Input, WOSHouseDetailsObj.InactiveDate);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_IsClosed", SqlDbType.Bit, 0, ParameterDirection.Input, WOSHouseDetailsObj.IsClosed);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@RETURNSTATUS", SqlDbType.Int, 0, ParameterDirection.ReturnValue);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_OutMsg", SqlDbType.VarChar, 100, ParameterDirection.Output);
                        conn.ExecuteProcedure(ProcedureReturnType.SingleValue);

                        if (!conn.IsError)
                        {
                            int ReturnStatus = Convert.ToInt32(conn.GetParameterValue(ParameterOF.PROCEDURE, "@RETURNSTATUS"));
                            result = Convert.ToString(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_OutMsg"));

                            if (ReturnStatus == 0)
                            {
                                WOSHouseDetailsObj.HouseJobTransID = Convert.ToInt64(conn.GetParameterValue(ParameterOF.PROCEDURE, "@SP_HouseJobTransID"));
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "SaveWOSHouseDetails", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool CancelWOSJob(Entities.WOSJobOpening WOSJobOpeningObj, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[AddEditCancelJob]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WOSMoveID", SqlDbType.Int, 0, ParameterDirection.Input, WOSJobOpeningObj.WOSMoveID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_JobCancelRemarks", SqlDbType.VarChar, 1000, ParameterDirection.Input, WOSJobOpeningObj.JobCancelRemark);
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "CancelWOSJob", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }

        public bool SendLinkToAssignee(Int64 WOSMoveID, string EmailTo, string EmailCc, string EmailBcc, string Url, int LoginID, out string result)
        {
            result = string.Empty;
            try
            {
                using (CDALSQL conn = new CDALSQL(System.Configuration.ConfigurationManager.ConnectionStrings["RELODB"].ToString()))
                {
                    if (conn.Connect())
                    {
                        conn.AddCommand("[WOS].[AddEditSendLinkToAssignee]", QueryType.Procedure);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_LoginID", SqlDbType.Int, 0, ParameterDirection.Input, LoginID);
                        conn.AddParameter(ParameterOF.PROCEDURE, "@SP_WOSMoveID", SqlDbType.BigInt, 0, ParameterDirection.Input, WOSMoveID);
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
                throw new DataAccessException(Convert.ToString(LoginID), "WOSJobOpeningDAL", "SendLinkToAssignee", Properties.Resources.UnExpectedErrorAtDAL, ex);
            }
        }
    }
}