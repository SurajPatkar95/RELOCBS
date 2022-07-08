using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RELOCBS.CustomAttributes
{
    public class JsonExceptionFilterAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 500;
                filterContext.ExceptionHandled = true;

                string msg = filterContext.Exception.Message;
                if (filterContext.Exception.GetType() == Type.GetType("System.UnauthorizedAccessException"))
                {
                    msg = "Unauthorized access";
                }

                filterContext.Result = new JsonResult
                {
                    Data = new
                    {
                        errorMessage = msg
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }
    }
}