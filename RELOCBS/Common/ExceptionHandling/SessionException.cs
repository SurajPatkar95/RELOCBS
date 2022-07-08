using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RELOCBS.Common.ExceptionHandling
{
    public class SessionException : BaseException, ISerializable
    {

        RELOCBS.App_Code.CommonSubs _CSubs;

        public RELOCBS.App_Code.CommonSubs CSubs
        {
            get
            {
                if (this._CSubs == null)
                    this._CSubs = new RELOCBS.App_Code.CommonSubs();
                return this._CSubs;
            }
        }

        public SessionException()
           : base()
        {
            // Add implementation (if required)
        }

        public SessionException(string message)
           : base(message)
        {
            // Add implemenation (if required)
        }

        public SessionException(string LoginID, string ClassName, string Module, string message, System.Exception inner)
           : base(message, inner)
        {
            CSubs.LogError(ClassName, Module, inner.ToString());
        }

        protected SessionException(SerializationInfo info, StreamingContext context)
           : base(info, context)
        {
            //Add implemenation
        }
    }
}