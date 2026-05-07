using AS.Security.WS.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class rm_MCF_DQNextQWebMethod : System.Web.UI.Page
{
    public static int LENGHT_OF_QUEUE
    {
        get
        {
            int len = 2;
            int.TryParse(ConfigurationManager.AppSettings["NextQueue_BufferMerchants"], out len);
            if (len < 2)
                return 2;
            else
                return len;
        }
    }

    #region Web Methods

    /// <summary>
    /// Preload merchant to user's session
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [WebMethod(EnableSession = true)]
    public static string PreloadMerchant(int assignmentID, string reportDate, string orderBy, string applyFilterId)
    {
        string result = string.Empty;

        if (RiskSessionManager.MCF_RiskNextQueue_Cache.Count < LENGHT_OF_QUEUE)
        {
            result = NextQueueService.DoGetNextQueueDetail(false, false, true, assignmentID, reportDate, orderBy, applyFilterId);
        }

        if (result != "end" && result != "error" && RiskSessionManager.MCF_RiskNextQueue_Cache.Count() != LENGHT_OF_QUEUE)
            return PreloadMerchant(assignmentID, reportDate, orderBy, applyFilterId);
        return result;
    }

    [WebMethod(EnableSession = true)]
    public static string CheckDisposition(string Id, string reportDate)
    {
        return RM_MCF_GeneralFuncsLib.CheckDisposition(Id, reportDate);
    }

    [WebMethod(EnableSession = true)]
    public static string UpdateDisposition(string dispositionList, string workStateID, string feWorkStateID, string cycleID, string ParentCycleID, string reportDate,
        string assignmentID, string merchantNumber, string viewCode)
    {
        string url = string.Empty;
        switch (viewCode.ToLower().Trim())
        {
            case "dq":
                url = "risk_MCF\\rm_MCF_DQBarometerReportPopup.aspx\\UpdateDisposition";
                break;
            case "sr":
                url = "risk_MCF\\rm_MCF_DQSecurityReportPopup.aspx\\UpdateDisposition";
                break;
            case "nq":
                url = "risk_MCF\\rm_MCF_DQNextQReportPopup.aspx\\UpdateDisposition";
                break;
        }

        AspxTracking requestTracking = new AspxTracking()
        {
            LogWebServerDts = DateTime.Now,
            LogData1 = url,
            LogData2 = merchantNumber,
            LogData3 = assignmentID,
            LogData4 = viewCode
        };

        string updateDisposition = RM_MCF_GeneralFuncsLib.UpdateDisposition(dispositionList, workStateID, feWorkStateID, cycleID,
            ParentCycleID, reportDate, assignmentID, merchantNumber, viewCode);

        GeneralFuncsLib.WriteRequestLog(requestTracking);

        return updateDisposition;
    }

    [WebMethod(EnableSession = true)]
    public static string GetAssignmentsForDetectionQueue(string assignmentID, string reportDate, string applyFilterId)
    {
        return RM_MCF_GeneralFuncsLib.GetAssignmentsForDetectionQueue(assignmentID, reportDate, applyFilterId);
    }

    #endregion
}