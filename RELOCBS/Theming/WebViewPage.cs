using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MwxCore;
using MwxCore.Core.Localization;
using MwxCore.Core.Logging;
using RELOCBS.CustomAttributes;
using RELOCBS.App_Start;

namespace RELOCBS.Theming
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        //private IWorkContext _workContext;

        private IList<NotifyEntry> _internalNotifications;

        public string ShortDateFormatJs = "DD/MM/YYYY";
        public string ShortDateTimeFormatJs = "DD/MM/YYYY hh:mm:ss";
        public string ShortDateTimeampmFormatJs = "DD/MM/YYYY hh:mm:ss a";
        public string LongDateFormatJs = "DDDD, MMMM D, YYYY";
        public string LongDateTimeFormatJs = "DDDD, MMMM D, YYYY H:m:s";

        public string ShortDateFormatDatePickerJs = "DD/MM/YYYY";
        public string ShortDateTimeFormatDatePickerJs = "DD/MM/YYYY hh:mm:ss a";
        public string ShortDateTimeFormatNonSSJs = "DD/MM/YYYY hh:mm a";

        public string ShortDateFormat = "dd/MM/yyyy";
        public string ShortDateTimeFormat = "dd/MM/yyyy H:m:s";
        public string ShortDateTimeampmFormat = "dd/MM/yyyy HH:mm:ss tt";
        public string LongDateFormat = "dddd, MMMM d, yyyy";
        public string LongDateTimeFormat = "dddd, MMMM d, yyyy H:m:s";

        //[Microsoft.Practices.Unity.Dependency]
        //public IWorkContext WorkContext
        //{
        //    get
        //    {
        //        return _workContext;
        //    }
        //}

        /// <summary>
        /// Get a localized resource
        /// </summary>
        public string I18N(string key)
        {
            return key;
        }

        public override void InitHelpers()
        {
            base.InitHelpers();

            var container = UnityConfig.Container;
            //_workContext = container.Resolve<IWorkContext>();
        }

        protected bool HasMessages
        {
            get
            {
                return ResolveNotifications(null).Any();
            }
        }

        protected ICollection<LocalizedString> GetMessages(NotifyType type)
        {
            return ResolveNotifications(type).AsReadOnly();
        }

        private IEnumerable<LocalizedString> ResolveNotifications(NotifyType? type)
        {

            IEnumerable<NotifyEntry> result = Enumerable.Empty<NotifyEntry>();

            if (_internalNotifications == null)
            {
                string key = NotifyAttribute.NotificationsKey;
                IList<NotifyEntry> entries;

                if (this.TempData.ContainsKey(key))
                {
                    entries = this.TempData[key] as IList<NotifyEntry>;
                    if (entries != null)
                    {
                        result = result.Concat(entries);
                    }
                }

                if (this.ViewData.ContainsKey(key))
                {
                    entries = this.ViewData[key] as IList<NotifyEntry>;
                    if (entries != null)
                    {
                        result = result.Concat(entries);
                    }
                }

                _internalNotifications = new List<NotifyEntry>(result);
            }

            if (type == null)
            {
                return _internalNotifications.Select(x => x.Message);
            }

            return _internalNotifications.Where(x => x.Type == type.Value).Select(x => x.Message);
        }
    }
}