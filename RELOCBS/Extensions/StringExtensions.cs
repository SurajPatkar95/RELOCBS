using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RELOCBS.Extensions
{
    public static class StringExtensions
    {

        public static String EmptyNull(this String instance)
        {
            return instance ?? String.Empty;
        }

    }
}