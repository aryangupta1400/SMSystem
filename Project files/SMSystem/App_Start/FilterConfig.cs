﻿using System.Web;
using System.Web.Mvc;

namespace SMSystem
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        // add the session and custom error page here.

    }
}
