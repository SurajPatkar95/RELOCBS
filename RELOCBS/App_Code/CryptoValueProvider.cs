using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;

namespace RELOCBS.App_Code
{
    public class CryptoValueProvider : IValueProvider
    {
        RouteData routeData = null;
        Dictionary<string, string> dictionary = null;

        //public CryptoValueProvider()
        //{ }

        public CryptoValueProvider(RouteData routeData)
        {
            this.routeData = routeData;
        }

        public bool ContainsPrefix(string prefix)
        {
            if (this.routeData.Values["id"] == null)
            {
                return false;
            }

            this.dictionary = Common.CommonService.Decrypt(this.routeData.Values["id"].ToString());

            return this.dictionary.ContainsKey(prefix.ToUpper());
        }

        public ValueProviderResult GetValue(string key)
        {
            ValueProviderResult result = null;
            if (this.dictionary != null)
            {
                result = new ValueProviderResult(this.dictionary[key.ToUpper()],
                this.dictionary[key.ToUpper()], CultureInfo.CurrentCulture);
            }
            else
            {
                string Id = Convert.ToString(HttpContext.Current.Request.Form[key]);
                if (!string.IsNullOrWhiteSpace(Id))
                {
                    result = new ValueProviderResult(Id, Id, CultureInfo.CurrentCulture);
                }
                
            }
            
            return result;
        }
    }

    public class CryptoValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            return new CryptoValueProvider(controllerContext.RouteData);
        }
    }

    public class CryptoValueProviderAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            filterContext.Controller.ValueProvider = new CryptoValueProvider(filterContext.RouteData);
        }
    }

}