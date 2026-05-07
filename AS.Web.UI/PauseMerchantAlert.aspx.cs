using AS.Web.Business.PauseMerchantAlert.Impl;
using AS.Web.Business.PauseMerchantAlert.Interfaces;
using AS.Web.Business.PauseMerchantAlert.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class PauseMerchantAlert : ReportPage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        this.IsBindDataOnLoad = true;
    }

    [WebMethod(EnableSession = true)]
    public static PauseMerchantAlertResponse GetMerchants(string customfilterstring = null)
    {
        var _pauseMerchantAlertBusiness = new PauseMerchantAlertBusiness(WebServices.RiskServices);
        var data = _pauseMerchantAlertBusiness.GetMerchantFilters(GeneralFuncsLib.GetUserMode(), SessionManager.CurrentUser, customfilterstring);
        return data;
    }
}