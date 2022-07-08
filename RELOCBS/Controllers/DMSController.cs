using RELOCBS.AjaxHelper;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.MoveMange;
using RELOCBS.CustomAttributes;
using RELOCBS.Entities;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class DMSController : BaseController
    {
        private string _PageID = "10";

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

        private MoveManageBL _moveManageBL;
        public MoveManageBL moveManageBL
        {
            get
            {
                if (this._moveManageBL == null)
                    this._moveManageBL = new MoveManageBL();
                return this._moveManageBL;

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

        private CommanBL _commanBL;
        public CommanBL commanBL
        {
            get
            {
                if (this._commanBL == null)
                    this._commanBL = new CommanBL();
                return this._commanBL;

            }
        }

        // GET: DMS
        public ActionResult Index()
        {
            return View();
        }

        // GET: DMS/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        private string FillCombo(string Type,string DocName="")
        {
            String FromType = Type == "ENQDETAIL" ? "ENQUIRY" : Type;

            if (!string.IsNullOrWhiteSpace(DocName))
            {
                DocName = DocName.ToUpper();
            }
            else
            {
                DocName = "";
            }
            
            List <SelectListItem> items = comboBL.GetJobDocTypelDropdown(DocFromType: FromType).ToList();
            string value="-1";

            if (items.Count>0)
            {
                if (items.Count == 1 || Type.ToUpper() == "ENQUIRY")
                {
                    items.First().Selected = true;
                    value = items.First().Value;

                }
                else
                {
                    items.Last().Selected = true;
                    value = items.Last().Value;
                    Type = "ENQUIRY";
                }

            }
            
            ViewData["DocTypeList"] = items;

            List<SelectListItem> DocItems = comboBL.GetJobDocNamelDropdown(DocTypeID: Convert.ToInt32(value)).ToList();
            if (!string.IsNullOrWhiteSpace(DocName))
            {
                DocItems = DocItems.Where<SelectListItem>(m => m.Text.ToUpper() == DocName).ToList();
                DocItems.Last().Selected = true;
            }
            ViewData["DocNameList"] = DocItems;

            return value;

        }

        // GET: DMS/Create
        //[OutputCache(Duration = 0)]
        public ActionResult Create(int id,string Type,string DocName)
        {
           
            string DocType = FillCombo(Type.ToUpper(), DocName);
            
            int DocTypeID = -1;

            if (!string.IsNullOrWhiteSpace(DocType))
            {
                DocTypeID = Convert.ToInt32(DocType);
            }

            string FromType = Type;
            switch (Type.ToUpper())
            {
                case "ENQDETAIL":

                    FromType ="Enquiry";

                    break;

                default:
                    break;
            }

            JobDocUpload Model = moveManageBL.GetDocumentGrid(id,Type, FromType, DocTypeID);
            Model.DocNameText = DocName;
            return Request.IsAjaxRequest()
                ? (ActionResult)PartialView("Create", Model)
                : View(Model);
        }

        // POST: DMS/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobDocUpload Model,int id, string Type)
        {
            try
            {
                string DocType = FillCombo(Type.ToUpper(), Model.DocNameText);

                if (ModelState.IsValid)
                {
                    string message = string.Empty;
                    bool res = false;
                    AjaxResponse result = new AjaxResponse();
                    MoveManageViewModel JobModel = new MoveManageViewModel();
                    JobModel.jobDocUpload = Model;

                    res = moveManageBL.InsertDocument(JobModel, UserSession.GetUserSession().LoginID, out message);

                    if (res)
                    {

                        string FromType = Type;
                        switch (Type.ToUpper())
                        {
                            case "ENQDETAIL":

                                FromType = "Enquiry";

                                break;

                            default:
                                break;
                        }

                        Model = moveManageBL.GetDocumentGrid(id, Type, FromType, Model.DocTypeID);
                        //Model.DocName = DocName;
                        result.Result = this.RenderPartialViewToString("Create", Model);
                        return Json(result);

                        //result.Success = false;
                        //ModelState.AddModelError(string.Empty, message);
                        //result.Message = message;

                        //this.AddToastMessage("RELOCBS", message, ToastType.Error);

                    }
                    else
                    {
                        //result.Success = true;
                        //result.Message = message;
                        ModelState.AddModelError(string.Empty, message);
                    }

                    
                }

                return Request.IsAjaxRequest()
                  ? (ActionResult)PartialView("Create", Model)
                  : View(Model);
            }
            catch
            {
                return Request.IsAjaxRequest()
                  ? (ActionResult)PartialView("Create", Model)
                  : View(Model);
            }
        }


		public ActionResult GCCInsCreate(int id, string Type, string DocName)
		{

			string DocType = FillCombo(Type.ToUpper(), DocName);

			int DocTypeID = -1;

			if (!string.IsNullOrWhiteSpace(DocType))
			{
				DocTypeID = Convert.ToInt32(DocType);
			}
			
			string FromType = Type;
			
			JobDocUpload Model = moveManageBL.GetDocumentGrid(id, Type, FromType, DocTypeID);
			Model.DocNameText = DocName;
			if (Model.DocNameID <= 0)
			{
				Model.DocNameID = Convert.ToInt32(((List<SelectListItem>)ViewData["DocNameList"]).Last().Value);
			}
			return Request.IsAjaxRequest()
				? (ActionResult)PartialView("GCCInsCreate", Model)
				: View(Model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult GCCInsCreate(JobDocUpload Model, int id, string Type)
		{
			try
			{
				string DocType = FillCombo(Type.ToUpper(), Model.DocNameText);

				if (ModelState.IsValid)
				{
					string message = string.Empty;
					bool res = false;
					AjaxResponse result = new AjaxResponse();
					MoveManageViewModel JobModel = new MoveManageViewModel();
					JobModel.jobDocUpload = Model;

					res = moveManageBL.InsertGCCDocument(JobModel, UserSession.GetUserSession().LoginID, out message);

					if (res)
					{

						string FromType = Type;
						Model = moveManageBL.GetDocumentGrid(id, Type, FromType, Model.DocTypeID);
						//Model.DocName = DocName;
						result.Result = this.RenderPartialViewToString("GCCInsCreate", Model);
						return Json(result);

						//result.Success = false;
						//ModelState.AddModelError(string.Empty, message);
						//result.Message = message;

						//this.AddToastMessage("RELOCBS", message, ToastType.Error);

					}
					else
					{
						//result.Success = true;
						//result.Message = message;
						ModelState.AddModelError(string.Empty, message);
					}


				}

				return Request.IsAjaxRequest()
				  ? (ActionResult)PartialView("GCCInsCreate", Model)
				  : View(Model);
			}
			catch
			{
				return Request.IsAjaxRequest()
				  ? (ActionResult)PartialView("GCCInsCreate", Model)
				  : View(Model);
			}
		}

		// GET: DMS/Edit/5
		public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DMS/Edit/5
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

        // GET: DMS/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DMS/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DocDelete(int FileID, JobDocUpload Model)
        {
            try
            {
                string DocType = FillCombo(Model.DocFromType.ToUpper(),Model.DocNameText);
                string DocName = Model.DocNameText;

                /////Remove the model Error in delete
                foreach (var modelValue in ModelState.Values)
                {
                    modelValue.Errors.Clear();
                }
                ////if (ModelState.IsValid)
                {
                    string message = string.Empty;
                    bool res = false;
                    AjaxResponse result = new AjaxResponse();

                    res = moveManageBL.DeleteDocument(FileID, out message);

                    if (res)
                    {

                        string FromType = Model.DocFromType;
                        switch (Model.DocFromType)
                        {
                            case "ENQDETAIL":

                                FromType = "Enquiry";

                                break;

                            default:
                                break;
                        }

                        Model = moveManageBL.GetDocumentGrid(Model.ID, Model.DocFromType, FromType, Model.DocTypeID);
                        Model.DocNameText = DocName;
                        result.Result = this.RenderPartialViewToString("Create", Model);
                        return Json(result);

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }


                }

                return Request.IsAjaxRequest()
                  ? (ActionResult)PartialView("Create", Model)
                  : View(Model);
            }
            catch
            {
                return Request.IsAjaxRequest()
                  ? (ActionResult)PartialView("Create", Model)
                  : View(Model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewDocument(int id)
        {
            //string path = Server.MapPath("~/Content/JViewFiles/");
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}
            JobDocument jobDocument = moveManageBL.GetDownloadFile(id);
            if (!string.IsNullOrWhiteSpace(jobDocument.FilePath) && System.IO.File.Exists(jobDocument.FilePath))
            {
                //var filepath = System.IO.Path.Combine(Server.MapPath("/Files/"), fileName);
                
                byte[] FileBytes = System.IO.File.ReadAllBytes(jobDocument.FilePath);
                Response.Headers.Add("Content-Disposition", "inline;filename=\"" + jobDocument.FileName + "\"");
                //HttpContext.Response.AppendHeader("Content-Disposition", "inline;filename=" + jobDocument.FileName + "");
                return File(FileBytes, MimeMapping.GetMimeMapping(jobDocument.FilePath));

                ////Using the FileStreamResult 
                //var fileStream = new FileStream(jobDocument.FilePath, FileMode.Open, FileAccess.Read);
                //var fsResult = new FileStreamResult(fileStream, MimeMapping.GetMimeMapping(jobDocument.FileName));
                ////fsResult.FileDownloadName = jobDocument.FileName;
                //return fsResult;
            }

            return new HttpStatusCodeResult(404);
            
        }


        [HttpGet]
        public ActionResult AgentInvoiceCreate(int id, int Type, int DocName)
        {
            AgentInvoice Model =new AgentInvoice();
            Model.DocTypeID = Type;
            Model.DocNameID = DocName;
            Model.DocFromType = "moveman";
            Model = moveManageBL.GetAgentInvDocumentGrid(id, Model.DocFromType, Model.DocFromType, Type, DocName);
            return Request.IsAjaxRequest()? (ActionResult)PartialView("AgentInvoiceCreate", Model): View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgentInvoiceCreate(AgentInvoice Model, int id, string Type)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string message = string.Empty;
                    bool res = false;
                    AjaxResponse result = new AjaxResponse();
                    res = moveManageBL.InsertAgentInvDocument(Model, UserSession.GetUserSession().LoginID, out message);

                    if (res)
                    {
                        string FromType = Type;
                        Model= moveManageBL.GetAgentInvDocumentGrid(id, Model.DocFromType, Model.DocFromType,Model.DocTypeID,Model.DocNameID);
                        result.Result = this.RenderPartialViewToString("AgentInvoiceCreate", Model);
                        return Json(result);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, message);
                    }
                }

                return Request.IsAjaxRequest()? (ActionResult)PartialView("AgentInvoiceCreate", Model): View(Model);
            }
            catch
            {
                return Request.IsAjaxRequest()? (ActionResult)PartialView("AgentInvoiceCreate", Model): View(Model);
            }
        }

        [HttpGet]
        public JsonResult GetVendorDetails(int Id)
        {
            var AgentDetail = new RELOCBS.BL.Common.AgentBL().GetDetailById(Id);

            return Json( new { vendorCode= AgentDetail.VendorCode,address = AgentDetail.Address1+" "+AgentDetail.Address2 +" "+AgentDetail.CityName+" "+AgentDetail.PinCode }, JsonRequestBehavior.AllowGet);


        }
    }
}
