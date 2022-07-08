using RELOCBS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RELOCBS.Entities;
using RELOCBS.BL.Account;
using System.Web.Routing;
using RELOCBS.BL;
using RELOCBS.Extensions;
using RELOCBS.Utility;
using RELOCBS.CustomAttributes;
using RELOCBS.AjaxHelper;

namespace RELOCBS.Controllers
{
    public class AccountController : BaseController
    {
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        private  UserBL  _userBL;

        public UserBL userBL
        {
            get
            {
                if (this._userBL == null)
                    this._userBL = new UserBL();
                return this._userBL;
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

		private CommanBL _commonBL;

		public CommanBL commonBL
		{
			get
			{
				if (this._commonBL == null)
					this._commonBL = new CommanBL();
				return this._commonBL;
			}
		}

		//
		// POST: /Account/Login

		[HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  ActionResult Login(LoginViewModel model, string returnUrl, string CompId)
        {
            if (ModelState.IsValid)
            {
                UserInformationModel user;
                string Msg = "Incorrect credentials.";

                if (userBL.ValidateUser(model.Username, model.Password, out user, out Msg))
                {
                    //sign in user
                    user.CompanyID = model.CompId;
                    user.CompanyName = model.CompName;
                    user.BussinessLine = model.BussinessLine;
					user.BaseCurrID = commonBL.GetBaseCurrByRMC(!model.BussinessLine.Equals("NON RMC-BUSINESS"), null, model.CompId);

					userBL.SignIn(user, model.RememberMe);
					userBL.UpdateCompany(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", Msg);
                    this.AddToastMessage("RELOCBS", Msg, ToastType.Error);
                }
            }
            return View(model);
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = userBL.GetUserByUsername(model.Email);
                if (user != null && (bool)user.isActive)
                {

                    try
                    {
                        
                        //SendEmail objSendEmail = (SendEmail)Session["SendData"];
                        //EmailSendResult resemail = _emailService.SaveEmail(objSendEmail);
                        //var resemail = new object();
                        //if (resemail.OutErr == 0)
                        //{
                        //    //var msg = new EmailMessage(user.LoginID, objSendEmail.EmailSubject, objSendEmail.EmailBody, _commonServices.Settings.GetSettingByKey<string>("default_email_from"));
                        //    //_emailSender.SendEmail(msg);
                        //}
                        //else
                        //{
                        //    ModelState.AddModelError(string.Empty, "Unable to Send Email. Kindly try again.");
                        //}

                        //NotifySuccess(_localizationService.GetResource("Admin.Configuration.EmailAccounts.SendTestEmail.Success"), false);
                    }
                    catch (Exception exc)
                    {
                        NotifyError(exc.Message, false);
                    }

                    

                    return RedirectToAction("ForgotPasswordConfirmation", "Account");
                }
                else
                {
                    ModelState.AddModelError(model.Email, "User does not exists.");
                }
            }

            return View(model);
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            if (code == null)
            {
                ModelState.AddModelError(String.Empty, "Security code for the current reset password request is not valid.");
            }
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                //User user = _userService.GetUserByUsername(model.Email);
                //if (user != null && (bool)user.isActive)
                //{
                //    var prToken = _commonServices.Settings.GetSettingByKey<string>(UserAttributeNames.PasswordRecoveryToken, userId: user.UserID);
                //    if (prToken == null)
                //    {
                //        ModelState.AddModelError(nameof(model.Email), I18N("Password reset failed."));
                //        return View();
                //    }

                //    var prTokenCreatedAt = _commonServices.Settings.GetSettingByKey<string>(UserAttributeNames.PasswordRecoveryTokenCreatedAt, userId: user.UserID);

                //    DateTime dtTokenCreatedAt = DateTime.ParseExact(prTokenCreatedAt, ShortDateTimeFormat, CultureInfo.InvariantCulture);
                //    DateTime dtNow = DateTime.UtcNow;
                //    TimeSpan span = dtNow.Subtract(dtTokenCreatedAt);

                //    var prTokenD = model.Code;
                //    if (prToken == prTokenD && span.Hours < 24)
                //    {
                //        int i = _userService.ResetPassword(user.UserID, model.Password);
                //        if (i == 0)
                //        {
                //            _commonServices.Settings.DeleteSetting(UserAttributeNames.PasswordRecoveryToken, user.UserID);
                //            _commonServices.Settings.DeleteSetting(UserAttributeNames.PasswordRecoveryTokenCreatedAt, user.UserID);

                //            // TODO - send mail
                //            if (System.Web.HttpContext.Current.Session["SendData"] == null)
                //            {
                //                _objEmailSend.SendEmailbyCode((int)UtilityEnums.Page.Move, user.UserID, (int)EmailUtility.EnumEmailTemplate.PasswordChanged);
                //                // TODO - rcheck page id
                //            }

                //            SendEmail objSendEmail = (SendEmail)Session["SendData"];
                //            objSendEmail.EmailTo = user.LoginID;
                //            objSendEmail.UserID = user.UserID;

                //            EmailSendResult resemail = _emailService.SaveEmail(objSendEmail);
                //            if (resemail.OutErr == 0)
                //            {
                //                System.Web.HttpContext.Current.Session["SendData"] = null;
                //                var msg = new EmailMessage(user.LoginID, objSendEmail.EmailSubject, objSendEmail.EmailBody, _commonServices.Settings.GetSettingByKey<string>("default_email_from"));
                //                //_emailSender.SendEmail(msg);
                //                this.NotifySuccess(I18N("Password changed successfully."));
                //            }
                //            else
                //            {
                //                this.NotifySuccess(I18N("Password changed successfully. But confirmation mail sending failed."));
                //            }

                //            return RedirectToAction("ResetPasswordConfirmation", "Account");
                //        }
                //    }
                //    ModelState.AddModelError(nameof(model.Email), I18N("Password reset failed."));
                //}
                //else
                //{
                //    ModelState.AddModelError(nameof(model.Email), I18N("User does not exists."));
                //}
            }
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [AuthorizeUser]
        public ActionResult profile()
        {

            return View();
        }

        // GET: /Accounts/LogOut
        /// <summary>
        /// The log out.
        /// </summary>

        [AuthorizeUser]
        public ActionResult LogOut()
        {
            userBL.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public JsonResult GetCompany(string UserName, string Password)
        {
            try
            {
                List<SelectListItem> CompanyList = new List<SelectListItem>();
                List<SelectListItem> BussinessLineList = new List<SelectListItem>();
                string errormsg = null;
                if (ModelState.IsValid)
                {
                    UserInformationModel user;
                    if (userBL.ValidateUser(UserName, Password, out user,out errormsg))
                    {
                        errormsg = null;
                        CompanyList = comboBL.GetUserCompanyMapDropdown(user.LoginID).ToList();

                        BussinessLineList = comboBL.GetUserBussinessLineDropdown(user.LoginID).ToList();
                        //CompanyList.Add(new SelectListItem() { Text = "Suraj", Value = "2" });
                        if (CompanyList.Count() == 0 )
                        {
                            errormsg = "No Company is mapped for this login Credentials.";
                        }
                        if (BussinessLineList.Count() == 0 )
                        {
                            errormsg += " \n No BussinessLine is mapped for this login Credentials.";
                        }
                    }
                }
                return Json(new { CompanyList = CompanyList, BussinessLineList = BussinessLineList, errormsg = errormsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [AuthorizeUser]
        public ActionResult ChangePassword()
        {

            ChangePasswordViewModel model = new ChangePasswordViewModel();
            model.UserID = UserSession.GetUserSession().LoginID;
            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AuthorizeUser]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel data)
        {
            if (ModelState.IsValid)
            {
                AjaxResponse result = new AjaxResponse();
                if (ModelState.IsValid)
                {
                    
                    string message=string.Empty;
                    result.Success = userBL.ChangePassword(data,out message);
                    if (result.Success)
                    {
                        this.AddToastMessage("RELOCBS", message, ToastType.Success);
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        this.AddToastMessage("RELOCBS", message,ToastType.Error);
                        ModelState.AddModelError(string.Empty, message);
                    }
                }

            }

            return View(data);

        }
    }
}
