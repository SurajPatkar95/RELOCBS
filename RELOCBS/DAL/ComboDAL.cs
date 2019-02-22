using RELOCBS.App_Code;
using RELOCBS.DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_City] @SP_Type={0},@SP_ContinentID={1},@SP_CountryID={2},@SP_Loginid={3}", CSubs.QSafeValue(SPTYPE),
                CSubs.QSafeValue(Convert.ToString(ContinentID)), CSubs.QSafeValue(Convert.ToString(CountryID)),
                CSubs.QSafeValue(LoginID)));

        }


        public IEnumerable<SelectListItem> PortsByCityShipmentModeCombo(string LoginID, int CityID, int shipmentModeID)
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_City] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(Convert.ToString(CityID)), CSubs.QSafeValue(LoginID)));

        }

        public IEnumerable<SelectListItem> GetRMCDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_RMC] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetAgentDropdown(string LoginID, string SPTYPE = "ALL")
        {

            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Agent] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));

        }

        public IEnumerable<SelectListItem> OriginCityDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_City] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> DestinationCityDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_City] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetServiceLineDropdown(string LoginID, string SPTYPE = "ALL")
        {

            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_ServiceLine] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetBusinessLineDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_BussinessLine] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetGoodsDescriptionDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_GoodsDescription] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetModeDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_TransportMode] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetRateComponentDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_RateComponent] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetRateAgentDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Agent] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
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
        public IEnumerable<SelectListItem> GetWeightUnitDropdown(string LoginID, string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_WeightUnit] @SP_Type={0},@SP_Loginid={1}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID)));
        }

        public IEnumerable<SelectListItem> GetCostHeadDropdown(string LoginID, int RMC_ID, string SPTYPE = "ALLACTIVE")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_CostHead] @SP_Type={0},@SP_Loginid={1},@SP_RMCID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(RMC_ID))));
        }

        public IEnumerable<SelectListItem> GetPortDropdown(string LoginID, int? CityID,string SPTYPE = "ALL")
        {
            return CSubs.BindDropdown(string.Format("[Comm].[ForCombo_Port] @SP_Type={0},@SP_Loginid={1},@SP_CityID={2}", CSubs.QSafeValue(SPTYPE), CSubs.QSafeValue(LoginID), CSubs.QSafeValue(Convert.ToString(CityID)) ));
        }

    }
}