using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace RELOCBS.Extensions
{
    internal static class HtmlExts
    {
        public static bool containsHtmlTag(this string text, string tag)
        {
            var pattern = @"<\s*" + tag + @"\s*\/?>";
            return Regex.IsMatch(text, pattern, RegexOptions.IgnoreCase);
        }

        public static bool containsHtmlTags(this string text, string tags)
        {
            var ba = tags.Split('|').Select(x => new { tag = x, hastag = text.containsHtmlTag(x) }).Where(x => x.hastag);

            return ba.Count() > 0;
        }

        public static bool containsHtmlTags(this string text)
        {
            return
                text.containsHtmlTags(
                    "a|abbr|acronym|address|area|b|base|bdo|big|blockquote|body|br|button|caption|cite|code|col|colgroup|dd|del|dfn|div|dl|DOCTYPE|dt|em|fieldset|form|h1|h2|h3|h4|h5|h6|head|html|hr|i|img|input|ins|kbd|label|legend|li|link|map|meta|noscript|object|ol|optgroup|option|p|param|pre|q|samp|script|select|small|span|strong|style|sub|sup|table|tbody|td|textarea|tfoot|th|thead|title|tr|tt|ul|var");
        }
    }
}