using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WLC.Services
{
    public class Globals
    {
        public static int GetActiveYear(HttpContext httpContext)
        {
            if (!httpContext.Session.GetInt32("ActiveYear").HasValue)
                httpContext.Session.SetInt32("ActiveYear", DateTime.Now.Year);

            Int32 activeYear = httpContext.Session.GetInt32("ActiveYear").GetValueOrDefault();

            return activeYear;

        }

        public static void SetActiveYear(HttpContext httpContext, Int32 activeYear)
        {
            httpContext.Session.SetInt32("ActiveYear",activeYear);

        }
    }
}
