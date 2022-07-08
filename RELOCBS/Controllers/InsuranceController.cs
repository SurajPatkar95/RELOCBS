using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Insurance;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{

    [AuthorizeUser]
    public class InsuranceController : BaseController
    {

        private string _PageID = "24";
        private CommonSubs _cSubs;
        public CommonSubs CSubs
        {
            get
            {
                if (this._cSubs == null)
                    this._cSubs = new CommonSubs();
                return this._cSubs;
            }
        }
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

        private InsuranceBL _insuranceBL;
        public InsuranceBL insuranceBL
        {
            get
            {
                if (this._insuranceBL == null)
                    this._insuranceBL = new InsuranceBL();
                return this._insuranceBL;
            }
        }

        // GET: Insurance
        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Insurance");
            string sort = "JobNo";
            string sortdir = "desc";
            string search = "";
            Int64 JobNo=-1;
            Int64 Insurance_ID=-1;
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            DateTime? Fromdate = null;//System.DateTime.Now.Date.AddDays(-2);
            DateTime? Todate = null;//System.DateTime.Now;
            string Shipper = "";
            bool IsInsuranceDate = false;
            bool IsJobDate = false;


            string SearchKey = string.Empty;
            if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
            {
                Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
            }
            if (Request.Form["JobNo"] != null && Request.Form["JobNo"].Trim() != "")
            {
                JobNo = Convert.ToInt64(Request.Form["JobNo"]);
            }

            if (Request.Form["Insurance_ID"] != null && Request.Form["Insurance_ID"].Trim() != "")
            {
                Insurance_ID = Convert.ToInt64(Request.Form["Insurance_ID"]);
            }

            if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
            {
                Todate = Convert.ToDateTime(Request.Form["ToDate"]);
            }

            if (Request.Params["grid-column"] != null && Request.Params["grid-column"].Trim() != "")
            {
                sort = Request.Params["grid-column"].Trim().ToString();
            }
            if (Request.Params["grid-dir"] != null && Request.Params["grid-dir"].Trim() != "")
            {
                Order = Convert.ToInt32(Request.Params["grid-dir"].Trim().ToString());

                if (Order == 1)
                {
                    sortdir = "asc";
                }
                else
                {
                    sortdir = "desc";
                }
            }
            if (Request.Params["Shipper"] != null && Request.Params["Shipper"].Trim() != "")
            {
                Shipper = Request.Params["Shipper"].Trim().ToString();
            }
            if (Request.Params["IsJobDate"] != null && Request.Params["IsJobDate"].Trim() != "")
            {

                if (Request.Form.GetValues("IsJobDate") != null && Convert.ToString(Request.Form.GetValues("IsJobDate")[0]).Trim().ToUpper() == "TRUE")
                {
                    IsJobDate = true;
                }

            }
            if (Request.Params["IsInsuranceDate"] != null && Request.Params["IsInsuranceDate"].Trim() != "")
            {
                if (Request.Form.GetValues("IsInsuranceDate") != null && Convert.ToString(Request.Form.GetValues("IsInsuranceDate")[0]).Trim().ToUpper() == "TRUE")
                {
                    IsInsuranceDate = true;
                }

            }

            int totalRecord = 0;
            if (page < 1) page = 1;
            //int skip = (pageNo * pageSize) - pageSize;
            int skip = pageSize;
            pageSize = page;
            var data = insuranceBL.GetInsuranceGrid(Fromdate, Todate, IsJobDate, IsInsuranceDate, JobNo, Insurance_ID, sort, sortdir, skip, pageSize, out totalRecord);
            FillCombo(Fromdate,Todate, JobNo, Insurance_ID);


            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;

            var itemsAsIPagedList = new StaticPagedList<Entities.InsuranceGrid >(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }

        private void FillCombo(DateTime? from,DateTime? to,Int64? MoveID,Int64? InsuranceID,int InsCompID=-1)
        {

            ViewData["InsuranceId"] = InsuranceID;
            ViewData["MoveId"] = MoveID;

            ViewData["InsuranceList"]=comboBL.getInsuranceNoDropdown();
            ViewData["Currency"] = comboBL.GetCurrencyDropdown();
            ViewData["StatusList"] = comboBL.GetClaimStatusDropdown();
            string SPTYPE = "ALLACTIVE";
            if (MoveID > 0 && InsuranceID>0)
            {
                SPTYPE = "MOVEIDCOMP";
            }
            ViewData["InsuranceCompList"] = comboBL.GetInsuranceCompanyDropdown(SPTYPE: SPTYPE, MoveID: MoveID);
            ViewData["JobNoList"] = comboBL.getJobNolDropdown(SPTYPE: "ALLACTIVEINSURANCEJOB"); ///new List<SelectListItem>();
            ViewData["Policy_NoList"] = comboBL.getInsurancePolicyNoDropdown(InsuranceCompID : InsCompID); ///new List<SelectListItem>();
			ViewData["InsDelayReasonList"] = comboBL.getInsDelayReasonListDropdown(); ///new List<SelectListItem>();
		


		//ViewData["ControllerList"] = comboBL.GetBusinessLineDropdown();
	}

        // GET: Insurance/Details/5
        public ActionResult Details(int id)
        {
            FillCombo(System.DateTime.Now, System.DateTime.Now, -1, -1);

            return View();
        }

        // GET: Insurance/Create
        public ActionResult Create(string Key)
        {
            if (!string.IsNullOrEmpty(Key) || Key!="-1")
            {
                InsuranceViewModel model = new InsuranceViewModel();
                

                Dictionary<string, string> list = CommonService.GetQueryString(Key);
                
                Int64 JobNo = -1, InsuranceID = -1;


                if (list.ContainsKey("MoveID"))
                {
                    JobNo = Convert.ToInt64(list["MoveID"]);
                }
                if (list.ContainsKey("InsuranceID"))
                {
                    InsuranceID = Convert.ToInt32(list["InsuranceID"]);
                }


                model = insuranceBL.GetInsuranceDetails(JobNo, InsuranceID);

                if (model.BaseCurrencyID<=0)
                {
                    model.BaseCurrencyID = UserSession.GetUserSession().BaseCurrID;
                    model.ExRate = 1;
                    model.RateCurrencyID = UserSession.GetUserSession().BaseCurrID;
                }
                if (model.ControllerID<=0)
                {
                    model.ControllerID = 1;
                }
                
                FillCombo(System.DateTime.Now, System.DateTime.Now, model.MoveID, model.Insurance_ID,model.InsuranceCompanyID);

                return View(model);
            }

            return Redirect("Index");
        }

        // POST: Insurance/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InsuranceViewModel model, string Key)
        {
            try
            {
                FillCombo(System.DateTime.Now,System.DateTime.Now, model.MoveID, model.Insurance_ID, model.InsuranceCompanyID);
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                    bool res = false;

                    res = insuranceBL.Inset(model, out message);
                    if (!res)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save data.");
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                        //return Json(result);
                        return View("Create", model);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        return RedirectToAction("Create", new { Key = CommonService.GenerateQueryString("MoveID=ParamValue0&InsuranceID=ParamValue1", new string[] { Convert.ToString(model.MoveID), (string.IsNullOrWhiteSpace(Convert.ToString(model.Insurance_ID)) ? "-1" : Convert.ToString(model.Insurance_ID)) }) });
                        //return Json(result);
                    }
                }
                else
                {
                    return View("Create", model);
                }
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Insurance/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Insurance/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Insurance/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Insurance/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public JsonResult GetaJAXQueryString(int JobNo)
        {
            string querystring = string.Empty;
            try
            {
                querystring = CommonService.GenerateQueryString("MoveID=ParamValue0", new string[] { Convert.ToString(JobNo) });
            }
            catch (Exception)
            {
            }

            return Json(new { querystring = querystring }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetInsuranceAmounts(int InsCompID,Int64 policyNo, decimal Sum_Ins_Amt)
        {
            InsuranceAmoutDTO dto = insuranceBL.GetInsuranceAmounts(InsCompID, policyNo, Sum_Ins_Amt);
            return Json(dto, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetInsurancePrint(string Key)
        {
            Dictionary<string, string> list = CommonService.GetQueryString(Key);

            Int64 JobNo = -1, InsuranceID = -1;


            if (list.ContainsKey("MoveID"))
            {
                JobNo = Convert.ToInt64(list["MoveID"]);
            }
            if (list.ContainsKey("InsuranceID"))
            {
                InsuranceID = Convert.ToInt32(list["InsuranceID"]);
            }

            InsurancePrint model = insuranceBL.GetPrintDetail(InsuranceID);
            
            return View("InsurancePrint", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DocDownload(int id)
        {
            JobDocument jobDocument = insuranceBL.GetDownloadFile(id);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                //var filepath = System.IO.Path.Combine(Server.MapPath("/Files/"), fileName);
                return File(jobDocument.FilePath, MimeMapping.GetMimeMapping(jobDocument.FilePath), jobDocument.FileName);
            }

            return new HttpStatusCodeResult(404);

        }
    }
}
