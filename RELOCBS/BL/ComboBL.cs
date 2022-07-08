using RELOCBS.DAL;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace RELOCBS.BL
{
    public class ComboBL
    {
        private ComboDAL _comboDAL;

        public ComboDAL comboDAL
        {

            get
            {
                if (this._comboDAL == null)
                    this._comboDAL = new ComboDAL();
                return this._comboDAL;
            }
        }

        public IEnumerable<SelectListItem> GetCurrencyDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetCurrencyDropdown(Convert.ToString(UserSession.GetUserSession()?.LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetCityDropdown(string SPTYPE = "ALL", int? ContinentID = -1, int? CountryID = -1, int? CityID = -1)
        {
            return comboDAL.GetCityDropdown(Convert.ToString(UserSession.GetUserSession()?.LoginID ?? 0), SPTYPE, ContinentID, CountryID, CityID);
        }

        public IEnumerable<SelectListItem> GETCountryDropdown(string SPTYPE = "ALL", int? ContinentID = -1, int? CountryID = -1)
        {
            return comboDAL.GETCountryDropdown(Convert.ToString(UserSession.GetUserSession()?.LoginID ?? 0), SPTYPE, ContinentID, CountryID);
        }

        public IEnumerable<SelectListItem> GETContinentDropdown(string SPTYPE = "ALL", int? ContinentID = -1)
        {
            return comboDAL.GETContinentDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, ContinentID);
        }

        public IEnumerable<SelectListItem> PortsByCityShipmentModeCombo(int CityID, int shipmentModeID)
        {
            if (CityID == 0)
            { CityID = -1; }

            return comboDAL.PortsByCityShipmentModeCombo(Convert.ToString(UserSession.GetUserSession().LoginID), CityID, shipmentModeID);
        }

        public IEnumerable<SelectListItem> GetRMCDropdown(string SPTYPE = "ALL", int? RMCID = -1)
        {
            return comboDAL.GetRMCDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, RMCID);
        }

        public IEnumerable<SelectListItem> GetAgentDropdown(string SPTYPE = "ALL", string CORA = null, int? AgentId = null, string searchstring = null)
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            if (CORA == "CL")
            {
                CORA = IsRMCBUss ? "R" : null;
            }
            return comboDAL.GetAgentDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, CORA, IsRMCBUss, AgentId, searchstring);
        }

        public IEnumerable<SelectListItem> OriginCityDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.OriginCityDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> DestinationCityDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.DestinationCityDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetServiceLineDropdown(string SPTYPE = "ALL", bool RMCBuss = false, string ForPage = "", string BussLine = null)
        {
            int CompID = UserSession.GetUserSession().CompanyID;
            return comboDAL.GetServiceLineDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, RMCBuss, ForPage, CompID, BussLine);
        }

        public IEnumerable<SelectListItem> GetBusinessLineDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetBusinessLineDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetGoodsDescriptionDropdown(string SPTYPE = "ALL", int ServiceLineID = 0)
        {
            return comboDAL.GetGoodsDescriptionDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, ServiceLineID);
        }

        public IEnumerable<SelectListItem> GetModeDropdown(string SPTYPE = "ALL", int ServiceLineID = 0)
        {
            return comboDAL.GetModeDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, ServiceLineID);
        }

        public IEnumerable<SelectListItem> GetRateComponentDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetRateComponentDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetRateAgentDropdown(string SPTYPE = "ALL")
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            return comboDAL.GetRateAgentDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, IsRMCBUss);
        }
        public IEnumerable<SelectListItem> GetFromLocationDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetFromLocationDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }
        public IEnumerable<SelectListItem> GetToLocationDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetToLocationDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }
        public IEnumerable<SelectListItem> GetRateCurrencyDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetRateCurrencyDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }
        public IEnumerable<SelectListItem> GetBaseCurrencyRateDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetBaseCurrencyRateDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }
        public IEnumerable<SelectListItem> GetMeasurementUnitDropdown(char UnitType, int? WeightUnitID = -1, string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetMeasurementUnitDropdown(UnitType, Convert.ToString(UserSession.GetUserSession().LoginID), WeightUnitID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetCostHeadDropdown(int? RMC_ID = -1, int? MoveCompID = -1, string ForCombo = null, string SPTYPE = "ALLACTIVE")
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            return comboDAL.GetCostHeadDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), RMC_ID, MoveCompID, IsRMCBUss, ForCombo, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetPortDropdown(int? CityID = -1, string SeaOrAir = "", string SPTYPE = "ALLACTIVE", int PortID = -1, string Search = "", bool Allitems = false)
        {
            return comboDAL.GetPortDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE: SPTYPE, CityID: CityID, SeaOrAir: SeaOrAir, PortID: PortID, Search: Search, Allitems: Allitems);
        }


        public IEnumerable<SelectListItem> GetEnqInfoSourceDropdown(int? EnqSourceID = -1, string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetEnqInfoSourceDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), EnqSourceID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetMoveQuoteClassDropdown(int? MoveQuoteID = -1, string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetMoveQuoteClassDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), MoveQuoteID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetShipmentTypeDropdown(int? ShipmentTypeID = -1, string SPTYPE = "ALLACTIVE", int ServiceLineID = 0)
        {
            return comboDAL.GetShipmentTypeDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), ShipmentTypeID, SPTYPE, ServiceLineID);
        }

        public IEnumerable<SelectListItem> GetCompanyDropdown(int? CountryID = -1, int? CityID = -1, int? CompanyID = -1, string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetCompanyDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), CountryID, CityID, CompanyID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetCompanyBranchDropdown(int? CompanyBranchID = -1, int? CompanyID = -1, string SPTYPE = "ALLACTIVE", bool IsRMCBuss = false, bool IsRev = false, int? RMCID = null, string ForPage = "", int? SBUId = null)
        {
            CompanyID = UserSession.GetUserSession().CompanyID;
            return comboDAL.GetCompanyBranchDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), CompanyBranchID, CompanyID, SPTYPE, IsRMCBuss, IsRev, RMCID, ForPage, SBUId);
        }

        public IEnumerable<SelectListItem> GetClickRestrictDropdown(int? ClickRestrictID = -1, string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetClickRestrictDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), ClickRestrictID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetContainerSizeDropdown(int? ContainerSizeID = -1, string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetContainerSizeDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), ContainerSizeID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetCompetitorDropdown(int? CompanyID = -1, int? CompetitorID = -1, string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetCompetitorDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), CompanyID, CompetitorID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetEmployeeDropdown(int? CountryID = -1, int? CityID = -1, int? CompanyID = -1, int BranchID = -1, string SPTYPE = "ALLACTIVE", bool IsMoveCordination = false)
        {
            bool IsRMCBuss = false;
            if (IsMoveCordination)
            {
                if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
                {
                    IsRMCBuss = false;
                }
                else
                {
                    IsRMCBuss = true;
                }
            }
            return comboDAL.GetEmployeeDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), CountryID, CityID, UserSession.GetUserSession().CompanyID, BranchID, SPTYPE, IsRMCBuss);
        }

        public IEnumerable<SelectListItem> GetEnquiryLostReasonDropdown(int? EnqLostReasonID = -1, string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetEnquiryLostReasonDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), EnqLostReasonID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetShipperCategoryDropdown(int? ShipperCategoryID = -1, string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetShipperCategoryDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), ShipperCategoryID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetShipperTypeDropdown(int? ShipperTypeID = -1, string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetShipperTypeDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), ShipperTypeID, SPTYPE);
        }


        public IEnumerable<SelectListItem> GetShippingLineDropdown(string ModeID, int? ShippingLineID = -1, string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetShippingLineDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), ModeID, ShippingLineID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetInsuranceTypeDropdown(int InsuranceTypeID = -1, String SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetInsuranceTypeDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), InsuranceTypeID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetUserCompanyMapDropdown(int LoginId = -1)
        {
            return comboDAL.GetUserCompanyMapDropdown(LoginId);
        }
        public IEnumerable<SelectListItem> GetUserBussinessLineDropdown(int LoginId = -1)
        {
            return comboDAL.GetUserBussinessLineDropdown(LoginId);
        }


        public IEnumerable<SelectListItem> GetLoginTypeDropdown(string LoginType = null, String SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetLoginTypeDropdown(UserSession.GetUserSession().LoginID, LoginType, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetSFRCalculationMethodDropdown()
        {
            return comboDAL.GetSFRCalculationMethodDropdown();
        }

        public DataTable GetCityData()
        {
            return comboDAL.GetCityData(UserSession.GetUserSession().LoginID.ToString());
        }

        public IEnumerable<SelectListItem> GetVehicleDropdown(int? BranchID = -1, int? VendorID = -1, int? CompanyID = -1, string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetVehicleDropdown(UserSession.GetUserSession().LoginID, BranchID, VendorID, UserSession.GetUserSession().CompanyID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetJobAllocationStatusDropdown(string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetJobAllocationStatusDropdown(UserSession.GetUserSession().LoginID, SPTYPE);
        }

        public IEnumerable<SelectListItem> getCrewDropdown(string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.getCrewDropdown(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, SPTYPE, Allitems);
        }


        public IEnumerable<SelectListItem> GetVendorDropdown(string SPTYPE = "ALLACTIVE", int? VendorID = -1, bool Allitems = false)
        {
            return comboDAL.GetVendorDropdown(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, SPTYPE, VendorID, Allitems);
        }

        public IEnumerable<SelectListItem> getPurposeDropdown(string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.getPurposeDropdown(UserSession.GetUserSession().LoginID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetDocTypeDropdown(string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetDocTypeDropdown(UserSession.GetUserSession().LoginID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetActivityTypeDropdown(string SPTYPE = "ALLACTIVE", int JobTypeId = -1)
        {
            return comboDAL.GetActivityTypeDropdown(UserSession.GetUserSession().LoginID, SPTYPE, JobTypeId);
        }

        public IEnumerable<SelectListItem> getMaterialDropdown(string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.getMaterialDropdown(UserSession.GetUserSession().LoginID, SPTYPE, Allitems);
        }

        public IEnumerable<SelectListItem> getDesignationDropdown(string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.getDesignationDropdown(UserSession.GetUserSession().LoginID, SPTYPE, Allitems);
        }


        public IEnumerable<SelectListItem> getShipperDesignationDropdown(string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.getShipperDesignationDropdown(UserSession.GetUserSession().LoginID, SPTYPE, Allitems);
        }

        public IEnumerable<SelectListItem> getJobNolDropdown(Int64 MoveId = -1, string Shipper = "", bool IsRMCBuss = false, string SPTYPE = "ALLACTIVE", bool Allitems = false, int ServiceLineID = -1, bool IsStorage = false, string SearchStr = "", int Modeid = -1, Int64? MasterID = -1)
        {

            if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
            {
                IsRMCBuss = false;
            }
            else
            {
                IsRMCBuss = true;
            }

            return comboDAL.getJobNolDropdown(MoveId, Shipper, IsRMCBuss, UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, SPTYPE, Allitems, ServiceLineID, IsStorage, SearchStr, Modeid, MasterID);

        }

        public IEnumerable<SelectListItem> GetSFRCalculationMethodDropdown(bool IsGetUnit = false)
        {
            return comboDAL.GetSFRCalculationMethodDropdown(IsGetUnit);
        }

        public IEnumerable<SelectListItem> GetJobActivityList(string MoveID = null)
        {
            return comboDAL.GetJobActivityList(UserSession.GetUserSession().LoginID, MoveID, "ALL");
        }

        public IEnumerable<SelectListItem> GetJobDocTypelDropdown(string SPTYPE = "ALLACTIVE", int? DocTypeID = -1, string DocFromType = "", bool Allitems = false)
        {
            return comboDAL.GetJobDocTypelDropdown(UserSession.GetUserSession().LoginID, SPTYPE, DocTypeID, DocFromType, Allitems);
        }

        public IEnumerable<SelectListItem> GetJobDocNamelDropdown(string SPTYPE = "ALLACTIVE", int? DocNameID = -1, int DocTypeID = -1, bool Allitems = false)
        {
            return comboDAL.GetJobDocNamelDropdown(UserSession.GetUserSession().LoginID, SPTYPE, DocNameID, DocTypeID, Allitems);
        }
        public IEnumerable<SelectListItem> GetCaseTypeDropdown(string SPTYPE = "ALLACTIVE", int? CaseTypeID = -1, bool Allitems = false)
        {
            return comboDAL.GetCaseTypeDropdown(UserSession.GetUserSession().LoginID, SPTYPE, CaseTypeID, Allitems);
        }

        public IEnumerable<SelectListItem> GetInst_SubQuestionDropdown(string SPTYPE = "ALLACTIVE", string DropdownType = "", int? AnswerID = -1, bool Allitems = false)
        {
            return comboDAL.GetInst_SubQuestionDropdown(UserSession.GetUserSession().LoginID, SPTYPE, DropdownType, AnswerID, Allitems);
        }

        public IEnumerable<SelectListItem> GetWarehouseDropdown(string SPTYPE = "ALLACTIVE", int? BranchID = -1, int? WarehouseID = -1, bool Allitems = false)
        {
            return comboDAL.GetWarehouseDropdown(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, SPTYPE, BranchID, WarehouseID, Allitems);
        }

        public IEnumerable<SelectListItem> GetWarehouseStrgDropdown(string SPTYPE = "ALLACTIVE", int? BranchID = -1, int? WarehouseID = -1, bool Allitems = false)
        {
            return comboDAL.GetWarehouseStrgDropdown(UserSession.GetUserSession().LoginID, SPTYPE, BranchID, WarehouseID, Allitems);
        }

        public IEnumerable<SelectListItem> GetInstQuestionAnswerDropdown(string SPTYPE = "ALLACTIVE", string DropdownType = "", int? AnswerID = -1, bool Allitems = false)
        {
            return comboDAL.GetInstQuestionAnswerDropdown(UserSession.GetUserSession().LoginID, SPTYPE, DropdownType, AnswerID, Allitems);
        }

        public IEnumerable<SelectListItem> GetInvoiceForMoveDropdown(int MoveID = -1, bool IsStatement = false)
        {
            return comboDAL.GetInvoiceForMoveDropdown(MoveID, UserSession.GetUserSession().LoginID, IsStatement);
        }

        public IEnumerable<SelectListItem> GetSearchTypeDropdown(string SearchType = null)
        {
            return comboDAL.GetSearchTypeDropdown(SearchType, UserSession.GetUserSession().LoginID);
        }

        public IEnumerable<SelectListItem> GetInsuranceCompanyDropdown(string SPTYPE = "ALLACTIVE", Int64? MoveID = -1, bool Allitems = false)
        {
            return comboDAL.GetInsuranceCompanyDropdown(SPTYPE, Convert.ToInt64(MoveID), UserSession.GetUserSession().LoginID, Allitems);
        }

        public IEnumerable<SelectListItem> getInsurancePolicyNoDropdown(int InsuranceCompID = -1, int PolicyID = -1, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.getInsurancePolicyNoDropdown(InsuranceCompID, PolicyID, SPTYPE, Allitems, UserSession.GetUserSession().LoginID);

        }

        public IEnumerable<SelectListItem> getInsDelayReasonListDropdown(string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.getInsDelayReasonListDropdown(SPTYPE, UserSession.GetUserSession().LoginID, Allitems);

        }




        public IEnumerable<SelectListItem> GetClaimNatureDropdown(int ClaimNatureID = -1, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.GetClaimNatureDropdown(ClaimNatureID, SPTYPE, Allitems, UserSession.GetUserSession().LoginID);
        }

        public IEnumerable<SelectListItem> GetClaimStatusDropdown(int ClaimStatusID = -1, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.GetClaimStatusDropdown(ClaimStatusID, SPTYPE, Allitems, UserSession.GetUserSession().LoginID);
        }

        public IEnumerable<SelectListItem> GetClaimCategoryDropdown(int ClaimCategoryID = -1, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.GetClaimCategoryDropdown(ClaimCategoryID, SPTYPE, Allitems, UserSession.GetUserSession().LoginID);
        }

        public IEnumerable<SelectListItem> getClaimItemDetailIsDropdown(int ClaimItemDetailIsID = -1, int ClaimCategoryID = -1, string SearchString = "", string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.getClaimItemDetailIsDropdown(ClaimItemDetailIsID, ClaimCategoryID, SearchString, SPTYPE, Allitems, UserSession.GetUserSession().LoginID);

        }

        public IEnumerable<SelectListItem> GetPaymodeDropdown(string Paymode = "", string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.GetPaymodeDropdown(Paymode, SPTYPE, Allitems, UserSession.GetUserSession().LoginID);
        }

        public IEnumerable<SelectListItem> GetSettlementTypeDropdown(string SettlementType = "", string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.GetSettlementTypeDropdown(SettlementType, SPTYPE, Allitems, UserSession.GetUserSession().LoginID);
        }

        public IEnumerable<SelectListItem> GetChequeStatusDropdown(string ChequeStatus = "", string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.GetChequeStatusDropdown(ChequeStatus, SPTYPE, Allitems, UserSession.GetUserSession().LoginID);
        }

        public IEnumerable<SelectListItem> GetControllerDropdown(int ControllerID = -1, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.GetControllerDropdown(ControllerID, SPTYPE, Allitems, UserSession.GetUserSession().LoginID);
        }

        public IEnumerable<SelectListItem> getInsuranceNoDropdown(Int64 InsuranceID = -1, Int64? MoveID = -1, int CompID = -1, string SearchString = "", string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.getInsuranceNoDropdown(InsuranceID, MoveID, CompID, SearchString, SPTYPE, Allitems, UserSession.GetUserSession().LoginID);
        }

        public IEnumerable<SelectListItem> getClaimNoDropdown(Int64 ClaimID = -1, Int64 MoveID = -1, int CompID = -1, string SearchString = "", string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.getClaimNoDropdown(ClaimID, MoveID, CompID, SearchString, SPTYPE, Allitems, UserSession.GetUserSession().LoginID);
        }

        public IEnumerable<SelectListItem> GetStrgBillingPeriodDropdown(Int64 BillingPeriodID = -1, string SearchString = "", string SPTYPE = "ALLACTIVE", bool Allitems = false, string ComboType = "Act")
        {
            return comboDAL.GetStrgBillingPeriodDropdown(BillingPeriodID, SearchString, SPTYPE, Allitems, UserSession.GetUserSession().LoginID, ComboType);
        }

        public IEnumerable<SelectListItem> GetStrgCostHeadDropdown(Int64 StrgCostHeadID = -1, string StrgCostType = "", string SearchString = "", string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            return comboDAL.GetStrgCostHeadDropdown(StrgCostHeadID, StrgCostType, SearchString, SPTYPE, Allitems, UserSession.GetUserSession().LoginID);
        }

        public IEnumerable<SelectListItem> GetStateDropdown(string SPTYPE = "ALL", string SearchString = "", int ContinentID = -1, int CountryID = -1, int StateID = -1, bool Allitems = false)
        {
            return comboDAL.GetStateDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, SearchString, ContinentID, CountryID, StateID, Allitems);
        }

        public IEnumerable<SelectListItem> GetLoadedAtDropdown(string SPTYPE = "ALL", string SearchString = "", bool Allitems = false)
        {
            return comboDAL.GetLoadedAtDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, SearchString, Allitems);
        }

        public IEnumerable<SelectListItem> GetVehicleTypeDropdown(string SPTYPE = "ALL", string SearchString = "", int VehicleTypeID = -1, bool Allitems = false)
        {
            return comboDAL.GetVehicleTypeDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), VehicleTypeID, SPTYPE, SearchString, Allitems);
        }

        public IEnumerable<SelectListItem> GetRoleDropdown(string SPTYPE = "ALL", string SearchString = "", int RoleID = -1, int UserID = -1, bool Allitems = false)
        {
            return comboDAL.GetRoleDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), RoleID, UserID, SPTYPE, SearchString, Allitems);
        }

        public IEnumerable<SelectListItem> GetLoadChartNoDropdown(string SPTYPE = "ALL", string SearchString = "", int LoadChartID = -1, bool Allitems = false)
        {
            return comboDAL.GetLoadChartNoDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), LoadChartID, SPTYPE, SearchString, Allitems);
        }

        public IEnumerable<SelectListItem> GetAgentGroupDropdown(string SPTYPE = "ALL", string SearchString = "", int AgentGroupID = -1, string CorA = "A", bool Allitems = false)
        {
            return comboDAL.GetAgentGroupDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), AgentGroupID, CorA, SPTYPE, SearchString, Allitems);
        }

        public IEnumerable<SelectListItem> GetCompBranchList(string SPTYPE = "ALL", string CompIDXml = "")
        {
            return comboDAL.GetCompBranchList(Convert.ToString(UserSession.GetUserSession().LoginID), CompIDXml, SPTYPE);
        }
        public IEnumerable<SelectListItem> GetNationalityDropDown(string SPTYPE = "ALL")
        {
            return comboDAL.GetNationalityDropDown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetClaimDocTypeDropdown(string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetClaimDocTypeDropdown(UserSession.GetUserSession().LoginID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetShipperNameDropdown(string SPTYPE = "ALL", string SearchString = "", string ShipperName = "", bool Allitems = false)
        {
            return comboDAL.GetShipperNameDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), ShipperName, SPTYPE, SearchString, Allitems);
        }

        public IEnumerable<SelectListItem> GetCopyEnqShipmentDropdown(Int64 EnqDetailID)
        {
            return comboDAL.GetCopyEnqShipmentDropdown(UserSession.GetUserSession().LoginID, EnqDetailID);
        }

        public IEnumerable<SelectListItem> GetCourierDropDown(string SPTYPE = "ALL")
        {
            return comboDAL.GetCourierDropDown(SPTYPE, UserSession.GetUserSession().LoginID);
        }

        public string HtmlCurrencyList(int CurrID)
        {
            string htmlstring = string.Empty;
            foreach (var item in GetCurrencyDropdown())
            {
                string selected = CurrID == Convert.ToInt32(item.Value) ? " selected " : "";
                htmlstring = htmlstring + "<option value=\"" + item.Value + "\"" + selected + ">" + item.Text + "</option>";
            }
            return htmlstring;
        }

        public IEnumerable<SelectListItem> SequenceDropDown(int? Count)
        {
            List<SelectListItem> Sequence = new List<SelectListItem>();

            if (Count > 0)
            {
                List<int> list = Enumerable.Range(1, Convert.ToInt32(Count)).ToList();
                Sequence = list.AsEnumerable().Select(dataRow => new SelectListItem { Value = dataRow.ToString(), Text = dataRow.ToString() }).ToList();
                //.Select(dataRow => new SelectListItem { Value = list.Value }).toList() ;
            }

            return Sequence;

        }

        public IEnumerable<SelectListItem> LoosedCasedDropDown(int? ModeID)
        {
            List<SelectListItem> LoosedCasedList = RELOCBS.Common.CommonService.Loosecased.ToList();
            switch (ModeID)
            {
                case 1:
                    LoosedCasedList = LoosedCasedList.Where(x => x.Text != "Cased").ToList();
                    LoosedCasedList = LoosedCasedList.Where(x => x.Text != " ").ToList();
                    break;
                case 2:
                    LoosedCasedList = LoosedCasedList.Where(x => x.Text == "Cased").ToList();
                    break;
                case 3:
                    LoosedCasedList = LoosedCasedList.Where(x => x.Text == "Loose").ToList();
                    break;
                case 4:
                    LoosedCasedList = LoosedCasedList.Where(x => x.Text == " ").ToList();
                    break;
                default:
                    LoosedCasedList.Clear();
                    break;
            }
            return LoosedCasedList;
        }

        public IEnumerable<SelectListItem> LCLFCLDropDown(string LOOSEDCASED, int? Mode)
        {
            List<SelectListItem> LCLFCLList = RELOCBS.Common.CommonService.LCLFCL.ToList();
            if (Mode != 4)
            {
                if (UserSession.GetUserSession().CompanyID == 2 && Mode == 3)
                {
                    LCLFCLList = LCLFCLList.Where(x => x.Text != "Cased" && x.Text != "FCL" && x.Text != "LCL" && x.Text != " ").ToList();
                }
                else
                {
                    switch (LOOSEDCASED)
                    {
                        case "Loose":
                            if (Mode == 1)
                            {
                                LCLFCLList = LCLFCLList.Where(x => x.Text == "FCL" || x.Text == "GRPG").ToList();
                            }
                            else if (Mode == 3)
                            {
                                LCLFCLList = LCLFCLList.Where(x => x.Text != "Cased" && x.Text != "Part Load" && x.Text != "Direct" && x.Text != " ").ToList();
                            }
                            else
                            {
                                LCLFCLList.Clear();
                            }
                            break;
                        case "Cased":
                            LCLFCLList = LCLFCLList.Where(x => x.Text == "Cased").ToList();
                            break;
                        case "LiftVan":
                            LCLFCLList = LCLFCLList.Where(x => x.Text != "Cased").ToList();
                            LCLFCLList = LCLFCLList.Where(x => x.Text != "Part Load").ToList();
                            LCLFCLList = LCLFCLList.Where(x => x.Text != "Direct").ToList();
                            break;
                        default:
                            LCLFCLList.Clear();
                            break;
                    }
                }



            }
            else
            {
                LCLFCLList = LCLFCLList.Where(x => x.Text == " ").ToList();
            }
            return LCLFCLList;
        }

        public IEnumerable<SelectListItem> GetWarehouseJobNolDropdown(Int64? JobID = null, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            bool IsRMCBuss = false;

            if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
            {
                IsRMCBuss = false;
            }
            else
            {
                IsRMCBuss = true;
            }

            return comboDAL.GetWarehouseJobNolDropdown(JobID, IsRMCBuss, UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID, SPTYPE, Allitems);
        }

        public IEnumerable<SelectListItem> GetShippingLineAgentDropdown(int? ShippingLineAgentID = -1, string SPTYPE = "ALLACTIVE", string Search = "", bool Allitems = false)
        {
            return comboDAL.GetShippingLineAgentDropdown(UserSession.GetUserSession().LoginID, ShippingLineAgentID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetShippingCarrierDropdown(int ModeID, int? ShippingCarrierID = -1, string SPTYPE = "ALLACTIVE", string Search = "", bool Allitems = false)
        {
            return comboDAL.GetShippingCarrierDropdown(UserSession.GetUserSession().LoginID, ModeID, ShippingCarrierID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetStorageStateDropdown(int MoveID)
        {
            return comboDAL.GetStorageStateDropdown(MoveID);
        }

        public IEnumerable<SelectListItem> GetTransitInvoiceTypeDropdown(int InvoiceTypeID = -1, string SPTYPE = "ALLACTIVE", string Search = "", string type = "I", bool Allitems = false)
        {
            return comboDAL.GetTransitInvoiceTypeDropdown(UserSession.GetUserSession().LoginID, InvoiceTypeID, SPTYPE, Search, type, Allitems);
        }

        public IEnumerable<SelectListItem> GetRateTypeGrpDropdown(int RateTypeGrpID = -1, string SPTYPE = "ALLACTIVE", string Search = "", bool Allitems = false)
        {
            return comboDAL.GetRateTypeGrpDropdown(UserSession.GetUserSession().LoginID, RateTypeGrpID, SPTYPE, Search, Allitems);
        }
        public IEnumerable<SelectListItem> GetTitleDropdown(int TitleID = -1, string SPTYPE = "ALLACTIVE", string Search = "")
        {
            return comboDAL.GetTitleDropdown(UserSession.GetUserSession()?.LoginID ?? 0, TitleID, SPTYPE, Search);
        }

        public IEnumerable<SelectListItem> GetReportDropdown()
        {
            return comboDAL.GetReportDropdown(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID);
        }

        public IEnumerable<SelectListItem> GetTransFA_AppDropdown(int AppID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return comboDAL.GetTransFA_AppDropdown(UserSession.GetUserSession().LoginID, AppID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetCreditLimitEntityCategoryDropdown(int CompCategoryID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return comboDAL.GetCreditLimitEntityCategoryDropdown(UserSession.GetUserSession().LoginID, CompCategoryID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetCreditPeriodBasisDropdown(int PeriodBasisID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            var USER = UserSession.GetUserSession();
            bool IsRMCBUss = USER.BussinessLine != "NON RMC-BUSINESS";
            int CompID = USER.CompanyID;
            return comboDAL.GetCreditPeriodBasisDropdown(UserSession.GetUserSession().LoginID, IsRMCBUss, CompID, PeriodBasisID, SPTYPE, Search, Allitems);
        }
        public IEnumerable<SelectListItem> GetPaymentProcessTypeDropdown(int TypeID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return comboDAL.GetPaymentProcessTypeDropdown(UserSession.GetUserSession().LoginID, TypeID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetCreditWriterEntityDropdown(int EntityID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return comboDAL.GetCreditWriterEntityDropdown(UserSession.GetUserSession().LoginID, EntityID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetProjectDropdown(int ProjectID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return comboDAL.GetProjectDropdown(UserSession.GetUserSession().LoginID, ProjectID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetProjectModuleDropdown(int ProjectID = -1, int ModuleID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return comboDAL.GetProjectModuleDropdown(UserSession.GetUserSession().LoginID, ProjectID, ModuleID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetDevTeamDropdown(int MemberID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return comboDAL.GetDevTeamDropdown(UserSession.GetUserSession().LoginID, MemberID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetCRRequestTypeDropdown(int TypeID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return comboDAL.GetCRRequestTypeDropdown(UserSession.GetUserSession().LoginID, TypeID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetCRStatusDropdown(int StatusID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return comboDAL.GetCRStatusDropdown(UserSession.GetUserSession().LoginID, StatusID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetCRRequstorDropdown(int RequestorID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return comboDAL.GetCRRequstorDropdown(UserSession.GetUserSession().LoginID, RequestorID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetCRNumberDropdown(int RequestID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return comboDAL.GetCRNumberDropdown(UserSession.GetUserSession().LoginID, RequestID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetCRDepartmentDropdown(int DeptID = -1, string SPTYPE = "ALLACTIVE", string Search = null, bool Allitems = false)
        {
            return comboDAL.GetCRDepartmentDropdown(UserSession.GetUserSession().LoginID, DeptID, SPTYPE, Search, Allitems);
        }

        public IEnumerable<SelectListItem> GetJobStatusSDDropdown(string SPTYPE = "ALLACTIVE", int JobStatusSDId = -1, string Search = "")
        {
            return comboDAL.GetJobStatusSDDropdown(UserSession.GetUserSession().LoginID, SPTYPE, JobStatusSDId, Search);
        }

        public IEnumerable<SelectListItem> GetBillingStatusDropdown(string SPTYPE = "ALLACTIVE", int BillingStatusId = -1, string Search = "")
        {
            return comboDAL.GetBillingStatusDropdown(UserSession.GetUserSession().LoginID, SPTYPE, BillingStatusId, Search);
        }

        public IEnumerable<SelectListItem> GetApprovalUserList(string SPTYPE = "ALLACTIVE", bool IsRMCBuss = false, string MoveId = null)
        {
            return comboDAL.GetApprovalUserList(UserSession.GetUserSession().LoginID, SPTYPE, IsRMCBuss, UserSession.GetUserSession().CompanyID, MoveId);
        }

        public IEnumerable<SelectListItem> GetGPApprovalUserList(string SPTYPE = "ALLACTIVE", bool IsRMCBuss = false, string MoveId = null, bool IsSendForApproval = false, int LoginID = 0, int CompanyID = 0)
        {
            return comboDAL.GetGPApprovalUserList(LoginID, SPTYPE, IsRMCBuss, CompanyID, MoveId, IsSendForApproval);
        }

        public IEnumerable<SelectListItem> GetBankDetailDropdown()
        {
            return comboDAL.GetBankDetailDropdown(UserSession.GetUserSession().LoginID, UserSession.GetUserSession().CompanyID);
        }

        public IEnumerable<SelectListItem> GetQuoteApprovalUserList(string SPTYPE = "ALLACTIVE", string SurveyID = null)
        {
            bool IsRMCBuss = false;

            if (UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS")
            {
                IsRMCBuss = false;
            }
            else
            {
                IsRMCBuss = true;
            }
            return comboDAL.GetQuoteApprovalUserList(UserSession.GetUserSession().LoginID, SPTYPE, IsRMCBuss, UserSession.GetUserSession().CompanyID, SurveyID);
        }

        public IEnumerable<SelectListItem> GetPaymentTermList(string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetPaymentTermList(UserSession.GetUserSession().LoginID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetBTRServiceList(string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetBTRServiceList(UserSession.GetUserSession().LoginID, SPTYPE);
        }

        public IEnumerable<SelectListItem> GetBillingEntityList(int RMCID, char BillType, char BussLine ='R')
        {
            Int64 CompID = UserSession.GetUserSession().CompanyID;
            Int64 LoginID = UserSession.GetUserSession().LoginID;
            return comboDAL.GetBillingEntityList(LoginID, CompID, RMCID, BillType,BussLine);
        }

        public IEnumerable<SelectListItem> GetDebtorDropdown(string SPTYPE = "ALLACTIVE", int? DebtorId = null, string SearchString = null)
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            return comboDAL.GetDebtorDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, IsRMCBUss, DebtorId, SearchString);
        }

        public IEnumerable<SelectListItem> GetDebitNoteTypeDropdown(string SPTYPE = "ALLACTIVE", int? DNTypeId = null, string SearchString = null)
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            return comboDAL.GetDebitNoteTypeDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, IsRMCBUss, DNTypeId, SearchString);
        }

        public IEnumerable<SelectListItem> GetDebitNoteCostHeadDropdown(string SPTYPE = "ALLACTIVE", int? DNCostHeadID = null, int? DNTypeId = null, string SearchString = null)
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            return comboDAL.GetDebitNoteCostHeadDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, IsRMCBUss, DNCostHeadID, DNTypeId, SearchString);
        }

        public IEnumerable<SelectListItem> GetSBUDropdown(string SPTYPE = "ALLACTIVE", int? SBUId = null, string SearchString = null)
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            return comboDAL.GetSBUDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, IsRMCBUss, SBUId, SearchString);
        }

        public IEnumerable<SelectListItem> GetDebitNoteUnitDropdown(string SPTYPE = "ALLACTIVE", int? DNUnitId = null, string SearchString = null)
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            return comboDAL.GetDebitNoteUnitDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, IsRMCBUss, DNUnitId, SearchString);
        }

        public IEnumerable<SelectListItem> GetCreditApprovalCorporateDropdown(string SPTYPE = "ALL", string CORA = "C", int? AgentId = null, int? CreditLimitEntityID = -1, string searchstring = null, bool Allitems = false)
        {
            var USER = UserSession.GetUserSession();
            bool IsRMCBUss = USER.BussinessLine != "NON RMC-BUSINESS";
            int CompID = USER.CompanyID;
            if (CORA == "CL")
            {
                CORA = IsRMCBUss ? "R" : null;
            }
            return comboDAL.GetCreditApprovalCorporateDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, CORA, IsRMCBUss, CompID, AgentId, CreditLimitEntityID, searchstring, Allitems);
        }

        public IEnumerable<SelectListItem> GetCreditApprovalProjectDropdown(string SPTYPE = "ALL", int ProejctID = 0, string searchstring = null, bool Allitems = false)
        {
            var USER = UserSession.GetUserSession();
            bool IsRMCBUss = USER.BussinessLine != "NON RMC-BUSINESS";
            int CompID = USER.CompanyID;
            return comboDAL.GetCreditApprovalProjectDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, IsRMCBUss, CompID, ProejctID, searchstring, Allitems);
        }

        public IEnumerable<SelectListItem> GetCreditApprovalServiceLineDropdown(string SPTYPE = "ALL", int ProjectID = 0, int ServiceLineID = 0, string searchstring = null, bool Allitems = false)
        {
            var USER = UserSession.GetUserSession();
            bool IsRMCBUss = USER.BussinessLine != "NON RMC-BUSINESS";
            int CompID = USER.CompanyID;

            return comboDAL.GetCreditApprovalServiceLineDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, IsRMCBUss, CompID, ProjectID, ServiceLineID, searchstring, Allitems);
        }

        public IEnumerable<SelectListItem> GetCreditApprovalSendToDropdown(string SPTYPE = "ALL", int SendToApproverID = 0, string searchstring = null, float Amount = 0, bool Allitems = false)
        {
            var USER = UserSession.GetUserSession();
            bool IsRMCBUss = USER.BussinessLine != "NON RMC-BUSINESS";
            int CompID = USER.CompanyID;

            return comboDAL.GetCreditApprovalSendToDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, IsRMCBUss, CompID, SendToApproverID, Amount, searchstring, Allitems);
        }

        public IEnumerable<SelectListItem> GetVehicleMovementMasterDropdown(string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return comboDAL.GetVehicleMovementMasterDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, id, searchstring, Allitems);
        }

        public IEnumerable<SelectListItem> GetVehicleDimensionDropdown(string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return comboDAL.GetVehicleDimensionDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, id, searchstring, Allitems);
        }

        public IEnumerable<SelectListItem> GetVehicleReasonDropdown(string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return comboDAL.GetVehicleReasonDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, id, searchstring, Allitems);
        }

        public IEnumerable<SelectListItem> GetWHAssessmentStatusDropdown(string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return comboDAL.GetWHAssessmentStatusDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, id, searchstring, Allitems);
        }

        public IEnumerable<SelectListItem> GetWHAssessmentPriorityDropdown(string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return comboDAL.GetWHAssessmentPriorityDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, id, searchstring, Allitems);
        }

        public IEnumerable<SelectListItem> GetWHAssessmentResponsibilityDropdown(string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return comboDAL.GetWHAssessmentResponsibilityDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, id, searchstring, Allitems);
        }

        public IEnumerable<SelectListItem> GetWHAssessmentCategoryDropdown(string SPTYPE = "ALL", int id = 0, string searchstring = null, bool Allitems = false)
        {
            return comboDAL.GetWHAssessmentCategoryDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, id, searchstring, Allitems);
        }

        public IEnumerable<SelectListItem> GetWHDocTypeDropdown(string SPTYPE = "ALLACTIVE", int? DocTypeID = -1, string DocFromType = "", bool Allitems = false)
        {
            return comboDAL.GetWHDocTypeDropdown(UserSession.GetUserSession().LoginID, SPTYPE, DocTypeID, DocFromType, Allitems);
        }

        public IEnumerable<SelectListItem> GetWHDocNameDropdown(string SPTYPE = "ALLACTIVE", int? DocNameID = -1, int DocTypeID = -1, bool Allitems = false)
        {
            return comboDAL.GetWHDocNameDropdown(UserSession.GetUserSession().LoginID, SPTYPE, DocNameID, DocTypeID, Allitems);
        }

        public IEnumerable<SelectListItem> GetGenderDropdown(string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetGenderDropdown(Convert.ToString(UserSession.GetUserSession()?.LoginID ?? 0), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetMaritalStatusDropdown(string SPTYPE = "ALLACTIVE")
        {
            return comboDAL.GetMaritalStatusDropdown(Convert.ToString(UserSession.GetUserSession()?.LoginID ?? 0), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetATRStatusDropdown(string SPTYPE = "ALLACTIVE", int? StatusID = -1, bool IsCompliance = false, bool IsAuitee = false, Int64? ATRPointId = -1, bool Allitems = false)
        {
            return comboDAL.GetATRStatusDropdown(UserSession.GetUserSession().LoginID, SPTYPE, StatusID, IsCompliance, IsAuitee, ATRPointId, Allitems);
        }

        public IEnumerable<SelectListItem> GetATRRiskDropdown(string SPTYPE = "ALLACTIVE", int? RiskID = -1, bool Allitems = false)
        {
            return comboDAL.GetATRRiskDropdown(UserSession.GetUserSession().LoginID, SPTYPE, RiskID, Allitems);
        }

        public IEnumerable<SelectListItem> GetATRDepartmentDropdown(string SPTYPE = "ALLACTIVE", int? DeptID = -1, bool Allitems = false)
        {
            return comboDAL.GetATRDepartmentDropdown(UserSession.GetUserSession().LoginID, SPTYPE, DeptID, Allitems);
        }

        public IEnumerable<SelectListItem> GetATREmployeeDropdown(string SPTYPE = "ALLACTIVE", int? EmpID = -1, bool Allitems = false)
        {
            return comboDAL.GetATREmployeeDropdown(UserSession.GetUserSession().LoginID, SPTYPE, EmpID, Allitems);
        }

        public IEnumerable<SelectListItem> GetATRCategoryDropdown(string SPTYPE = "ALLACTIVE", int? CategoryID = -1, bool Allitems = false)
        {
            return comboDAL.GetATRCategoryDropdown(UserSession.GetUserSession().LoginID, SPTYPE, CategoryID, Allitems);
        }

        public IEnumerable<SelectListItem> GetComplaintsClassificationDropdown(string SPTYPE = "ALLACTIVE", int? ClassificationID = -1, bool Allitems = false)
        {
            return comboDAL.GetComplaintsClassificationDropdown(UserSession.GetUserSession().LoginID, SPTYPE, ClassificationID, Allitems);
        }

        public IEnumerable<SelectListItem> GetComplaintsSourceDropdown(string SPTYPE = "ALLACTIVE", int? SourceID = -1, bool Allitems = false)
        {
            return comboDAL.GetComplaintsSourceDropdown(UserSession.GetUserSession().LoginID, SPTYPE, SourceID, Allitems);
        }

        public IEnumerable<SelectListItem> GetComplaintsStatusDropdown(string SPTYPE = "ALLACTIVE", int? StatusID = -1, bool Allitems = false)
        {
            return comboDAL.GetComplaintsStatusDropdown(UserSession.GetUserSession().LoginID, SPTYPE, StatusID, Allitems);
        }

        public IEnumerable<SelectListItem> GetComplaintsEnquiryNoDropdown(string SPTYPE = "ALLACTIVE", string Search = "", int? EnqID = -1, bool Allitems = false)
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            return comboDAL.GetComplaintsEnquiryNoDropdown(UserSession.GetUserSession().LoginID, IsRMCBUss, UserSession.GetUserSession().CompanyID, SPTYPE, Search, EnqID, Allitems);
        }

        public IEnumerable<SelectListItem> GetComplaintsJobNoDropdown(string SPTYPE = "ALLACTIVE", string Search = "", int? MoveID = -1, bool Allitems = false)
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            return comboDAL.GetComplaintsJobNoDropdown(UserSession.GetUserSession().LoginID, IsRMCBUss, UserSession.GetUserSession().CompanyID, SPTYPE, Search, MoveID, Allitems);
        }

        public IEnumerable<SelectListItem> GetComplaintsShipmentDropdown(string SPTYPE = "ALLACTIVE", string Search = "", int? EnqID = -1, int? EnqDetailID = -1, bool Allitems = false)
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            return comboDAL.GetComplaintsShipmentDropdown(UserSession.GetUserSession().LoginID, IsRMCBUss, UserSession.GetUserSession().CompanyID, SPTYPE, Search, EnqID, EnqDetailID, Allitems);
        }

        public IEnumerable<SelectListItem> GetInvDmsAgentDropdown(string CorA = "A", string Search = "", int? AgentID = -1, string SPTYPE = "ALLACTIVE", bool Allitems = false)
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            return comboDAL.GetInvDmsAgentDropdown(UserSession.GetUserSession().LoginID, IsRMCBUss, UserSession.GetUserSession().CompanyID, CorA, Search, SPTYPE, AgentID, Allitems);
        }

        public IEnumerable<SelectListItem> GetWH_JobTypeDropdown(string Search = "", string SPTYPE = "ALLACTIVE", int? JobTypeId = -1, bool Allitems = false)
        {
            bool IsRMCBUss = UserSession.GetUserSession().BussinessLine != "NON RMC-BUSINESS";
            return comboDAL.GetWH_JobTypeDropdown(UserSession.GetUserSession().LoginID, IsRMCBUss, UserSession.GetUserSession().CompanyID, Search, SPTYPE, JobTypeId, Allitems);
        }

        public IEnumerable<SelectListItem> GetVendorContractServiceCategoryDropdown(string Search = "", string SPTYPE = "ALLACTIVE", int? CategoryID = -1, bool Allitems = false)
        {
            return comboDAL.GetVendorContractServiceCategoryDropdown(UserSession.GetUserSession().LoginID, Search, SPTYPE, CategoryID, Allitems);
        }


        public IEnumerable<SelectListItem> GetVendorContractBuinessUnitDropdown(string Search = "", string SPTYPE = "ALLACTIVE", int? BUID = -1, bool Allitems = false)
        {
            return comboDAL.GetVendorContractBuinessUnitDropdown(UserSession.GetUserSession().LoginID, Search, SPTYPE, BUID, Allitems);
        }

        public IEnumerable<SelectListItem> GetVendorContractDocTypeDropdown(string SPTYPE = "ALLACTIVE", int? DocTypeID = -1, string DocFromType = "", bool Allitems = false)
        {
            return comboDAL.GetVendorContractDocTypelDropdown(UserSession.GetUserSession().LoginID, SPTYPE, DocTypeID, DocFromType, Allitems);
        }

        public IEnumerable<SelectListItem> GetVendorContractDocumentDropdown(string SPTYPE = "ALLACTIVE", int? DocNameID = -1, int DocTypeID = -1, bool Allitems = false)
        {
            return comboDAL.GetVendorContractDocumentDropdown(UserSession.GetUserSession().LoginID, SPTYPE, DocNameID, DocTypeID, Allitems);
        }

        public IEnumerable<SelectListItem> GetInAssetDetails(string SPTYPE = "ALLACTIVE", Int64? MoveID = null, Int64? InMastID = null, Int64? AssetDetID = null, string SearchString = null)
        {
            return comboDAL.GetInAssetDetails(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, MoveID, InMastID, AssetDetID, SearchString);
        }

        public IEnumerable<SelectListItem> GetServiceEmpType(string SPTYPE = "ALLACTIVE", int? EmpTypeID = null, string SearchString = null)
        {
            return comboDAL.GetServiceEmpType(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, EmpTypeID, SearchString);
        }

        public IEnumerable<SelectListItem> GetLiftVanList(string SPTYPE = "ALLACTIVE", Int64? LiftVanID = null, Int64? MoveID = null, string SearchString = null)
        {
            return comboDAL.GetLiftVanList(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, LiftVanID, MoveID, SearchString);
        }

        public IEnumerable<SelectListItem> GetWOSCSApprovalUserList(string SPTYPE = "ALLACTIVE", Int64? WOSMoveID = null)
        {
            return comboDAL.GetWOSCSApprovalUserList(UserSession.GetUserSession().LoginID, SPTYPE, UserSession.GetUserSession().CompanyID, WOSMoveID);
        }

        public IEnumerable<SelectListItem> GetJobNoForAssetManagement(string SPTYPE = "ALLACTIVE", Int64? MoveID = null, int? CompID = null, string SearchString = null)
        {
            return comboDAL.GetJobNoForAssetManagement(UserSession.GetUserSession().LoginID, SPTYPE, MoveID, CompID, SearchString);
        }
        public IEnumerable<SelectListItem> GetBillCategory(string SPTYPE = "ALLACTIVE", int? BillCategoryID = null, string SearchString = null)
        {
            return comboDAL.GetBillCategory(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, BillCategoryID, SearchString);
        }
        public IEnumerable<SelectListItem> GetJobGridSearchList(string SPTYPE = "ALLACTIVE", int? JobSearchID = null, string SearchString = null)
        {
            return comboDAL.GetJobGridSearchList(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, JobSearchID, SearchString);
        }

        public IEnumerable<SelectListItem> GetVCStatusList(string SPTYPE = "ALLACTIVE", int? StatusID = null, string SearchString = null)
        {
            return comboDAL.GetVCStatusList(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, StatusID, SearchString);
        }

        public IEnumerable<SelectListItem> GetDashboardReasonList(string SPTYPE = "ALLACTIVE", int? ReasonID = null, string SearchString = null, int JobStatusID = 0)
        {
            return comboDAL.GetDashboardReasonList(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, ReasonID, SearchString,JobStatusID);
        }
        
    }
}