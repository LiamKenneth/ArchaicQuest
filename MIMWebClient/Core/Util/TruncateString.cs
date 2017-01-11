using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIMWebClient.Core.Util
{
    public static class TruncateString
    {
        /// <summary>
        /// truncates string to max length given
        /// </summary>
        /// <param name="maxLength">limit string to this count</param>
        /// <returns></returns>
        public static string Truncate(this string value, int maxLength)
        {
            return string.IsNullOrEmpty(value) ? value : value.Substring(0, Math.Min(value.Length, maxLength));
        }
    }
}