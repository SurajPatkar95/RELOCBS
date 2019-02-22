using RELOCBS.DAL;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            return comboDAL.GetCurrencyDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetCityDropdown(string SPTYPE = "ALL", int? ContinentID = -1, int? CountryID = -1, int? CityID = -1)
        {
            return comboDAL.GetCityDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, CountryID, ContinentID, CityID);
        }

        public IEnumerable<SelectListItem> GETCountryDropdown(string SPTYPE = "ALL", int? ContinentID = -1, int? CountryID = -1)
        {
            return comboDAL.GETCountryDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE, CountryID, ContinentID);
        }

        public IEnumerable<SelectListItem> PortsByCityShipmentModeCombo(int CityID, int shipmentModeID)
        {
            if (CityID == 0)
            { CityID = -1; }

            return comboDAL.PortsByCityShipmentModeCombo(Convert.ToString(UserSession.GetUserSession().LoginID), CityID, shipmentModeID);
        }

        public IEnumerable<SelectListItem> GetRMCDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetRMCDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetAgentDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetAgentDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> OriginCityDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.OriginCityDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> DestinationCityDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.DestinationCityDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetServiceLineDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetServiceLineDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetBusinessLineDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetBusinessLineDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetGoodsDescriptionDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetGoodsDescriptionDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetModeDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetModeDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetRateComponentDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetRateComponentDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetRateAgentDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetRateAgentDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
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
        public IEnumerable<SelectListItem> GetWeightUnitDropdown(string SPTYPE = "ALL")
        {
            return comboDAL.GetWeightUnitDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), SPTYPE);
        }

        public IEnumerable<SelectListItem> GetCostHeadDropdown(int RMC_ID)
        {
            return comboDAL.GetCostHeadDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), RMC_ID);
        }

        public IEnumerable<SelectListItem> GetPortDropdown(int? CityID)
        {
            return comboDAL.GetPortDropdown(Convert.ToString(UserSession.GetUserSession().LoginID), CityID);

        }
    }
}