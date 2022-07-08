using PagedList;
using RELOCBS.App_Code;
using RELOCBS.BL;
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
    public class JobReportController : BaseController
	{
		private string _PageID = "20";
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

		private JobReportBL _jobreportBL;
		public JobReportBL jobreportBL
		{
			get
			{
				if (this._jobreportBL == null)
					this._jobreportBL = new JobReportBL();
				return this._jobreportBL;
			}
		}

		// GET: JobReport
		public ActionResult Index(int page = 1, string Submit = "Search")
        {
			if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
			{
				return new HttpStatusCodeResult(403);
			}

			session.Set<string>("PageSession", "Job Report");
			string sort = "JobNo";
			string sortdir = "desc";
			string search = "";
			string JobNo = "";
			int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
			string OrderBy = "";
			int Order = 0;
			DateTime? Fromdate = null;//System.DateTime.Now.Date.AddDays(-2);
			DateTime? Todate = null;//System.DateTime.Now;
			string Shipper = "";
            int Status = -1;
            bool IsPackComplete=false;
            bool IsPackStart = false;
            Int16 JobType = 1;

            string SearchKey = string.Empty;
			if (Request.Form["FromDate"] != null && Request.Form["FromDate"].Trim() != "")
			{
				Fromdate = Convert.ToDateTime(Request.Form["FromDate"]);
			}
			if (Request.Form["JobNo"] != null && Request.Form["JobNo"].Trim() != "")
			{
				JobNo = Convert.ToString(Request.Form["JobNo"]);
			}
			if (Request.Form["ToDate"] != null && Request.Form["ToDate"].Trim() != "")
			{
				Todate = Convert.ToDateTime(Request.Form["ToDate"]);
			}

            if (Request.Form["Status"] != null && Request.Form["Status"].Trim() != "")
            {
                Status = Convert.ToInt32(Request.Form["Status"]);
            }
             if (Request.Form["JobType"] != null && Request.Form["JobType"].Trim() != "")
            {
                JobType = Convert.ToInt16(Request.Form["JobType"]);
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
            if (Request.Params["IsPackStart"] != null && Request.Params["IsPackStart"].Trim() != "")
            {
                
                if (Request.Form.GetValues("IsPackStart")!=null && Convert.ToString(Request.Form.GetValues("IsPackStart")[0]).Trim().ToUpper()=="TRUE")
                {
                    IsPackStart = true;
                }

            }
            if (Request.Params["IsPackComplete"] != null && Request.Params["IsPackComplete"].Trim() != "")
            {
                if (Request.Form.GetValues("IsPackComplete") != null && Convert.ToString(Request.Form.GetValues("IsPackComplete")[0]).Trim().ToUpper() == "TRUE")
                {
                    IsPackComplete = true;
                }

            }
            ViewData["SelectedJobType"] = JobType;
            ViewData["IsPackStartChecked"] = IsPackStart;
            ViewData["IsPackCompleteChecked"] = IsPackComplete;
            int totalRecord = 0;
            if (page < 1) page = 1;
            //int skip = (pageNo * pageSize) - pageSize;
            int skip = pageSize;
            pageSize = page;
            var data = jobreportBL.GetJobReportList(Fromdate, Todate, sort, sortdir, skip, pageSize, out totalRecord, IsPackStart,IsPackComplete,JobNo, Shipper, Status, JobType);
			FillCombo();
		    ViewBag.TotalRows = totalRecord;
			ViewBag.search = search;

		    var itemsAsIPagedList = new StaticPagedList<PJR_DJR>(data, pageSize, skip, totalRecord);
		    return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
		}


		
		public ActionResult Create(string Key)//key = MoveID&ComponentID&PJRDJRID
        {
            session.Set<string>("PageSession", "Job Report");
            Dictionary<string,string> list = CommonService.GetQueryString(Key);
			PJR_DJR jobreportmodel = new PJR_DJR();
			Int64 JobNo = 0, PJRDJRID = 0;
            int ComponentID=0;Int16 JobType = 1;

            if (list.ContainsKey("MoveID"))
			{
				JobNo = Convert.ToInt64(list["MoveID"]);
			}
            if (list.ContainsKey("ComponentID"))
            {
                ComponentID = Convert.ToInt32(list["ComponentID"]);
            }
            if (list.ContainsKey("PJRDJRID"))
			{
				PJRDJRID = Convert.ToInt64(list["PJRDJRID"]);
			}
            if (list.ContainsKey("JobType"))
            {
                JobType = Convert.ToInt16(list["JobType"]);
            }

            //jobreportBL.EnqID = EnqId;
            jobreportmodel = jobreportBL.GetPJR_DJR_Details(JobNo, ComponentID, PJRDJRID, JobType);
			FillCombo();
			return View(jobreportmodel);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PJR_DJR model, string Key,string submit)
        {
            try
            {
                session.Set<string>("PageSession", "Job Report");
                FillCombo();
                string message = string.Empty;
                if (ModelState.IsValid)
                {
                        bool res = false;
                        
                        res = jobreportBL.InsetPJR_DJR(model,submit, out message);
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
                            return RedirectToAction("Create", new { Key = CommonService.GenerateQueryString("MoveID=ParamValue0&ComponentID=ParamValue1&PJRDJRID=ParamValue2&JobType=ParamValue3", new string[] { Convert.ToString(model.MoveID),Convert.ToString(model.RateComponentID), (string.IsNullOrWhiteSpace(Convert.ToString(model.PJR_DJR_ID)) ? "-1" : Convert.ToString(model.PJR_DJR_ID)),Convert.ToString(model.JobType)  }) });
                            //return Json(result);
                        }
                }
                else
                {
                    return View("Create", model);
                }
            }
            catch (Exception e)
            {
                return View();
            }
        }


		private void FillCombo()
		{
			ViewData["JobNoList"] = comboBL.getJobNolDropdown();
			ViewData["VendorList"] = comboBL.GetVendorDropdown();
            ViewData["DocTypeList"] = comboBL.GetDocTypeDropdown();
            ViewData["StatusList"] = new List<SelectListItem>() { new SelectListItem() { Text = "In Progres", Value = "1" }, new SelectListItem() { Text = "Completed", Value = "2" } };
        }

		public JsonResult GetaJAXQueryString(string JobNo,string ComponentID)
		{
            //string querystring = CommonService.GenerateQueryString("MoveID=ParamValue0&PJRDJRID=ParamValue1", new string[] { JobNo,"0"  });

            string querystring = CommonService.GenerateQueryString("MoveID=ParamValue0&ComponentID=ParamValue1", new string[] { JobNo, ComponentID });

            return Json(new { querystring = querystring}, JsonRequestBehavior.AllowGet);
		}
		
        public ActionResult GetInstActivity(Int64 MoveID,int ComponentID,Int16 JobType)
        {
            JobDiaryModel jobreportmodel = new JobDiaryModel();
            FillCombo();
            if (MoveID!=0 && (JobType==1 || JobType==0))
            {
                jobreportmodel = jobreportBL.GetInstructionSheetDetails(MoveID, ComponentID, JobType);
            }
            
            return PartialView("_InstActivityGridPartial",jobreportmodel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DownloadDoc(Int64 DocID, Int64? PRJ_DJR_ID)
        {

            JobDocument jobDocument = jobreportBL.GetDownloadFile(DocID, PRJ_DJR_ID);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                //var filepath = System.IO.Path.Combine(Server.MapPath("/Files/"), fileName);
                return File(jobDocument.FilePath, MimeMapping.GetMimeMapping(jobDocument.FilePath), jobDocument.FileName);
            }

            return new HttpStatusCodeResult(404);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string Key)
        {
            try
            {
                FillCombo();

                Dictionary<string, string> list = CommonService.GetQueryString(Key);
                PJR_DJR jobreportmodel = new PJR_DJR();
                Int64 JobNo = 0, PJRDJRID = 0,ID=0;
                int ComponentID = 0;Int32 JobType = 1;
                string Type = string.Empty;

                if (list.ContainsKey("MoveID"))
                {
                    JobNo = Convert.ToInt64(list["MoveID"]);
                }
                if (list.ContainsKey("ComponentID"))
                {
                    ComponentID = Convert.ToInt32(list["ComponentID"]);
                }
                if (list.ContainsKey("PJRDJRID"))
                {
                    PJRDJRID = Convert.ToInt64(list["PJRDJRID"]);
                }
                if (list.ContainsKey("ID"))
                {
                    ID = Convert.ToInt64(list["ID"]);
                }
                if (list.ContainsKey("Type"))
                {
                    Type = Convert.ToString(list["Type"]);
                }
                if (list.ContainsKey("JobType"))
                {
                    JobType = Convert.ToInt16(list["JobType"]);
                }

                string message = string.Empty;
                if (ID > 0 && PJRDJRID > 0 && !string.IsNullOrWhiteSpace(Type) && (JobType==0 || JobType==1))
                {
                    bool res = false;

                    res = jobreportBL.Delete_PJR_DJR_Cost(ID, PJRDJRID, Type, out message);
                    if (!res)
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    }
                }
                else
                {
                    this.AddToastMessage("RELOCBS", "Invalid record to delete", ToastType.Success);
                }

                return RedirectToAction("Create", new { Key = CommonService.GenerateQueryString("MoveID=ParamValue0&ComponentID=ParamValue1&PJRDJRID=ParamValue2&JobType=ParamValue3", new string[] { Convert.ToString(JobNo), Convert.ToString(ComponentID), (string.IsNullOrWhiteSpace(Convert.ToString(PJRDJRID)) ? "-1" : Convert.ToString(PJRDJRID)), Convert.ToString(JobType) }) });
            }
            catch (Exception e)
            {
                return View();
            }

        }    

        public ActionResult GetJobReport(string Key)
        {
            Dictionary<string, string> list = CommonService.GetQueryString(Key);
            Int64 MoveID = 0, PJRDJRID = 0;
            int ComponentID = 0;Int16 JobType = 1;
            string Type = string.Empty;

            if (list.ContainsKey("MoveID"))
            {
                MoveID = Convert.ToInt64(list["MoveID"]);
            }
            if (list.ContainsKey("ComponentID"))
            {
                ComponentID = Convert.ToInt32(list["ComponentID"]);
            }
            if (list.ContainsKey("PJRDJRID"))
            {
                PJRDJRID = Convert.ToInt64(list["PJRDJRID"]);
            }
            if (list.ContainsKey("JobType"))
            {
                JobType = Convert.ToInt16(list["JobType"]);
            }

            var jobreportmodel = jobreportBL.GetPJR_DJR_Report(MoveID, ComponentID, PJRDJRID, JobType);

            return View("Job_Report", jobreportmodel);
        }

		public ActionResult GetJobReportForMove(string Key)
		{
			Dictionary<string, string> list = CommonService.GetQueryString(Key);
			Int64 MoveID = 0, PJRDJRID = 0;
			int ComponentID = 0;
			string Type = string.Empty;

			if (list.ContainsKey("MoveID"))
			{
				MoveID = Convert.ToInt64(list["MoveID"]);
			}
			if (list.ContainsKey("ComponentID"))
			{
				ComponentID = Convert.ToInt32(list["ComponentID"]);
			}
			if (list.ContainsKey("PJRDJRID"))
			{
				PJRDJRID = Convert.ToInt64(list["PJRDJRID"]);
			}
			string msg = "";
			var jobreportmodel = jobreportBL.GetPJR_DJR_ReportForMove(MoveID, ComponentID, PJRDJRID, out msg);
			if (!string.IsNullOrEmpty(msg))
			{
				jobreportmodel.ReportType = ComponentID == 3 ? "DJR" : "PJR";
				//this.AddToastMessage("RELOCBS", msg, ToastType.Error);
			}



			return View("Job_Report", jobreportmodel);
		}


        public ActionResult GetDigitalInventory(int id)
        {

            string FilePath = jobreportBL.GetDigitalInventoryPDF(id);
            if (!string.IsNullOrWhiteSpace(FilePath) && System.IO.File.Exists(FilePath))
            {

                byte[] fileBytes = GetFile(FilePath);
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FilePath);
            }
            return HttpNotFound();
        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

}
}