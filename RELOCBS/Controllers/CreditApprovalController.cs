using RELOCBS.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RELOCBS.Entities;
using RELOCBS.App_Code;
using RELOCBS.BL;
using RELOCBS.BL.CreditApproval;
using PagedList;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using Newtonsoft.Json;
using RELOCBS.Common;
using System.IO;
using RELOCBS.AjaxHelper;
using RELOCBS.DAL;
using System.Web.Helpers;

namespace RELOCBS.Controllers
{
    [AuthorizeUser]
    public class CreditApprovalController : BaseController
    {

        private string _PageID = "65";
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

        private CreditApprovalBL _creditBL;
        public CreditApprovalBL creditBL
        {
            get
            {
                if (this._creditBL == null)
                    this._creditBL = new CreditApprovalBL();
                return this._creditBL;
            }
        }

        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG",".PDF" };

        // GET: CreditApproval
        public ActionResult Index(int page = 1)
        {
            if (!CSubs.CheckPageRights(_PageID, PermissionType.VIEW))
            {
                return new HttpStatusCodeResult(403);
            }

            session.Set<string>("PageSession", "Credit Approval");
            string sort = "CreditLimitEntityID";
            string sortdir = "desc";
            string search = "";
            int pageSize = settings.GetSettingByKey<int>("pagination_pagesize");
            int CorporateId = -1;
            string OrderBy = "";
            int Order = 0;
            string Status = "";
            if (Request.Form["search"] != null && Request.Form["search"].Trim() != "")
            {
                search = Convert.ToString(Request.Form["search"]);
            }
            if (Request.Form["Status"] != null && Request.Form["Status"].Trim() != "")
            {
                Status = Convert.ToString(Request.Form["Status"]);
            }
            if (Request.Form["CorporateId"] != null && Request.Form["CorporateId"].Trim() != "")
            {
                CorporateId = Convert.ToInt32(Request.Form["CorporateId"]);
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
            
            var data = creditBL.GetBusinessEntityGrid(search , Status, CorporateId, sort, sortdir, skip, pageSize, out totalRecord);
            fillCombo(Allitems:true);
            ViewBag.TotalRows = totalRecord;
            ViewBag.search = search;
            var itemsAsIPagedList = new StaticPagedList<Entities.CreditApprovalGrid>(data, pageSize, skip, totalRecord);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("_AjaxPartial", itemsAsIPagedList) : View(itemsAsIPagedList);
        }


        private void fillCombo(bool Allitems=false,int CreditLimitEntityID=-1,float Amount=0,int UserID=0,int CompID=1,bool IsRmc=false)
        {
            if (UserID>0)
            {
                ViewData["CreditAgentList"] = comboDAL.GetCreditApprovalCorporateDropdown(Convert.ToString(UserID),CompID: CompID,IsRMCBuss:IsRmc, CreditLimitEntityID: CreditLimitEntityID, Allitems: Allitems);
                ViewData["CityList"] = comboDAL.GetCityDropdown(Convert.ToString(UserID));
                ViewData["PeriodBasisList"] = comboDAL.GetCreditPeriodBasisDropdown(UserID, CompID: CompID, RMCBuss: IsRmc, Allitems: Allitems);
                ViewData["ProjectList"] = comboDAL.GetCreditApprovalProjectDropdown(Convert.ToString(UserID), CompID: CompID, RMCBuss: IsRmc, Allitems: Allitems);
                ViewData["RateCurrency"] = comboDAL.GetBaseCurrencyRateDropdown(Convert.ToString(UserID));
                ViewData["ApprovalUserList"] = comboDAL.GetCreditApprovalSendToDropdown(Convert.ToString(UserID), CompID: CompID, RMCBuss: IsRmc, Amount: Amount);
            }
            else
            {
                ViewData["CreditAgentList"] = comboBL.GetCreditApprovalCorporateDropdown(CreditLimitEntityID: CreditLimitEntityID, Allitems: Allitems);
                ViewData["CityList"] = comboBL.GetCityDropdown();
                ViewData["PeriodBasisList"] = comboBL.GetCreditPeriodBasisDropdown(Allitems: Allitems);
                ViewData["ProjectList"] = comboBL.GetCreditApprovalProjectDropdown(Allitems: Allitems);
                ViewData["RateCurrency"] = comboBL.GetBaseCurrencyRateDropdown();
                ViewData["ApprovalUserList"] = comboBL.GetCreditApprovalSendToDropdown(Amount: Amount);
            }
            ViewData["CustApproveTypeList"] = CommonService.CreditCustApprovalType;
            
        }

        [HttpGet]
        public ActionResult Create(int index=0,int EntityID = -1)
        {
            session.Set<string>("PageSession", "Credit Approval Form");
            
            CreditLimitEntity model = creditBL.GetDetails(EntityID);
            model.TabIndex = index;
            fillCombo(CreditLimitEntityID: EntityID,Amount:model.TotalCreditLimit);
            //model.buss_Dev.periods.Add(new CreditEntityPeriods { Buss_Dev_FeedbackID = 1, Credit_Entity_PeriodID = 1, Credit_period_basis = "test", Credit_period_basisID = 2, Credit_days = 30, IsActive = true });

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Create(int index, CreditLimitEntity model,string Save)
        {
            int EntityID = model.CreditLimitEntityID;
            
            try
            {
                string message = string.Empty;
                Save = Save.Trim();
                fillCombo(CreditLimitEntityID: EntityID,Amount :model.TotalCreditLimit);
                if ((index == 0 && ModelState.IsValid) || index>=1)
                {
                    bool res = false;

                    /////validation for the Credit period list
                    if (index == 0)
                    {
                        
                        if (!string.IsNullOrWhiteSpace(model.HFCredits))
                        {
                            CreditRootObject credits = (CreditRootObject)JsonConvert.DeserializeObject(Convert.ToString(model.HFCredits), (typeof(CreditRootObject)));  //JsonConvert.DeserializeObject<List<Inst_Activities>>(model.HVactivityList);
                            model.buss_Dev = credits.buss_Devs;
                        }

                        if (model.buss_Dev==null || model.buss_Dev.Count <= 0)
                        {
                            ModelState.AddModelError(string.Empty, "Credit Limit Amount is required");
                            this.AddToastMessage("RELOCBS", "Credit Limit Amount is required", ToastType.Error);
                            return View("Create", model);
                        }
                    }
                    else
                    {
                        if (model.ClientMap.CreditLimitEntityID<=0)
                        {
                            ModelState.AddModelError(string.Empty, "Invalid entry for Client map");
                            this.AddToastMessage("RELOCBS", "Invalid entry for Client map", ToastType.Error);
                            return View("Create", model);
                        }

                    }

                    ////swith for the save operation based on the index value ( 1: Credit Entity Insert) ( 2: Business Development Feedback)
                    switch (index)
                    {
                        case 0:

                            model.ApproveType = Save;
                            if (Save.Equals("Save", StringComparison.OrdinalIgnoreCase) || Save.Equals("Send for Approval", StringComparison.OrdinalIgnoreCase) || Save.Equals("Approve", StringComparison.OrdinalIgnoreCase))
                            {
                                res = creditBL.InsetCreditEntity(model, out message);
                            }
                            else if (Save.Equals("Approval Awaiting", StringComparison.OrdinalIgnoreCase)  || Save.Equals("Send for remove approval", StringComparison.OrdinalIgnoreCase) || Save.Equals("Remove Approval Awaiting", StringComparison.OrdinalIgnoreCase))
                            {
                                res = creditBL.UpdateApprovalStatus(model, out message);
                            }


                            EntityID = model.CreditLimitEntityID;
                            break;
                        case 1:
                            res = creditBL.InsertClientEntityMap(model.ClientMap, out message);
                            EntityID = model.ClientMap.CreditLimitEntityID;
                            break;

                        default:
                            break;
                    }
                    
                    if (!res)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save data.");
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                    }
                }
                else if(index == 0 && !ModelState.IsValid)
                {
                    return View("Create", model);
                }
                return RedirectToAction("Create", new { index = index, EntityID = EntityID });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Create", new { index = index, EntityID = EntityID });
            }

        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult RequestApprovalCAF(string V1, int V2,int V3,string qCode)
        {
            session.Set<string>("PageSession", "Credit Approval Form");
            bool isFromMail = true;
            if (CommonService.CheckValidApprovalLink(V3, qCode))
            {
                CreditLimitEntity model = creditBL.GetDetails(V2, isFromMail, V3);
                if (model.IsApprover == false || (model.IsApprover && model.StatusID < 2))
                {
                    return RedirectToActionPermanent("Login", "Account");
                }
                model.TabIndex = 0;
                model.SendApprovalToEmpID = V3;
                ViewBag.Code = qCode;
                fillCombo(CreditLimitEntityID: V2, Amount: model.TotalCreditLimit, UserID: V3, CompID: model.CompID, IsRmc: model.IsRMC);
                //model.buss_Dev.periods.Add(new CreditEntityPeriods { Buss_Dev_FeedbackID = 1, Credit_Entity_PeriodID = 1, Credit_period_basis = "test", Credit_period_basisID = 2, Credit_days = 30, IsActive = true });

                return View("Create", model);
            }

            return RedirectToActionPermanent("Login", "Account");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult RequestApprovalCAF(int index,string qCode,CreditLimitEntity model, string Save)
        {
            bool isFromMail = true;
            ViewBag.Code = qCode;
            try
            {
                fillCombo(CreditLimitEntityID: model.CreditLimitEntityID, Amount: model.TotalCreditLimit, UserID:Convert.ToInt32(model.SendApprovalToEmpID), CompID: model.CompID, IsRmc: model.IsRMC);
                if (ModelState.IsValid)
                {
                    string message = string.Empty;
                    bool res = false;
                    AjaxResponse result = new AjaxResponse();
                    model.ApproveType = Save;
                    res = creditBL.UpdateApprovalStatus(model, out message, isFromMail, model.SendApprovalToEmpID);
                    if (res)
                    {
                        result.Success = true;
                        result.Message = message;
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);

                    }
                    else
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to update status.");
                        result.Message = message;
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }


                }

                return RedirectToAction("RequestApprovalCAF", new { V1 = 'A', V2 = model.CreditLimitEntityID,V3= model.SendApprovalToEmpID });

            }
            catch
            {
                return RedirectToAction("RequestApprovalCAF", new { V1 = 'A', V2 = model.CreditLimitEntityID, V3 = model.SendApprovalToEmpID });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadFile(int index, CreditLimitEntity model)
        {
            try
            {
                 fillCombo();

                if (ModelState.IsValid)
                {
                    string message = string.Empty;
                    bool res = false;
                    AjaxResponse result = new AjaxResponse();

                    res = creditBL.InsertDocument(model.CustApprovalUpload, out message);

                    if (res)
                    {
                        result.Success = true;
                        result.Message = message;
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);

                    }
                    else
                    {
                        result.Success = false;
                        ModelState.AddModelError(string.Empty, "Unable to save document.");
                        result.Message = message;
                        this.AddToastMessage("RELOCBS", message, ToastType.Error);
                    }


                }

                return RedirectToAction("Create", new { index = index, EntityID = model.CreditLimitEntityID });

            }
            catch
            {
                return RedirectToAction("Create", new { index = index, EntityID = model.CreditLimitEntityID });
            }
        }


        public ActionResult ViewFile(int index,int EntityID,int FeedbackID,int FileID)
        {
            try
            {
                CustApprovalFiles Document = creditBL.GetCustApprovalFile(FileID,EntityID,FeedbackID);
                string FolderPath = System.Configuration.ConfigurationManager.AppSettings["CreditApprovalFiles"].ToString();
                Document.FilePath = System.IO.Path.Combine(FolderPath, Document.FilePath);
                if (!string.IsNullOrWhiteSpace(Document.FilePath) && System.IO.File.Exists(Document.FilePath))
                {
                    if (ImageExtensions.Contains(Path.GetExtension(Document.FilePath).ToUpperInvariant()))
                    {
                        byte[] FileBytes = System.IO.File.ReadAllBytes(Document.FilePath);
                        Response.Headers.Add("Content-Disposition", "inline;filename=\"" + Document.FileName + "\"");
                        return File(FileBytes, MimeMapping.GetMimeMapping(Document.FilePath));
                    }
                    else
                    {
                        return File(Document.FilePath, MimeMapping.GetMimeMapping(Document.FilePath), Document.FileName);
                    }
                }

                return new HttpStatusCodeResult(404);
            }
            catch (Exception)
            {
                return RedirectToAction("Create", new { index = index, EntityID = EntityID });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile(int index, int EntityID, int FeedbackID, int FileID)
        {
            bool res = false;
            string message = string.Empty;
            AjaxResponse result = new AjaxResponse();
            if (ModelState.IsValid)
            {
                res = creditBL.DeleteDocument(FileID, EntityID, FeedbackID, out message);
                if (!res)
                {
                    result.Success = false;
                    ModelState.AddModelError(string.Empty, "Unable to delete document.");
                    result.Message = message;
                    this.AddToastMessage("RELOCBS", message, ToastType.Error);
                }
                else
                {
                    result.Success = true;
                    result.Message = message;
                    this.AddToastMessage("RELOCBS", message, ToastType.Success);
                }
                return RedirectToAction("Create", new { index = index, EntityID = EntityID });
            }
            else
            {
                return RedirectToAction("Create", new { index = index, EntityID = EntityID });
            }

        }

        public  JsonResult ComboCreditServiceline(int Project, bool Allitems)
        {
            var lst = comboBL.GetCreditApprovalServiceLineDropdown(ProjectID: Project, Allitems:Allitems).Select(i => new { i.Value, i.Text }).ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAgentInfo(int AgentID)
        {
            var agentModel = new BL.Common.AgentBL().GetDetailById(AgentID);
            return Json(new { agentModel.CityID, Address = agentModel.Address1+" "+agentModel.Address2, agentModel.ContactPerson,agentModel.ContactPhone,agentModel.EmailID,agentModel.GST }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetCFAPrint(int Id)
        {
            
            CFAPrint model = creditBL.GetPrint(Id);

            return View("CFAPrint", model);
        }
        [HttpGet]
        public ActionResult GetSentToApprover(float Total)
        {
            var dataArray = comboBL.GetCreditApprovalSendToDropdown(Amount: Total).Select(s => new { id = s.Value, text = s.Text }).ToList();
            return Json(dataArray, JsonRequestBehavior.AllowGet);
        }

    }
}