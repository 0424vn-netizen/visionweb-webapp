using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AS.VW.Api.WCF.ServiceInterface
{
    public static class SessionManager
    {

        public static int CurrentClient
        {
            get
            {
                if (HttpContext.Current.Session["CurrentClient"] != null)
                {
                    return (int)HttpContext.Current.Session["CurrentClient"];
                }
                else
                {
                    return 0;
                }

            }
            set
            {
                HttpContext.Current.Session["CurrentClient"] = value;
            }
        }

    }
}