using System;
using System.Web.Services;
using AS.Security.WS.Entities;

using AS.Common.Logger;

public partial class SecurityService
{

    [WebMethod(Description = "Get all menu items by a user.")]
    public SecMenuItemCollection GetMenuItemsByUser(int clientid, string username, int hierarchyid, int languageId, bool manageMode)
    {
        try
        {
            var menu = _SiteMapService.GetMenuItemsByUser(clientid, username, hierarchyid, languageId, manageMode);
            return menu;
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetMenuItemsByUser: \n" + ex.ToString());
            return new SecMenuItemCollection();
        }
    }

    [WebMethod(Description = "Get all menu items by a user.")]
    public SecMenuItemCollection GetMenuItemsByUserSSO(int clientid, string username, int hierarchyid, int ssoLevel, int languageId, bool manageMode)
    {
        try
        {

            return _SiteMapService.GetMenuItemsByUserSSO(clientid, username, hierarchyid, ssoLevel, languageId, manageMode);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetMenuItemsByUserSSO: \n" + ex.ToString());
            return new SecMenuItemCollection();
        }
    }

    [WebMethod(Description = "Get all MS menu items.")]
    public SecMenuItemCollection GetMSMenuItems(int clientId, string hierarchyLevel, string hierarchyCode, int languageId)
    {
        try
        {
            return _SiteMapService.GetMSMenuItems(clientId, hierarchyLevel, hierarchyCode, languageId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetMSMenuItems: \n" + ex.ToString());
            return new SecMenuItemCollection();
        }
    }

    [WebMethod(Description = "Get all menu items by a user.")]
    public SecMenuItemCollection GetMenuItemForCreateRole(int clientID, string username, int systemId, string group)
    {
        try
        {

            return _SiteMapService.GetMenuItemForCreateRole(clientID, username, systemId, group);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetMenuItemForCreateRole: \n" + ex.ToString());
            return new SecMenuItemCollection();
        }
    }

    [WebMethod]
    public void InsertExcludeMenuForUser(int clientID, string username, int menuId, DateTime startDate, DateTime endDate)
    {

        _SiteMapService.InsertExcludeMenuForUser(clientID, username, menuId, startDate, endDate);

    }
    [WebMethod]
    public void DeleteExcludeMenuForUser(int clientID, string username, int menuId)
    {


        _SiteMapService.DeleteExcludeMenuForUser(clientID, username, menuId);

    }
}
