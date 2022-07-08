using RELOCBS.BL;
using RELOCBS.CustomAttributes;
using RELOCBS.Utility;
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
			bool RMCBuss = !(UserSession.GetUserSession().BussinessLine == "NON RMC-BUSINESS");

			var lstItem = comboBL.GetServiceLineDropdown(RMCBuss:RMCBuss).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCity(int? ContienentID,int? CountryID)
        {
            var lstItem = comboBL.GetCityDropdown(ContinentID:ContienentID,CountryID:CountryID).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCostHead(int RMCID,int? MoveCompID = null, int? ModeID = null)
        {
			var ForCombo = ModeID == 4 ? "For Strg" : null;
			ForCombo = ModeID == 5 ? "For DSP" : null;
			var lstItem = comboBL.GetCostHeadDropdown(RMC_ID: RMCID, MoveCompID: MoveCompID,ForCombo:ForCombo).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPortList(int CityID)
        {

            var lstItem = comboBL.GetPortDropdown(CityID).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult GetShippingLineList(int mode)
        {
            var lstItem = comboBL.GetShippingLineDropdown(Convert.ToString(mode)).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWarehouseList(int BranchID)
        {
            var lstItem = comboBL.GetWarehouseDropdown(BranchID: BranchID).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJobNoList(string shipper,int WarehouseID=-1, string Type="")
        {
            Type = "ALLACTIVEWAREHOUSEJOB";
            if (Type =="I")
            {
                Type = "ALLACTIVESHIPPERWISE";
            }
            var lstItem = comboBL.getJobNolDropdown(Shipper: shipper, SPTYPE: Type, Allitems:false).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInsurancePolicyList(int InsuranceCompID)
        {
            var lstItem = comboBL.getInsurancePolicyNoDropdown(InsuranceCompID : InsuranceCompID,SPTYPE : "ALLACTIVE", Allitems: false).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemDetailList(int ItemCategoryID)
        {
            var lstItem = comboBL.getClaimItemDetailIsDropdown(ClaimCategoryID : ItemCategoryID, SPTYPE: "ALLACTIVE", Allitems: false).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInsuranceNoList(Int64 JobNo,int InsuranceCompID)
        {
            var lstItem = comboBL.getInsuranceNoDropdown(MoveID: JobNo, CompID: InsuranceCompID, SPTYPE: "ALLACTIVE", Allitems: false).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStateList(string term, string Value)
        {
            IEnumerable<SelectListItem> CountryList = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Value))
            {
                CountryList = comboBL.GetStateDropdown(SPTYPE: "ALLACTIVE", SearchString: term);
            }
            else
            {
                CountryList = comboBL.GetStateDropdown(SPTYPE: "SINGLE", StateID: Convert.ToInt32(Value));
            }

            return Json(new { CountryList = CountryList }, JsonRequestBehavior.AllowGet);
        }

       
        public JsonResult GetServiceLineJobNoList(int ServiceLineID)
        {
            var lstItem = comboBL.getJobNolDropdown(ServiceLineID: ServiceLineID, SPTYPE: "ALLACTIVESERVIELINEWISE", Allitems: false).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetShipperNameList(string term, string Value)
        {
            var lstItem = comboBL.GetShipperNameDropdown(Allitems: false).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDocName(int DocType)
        {
            var lstItem = comboBL.GetJobDocNamelDropdown(DocTypeID: DocType, Allitems: false).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJobNo()
        {
            var lstItem = comboBL.getJobNolDropdown(Allitems: false).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClaimItemDetailsList(string term, string Value)
        {
            IEnumerable<SelectListItem> CountryList = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Value))
            {
                CountryList = comboBL.getClaimItemDetailIsDropdown(SPTYPE: "ALLACTIVE", SearchString: term);
            }
            else
            {
                CountryList = comboBL.getClaimItemDetailIsDropdown(SPTYPE: "SINGLE", ClaimItemDetailIsID: Convert.ToInt32(Value));
            }

            return Json(new { CountryList = CountryList }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyBranch(int CompID)
        {

            var lstItem = comboBL.GetCompanyBranchDropdown(CompanyID: CompID).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWarehouseJobNoList(string Type)
        {
            var lstItem = comboBL.GetWarehouseJobNolDropdown(SPTYPE: "ALLACTIVE", Allitems: false).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lstItem, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetJobNoSearchList(string term, string Value,bool IsStorageJob = false) 
        {
            IEnumerable<SelectListItem> List = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Value))
            {
                List = comboBL.getJobNolDropdown(SPTYPE: "ALLACTIVE",IsStorage : IsStorageJob, SearchStr: term);
            }
            else
            {
                List = comboBL.getJobNolDropdown(SPTYPE: "SINGLE", MoveId: Convert.ToInt32(Value));
            }

            return Json(new { CountryList = List }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLoadChartJobSearchList(string term, string Value, bool IsStorageJob = false)
        {
            IEnumerable<SelectListItem> List = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Value))
            {
                List = comboBL.getJobNolDropdown(SPTYPE: "ALLACTIVEWAREHOUSEJOB", IsStorage: IsStorageJob, SearchStr: term);
            }
            else
            {
                List = comboBL.getJobNolDropdown(SPTYPE: "SINGLE", MoveId: Convert.ToInt32(Value));
            }

            return Json(new { CountryList = List }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTransitJobNoList(string term, string Value,int Mode=-1,Int64 MasterID=-1,bool IsStorageJob = false)
        {
            IEnumerable<SelectListItem> List = new List<SelectListItem>();
            string SPTYPE = "ALLACTIVETRAINSITINVJOB";
            if (string.IsNullOrEmpty(Value))
            {
                List = comboBL.getJobNolDropdown(SPTYPE: SPTYPE, IsStorage: IsStorageJob, SearchStr: term,Modeid: Mode, MasterID : MasterID);
            }
            else
            {
                SPTYPE = "SINGLE";
                List = comboBL.getJobNolDropdown(SPTYPE: SPTYPE, MoveId: Convert.ToInt32(Value));
            }

            return Json(new { CountryList = List }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetTransitInvTypeList(string type)
        {
            IEnumerable<SelectListItem> List = new List<SelectListItem>();
            List = comboBL.GetTransitInvoiceTypeDropdown(SPTYPE: "ALLACTIVE", type: type);
            return Json(new { CountryList = List }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShippingLineAgentSearchList(string term, string Value)
        {
            IEnumerable<SelectListItem> List = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Value))
            {
                List = comboBL.GetShippingLineAgentDropdown(SPTYPE: "ALLACTIVE", Search: term);
            }
            else
            {
                List = comboBL.GetShippingLineAgentDropdown(SPTYPE: "SINGLE", ShippingLineAgentID: Convert.ToInt32(Value));
            }

            return Json(new { CountryList = List }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShippingCarrierSearchList(string term, string Value,int ModeId)
        {
            IEnumerable<SelectListItem> List = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Value))
            {
                List = comboBL.GetShippingCarrierDropdown(ModeID: ModeId, SPTYPE: "ALLACTIVE", Search: term);
            }
            else
            {
                List = comboBL.GetShippingCarrierDropdown(ModeID: ModeId, SPTYPE: "SINGLE", ShippingCarrierID: Convert.ToInt32(Value));
            }

            return Json(new { CountryList = List }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPortSearchList(string term, string Value,string Mode)
        {
            IEnumerable<SelectListItem> List = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Value))
            {
                List = comboBL.GetPortDropdown(SPTYPE: "ALLACTIVE", Search: term,SeaOrAir : Mode);
            }
            else
            {
                List = comboBL.GetPortDropdown(SPTYPE: "SINGLE", PortID: Convert.ToInt32(Value));
            }

            return Json(new { CountryList = List }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCRProjectModuleList(string term, string Value, int ProjectId)
        {
            IEnumerable<SelectListItem> List = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Value))
            {
                List = comboBL.GetProjectModuleDropdown(ProjectID: ProjectId, SPTYPE: "ALLACTIVE", Search: term);
            }
            else
            {
                List = comboBL.GetProjectModuleDropdown(ProjectID: ProjectId, SPTYPE: "SINGLE", ModuleID: Convert.ToInt32(Value));
            }

            return Json(new { CountryList = List }, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult GetInvDmsAgentList(string term,string value)
        {
            IEnumerable<SelectListItem> List = new List<SelectListItem>();
            if (string.IsNullOrEmpty(value))
            {
                List = comboBL.GetInvDmsAgentDropdown(CorA: "A", SPTYPE: "ALLACTIVE", Search: term);
            }
            else
            {
                List = comboBL.GetInvDmsAgentDropdown(CorA:"A",SPTYPE: "SINGLE", AgentID: Convert.ToInt32(value));
            }
            return Json(new { CountryList = List }, JsonRequestBehavior.AllowGet);
        }

    }
}