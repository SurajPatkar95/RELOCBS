using RELOCBS.BL;
using RELOCBS.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.BL.Complaint;
using RELOCBS.App_Code;
using PagedList;
using RELOCBS.Models;
using RELOCBS.Extensions;
using RELOCBS.AjaxHelper;
using RELOCBS.Utility;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class ComplaintsController : BaseController
    {
        private string _PageID = "75";
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

        private ComplaintBL  _bL;
        public ComplaintBL bL
        {
            get
            {
                if (this._bL == null)
                    this._bL = new ComplaintBL();
                return this._bL;
            }
        }


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

        // GET: ATR
        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Complaints");
            string sort = "ComplaintId";
            string sortdir = "desc";
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            string OrderBy = "";
            int Order = 0;
            int? classificationId = -1;
            int? statusId = -1;
            string filterType = string.Empty;
            string filterValue = string.Empty;
            string shipper =string.Empty;
            string loggerName = string.Empty;


            string SearchKey = string.Empty;
            if (Request.Form["classificationId"] != null && Request.Form["classificationId"].Trim() != "")
            {
                classificationId = Convert.ToInt32(Request.Form["classificationId"]);
            }
            if (Request.Form["filterType"] != null && Request.Form["filterType"].Trim() != "")
            {
                filterType = Convert.ToString(Request.Form["filterType"]);
            }

            if (Request.Form["filterValue"] != null && Request.Form["filterValue"].Trim() != "")
            {
                filterValue = Convert.ToString(Request.Form["filterValue"]);
            }
            if (Request.Form["shipper"] != null && Request.Form["shipper"].Trim() != "")
            {
                shipper = Convert.ToString(Request.Form["shipper"]);
            }
            if (Request.Form["loggerName"] != null && Request.Form["loggerName"].Trim() != "")
            {
                loggerName = Convert.ToString(Request.Form["loggerName"]);
            }
            if (Request.Form["statusId"] != null && Request.Form["statusId"].Trim() != "")
            {
                statusId = Convert.ToInt32(Request.Form["statusId"]);
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

            int totalRecord = 0;
            if (page < 1) page = 1;
            //int skip = (pageNo * pageSize) - pageSize;
            int skip = pageSize;
            pageSize = page;

            var data = bL.GetForGrid(classificationId, statusId, shipper, loggerName,filterType,filterValue, sort, sortdir, skip, pageSize, out totalRecord);
            FillCombo();
            ViewData["classificationId"] = classificationId;
            ViewData["filterType"] = filterType;
            ViewData["statusId"] = statusId;
            ViewBag.TotalRows = totalRecord;

            var itemsAsIPagedList = new StaticPagedList<Entities.ComplaintGrid>(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }

        private void FillCombo()
        {
            ViewData["ClassificationList"] = comboBL.GetComplaintsClassificationDropdown();
            ViewData["StatusList"] = comboBL.GetComplaintsStatusDropdown();
            ViewData["SourceList"] = comboBL.GetComplaintsSourceDropdown();
        }

        // GET: ATR/Create
        public ActionResult Create(int? id)
        {
            Entities.Complaints data = new Entities.Complaints();
            FillCombo();
            if (id != null)
            {
                data = bL.GetDetailById(Convert.ToInt32(id));
            }

            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Create", data) : View(data);

        }

        // POST: ATR/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Entities.Complaints model)
        {
            try
            {
                AjaxResponse result = new AjaxResponse();
                string message = string.Empty;
                ViewBag.Flag = "1";
                FillCombo();
                if (ModelState.IsValid)
                {
                    result.Success = bL.Insert(model, out message);

                    if (result.Success)
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        return RedirectToAction("Create", new { id = model.ComplaintId });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }
                }
                
                return View("Create", model);
            }
            catch
            {
                return View("Create", model);
            }
        }
        
        
        // POST: Complaints/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                AjaxResponse result = new AjaxResponse();
                string Message = string.Empty;

                if (ModelState.IsValid)
                {
                    result.Success = bL.Delete(id, out Message);

                    if (result.Success)
                    {
                        result.Result = Message;
                    }
                    else
                    {
                        ModelState.AddModelError(String.Empty, Message);
                    }

                }
                else
                {
                    result.Success = false;
                    result.Result = "Complaint delete failed. Please try again.";
                }

                return Json(result);
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public JsonResult GetEnqNoSearchList(string term, string Value="", int page = 1)
        {
            IEnumerable<SelectListItem> List = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Value))
            {
                List = comboBL.GetComplaintsEnquiryNoDropdown(SPTYPE: "ALLACTIVE", Search: term);
            }
            else
            {
                List = comboBL.GetComplaintsEnquiryNoDropdown(SPTYPE: "SINGLE", EnqID: Convert.ToInt32(Value));
            }

            return Json(new { items = List }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetJobNoSearchList(string term, string Value="",int page=1)
        {
            IEnumerable<SelectListItem> List = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Value))
            {
                List = comboBL.GetComplaintsJobNoDropdown(SPTYPE: "ALLACTIVE", Search: term);
            }
            else
            {
                List = comboBL.GetComplaintsJobNoDropdown(SPTYPE: "SINGLE", MoveID: Convert.ToInt32(Value));
            }

            return Json(new { items = List }, JsonRequestBehavior.AllowGet);
        }

        // GET api/<EnqJobController>/5
        [HttpGet]
        public JsonResult GetEnqJobDetail(int EnqDetailId = 0, int MoveId = 0)
        {
            if (EnqDetailId > 0 || MoveId > 0)
            {
                var model = bL.GetEnqJobDetailById(EnqDetailId, MoveId);

                return Json(model, JsonRequestBehavior.AllowGet);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetComplaintShipmentList(string term = "", string Value = "", int EnqId = -1)
        {
            IEnumerable<SelectListItem> List = new List<SelectListItem>();
            if (string.IsNullOrEmpty(Value))
            {
                List = comboBL.GetComplaintsShipmentDropdown(SPTYPE: "ALLACTIVE", Search: term, EnqID: EnqId);
            }
            else
            {
                List = comboBL.GetComplaintsShipmentDropdown(SPTYPE: "SINGLE", EnqID: EnqId, EnqDetailID: Convert.ToInt32(Value));
            }

            return Json(new { items = List }, JsonRequestBehavior.AllowGet);
        }

    }
}
