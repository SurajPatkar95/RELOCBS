using Newtonsoft.Json;
using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.Claims;
using RELOCBS.Common;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class ClaimController : BaseController
    {
        private string _PageID = "27";
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

        private ClaimsBL _claimsBL;
        public ClaimsBL claimsBL
        {
            get
            {
                if (this._claimsBL == null)
                    this._claimsBL = new ClaimsBL();
                return this._claimsBL;
            }
        }

        // GET: Claim
        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Claim");
            string sort = "JobNo";
            string sortdir = "desc";
            string search = "";
            string JobNo = "-1";
            Int64 ClaimID = -1;
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
                JobNo = Convert.ToString(Request.Form["JobNo"]);
            }

            if (Request.Form["ClaimNo"] != null && Request.Form["ClaimNo"].Trim() != "")
            {
                ClaimID = Convert.ToInt64(Request.Form["ClaimNo"]);
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
            int IsFinanceUser = 0;
            var data = claimsBL.GetClaimGrid(Fromdate, Todate, IsJobDate, IsInsuranceDate, JobNo, ClaimID, sort, sortdir, skip, pageSize, out totalRecord,out IsFinanceUser);
            FillCombo(Fromdate, Todate, -1, ClaimID);
            ViewBag.IsFinanceUser = IsFinanceUser;

            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;

            var itemsAsIPagedList = new StaticPagedList<Entities.ClaimGrid>(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }

        private void FillCombo(DateTime? from, DateTime? to, Int64? MoveID, Int64? ClaimID, int InsCompID = -1)
        {

            ViewData["ClaimId"] = ClaimID;
            ViewData["MoveId"] = MoveID;

            ViewData["ClaimStatusList"] = comboBL.GetClaimStatusDropdown(); 
            ViewData["ChqStatusList"] = comboBL.GetChequeStatusDropdown();
            ViewData["settlementTypeList"] = comboBL.GetSettlementTypeDropdown();
            ViewData["ClaimList"] = comboBL.getClaimNoDropdown();
            ViewData["JobNoList"] = comboBL.getJobNolDropdown(SPTYPE: "ALLACTIVEINSURANCEJOB");
            ViewData["PaymodeList"] = comboBL.GetPaymodeDropdown();
            ViewData["CurrancyList"] = comboBL.GetCurrencyDropdown();
            ViewData["NaturesClaimList"] = comboBL.GetClaimNatureDropdown();
            ViewData["ItemCategoryList"] = comboBL.GetClaimCategoryDropdown();
            ViewData["ItemDetailList"] = new List<SelectListItem>();//comboBL.getJobNolDropdown();
            ViewData["ControllerList"] = comboBL.GetControllerDropdown();
            string SPTYPE = "ALLACTIVE";
            if (MoveID > 0)
            {
                SPTYPE = "MOVEIDCOMP";
            }

            ViewData["InsuranceCompList"] = comboBL.GetInsuranceCompanyDropdown(SPTYPE: SPTYPE,MoveID:MoveID);
            ViewData["InsurancePANOList"] = comboBL.getInsuranceNoDropdown(MoveID: MoveID,CompID: InsCompID);
            ViewData["DocTypeList"] = comboBL.GetClaimDocTypeDropdown();
        }

        // GET: Claim/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Claim/Create
        public ActionResult Create(string Key)
        {
            session.Set<string>("PageSession", "Claim");

            if (!string.IsNullOrEmpty(Key) || Key != "-1")
            {
                Claim model = new Claim();


                Dictionary<string, string> list = CommonService.GetQueryString(Key);

                Int64 JobNo = -1, ClaimID = -1;


                if (list.ContainsKey("MoveID"))
                {
                    JobNo = Convert.ToInt64(list["MoveID"]);
                }
                if (list.ContainsKey("ClaimID"))
                {
                    ClaimID = Convert.ToInt32(list["ClaimID"]);
                }


                model = claimsBL.GetClaimDetails(JobNo, ClaimID);

                if (model.BaseCurrencyID <= 0)
                {
                    model.BaseCurrencyID = UserSession.GetUserSession().BaseCurrID;
                    model.RateCurrencyID = UserSession.GetUserSession().BaseCurrID;
                }

                FillCombo(System.DateTime.Now, System.DateTime.Now, model.MoveID, model.Claim_ID,model.InsuranceCompanyID);

                return View(model);
            }

            return Redirect("Index");
        }

        // POST: Claim/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(Claim model, string Key,string submit)
        {
            session.Set<string>("PageSession", "Claim");
            try
            {
                
                if (!string.IsNullOrWhiteSpace(model.HFdetails))
                {
                    List<ClaimDetails> details = (List<ClaimDetails>)JsonConvert.DeserializeObject(Convert.ToString(model.HFdetails), (typeof(List<ClaimDetails>)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
                    model.details = details;
                }

                //if (model.docUpload.docLists == null && !string.IsNullOrWhiteSpace(model.HFDocs))
                //{
                //    List<DocList> details = (List<DocList>)JsonConvert.DeserializeObject(Convert.ToString(model.HFDocs), (typeof(List<DocList>)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
                //    model.docUpload.docLists = details;
                //}

                string message = string.Empty;
                FillCombo(System.DateTime.Now, System.DateTime.Now, model.Claim_ID, model.InsuranceCompanyID);
                if (ModelState.IsValid)
                {
                    bool res = false;
                    bool IsApproved = false;

                    if (submit.ToUpper() == "APPROVE")
                    {
                        IsApproved = true;
                    }

                    res = submit.ToUpper()== "SUBMITFIANANCE" ? claimsBL.SentToFinance(model, out message) : claimsBL.Inset(model, out message,IsApproved);
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
                        return RedirectToAction("Create", new { Key = CommonService.GenerateQueryString("MoveID=ParamValue0&ClaimID=ParamValue1", new string[] { Convert.ToString(model.MoveID), (string.IsNullOrWhiteSpace(Convert.ToString(model.Claim_ID)) ? "-1" : Convert.ToString(model.Claim_ID)) }) });
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

        // GET: Claim/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Claim/Edit/5
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

        // GET: Claim/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Claim/Delete/5
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

        public JsonResult GetaJAXQueryString(string JobNo)
        {
            string querystring = string.Empty;
            try
            {
                Int64 MoveID = Convert.ToInt64(comboBL.getJobNolDropdown(SPTYPE: "ALLACTIVE").ToList().Where(m => m.Text == JobNo).FirstOrDefault().Value);

                if (MoveID >0)
                {
                    querystring = CommonService.GenerateQueryString("MoveID=ParamValue0", new string[] { Convert.ToString(MoveID) });
                }

                
            }
            catch (Exception)
            {
            }

            return Json(new { querystring = querystring }, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetInsurance(Int64 JobNo,Int64 PANO)
        {

           var model = claimsBL.GetInsuranceDetail(JobNo, PANO);

           FillCombo(from:System.DateTime.Now,to:System.DateTime.Now,ClaimID:-1,MoveID:JobNo,InsCompID : model.InsuranceCompanyID);

           return PartialView("_InsuranceDetail",model);
        }

        public ActionResult GetClaimPrint(string Key)
        {
            Dictionary<string, string> list = CommonService.GetQueryString(Key);

            Int64 JobNo = -1, ClaimID = -1,Type=-1;

            if (list.ContainsKey("MoveID"))
            {
                JobNo = Convert.ToInt64(list["MoveID"]);
            }

            if (list.ContainsKey("ClaimID"))
            {
                ClaimID = Convert.ToInt32(list["ClaimID"]);
            }

            if (list.ContainsKey("Type"))
            {
                Type = Convert.ToInt32(list["Type"]);
            }

            ClaimPrint model = claimsBL.GetPrintDetail(JobNo, ClaimID);

            if (Type==2)
            {
                return View("_ClaimPrint_B", model);
            }


            return View("_ClaimPrint_A", model);
        }

        public ActionResult GetEClaimPrint(string Key)
        {
            Dictionary<string, string> list = CommonService.GetQueryString(Key);

            Int64 JobNo = -1, ClaimID = -1;

            if (list.ContainsKey("MoveID"))
            {
                JobNo = Convert.ToInt64(list["MoveID"]);
            }

            if (list.ContainsKey("ClaimID"))
            {
                ClaimID = Convert.ToInt32(list["ClaimID"]);
            }

            ViewBag.Key = Key;

            EClaimPrint model = claimsBL.GetEPrintDetail(JobNo, ClaimID);
            return View("E_ClaimPrint",model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadDoc(Int64 DocID, Int64? Claim_ID)
        {

            JobDocument jobDocument = claimsBL.GetDownloadFile(DocID, Claim_ID);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                //var filepath = System.IO.Path.Combine(Server.MapPath("/Files/"), fileName);
                return File(jobDocument.FilePath, MimeMapping.GetMimeMapping(jobDocument.FilePath), jobDocument.FileName);
            }

            return new HttpStatusCodeResult(404);
        }

        [HttpPost]
        [WordDocument]
        public ActionResult EprintExport(string Key)
        {

            //StringBuilder builder = new StringBuilder();

            //Response.Clear();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment;filename=E-Claim.doc");
            //Response.Charset = "";
            //Response.ContentType = "application/vnd.ms-word";
            //Response.Output.Write(GridHtml);
            //Response.Flush();
            //Response.End();

            Dictionary<string, string> list = CommonService.GetQueryString(Key);

            Int64 JobNo = -1, ClaimID = -1;

            if (list.ContainsKey("MoveID"))
            {
                JobNo = Convert.ToInt64(list["MoveID"]);
            }

            if (list.ContainsKey("ClaimID"))
            {
                ClaimID = Convert.ToInt32(list["ClaimID"]);
            }

            EClaimPrint model = new EClaimPrint();
            if (JobNo>0 && ClaimID>0)
            {
                model = claimsBL.GetEPrintDetail(JobNo, ClaimID);
            }

            ViewBag.WordDocumentFilename = "E-Claim Report";

            return View("_EClaimPrintWord", model);
        }
    }
}
