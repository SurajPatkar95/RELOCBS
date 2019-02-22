using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.AjaxHelper
{
    public class AjaxResponse<TResult>
    {
        public AjaxResponse()
        {

        }
        public AjaxResponse(TResult result)
        {
            this.Result = result;
        }
        public AjaxResponse(bool success)
        {
            this.Success = success;
        }
        public AjaxResponse(ErrorInfo error, bool unAuthorizedRequest = false)
        {
            this.Error = error;
            this.UnAuthorizedRequest = unAuthorizedRequest;
        }

        public bool Success { get; set; }
        public string Message { get; set; }
        public TResult Result { get; set; }
        public ErrorInfo Error { get; set; }
        public bool UnAuthorizedRequest { get; set; }
    }

    public class AjaxResponse: AjaxResponse<object>
    {
        public AjaxResponse()
        {

        }
        public AjaxResponse(object result)
        {
            this.Result = result;
        }
        public AjaxResponse(bool success)
        {
            this.Success = success;
        }
        public AjaxResponse(ErrorInfo error, bool unAuthorizedRequest = false)
        {
            this.Error = error;
            this.UnAuthorizedRequest = unAuthorizedRequest;
        }
    }

    public class ErrorInfo
    {

        public ErrorInfo()
        {

        }
        public ErrorInfo(string message)
        {
            this.Message = Message;
        }
        public ErrorInfo(int code)
        {
            this.Code = code;

        }
        public ErrorInfo(int code, string message)
        {
            this.Code = code;
            this.Message = Message;
        }
        public ErrorInfo(string message, string details)
        {
            this.Message = Message;
            this.Details = Details;
        }
        public ErrorInfo(int code, string message, string details)
        {
            this.Code = code;
            this.Message = Message;
            this.Details = Details;
        }

        public int Code { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}