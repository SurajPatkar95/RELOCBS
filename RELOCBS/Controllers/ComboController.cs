using RELOCBS.BL;
using RELOCBS.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class ComboController : Controller
    {
        private ComboBL _comboBL;

        public ComboBL comboBL
        {

            get
            {
                if (this._comboBL == null)
                    this._comboBL = new ComboBL();
                return this._comboBL;
            }
        }


        // GET: Combo
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetAllCity()
        {
            var lst = comboBL.GetCityDropdown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PortsByCityShipmentMode(int CityID,int shipmentModeID)
        {

            var lst = comboBL.PortsByCityShipmentModeCombo(CityID, shipmentModeID).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRMCList()
        {
            return Json(comboBL.GetRMCDropdown().Select(i => new { i.Value, i.Text }).ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAgentList()
        {
            return Json(comboBL.GetAgentDropdown().Select(i => new { i.Value, i.Text }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOriginList()
        {
            
            var lstItem = comboBL.OriginCityDropdown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDestinationList()
        {
            
            var lstItem = comboBL.DestinationCityDropdown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }
        
        //public JsonResult GetPricingUserList()
        //{
            //var lstItem = _spService.BindDropdown("PricingUser", "", "").Select(i => new { i.Value, i.Text }).ToList();
            //return Json(lstItem, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetServiceLineList()
        {
            
            var lstItem = comboBL.GetServiceLineDropdown().Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCity(int? ContienentID,int? CountryID)
        {
            var lstItem = comboBL.GetCityDropdown(ContinentID:ContienentID,CountryID:CountryID).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCostHead(int RMCID)
        {
            var lstItem = comboBL.GetCostHeadDropdown(RMC_ID: RMCID).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetPortList(int CityID)
        {

            var lstItem = comboBL.GetPortDropdown(CityID).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

    }
}