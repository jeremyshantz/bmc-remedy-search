using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Remedy.Search.Query
{
    public static class DateTimeExtensions
    {
        public static string ToRemedyQualStringDate(this DateTime date)
        {
            return date.ToUniversalTime().ToString("s");            
        }
    }
}
