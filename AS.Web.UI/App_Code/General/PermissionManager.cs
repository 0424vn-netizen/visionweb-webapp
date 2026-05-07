using AS.Controls.Pages;
using DocumentFormat.OpenXml.Spreadsheet;

/// <summary>
/// Summary description for Permission
/// </summary>
public static class PermissionManager
{ 
    public static bool HasFullTaxView(SecurePage page)
    {
        if ((SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.AS
            || SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.CS)
            && page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_TAX_ID))
        {
            return true;
        }
        return false;
    }

    public static bool HasFullDDANumberView(SecurePage page)
    {
        if (page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA)
               || page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DDA_MS))
        {
            return true;
        }
        return false; 
    }

    public static bool CheckCSViewFullCard(SecurePage page)
    {
        var result = GeneralFuncsLib.CheckCSViewFullCard(page);
        return result;
    }
}