using RELOCBS.App_Code;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Mvc;

namespace RELOCBS.DAL
{
    public class ComboDAL
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

        public IEnumerable<SelectListItem> GetCurrencyDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Currency] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));

        }

        public IEnumerable<SelectListItem> GetCityDropdown(string LoginID, string SPTYPE = "ALL", int? ContinentID = -1, int? CountryID = -1, int? CityID = -1)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_City] @SP_Type={0},@SP_ContinentID={1},@SP_CountryID={2},@SP_CityID={3},@SP_Loginid={4}", CSubs.QSafeValue(SPTYPE),
                CSubs.QSafeValue(Convert.ToString(ContinentID)), CSubs.QSafeValue(Convert.ToString(CountryID)),
                CSubs.QSafeValue(Convert.ToString(CityID)), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GETCountryDropdown(string LoginID, string SPTYPE = "ALL", int? ContinentID = -1, int? CountryID = -1)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Country] @SP_Type={0},@SP_ContinentID={1},@SP_CountryID={2},@SP_Loginid={3}", CSubs.QSafeValue(SPTYPE),
                CSubs.QSafeValue(Convert.ToString(ContinentID)), CSubs.QSafeValue(Convert.ToString(CountryID)),
                CSubs.QSafeValue(LoginID)));

        }

        public IEnumerable<SelectListItem> GETContinentDropdown(string LoginID, string SPTYPE = "ALL", int? ContinentID = -1)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Continent] @SP_Type={0},@SP_ContinentID={1},@SP_Loginid={2}", CSubs.QSafeValue(SPTYPE),
                CSubs.QSafeValue(Convert.ToString(ContinentID)),
                CSubs.QSafeValue(LoginID)));

        }

        public IEnumerable<SelectListItem> PortsByCityShipmentModeCombo(string LoginID, int CityID, int shipmentModeID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_City] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(Convert.ToString(CityID)), CSubs.QSafeValue(LoginID)));

        }

        public IEnumerable<SelectListItem> GetRMCDropdown(string LoginID, string SPTYPE = "ALL", int? RMCID = -1)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_RMC] @SP_Type={0},@SP_Loginid={1},@SP_RMCID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(RMCID))));
        }

        public IEnumerable<SelectListItem> GetAgentDropdown(string LoginID, string SPTYPE = "ALL", string CORA = null, bool IsRMCBuss = false, int? AgentId = null, string searchstring = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Agent] @SP_Type={0},@SP_Loginid={1},@SP_CorA={2},@SP_CompanyID={3},@SP_ISRMCBuss={4},@SP_AgentID={5},@sp_searchstring={6}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(CORA), CSubs.QSafeValue(UserSession.GetUserSession().CompanyID.ToString()), IsRMCBuss, CSubs.QSafeValue(Convert.ToString(AgentId)), CSubs.QSafeValue(Convert.ToString(searchstring))));
        }

        public IEnumerable<SelectListItem> OriginCityDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_City] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> DestinationCityDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_City] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetServiceLineDropdown(string LoginID, string SPTYPE = "ALL", bool RMCBuss = false, string ForPage = "", int CompID = 0, string BussLine = null)
        {

            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ServiceLine] @SP_Type={0},@SP_Loginid={1},@Sp_IsRmcBuss={2},@SP_ForPage={3},@SP_CompID={4},@SP_BussLine={5}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), RMCBuss, CSubs.QSafeValue(ForPage), CompID, CSubs.QSafeValue(BussLine)));
        }

        public IEnumerable<SelectListItem> GetBusinessLineDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_BussinessLine] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetGoodsDescriptionDropdown(string LoginID, string SPTYPE = "ALL", int ServiceLineID = 0)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_GoodsDescription] @SP_Type={0},@SP_Loginid={1},@SP_ServiceLineID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(ServiceLineID))));
        }

        public IEnumerable<SelectListItem> GetModeDropdown(string LoginID, string SPTYPE = "ALL", int ServiceLineID = 0)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_TransportMode] @SP_Type={0},@SP_Loginid={1},@SP_ServiceLineID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(ServiceLineID))));
        }

        public IEnumerable<SelectListItem> GetRateComponentDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_RateComponent] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetRateAgentDropdown(string LoginID, string SPTYPE = "ALL", bool IsRMCBUss = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Agent] @SP_Type={0},@SP_Loginid={1},@SP_ISRMCBuss={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), IsRMCBUss));
        }
        public IEnumerable<SelectListItem> GetFromLocationDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ServiceLine] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }
        public IEnumerable<SelectListItem> GetToLocationDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ServiceLine] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }
        public IEnumerable<SelectListItem> GetRateCurrencyDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Currency] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }
        public IEnumerable<SelectListItem> GetBaseCurrencyRateDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Currency] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }
        public IEnumerable<SelectListItem> GetMeasurementUnitDropdown(char UnitType, string LoginID, int? WeightUnitID = -1, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_WeightUnit] @SP_Type={0},@SP_Loginid={1},@SP_WeightUnitID={2},@SP_UnitType={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(WeightUnitID)), CSubs.QSafeValue(Convert.ToString(UnitType))));
        }

        public IEnumerable<SelectListItem> GetCostHeadDropdown(string LoginID, int? RMC_ID, int? MoveCompID, bool IsRMCBUss, string ForCombo = null, string SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_CostHead] @SP_Type={0},@SP_Loginid={1},@SP_RMCID={2},@Sp_MoveCompID={3},@SP_ComboFor={4},@SP_IsRmcBuss={5}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(RMC_ID)), CSubs.QSafeValue(Convert.ToString(MoveCompID)), CSubs.QSafeValue(Convert.ToString(ForCombo)), IsRMCBUss));
        }

        public IEnumerable<SelectListItem> GetPortDropdown(string LoginID, int? CityID, string SPTYPE = "ALLACTIVE", string SeaOrAir = "", int PortID = -1, string Search = "", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Port] @SP_Type={0},@SP_Loginid={1},@SP_CityID={2},@SP_AirOrSea={3},@SP_PortID={4},@sp_searchstring={5}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(CityID)), CSubs.QSafeValue(SeaOrAir), CSubs.QSafeValue(Convert.ToString(PortID)), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetEnqInfoSourceDropdown(string LoginID, int? EnqSourceID, string SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_EnqInfoSource] @SP_Type={0},@SP_Loginid={1},@SP_EnqSourceID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(EnqSourceID))));
        }

        public IEnumerable<SelectListItem> GetMoveQuoteClassDropdown(string LoginID, int? MoveQuoteID, string SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_MoveQuoteClass] @SP_Type={0},@SP_Loginid={1},@SP_MoveQuoteID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(MoveQuoteID))));
        }

        public IEnumerable<SelectListItem> GetShipmentTypeDropdown(string LoginID, int? ShipmentTypeID, string SPTYPE = "ALLACTIVE", int ServiceLineID = 0)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ShipmentType] @SP_Type={0},@SP_Loginid={1},@SP_ShipmentTypeID={2},@SP_ServiceLineID={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(ShipmentTypeID)), CSubs.QSafeValue(Convert.ToString(ServiceLineID))));
        }

        public IEnumerable<SelectListItem> GetCompanyDropdown(string LoginID, int? CountryID, int? CityID, int? CompanyID, string SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Company] @SP_Type={0},@SP_Loginid={1},@SP_CompanyID={2},@SP_CountryID={3},@SP_CityID={4}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(CompanyID)), CSubs.QSafeValue(Convert.ToString(CountryID)), CSubs.QSafeValue(Convert.ToString(CityID))));
        }

        public IEnumerable<SelectListItem> GetCompanyBranchDropdown(string LoginID, int? CompanyBranchID, int? CompanyID, string SPTYPE = "ALLACTIVE", bool IsRMCBuss = false, bool IsRev = false, int? RMCID = null, string ForPage = "", int? SBUId = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_CompanyBranches] @SP_Type={0},@SP_Loginid={1},@SP_CompBranchID={2},@SP_CompId={3},@SP_IsRmcBuss={4},@SP_IsRevBr={5},@SP_RmcID={6},@SP_ForPage={7},@SP_SBUId={8}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(CompanyBranchID)), CSubs.QSafeValue(Convert.ToString(CompanyID)), IsRMCBuss, IsRev, CSubs.QSafeValue(Convert.ToString(RMCID)),
                CSubs.QSafeValue(Convert.ToString(ForPage)), CSubs.QSafeValue(Convert.ToString(SBUId))));
        }

        public IEnumerable<SelectListItem> GetClickRestrictDropdown(string LoginID, int? ClickRestrictID, string SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Access].[ForCombo_ClickRestrict] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetShipmentCategoryDropdown(string LoginID, int? ShipmentTypeID, string SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ShipmentType] @SP_Type={0},@SP_Loginid={1},@SP_ShipmentTypeID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(ShipmentTypeID))));
        }

        public IEnumerable<SelectListItem> GetContainerSizeDropdown(string LoginID, int? ContainerSizeID = -1, string SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ContainerSize] @SP_Type={0},@SP_Loginid={1},@SP_ContainerSizeID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(ContainerSizeID))));
        }

        public IEnumerable<SelectListItem> GetCompetitorDropdown(string LoginID, int? CompanyID = -1, int? CompetitorID = -1, string SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Competitor] @SP_Type={0},@SP_Loginid={1},@SP_CompetitorID={2},@SP_CompID={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(CompetitorID)), CSubs.QSafeValue(Convert.ToString(CompanyID))));
        }

        public IEnumerable<SelectListItem> GetEmployeeDropdown(string LoginID, int? CountryID = -1, int? CityID = -1, int? CompanyID = -1, int BranchID = -1, string SPTYPE = "ALLACTIVE", bool IsRMCBuss = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Employee] @SP_Type={0},@SP_Loginid={1},@SP_CountryID={2},@SP_CityID={3},@SP_CompanyID={4},@SP_IsRmcBuss={5},@SP_BranchID={6}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(CountryID)), CSubs.QSafeValue(Convert.ToString(CityID)), CSubs.QSafeValue(Convert.ToString(CompanyID)), IsRMCBuss, CSubs.QSafeValue(Convert.ToString(BranchID))));
        }

        public IEnumerable<SelectListItem> GetEnquiryLostReasonDropdown(string LoginID, int? EnqLostReasonID = -1, string SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_EnqLostReason] @SP_Type={0},@SP_Loginid={1},@SP_EnqLostReasonID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(EnqLostReasonID))));
        }

        public IEnumerable<SelectListItem> GetShipperCategoryDropdown(String LoginID, int? ShipperCategoryID, String SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ShipperCategory] @SP_Type={0},@SP_Loginid={1},@SP_ShipperCategoryID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(ShipperCategoryID))));
        }

        public IEnumerable<SelectListItem> GetShipperTypeDropdown(String LoginID, int? ShipperTypeID, String SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ShipperType] @SP_Type={0},@SP_Loginid={1},@SP_ShipperTypeID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(ShipperTypeID))));
        }

        public IEnumerable<SelectListItem> GetShippingLineDropdown(String LoginID, String ModeID, int? ShippingLineID, String SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ShippingLine] @SP_Type={0},@SP_Loginid={1},@SP_ShippingLineID={2},@SP_ModeID={3},@SP_CompID={4}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(ShippingLineID)), CSubs.QSafeValue(ModeID), CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetInsuranceTypeDropdown(String LoginID, int InsuranceTypeID = -1, String SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_InsuranceType] @SP_Type={0},@SP_Loginid={1},@SP_InsuranceTypeID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(InsuranceTypeID))));
        }

        public IEnumerable<SelectListItem> GetUserCompanyMapDropdown(int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Access].[GetUserCompany] @SP_Loginid={0}", CSubs.QSafeValue(Convert.ToString(LoginID))));
        }

        public IEnumerable<SelectListItem> GetUserBussinessLineDropdown(int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Access].[GetUserBusinessLine] @SP_Loginid={0}", CSubs.QSafeValue(Convert.ToString(LoginID))));
        }

        public IEnumerable<SelectListItem> GetLoginTypeDropdown(int LoginID, string LoginTypeCode = null, String SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_LoginType] @SP_Type={0},@SP_LoginTypeCode={1},@SP_Loginid={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginTypeCode), CSubs.QSafeValue(Convert.ToString(LoginID))));
        }

        public IEnumerable<SelectListItem> GetSFRCalculationMethodDropdown()
        {
            return CSubs.BindDropdown(string.Format("[RMC].[GetSFRCalculationMethod]"));
        }

        public IEnumerable<SelectListItem> GetVehicleDropdown(int LoginID, int? BranchID = -1, int? VendorID = -1, int? CompanyID = -1, String SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Vehicle] @SP_Type={0},@SP_Loginid={1},@SP_BranchID={2},@SP_VendorID={3},@SP_CompID={4}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(BranchID)), CSubs.QSafeValue(Convert.ToString(VendorID)), CSubs.QSafeValue(Convert.ToString(CompanyID))), Allitems);
        }

        //Report Data

        public DataTable GetCityData(string LoginID, string SPTYPE = "ALL", int? ContinentID = -1, int? CountryID = -1, int? CityID = -1)
        {
            string query = string.Format("exec Comm.GETCityForGrid @SP_PageIndex={0},@SP_PageSize={1},@SP_OrderBy={2},@SP_Order={3},@SP_CityID={4},@SP_isActive={5},@SP_SearchString={6},@SP_LoginID={7}",
                 CSubs.QSafeValue(Convert.ToString(1)),
                CSubs.QSafeValue(Convert.ToString(5)),
                CSubs.QSafeValue(""),
                CSubs.QSafeValue(Convert.ToString(0)),
                CSubs.QSafeValue(Convert.ToString(null)),
                 CSubs.QSafeValue(Convert.ToString(1)),
                CSubs.QSafeValue(null),
                Convert.ToString(LoginID)
                );

            DataTable dataTable = CSubs.GetDataTable(query);

            return dataTable;
        }

        public IEnumerable<SelectListItem> GetJobAllocationStatusDropdown(int LoginID, string SPTYPE = "ALLACTIVE", bool Allitems = true)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_JobAllocationStatus] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID))), Allitems);
        }

        public IEnumerable<SelectListItem> getCrewDropdown(int LoginID, int CompID, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_CrewCode] @SP_Type={0},@SP_Loginid={1},@SP_CompID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CompID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetVendorDropdown(int LoginID, int CompID, string SPTYPE, int? VendorID, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Vendor] @SP_Type={0},@SP_Loginid={1},@SP_CompID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CompID))), Allitems);
        }

        public IEnumerable<SelectListItem> getPurposeDropdown(int LoginID, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Purpose] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetDocTypeDropdown(int LoginID, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_DocType] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetActivityTypeDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int JobTypeId = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ActivityType] @SP_Type={0},@SP_Loginid={1},@SP_WH_JobTypeId={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), JobTypeId), Allitems);
        }

        public IEnumerable<SelectListItem> getMaterialDropdown(int LoginID, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Material] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID))), Allitems);
        }

        public IEnumerable<SelectListItem> getDesignationDropdown(int LoginID, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Designation] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID))), Allitems);
        }

        public IEnumerable<SelectListItem> getShipperDesignationDropdown(int LoginID, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[forcombo_ShipperDesig] "));//@SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID))), Allitems
        }

        public IEnumerable<SelectListItem> getJobNolDropdown(Int64 MoveID, string shipper, bool IsRMCBuss, int LoginID, int CompanyID, string SPTYPE, bool Allitems = false, int ServiceLineID = -1, bool IsStorage = false, string SearchStr = "", int Modeid = -1, Int64? MasterID = -1)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_JobNo] @SP_Type={0},@SP_Loginid={1},@sp_Compid={2},@SP_MoveId={3},@SP_Shipper={4},@sp_isRMCBuss={5},@SP_ServiceLineID={6},@SP_IsStorageJob={7},@sp_searchstring={8},@sp_ModeID={9},@SP_TransitMasterID={10}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CompanyID)), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(shipper), CSubs.QSafeValue(Convert.ToString(IsRMCBuss)), CSubs.QSafeValue(Convert.ToString(ServiceLineID)), CSubs.QSafeValue(Convert.ToString(IsStorage)), CSubs.QSafeValue(SearchStr), CSubs.QSafeValue(Convert.ToString(Modeid)), CSubs.QSafeValue(Convert.ToString(MasterID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetSFRCalculationMethodDropdown(bool IsGetUnit)
        {
            return CSubs.BindDropdown(string.Format("[RMC].[GetSFRCalculationMethod] @GetUnit={0},@SP_LoginID={1}", IsGetUnit, CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().LoginID))));
        }

        public IEnumerable<SelectListItem> GetJobActivityList(int LoginID, string MoveID = null, string SPTYPE = "ALL")
        {

            int CompID = UserSession.GetUserSession().CompanyID;
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_JobActivityList]@SP_Type={0},@SP_Loginid={1},@SP_CompID={2},@SP_MoveID={3}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CompID)), CSubs.QSafeValue(Convert.ToString(MoveID))));
        }

        public IEnumerable<SelectListItem> GetJobDocTypelDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? DocTypeID = -1, string DocFromType = "", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_JobDocType] @SP_Type={0},@SP_Loginid={1},@SP_DocTypeID={2},@SP_DocFromType={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(DocTypeID)), CSubs.QSafeValue(DocFromType)), Allitems);
        }

        public IEnumerable<SelectListItem> GetJobDocNamelDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? DocNameID = -1, int DocTypeID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_JobDocName] @SP_Type={0},@SP_Loginid={1},@SP_DocNameID={2},@SP_DocTypeID={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(DocNameID)), CSubs.QSafeValue(Convert.ToString(DocTypeID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetCaseTypeDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? CaseTypeID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_CaseType] @SP_Type={0},@SP_Loginid={1},@SP_CaseTypeID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CaseTypeID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetInst_SubQuestionDropdown(int LoginID, string SPTYPE, string DropdownType = "", int? AnswerID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Inst_SubQuestionDropdown] @SP_Type={0},@SP_Loginid={1},@SP_AnswerID={2},@sp_DropdownType={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(AnswerID)), CSubs.QSafeValue(Convert.ToString(DropdownType))), Allitems);
        }

        public IEnumerable<SelectListItem> GetWarehouseDropdown(int LoginID, int CompID, string SPTYPE, int? BranchID = -1, int? WarehouseID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Warehouse] @SP_Type={0},@SP_Loginid={1},@SP_WarehouseID={2},@SP_BranchID={3},@SP_CompID={4}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(WarehouseID)), CSubs.QSafeValue(Convert.ToString(BranchID)), CSubs.QSafeValue(Convert.ToString(CompID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetWarehouseStrgDropdown(int LoginID, string SPTYPE, int? BranchID = -1, int? WarehouseID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_StrgWarehouse] @SP_Type={0},@SP_Loginid={1},@SP_WarehouseID={2},@SP_BranchID={3},@SP_CompID={4}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(WarehouseID)), CSubs.QSafeValue(Convert.ToString(BranchID)), CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetInstQuestionAnswerDropdown(int LoginID, string SPTYPE, string DropdownType, int? AnswerID, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Inst_SubQuestionDropdown] @SP_Type={0},@SP_Loginid={1},@sp_DropdownType={2},@SP_AnswerID={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(DropdownType)), CSubs.QSafeValue(Convert.ToString(AnswerID))), Allitems);
        }
        public IEnumerable<SelectListItem> GetInvoiceForMoveDropdown(int MoveID, int LoginID, bool IsStatement = false)
        {
            return CSubs.BindDropdown(string.Format("[MoveMan].[ForCombo_Invoice] @SP_MoveID={0},@SP_LoginID={1},@SP_IsStatement={2}", CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(LoginID)), IsStatement));
        }


        public IEnumerable<SelectListItem> GetSearchTypeDropdown(string SearchType, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_SearchType] @SP_SearchType={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(SearchType)), CSubs.QSafeValue(Convert.ToString(LoginID))));
        }

        public IEnumerable<SelectListItem> GetInsuranceCompanyDropdown(string SPTYPE, Int64 MoveID, int LoginID, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_InsuranceCompany] @SP_SearchType={0},@SP_LoginID={1},@SP_MoveID={2},@SP_CompID={3}", CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID))), Allitems);
        }

        public IEnumerable<SelectListItem> getInsurancePolicyNoDropdown(int InsuranceCompID, int PolicyID, string SPTYPE, bool Allitems, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Ins].[ForCombo_InsurancePolicy] @SP_SearchType={0},@SP_LoginID={1},@SP_PolicyID={2},@SP_InsCompanyID={3},@SP_CompID={4}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                CSubs.QSafeValue(Convert.ToString(PolicyID)), CSubs.QSafeValue(Convert.ToString(InsuranceCompID))
                , CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID))
                ), Allitems);
        }

        public IEnumerable<SelectListItem> getInsDelayReasonListDropdown(string SPTYPE, int LoginID, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_InsDelayReason] @SP_Type={0},@SP_LoginID={1}", CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)), Allitems));
        }


        public IEnumerable<SelectListItem> GetClaimNatureDropdown(int ClaimNatureID, string SPTYPE, bool Allitems, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Claim].[ForCombo_ClaimNature] @SP_SearchType={0},@SP_LoginID={1},@SP_ClaimNatureID={2}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                CSubs.QSafeValue(Convert.ToString(ClaimNatureID))
                ), Allitems);
        }

        public IEnumerable<SelectListItem> GetClaimStatusDropdown(int ClaimStatusID, string SPTYPE, bool Allitems, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Claim].[ForCombo_ClaimStatus] @SP_SearchType={0},@SP_LoginID={1},@SP_ClaimStatusID={2}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                CSubs.QSafeValue(Convert.ToString(ClaimStatusID))
                ), Allitems);
        }

        public IEnumerable<SelectListItem> GetClaimCategoryDropdown(int ClaimCategoryID, string SPTYPE, bool Allitems, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Claim].[ForCombo_ClaimCategory] @SP_SearchType={0},@SP_LoginID={1},@SP_ClaimCategoryID={2}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                CSubs.QSafeValue(Convert.ToString(ClaimCategoryID))
                ), Allitems);
        }

        public IEnumerable<SelectListItem> getClaimItemDetailIsDropdown(int ClaimItemDetailIsID, int ClaimCategoryID, string SearchString, string SPTYPE, bool Allitems, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Claim].[ForCombo_ClaimItemDetailIs] @SP_SearchType={0},@SP_LoginID={1},@SP_ClaimItemDetailIsID={2},@SP_ClaimCategoryID={3},@sp_searchstring={4}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                CSubs.QSafeValue(Convert.ToString(ClaimItemDetailIsID)), CSubs.QSafeValue(Convert.ToString(ClaimCategoryID))
                , CSubs.QSafeValue(SearchString)
                ), Allitems);
        }

        public IEnumerable<SelectListItem> GetPaymodeDropdown(string Paymode, string SPTYPE, bool Allitems, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Paymode] @SP_Type={0},@SP_LoginID={1},@SP_Paymode={2}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                CSubs.QSafeValue(Convert.ToString(Paymode))
                ), Allitems);
        }

        public IEnumerable<SelectListItem> GetSettlementTypeDropdown(string SettlementType, string SPTYPE, bool Allitems, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_SettlementType] @SP_Type={0},@SP_LoginID={1},@SP_SettlementType={2}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                CSubs.QSafeValue(Convert.ToString(SettlementType))
                ), Allitems);
        }

        public IEnumerable<SelectListItem> GetChequeStatusDropdown(string ChequeStatus, string SPTYPE, bool Allitems, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ChequeStatus] @SP_Type={0},@SP_LoginID={1},@SP_ChequeStatus={2}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                CSubs.QSafeValue(Convert.ToString(ChequeStatus))
                ), Allitems);
        }

        public IEnumerable<SelectListItem> GetControllerDropdown(int ControllerID, string SPTYPE, bool Allitems, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Controller] @SP_Type={0},@SP_LoginID={1},@SP_ControllerID={2}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)),
                CSubs.QSafeValue(Convert.ToString(ControllerID))
                ), Allitems);
        }

        public IEnumerable<SelectListItem> getInsuranceNoDropdown(Int64 InsuranceID, Int64? MoveID, int CompID, string SearchString, string SPTYPE, bool Allitems, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Ins].[ForCombo_InsuranceNo] @SP_SearchType={0},@SP_LoginID={1},@SP_CompanyID={2},@sp_searchstring={3},@SP_InsuranceID={4},@SP_MoveID={5}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CompID)),
                CSubs.QSafeValue(Convert.ToString(SearchString)),
                CSubs.QSafeValue(Convert.ToString(InsuranceID)),
                CSubs.QSafeValue(Convert.ToString(MoveID))
                ), Allitems);

        }

        public IEnumerable<SelectListItem> getClaimNoDropdown(Int64 ClaimID, Int64 MoveID, int CompID, string SearchString, string SPTYPE, bool Allitems, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Claim].[ForCombo_ClaimNo] @SP_SearchType={0},@SP_LoginID={1},@SP_CompanyID={2},@sp_searchstring={3},@SP_ClaimID={4},@SP_MoveID={5}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CompID)),
                CSubs.QSafeValue(Convert.ToString(SearchString)),
                CSubs.QSafeValue(Convert.ToString(ClaimID)),
                CSubs.QSafeValue(Convert.ToString(MoveID))
                ), Allitems);
        }

        public IEnumerable<SelectListItem> GetStrgBillingPeriodDropdown(Int64 BillingPeriodID, string SearchString, string SPTYPE, bool Allitems, int LoginID, string ComboType)
        {
            return CSubs.BindDropdown(string.Format("[Strg].[ForCombo_StrgBillingPeriod] @SP_SearchType={0},@SP_LoginID={1},@SP_StrgBillingPeriodID={2},@sp_searchstring={3},@SP_ComboType={4}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(BillingPeriodID)),
                CSubs.QSafeValue(Convert.ToString(SearchString)), CSubs.QSafeValue(Convert.ToString(ComboType))), Allitems);

        }

        public IEnumerable<SelectListItem> GetStrgCostHeadDropdown(Int64 StrgCostHeadID, string StrgCostType, string SearchString, string SPTYPE, bool Allitems, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Strg].[ForCombo_StrgCostHead] @SP_SearchType={0},@SP_LoginID={1},@SP_StrgCostHeadID={2},@sp_searchstring={3},@SP_StrgCostType={4}",
                CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(StrgCostHeadID)),
                CSubs.QSafeValue(Convert.ToString(SearchString)), CSubs.QSafeValue(Convert.ToString(StrgCostType))), Allitems);

        }

        public IEnumerable<SelectListItem> GetStateDropdown(string LoginID, string SPTYPE, string SearchString, int ContinentID, int CountryID, int StateID, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_State] @SP_Type={0},@SP_ContinentID={1},@SP_CountryID={2},@SP_StateID={3},@SP_Loginid={4},@sp_searchstring={5}", CSubs.QSafeValue(SPTYPE),
                CSubs.QSafeValue(Convert.ToString(ContinentID)), CSubs.QSafeValue(Convert.ToString(CountryID)),
                CSubs.QSafeValue(Convert.ToString(StateID)), CSubs.QSafeValue(LoginID),
                CSubs.QSafeValue(SearchString)
                ), Allitems);
        }

        public IEnumerable<SelectListItem> GetLoadedAtDropdown(string LoginID, string SPTYPE, string SearchString, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForCombo_LoadedAt] @SP_SearchType={0},@SP_LoginID={1},@sp_searchstring={2}",
            CSubs.QSafeValue(Convert.ToString(SPTYPE)), CSubs.QSafeValue(Convert.ToString(LoginID)),
            CSubs.QSafeValue(Convert.ToString(SearchString))), Allitems);
        }

        public IEnumerable<SelectListItem> GetVehicleTypeDropdown(string LoginID, int VehTypeID, string SPTYPE, string SearchString, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForComboVehType] @SP_SearchType={0},@SP_LoginID={1},@sp_searchstring={2},@SP_VehTypeID={3}",
            CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID),
            CSubs.QSafeValue(SearchString),
            CSubs.QSafeValue(Convert.ToString(VehTypeID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetRoleDropdown(string LoginID, int RoleID, int UserID, string SPTYPE, string SearchString, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForComboUserRole] @SP_SearchType={0},@SP_LoginID={1},@sp_searchstring={2},@SP_RoleID={3},@SP_UserID={4}",
            CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID),
            CSubs.QSafeValue(SearchString),
            CSubs.QSafeValue(Convert.ToString(RoleID)),
            CSubs.QSafeValue(Convert.ToString(UserID))
            ), Allitems);
        }

        public IEnumerable<SelectListItem> GetLoadChartNoDropdown(String LoginID, int LoadChartID, string SPTYPE, string SearchString, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForComboLoadChartNo] @SP_SearchType={0},@SP_LoginID={1},@sp_searchstring={2},@SP_LoadChartID={3}",
            CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID),
            CSubs.QSafeValue(SearchString),
            CSubs.QSafeValue(Convert.ToString(LoadChartID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetAgentGroupDropdown(String LoginID, int AgentGroupID, string CorA, string SPTYPE, string SearchString, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForComboAgentGroup] @SP_SearchType={0},@SP_LoginID={1},@sp_searchstring={2},@SP_AgentGroupID={3},@SP_CorA={4},@SP_CompanyID={5}",
            CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID),
            CSubs.QSafeValue(SearchString),
            CSubs.QSafeValue(Convert.ToString(AgentGroupID)),
            CSubs.QSafeValue(CorA),
            CSubs.QSafeValue(UserSession.GetUserSession().CompanyID.ToString())
            ), Allitems);
        }

        public IEnumerable<SelectListItem> GetCompBranchList(String LoginID, string CompIDXml, string SPTYPE, string ForPage = "")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_CompanyBranches] @SP_Type={0},@SP_LoginID={1},@SP_CompIdXml={2},@SP_ForPage={3}",
            CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(CompIDXml), CSubs.QSafeValue(ForPage)), false);
        }

        public IEnumerable<SelectListItem> GetNationalityDropDown(String LoginID, string SPTYPE)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Nationality] @SP_LoginID={0}",
            CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetClaimDocTypeDropdown(int LoginID, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Claim].[ForCombo_ClaimDocType] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetShipperNameDropdown(string LoginID, string ShipperName = "", string SPTYPE = "ALLACTIVE", string SearchString = "", bool Allitems = false)
        {
            bool RMCBuss = false;
            if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
            {
                RMCBuss = false;
            }
            else
            {
                RMCBuss = true;
            }
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ShipperName] @SP_Type={0},@SP_LoginID={1},@sp_searchstring={2},@SP_ShipperName={3},@SP_CompanyID={4},@Sp_IsRmcBuss={5}",
               CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID),
               CSubs.QSafeValue(SearchString),
               CSubs.QSafeValue(Convert.ToString(ShipperName)),
               CSubs.QSafeValue(UserSession.GetUserSession().CompanyID.ToString()),
               CSubs.QSafeValue(Convert.ToString(RMCBuss))
               ), Allitems);
        }

        public IEnumerable<SelectListItem> GetCopyEnqShipmentDropdown(int LoginID, Int64 EnqDetailID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[GetCopyEnqshipmentCombo] @SP_EnqDetailID={0},@SP_LoginID={1}",
                CSubs.QSafeValue(Convert.ToString(EnqDetailID)), CSubs.QSafeValue(Convert.ToString(LoginID))
                ), false);
        }

        public IEnumerable<SelectListItem> GetCourierDropDown(string SPTYPE, int LoginID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Courier] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID))));
        }

        public IEnumerable<SelectListItem> GetWarehouseJobNolDropdown(Int64? JobID, bool IsRMCBuss, int LoginID, int CompanyID, string SPTYPE, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForCombo_WarehouseJobNo]  @SP_Type={0},@SP_Loginid={1},@sp_Compid={2},@SP_JobId={3},@sp_isRMCBuss={4}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CompanyID)), CSubs.QSafeValue(Convert.ToString(JobID)), CSubs.QSafeValue(Convert.ToString(IsRMCBuss))), Allitems);
        }

        public IEnumerable<SelectListItem> GetShippingLineAgentDropdown(int LoginID, int? ShippingLineAgentID, string SPTYPE, string Search, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ShippingLineAgent]  @SP_Type={0},@SP_Loginid={1},@sp_searchstring={2},@SP_ShippingLineAgentID={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Search), CSubs.QSafeValue(Convert.ToString(ShippingLineAgentID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetShippingCarrierDropdown(int LoginID, int ModeID, int? ShippingCarrierID, string SPTYPE, string Search, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ShippingCarrier]  @SP_Type={0},@SP_Loginid={1},@sp_searchstring={2},@SP_ShippingCarrierID={3},@sp_ModeID={4}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Search), CSubs.QSafeValue(Convert.ToString(ShippingCarrierID)), CSubs.QSafeValue(Convert.ToString(ModeID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetStorageStateDropdown(int MoveID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_GetStorageState] @SP_Moveid={0},@SP_CompID={1}", MoveID, CSubs.QSafeValue(Convert.ToString(UserSession.GetUserSession().CompanyID))));
        }

        public IEnumerable<SelectListItem> GetTransitInvoiceTypeDropdown(int LoginID, int InvoiceTypeID, string SPTYPE, string Search, string type, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[MoveMan].[ForCombo_transhipInvTypeMast] @SP_Type={0},@SP_Loginid={1},@sp_searchstring={2},@SP_InvoiceTypeID={3},@SP_InvCreditType={4}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Search), CSubs.QSafeValue(Convert.ToString(InvoiceTypeID)), CSubs.QSafeValue(type)), Allitems);
        }

        public IEnumerable<SelectListItem> GetRateTypeGrpDropdown(int LoginID, int RateTypeGrpID, string SPTYPE, string Search, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_RateTypeGrp] @SP_Type={0},@SP_Loginid={1},@sp_searchstring={2},@SP_RateTypeGrpID={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Search), CSubs.QSafeValue(Convert.ToString(RateTypeGrpID))), Allitems);
        }
        public IEnumerable<SelectListItem> GetTitleDropdown(int LoginID, int TitleID = 1, string SPTYPE = "ALLACTIVE", string Search = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Title] @SP_Type={0},@SP_Loginid={1},@sp_searchstring={2},@SP_TitleID={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Search), CSubs.QSafeValue(Convert.ToString(TitleID))));
        }

        public IEnumerable<SelectListItem> GetReportDropdown(int LoginID, int CompID)
        {
            return CSubs.BindDropdown(string.Format("[Access].[GetReportList] @SP_Loginid={0},@sp_CompID={1}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CompID))));
        }

        public IEnumerable<SelectListItem> GetTransFA_AppDropdown(int LoginID, int AppID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[E_Invoice].[ForCombo_AppList] @SP_Loginid={0},@SP_AppId={1},@SP_SearchType={2},@sp_searchstring={3}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(AppID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetCreditLimitEntityCategoryDropdown(int LoginID, int CompCategoryID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[MoveMan].[ForCombo_CreditLimitEntityCategory] @SP_Loginid={0},@SP_CompCategoryID={1},@SP_SearchType={2},@sp_searchstring={3}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CompCategoryID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetCreditPeriodBasisDropdown(int LoginID, bool RMCBuss = false, int CompID = 0, int PeriodBasisID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[MoveMan].[ForCombo_CreditPeriodBasis] @SP_Loginid={0},@SP_CreditPeriodBasisID={1},@SP_SearchType={2},@sp_searchstring={3}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(PeriodBasisID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetPaymentProcessTypeDropdown(int LoginID, int TypeID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[MoveMan].[ForCombo_PaymentProcessType] @SP_Loginid={0},@SP_TypeID={1},@SP_SearchType={2},@sp_searchstring={3}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(TypeID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetCreditWriterEntityDropdown(int LoginID, int EntityID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[MoveMan].[ForCombo_CreditWriterEntity] @SP_Loginid={0},@SP_EntityID={1},@SP_SearchType={2},@sp_searchstring={3}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(EntityID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetProjectDropdown(int LoginID, int ProjectID, string SPTYPE, string Search, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[CR].[GetComboForCRProject] @SP_Loginid={0},@SP_PROJECTID={1},@SP_TYPE={2},@SP_SEARCHSTRING={3}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(ProjectID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetProjectModuleDropdown(int LoginID, int ProjectID, int ModuleID, string SPTYPE, string Search, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[CR].[GetComboForCRProject] @SP_Loginid={0},@SP_PROJECTID={1},@SP_TYPE={2},@SP_SEARCHSTRING={3},@SP_MODULEID={4}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(ProjectID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search), CSubs.QSafeValue(Convert.ToString(ModuleID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetDevTeamDropdown(int LoginID, int MemberID, string SPTYPE, string Search, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[CR].[GetComboForDevTeam] @SP_Loginid={0},@SP_MEMBERID={1},@SP_TYPE={2},@SP_SEARCHSTRING={3}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(MemberID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetCRRequestTypeDropdown(int LoginID, int TypeID, string SPTYPE, string Search, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[CR].[GetComboForRequestType] @SP_Loginid={0},@SP_TYPEID={1},@SP_TYPE={2},@SP_SEARCHSTRING={3}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(TypeID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetCRStatusDropdown(int LoginID, int StatusID, string SPTYPE, string Search, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[CR].[GetComboForStatus] @SP_Loginid={0},@SP_STATUSID={1},@SP_TYPE={2},@SP_SEARCHSTRING={3}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(StatusID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetCRRequstorDropdown(int LoginID, int RequestorID, string SPTYPE, string Search, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[CR].[GetComboForRequestor] @SP_Loginid={0},@SP_REQUESTORID={1},@SP_TYPE={2},@SP_SEARCHSTRING={3}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(RequestorID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetCRNumberDropdown(int LoginID, int RequestID, string SPTYPE, string Search, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[CR].[GetComboForCRNumber] @SP_Loginid={0},@SP_REQID={1},@SP_TYPE={2},@SP_SEARCHSTRING={3}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(RequestID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetCRDepartmentDropdown(int LoginID, int DeptID, string SPTYPE, string Search, bool Allitems)
        {
            return CSubs.BindDropdown(string.Format("[CR].[GetComboForDepartment] @SP_Loginid={0},@SP_DEPARTMENTID={1},@SP_TYPE={2},@SP_SEARCHSTRING={3}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(DeptID)), CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetJobStatusSDDropdown(int LoginID, string SPTYPE, int JobStatusSDId, string Search)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_JobStatusSD] @SP_Type={0},@SP_LoginId={1},@SP_JobStatusSDId={2},@SP_SearchString={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(JobStatusSDId)), CSubs.QSafeValue(Search)));
        }

        public IEnumerable<SelectListItem> GetBillingStatusDropdown(int LoginID, string SPTYPE, int BillingStatusId, string Search)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_BillingStatus] @SP_Type={0},@SP_LoginId={1},@SP_BillingStatusId={2},@SP_SearchString={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(BillingStatusId)), CSubs.QSafeValue(Search)));
        }

        public IEnumerable<SelectListItem> GetApprovalUserList(int LoginID, string SPTYPE, bool IsRmcBuss, int CompID, string MoveId)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_CSApprovalUserList] @SP_Type={0},@SP_LoginId={1},@SP_RMCBuss={2},@SP_CompID={3},@SP_MoveID={4}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), IsRmcBuss, CSubs.QSafeValue(Convert.ToString(CompID)), CSubs.QSafeValue(Convert.ToString(MoveId))));
        }

        public IEnumerable<SelectListItem> GetGPApprovalUserList(int LoginID, string SPTYPE, bool IsRmcBuss, int CompID, string MoveId, bool IsSendForApproval)
        {
            if (IsSendForApproval)
            {
                return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_GPSendApprovalUserList] @SP_Type={0},@SP_LoginId={1},@SP_RMCBuss={2},@SP_CompID={3},@SP_MoveID={4}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), IsRmcBuss, CSubs.QSafeValue(Convert.ToString(CompID)), CSubs.QSafeValue(Convert.ToString(MoveId))));
            }
            else
            {
                return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_GPApprovalUserList] @SP_Type={0},@SP_LoginId={1},@SP_RMCBuss={2},@SP_CompID={3},@SP_MoveID={4}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), IsRmcBuss, CSubs.QSafeValue(Convert.ToString(CompID)), CSubs.QSafeValue(Convert.ToString(MoveId))));
            }

        }

        public IEnumerable<SelectListItem> GetBankDetailDropdown(int LoginID, int CompID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_DynamicBank] @SP_LoginId={0},@SP_CompID={1}", CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CompID))));
        }

        public IEnumerable<SelectListItem> GetQuoteApprovalUserList(int LoginID, string SPTYPE, bool IsRmcBuss, int CompID, string SurveyID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_QuoteApprovalUserList] @SP_Type={0},@SP_LoginId={1},@SP_RMCBuss={2},@SP_CompID={3},@SP_SurveyID={4}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), IsRmcBuss, CSubs.QSafeValue(Convert.ToString(CompID)), CSubs.QSafeValue(Convert.ToString(SurveyID))));
        }

        public IEnumerable<SelectListItem> GetBTRServiceList(int LoginID, string SPTYPE)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_BTRService] @SP_Type={0},@SP_LoginId={1},@SP_ServiceId={2},@SP_SearchString={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(null)), CSubs.QSafeValue(Convert.ToString(null))));
        }

        public IEnumerable<SelectListItem> GetPaymentTermList(int LoginID, string SPTYPE)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_BTRPaymentTerm] @SP_Type={0},@SP_LoginId={1},@SP_PaymentTermId={2},@SP_SearchString={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(null)), CSubs.QSafeValue(Convert.ToString(null))));
        }
        public IEnumerable<SelectListItem> GetBillingEntityList(Int64 LoginID, Int64 CompID, int RMCID, char BillType, char BussLine)
        {
            //CompID = 2;
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_RMCBillingEntity] @SP_RMCID={0},@SP_CompanyID={1},@SP_LoginID={2},@SP_BillType={3},@SP_BussLine={4}", 
                RMCID, CSubs.QSafeValue(Convert.ToString(CompID)), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(BillType)), CSubs.QSafeValue(Convert.ToString(BussLine))));
        }


        public IEnumerable<SelectListItem> GetDebtorDropdown(string LoginID, string SPTYPE = "ALLACTIVE", bool IsRMCBuss = false, int? DebtorId = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Debtor] @SP_Type={0},@SP_LoginId={1},@SP_DebtorId={2},@SP_SearchString={3}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(DebtorId)), CSubs.QSafeValue(Convert.ToString(SearchString))));
        }

        public IEnumerable<SelectListItem> GetDebitNoteTypeDropdown(string LoginID, string SPTYPE = "ALLACTIVE", bool IsRMCBuss = false, int? DNTypeId = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_DebitNoteType] @SP_Type={0},@SP_LoginId={1},@SP_DNTypeId={2},@SP_SearchString={3}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(DNTypeId)), CSubs.QSafeValue(Convert.ToString(SearchString))));
        }

        public IEnumerable<SelectListItem> GetDebitNoteCostHeadDropdown(string LoginID, string SPTYPE = "ALLACTIVE", bool IsRMCBUss = false, int? DNCostHeadID = null, int? DNTypeId = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_DebitNoteCostHead] @SP_Type={0},@SP_LoginId={1},@SP_DNCostHeadId={2},@SP_DNTypeId={3},@SP_SearchString={4}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(DNCostHeadID)), CSubs.QSafeValue(Convert.ToString(DNTypeId)), CSubs.QSafeValue(Convert.ToString(SearchString))));
        }

        public IEnumerable<SelectListItem> GetSBUDropdown(string LoginID, string SPTYPE = "ALLACTIVE", bool IsRMCBuss = false, int? SBUId = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_SBU] @SP_Type={0},@SP_LoginId={1},@SP_SBUId={2},@SP_SearchString={3}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(SBUId)), CSubs.QSafeValue(Convert.ToString(SearchString))));
        }

        public IEnumerable<SelectListItem> GetDebitNoteUnitDropdown(string LoginID, string SPTYPE = "ALLACTIVE", bool IsRMCBuss = false, int? DNUnitId = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_DebitNoteUnit] @SP_Type={0},@SP_LoginId={1},@SP_DNUnitId={2},@SP_SearchString={3}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(DNUnitId)), CSubs.QSafeValue(Convert.ToString(SearchString))));
        }

        public IEnumerable<SelectListItem> GetCreditApprovalCorporateDropdown(string LoginID, string SPTYPE = "ALL", string CORA = null, bool IsRMCBuss = false, int CompID = 0, int? AgentId = null, int? CreditLimitEntityID = -1, string searchstring = null, bool Allitems = false)
        {

            return CSubs.BindDropdown(string.Format("[MoveMan].[ForCombo_CreditApprovalAgent] @SP_Type={0},@SP_Loginid={1},@SP_CorA={2},@SP_CompanyID={3},@SP_ISRMCBuss={4},@SP_AgentID={5},@sp_searchstring={6},@SP_CreditEntityID={7}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(CORA), CSubs.QSafeValue(CompID.ToString()), IsRMCBuss, CSubs.QSafeValue(Convert.ToString(AgentId)), CSubs.QSafeValue(Convert.ToString(searchstring)), CSubs.QSafeValue(Convert.ToString(CreditLimitEntityID))), Allitems);

        }

        public IEnumerable<SelectListItem> GetCreditApprovalProjectDropdown(string LoginID, string SPTYPE = "ALL", bool RMCBuss = false, int CompID = 0, int ProejctID = 0, string searchstring = null, bool Allitems = false)
        {

            return CSubs.BindDropdown(string.Format("[MoveMan].[ForCombo_CreditApprovalProject] @SP_Type={0},@SP_Loginid={1},@SP_ProjectID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), ProejctID), Allitems);
        }

        public IEnumerable<SelectListItem> GetCreditApprovalServiceLineDropdown(string LoginID, string SPTYPE = "ALL", bool RMCBuss = false, int CompID = 0, int ProjectID = 0, int ServiceLineID = 0, string searchstring = null, bool Allitems = false)
        {

            return CSubs.BindDropdown(string.Format("[MoveMan].[ForCombo_CreditApprovalServiceLine] @SP_Type={0},@SP_Loginid={1},@SP_ProjectID={2},@SP_ServiceLineID={3},@SP_CompID={4},@SP_IsRMC={5}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), ProjectID, ServiceLineID, CompID, RMCBuss), Allitems);
        }

        public IEnumerable<SelectListItem> GetCreditApprovalSendToDropdown(string LoginID, string SPTYPE = "ALL", bool RMCBuss = false, int CompID = 0, int SendToApproverID = 0, float Amount = 0, string searchstring = null, bool Allitems = false)
        {

            return CSubs.BindDropdown(string.Format("[MoveMan].[ForCombo_CreditSendToApprover]  @SP_Type={0},@SP_Loginid={1},@SP_SendToApproverID={2},@SP_Amount={3},@SP_searchstring={4},@SP_CompID={5},@SP_IsRMC={6}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), SendToApproverID, Amount, CSubs.QSafeValue(searchstring), CompID, RMCBuss), Allitems);
        }

        public IEnumerable<SelectListItem> GetVehicleMovementMasterDropdown(string LoginID, string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForCombo_VehicleMovementMaster]  @SP_SearchType={0},@SP_Loginid={1},@SP_ID={2},@SP_searchstring={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), id, CSubs.QSafeValue(searchstring)), Allitems);
        }

        public IEnumerable<SelectListItem> GetVehicleDimensionDropdown(string LoginID, string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForCombo_VehicleDimension]  @SP_SearchType={0},@SP_Loginid={1},@SP_ID={2},@SP_searchstring={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), id, CSubs.QSafeValue(searchstring)), Allitems);
        }
        public IEnumerable<SelectListItem> GetVehicleReasonDropdown(string LoginID, string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForCombo_VehicleReasonMaster]  @SP_SearchType={0},@SP_Loginid={1},@SP_ID={2},@SP_searchstring={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), id, CSubs.QSafeValue(searchstring)), Allitems);
        }

        public IEnumerable<SelectListItem> GetWHAssessmentStatusDropdown(string LoginID, string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForCombo_WHAssessmentStatus]  @SP_SearchType={0},@SP_Loginid={1},@SP_ID={2},@SP_searchstring={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), id, CSubs.QSafeValue(searchstring)), Allitems);
        }


        public IEnumerable<SelectListItem> GetWHAssessmentPriorityDropdown(string LoginID, string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForCombo_WHAssessmentPriority]  @SP_SearchType={0},@SP_Loginid={1},@SP_ID={2},@SP_searchstring={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), id, CSubs.QSafeValue(searchstring)), Allitems);
        }

        public IEnumerable<SelectListItem> GetWHAssessmentResponsibilityDropdown(string LoginID, string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForCombo_WHAssessmentResponsibility]  @SP_SearchType={0},@SP_Loginid={1},@SP_ID={2},@SP_searchstring={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), id, CSubs.QSafeValue(searchstring)), Allitems);
        }

        public IEnumerable<SelectListItem> GetWHAssessmentCategoryDropdown(string LoginID, string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForCombo_WHAssessmentCategory]  @SP_SearchType={0},@SP_Loginid={1},@SP_ID={2},@SP_searchstring={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), id, CSubs.QSafeValue(searchstring)), Allitems);
        }

        public IEnumerable<SelectListItem> GetWHDocTypeDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? DocTypeID = -1, string DocFromType = "", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForCombo_WHDocType] @SP_Type={0},@SP_Loginid={1},@SP_DocTypeID={2},@SP_DocFromType={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(DocTypeID)), CSubs.QSafeValue(DocFromType)), Allitems);
        }

        public IEnumerable<SelectListItem> GetWHDocNameDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? DocNameID = -1, int DocTypeID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForCombo_WHDocName] @SP_Type={0},@SP_Loginid={1},@SP_DocNameID={2},@SP_DocTypeID={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(DocNameID)), CSubs.QSafeValue(Convert.ToString(DocTypeID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetGenderDropdown(string LoginID, string SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Gender] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetMaritalStatusDropdown(string LoginID, string SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_MaritalStatus] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetATRStatusDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? StatusID = -1, bool IsCompliance = false, bool IsAuitee = false, Int64? ATRPointId = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[ATR].[ForCombo_ATRStatusMaster] @SP_Type={0},@SP_Loginid={1},@SP_StatusID={2},@SP_IsCompliance={3},@SP_IsAuditee={4},@SP_ATRPointId={5}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(StatusID)), CSubs.QSafeValue(Convert.ToString(IsCompliance)), CSubs.QSafeValue(Convert.ToString(IsAuitee)), CSubs.QSafeValue(Convert.ToString(ATRPointId))), Allitems);
        }

        public IEnumerable<SelectListItem> GetATRRiskDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? RiskID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[ATR].[ForCombo_RiskTypeMaster] @SP_Type={0},@SP_Loginid={1},@SP_RiskID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(RiskID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetATRDepartmentDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? DeptID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[ATR].[ForCombo_ATRDepartment] @SP_Type={0},@SP_Loginid={1},@SP_DeptID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(DeptID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetATREmployeeDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? EmpID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[ATR].[ForCombo_ATREmployee] @SP_Type={0},@SP_Loginid={1},@SP_EmpID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(EmpID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetATRCategoryDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? CategoryID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[ATR].[ForCombo_ATRCategoryMaster] @SP_Type={0},@SP_Loginid={1},@SP_CategoryID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CategoryID))), Allitems);
        }
        public IEnumerable<SelectListItem> GetComplaintsClassificationDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? ClassificationID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[complaints].[ForCombo_ClassificationMaster] @SP_Type={0},@SP_Loginid={1},@SP_ClassificationID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(ClassificationID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetComplaintsSourceDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? SourceID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[complaints].[ForCombo_SourceMaster] @SP_Type={0},@SP_Loginid={1},@SP_SourceID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(SourceID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetComplaintsStatusDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? StatusID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[complaints].[ForCombo_StatusMaster] @SP_Type={0},@SP_Loginid={1},@SP_StatusID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(StatusID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetComplaintsEnquiryNoDropdown(int LoginID, bool IsRMCBuss, int CompanyID, string SPTYPE = "ALLACTIVE", string Search = "", int? EnqID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[complaints].[ForCombo_EnquiryNo] @SP_Type={0},@SP_Loginid={1},@SP_EnqID={2},@SP_IsRMC={3},@SP_CompanyID={4},@SP_SEARCHSTR={5}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(EnqID)), IsRMCBuss, CompanyID, CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetComplaintsJobNoDropdown(int LoginID, bool IsRMCBuss, int CompanyID, string SPTYPE = "ALLACTIVE", string Search = "", int? MoveID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[complaints].[ForCombo_JobNo] @SP_Type={0},@SP_Loginid={1},@SP_MoveID={2},@SP_IsRMC={3},@SP_CompanyID={4},@SP_SEARCHSTR={5}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(MoveID)), IsRMCBuss, CompanyID, CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetComplaintsShipmentDropdown(int LoginID, bool IsRMCBuss, int CompanyID, string SPTYPE = "ALLACTIVE", string Search = "", int? EnqID = -1, int? EnqDetailID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[complaints].[ForCombo_EnquiryDetailNo] @SP_Type={0},@SP_Loginid={1},@SP_EnqID={2},@SP_IsRMC={3},@SP_CompanyID={4},@SP_SEARCHSTR={5},@SP_EnqDetailID={6}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(EnqID)), IsRMCBuss, CompanyID, CSubs.QSafeValue(Search), CSubs.QSafeValue(Convert.ToString(EnqDetailID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetInvDmsAgentDropdown(int LoginID, bool IsRMCBUss, int CompanyID, string CorA = "A", string Search = "", string SPTYPE = "ALLACTIVE", int? AgentID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Moveman].[ForCombo_AgentInvoiceDMS] @SP_Type={0},@SP_Loginid={1},@SP_AgentID={2},@SP_IsRMCBuss={3},@SP_CompanyID={4},@sp_searchstring={5},@SP_CorA={6}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(AgentID)), IsRMCBUss, CompanyID, CSubs.QSafeValue(Search), CSubs.QSafeValue(CorA)), Allitems);
        }

        public IEnumerable<SelectListItem> GetWH_JobTypeDropdown(int LoginID, bool IsRMCBUss, int CompanyID, string Search = "", string SPTYPE = "ALLACTIVE", int? JobTypeId = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[Warehouse].[ForCombo_WH_JobType] @SP_SearchType={0},@SP_Loginid={1},@sp_id={2},@sp_searchstring={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(JobTypeId)), CSubs.QSafeValue(Search)), Allitems);
        }

        public IEnumerable<SelectListItem> GetVendorContractServiceCategoryDropdown(int LoginID, string Search = "", string SPTYPE = "ALLACTIVE", int? CategoryID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[VC].[ForCombo_ServiceCategoryMaster] @SP_SearchType={0},@SP_Loginid={1},@SP_ServiceCategoryID={2},@sp_searchstring={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CategoryID)), CSubs.QSafeValue(Convert.ToString(Search))), Allitems);
        }


        public IEnumerable<SelectListItem> GetVendorContractBuinessUnitDropdown(int LoginID, string Search = "", string SPTYPE = "ALLACTIVE", int? BUID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[VC].[ForCombo_BuinessUnitMaster] @SP_SearchType={0},@SP_Loginid={1},@SP_BUID={2},@sp_searchstring={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(BUID)), CSubs.QSafeValue(Convert.ToString(Search))), Allitems);
        }

        public IEnumerable<SelectListItem> GetVendorContractDocTypelDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? DocTypeID = -1, string DocFromType = "", bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[VC].[ForCombo_DocumentTypeMaster] @SP_Type={0},@SP_Loginid={1},@SP_DocTypeID={2},@SP_DocFromType={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(DocTypeID)), CSubs.QSafeValue(DocFromType)), Allitems);
        }

        public IEnumerable<SelectListItem> GetVendorContractDocumentDropdown(int LoginID, string SPTYPE = "ALLACTIVE", int? DocNameID = -1, int DocTypeID = -1, bool Allitems = false)
        {
            return CSubs.BindDropdown(string.Format("[VC].[ForCombo_DocumentNameMaster] @SP_Type={0},@SP_Loginid={1},@SP_DocNameID={2},@SP_DocTypeID={3}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(DocNameID)), CSubs.QSafeValue(Convert.ToString(DocTypeID))), Allitems);
        }

        public IEnumerable<SelectListItem> GetInAssetDetails(string LoginID, string SPTYPE = "ALLACTIVE", Int64? MoveID = null, Int64? InMastID = null, Int64? AssetDetID = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_InAssetDetails] @SP_Type={0},@SP_LoginId={1},@SP_MoveID={2},@SP_InMastID={3},@SP_AssetDetID={4},@SP_SearchString={5}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(InMastID)),
                CSubs.QSafeValue(Convert.ToString(AssetDetID)), CSubs.QSafeValue(Convert.ToString(SearchString))));
        }

        public IEnumerable<SelectListItem> GetServiceEmpType(string LoginID, string SPTYPE = "ALLACTIVE", int? EmpTypeID = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ServiceEmpType] @SP_Type={0},@SP_LoginID={1},@SP_EmpTypeID={2},@SP_SearchString={3}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(EmpTypeID)), CSubs.QSafeValue(Convert.ToString(SearchString))));
        }

        public IEnumerable<SelectListItem> GetLiftVanList(string LoginID, string SPTYPE = "ALLACTIVE", Int64? LiftVanID = null, Int64? MoveID = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_LiftVanList] @SP_Type={0},@SP_LoginID={1},@SP_LiftVanID={2},@SP_MoveID={3},@SP_SearchString={4}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(LiftVanID)), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(SearchString))));
        }

        public IEnumerable<SelectListItem> GetWOSCSApprovalUserList(int LoginID, string SPTYPE, int CompID, Int64? WOSMoveID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_WOSCSApprovalUserList] @SP_Type={0},@SP_LoginId={1},@SP_CompID={2},@SP_WOSMoveID={3}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(LoginID)), CSubs.QSafeValue(Convert.ToString(CompID)), CSubs.QSafeValue(Convert.ToString(WOSMoveID))));
        }

        public IEnumerable<SelectListItem> GetJobNoForAssetManagement(int LoginID, string SPTYPE, Int64? MoveID, int? CompID, string SearchString = "")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_JobNoForAssetManagement] @SP_Type={0},@SP_MoveID={1},@SP_CompID={2},@SP_SearchString={3},@SP_LoginID={4}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(MoveID)), CSubs.QSafeValue(Convert.ToString(CompID)),
                CSubs.QSafeValue(SearchString), CSubs.QSafeValue(Convert.ToString(LoginID))));
        }
        public IEnumerable<SelectListItem> GetBillCategory(string LoginID, string SPTYPE = "ALLACTIVE", int? BillCategoryID = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_BillCategory] @SP_Type={0},@SP_LoginID={1},@SP_BillingCategoryID={2},@SP_SearchString={3}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(BillCategoryID)), CSubs.QSafeValue(Convert.ToString(SearchString))));
        }

        public IEnumerable<SelectListItem> GetJobGridSearchList(string LoginID, string SPTYPE = "ALLACTIVE", int? JobSearchID = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("Moveman.ForComboJobFilter"));
        }

        public IEnumerable<SelectListItem> GetVCStatusList(string LoginID, string SPTYPE = "ALLACTIVE", int? StatusID = null, string SearchString = null)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_VCStatus] @SP_Type={0},@SP_LoginID={1},@SP_StatusID={2},@SP_SearchString={3}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(StatusID)), CSubs.QSafeValue(Convert.ToString(SearchString))));
        }

        public IEnumerable<SelectListItem> GetDashboardReasonList(string LoginID, string SPTYPE = "ALLACTIVE", int? ReasonID = null, string SearchString = null,int JobStatusID = 0)
        {
            return CSubs.BindDropdown(string.Format("[Dashboard].[ForCombo_ReasonList] @SP_Type={0},@SP_ReasonID={1},@SP_SearchString={2},@SP_JobStatusID={3}",
                CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(Convert.ToString(ReasonID)), CSubs.QSafeValue(Convert.ToString(SearchString)), CSubs.QSafeValue(Convert.ToString(JobStatusID))));
        }
        
    }
}