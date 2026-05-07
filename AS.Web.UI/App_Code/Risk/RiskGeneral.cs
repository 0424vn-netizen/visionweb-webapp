using AS.Common;
using AS.Controls.Pages;
using Resources;
using System.Text.RegularExpressions;
/// <summary>
/// Summary description for RiskGeneral
/// </summary>
public class RiskGeneral
{
    #region Constants

    public const string NBSP = "&nbsp;";
    //public string DEACTIVE_TEXT = HttpContext.GetGlobalResourceObject("LanguageResource","RiskGeneral_Deactive").ToString();
    //public string ACTIVE_TEXT = HttpContext.GetGlobalResourceObject("LanguageResource", "RiskGeneral_Active").ToString();

    // Format string
    public const string MUTED_TEXT_FORMAT = "<i class=\"text-muted\">{0}</i>";
    public const string ACTIVE_DEACTIVE_HYPERLINK_FORMAT =
        "<a href=\"#\" onclick=\"ActivateDeactivate({0}, {1}); return false;\">{2}</a>";
    public const string ACTIVE_DEACTIVE_HYPERLINK_FORMAT_4_PARAMS =
        "<a href=\"#\" onclick=\"ActivateDeactivate({0}, {1}, {2}, '{3}'); return false;\">{4}</a>";

    // CSS
    public const string CSS_ERROR_LABEL = "control-label label-error";
    public const string CSS_GRID_EDIT_ROW = "rgRow editRow";
    public const string CSS_GRID_EDIT_ALT_ROW = "rgAltRow editRow";

    #endregion Constants

    #region Common functions

    public static string GetCssGridEditableItem(int rowIndex)
    {
        return rowIndex % 2 == 0 ? CSS_GRID_EDIT_ROW : CSS_GRID_EDIT_ALT_ROW;
    }

    public static void ShowMessage(ReportPage page, string message)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "showMsg", string.Format("alert('{0}')", message), true);
    }

    public static void ShowMessageAjax(ReportPage page, string message)
    {
        page.AjaxAddResponseScript(string.Format("ShowMsg('{0}');", message));
    }

    public static string BuildActiveDeactiveLink(object objectId, bool isActive)
    {
        return VeraCodeExtensions.DoVeraCode(string.Format(RiskGeneral.ACTIVE_DEACTIVE_HYPERLINK_FORMAT,
            objectId,
            isActive.ToString().ToLower(),
            isActive ? Resources.LanguageResource.RiskGeneral_Deactive : Resources.LanguageResource.RiskGeneral_Active));
    }

    public static string BuildActiveDeactiveLink(object objectId, bool isActive, bool deleteAllowed, string queryString)
    {
        return VeraCodeSolution.DoVeraCode(string.Format(
            RiskGeneral.ACTIVE_DEACTIVE_HYPERLINK_FORMAT_4_PARAMS,
            objectId,
            isActive.ToString().ToLower(),
            deleteAllowed.ToString().ToLower(),
            queryString,
            isActive ? Resources.LanguageResource.RiskGeneral_Deactive : Resources.LanguageResource.RiskGeneral_Active));
    }

    public static bool CheckValidString(string text)
    {
        Regex reg = new Regex(@"^[0-9a-zA-Z\.\\s-]{1,50}$/i");
        return reg.IsMatch(text);
    }

    public static string BuildMerchantHyperlinkInRisk(SecurePage page, object merchantNumberCell,
        bool isPopup, string rskRptIntruderQuery, string text)
    {
        return BuildMerchantHyperlinkInRisk(page, merchantNumberCell, isPopup, rskRptIntruderQuery, text, false);
    }

    public static string BuildMerchantHyperlinkInRisk(SecurePage page, object merchantNumberCell,
        bool isPopup, string rskRptIntruderQuery, string text, bool isMCFRisk)
    {
        string pageUrl = isMCFRisk ? "rm_MCF_RiskReport.aspx?" : "rm_RiskReport.aspx?";
        string url = pageUrl + page.BuildSecureQueryString(
              string.Format("merchantnumber={0}&IsPopup={1}{2}",
                            GeneralFuncsLib.NvlString(merchantNumberCell),
                            isPopup,
                            rskRptIntruderQuery));
        return string.Format(
            "<a class=\"link\" href=\"#\" style=\"cursor:pointer\" onclick=\"openPopupWindow('{0}','RiskReport'); return false;\">{1}</a>",
            url,
            text);
    }

    public static string GetColorOfMerchantName(string activeDayFlag)
    {
        string color = string.Empty;
        switch (activeDayFlag)
        {
            case "1":
                color = "Red";
                break;
            case "2":
                color = "Brown";
                break;
            case "3":
                color = "Blue";
                break;
            case "4":
                color = "Green";
                break;
        }
        return color;
    }

    public static string BuildMerchantNameWithFontTag(string merchantName, string activeDateFlag)
    {
        return VeraCodeSolution.GetOutputHtmlString(string.Format(
            "<font style=\"color:{0};\">{1}</font>",
            GetColorOfMerchantName(activeDateFlag),
            merchantName));
    }

    #endregion Common functions
}

/// <summary>
/// ErrorCode when calling service
/// </summary>
public static class RiskErrorCode
{
    public enum ObjectType
    {
        Escalation,
        Group,
        Resolution,
        Others
    }

    public const int ERROR_PROCESSING_FAILED = 0;
    public const int ERROR_FIELD_REQUIRE_AND_UNIQUE = 2;

    public static string GetErrorMessage(int errorCode, ObjectType type)
    {
        string message = string.Empty;
        switch (errorCode)
        {
            case RiskErrorCode.ERROR_PROCESSING_FAILED:
                message = MessageManager.Generic_ProcessingFailed;
                break;
            case RiskErrorCode.ERROR_FIELD_REQUIRE_AND_UNIQUE:
                switch (type)
                {
                    case RiskErrorCode.ObjectType.Escalation:
                        message = Resources.ValMsg.EscalationStatus_Duplicate;
                        break;
                    case RiskErrorCode.ObjectType.Group:
                        message = Resources.ValMsg.Group_Duplicate;
                        break;
                    case RiskErrorCode.ObjectType.Resolution:
                        message = Resources.ValMsg.Resolution_Duplicate;
                        break;
                    case RiskErrorCode.ObjectType.Others:
                        message = MessageManager.Field_RequireAndUnique;
                        break;
                }
                break;
        }
        return VeraCodeExtensions.DoVeraCode(message);
    }

    public static bool HasError(int errorCode)
    {
        return errorCode == ERROR_FIELD_REQUIRE_AND_UNIQUE
            || errorCode == ERROR_PROCESSING_FAILED;
    }
}

/// <summary>
/// An extensions method implementation for VeraCodeSolution for easier for developers to use.
/// </summary>
public static class VeraCodeExtensions
{
    /// <summary>
    /// VeraCodeSolution - Get a string without have '\r' and '\n'
    /// </summary>
    /// <param name="data">string to truncate cr and lf</param>
    /// <returns></returns>
    public static string GetRemovedCRLFString(this string data)
    {
        return VeraCodeSolution.RemoveCRLF(data);
    }
    /// <summary>
    /// VeraCodeSolution - Get a string which can be represent in client side (HtmlDecode)
    /// respone.write
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string GetOutputString(this string data)
    {
        return VeraCodeSolution.GetOutputHtmlString(data);
    }
    /// <summary>
    /// VeraCodeSolution - Get a string which is encoded for client side(HtmlEncode)
    /// response to display
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string ValidateResponseData(this string data)
    {
        return VeraCodeSolution.ValidateResponseData(data);
    }
    /// <summary>
    /// VeraCodeSolution - Do a checking on data for safer.
    /// browse file system
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string DoVeraCode(this string data)
    {
        return VeraCodeSolution.DoVeraCode(data);
    }
}

public enum ViewMoreModalType
{
    LoadHierarchys = 1,
    LoadStates = 2,
    LoadZips = 4,
    LoadProfiles = 5,
    LoadMarketDatas = 6,
    LoadHighRisk = 8,
    LoadMerchantClassifications = 9,
    LoadLeadSource = 10,
    LoadReferralSource = 11,
    LoadRiskLevel = 12,
    LoadRiskCategory = 13,
    LoadCampaignID = 14,
}

public class OpenClosedCode
{
    public static string OPEN = "Open";
    public static string CLOSED = "Closed";
    public static string ALL = "All";
    public static string OPEN_BETWEEN = "OpenBetween";
    public static string CLOSED_BETWEEN = "ClosedBetween";
}

public class FollowUpCode
{
    public static string ALL = "ALL";
    public static string TODAY = "TODAY";
    public static string YESTERDAY = "YESTERDAY";
    public static string THIS_WEEK = "THISWEEK";
    public static string LAST_WEEK = "LASTWEEK";
    public static string LAST_MONTH = "LASTMONTH";
    public static string NEXT_WEEK = "NEXTWEEK";
    public static string NEXT_MONTH = "NEXTMONTH";
    public static string DATE_RANGE = "DATERANGE";
    public static string PAST_DUE = "PASTDUE";
    public static string SET = "SET";
    public static string NOT_SET = "NOTSET";
}

public class EscalationFilterOption
{
    public static string TNO = "TNO";
    public static string MNO = "MNO";
    public static string MNAME = "MNAME";
    public static string FOLLOWUP = "FOLLOWUP";
    public static string CNAME = "CNAME";
}