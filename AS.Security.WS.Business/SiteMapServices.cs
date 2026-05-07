using System;
using System.Data;
using System.Collections.Generic;
using System.Text;

using AS.Security.WS.Entities;
using AS.Security.WS.Data;

namespace AS.Security.WS.Business
{
    public class SiteMapServices
    {
        readonly SiteMapDao _siteMapDAO;
        public SiteMapServices(string connString)
        {
            _siteMapDAO = new SiteMapDao(connString);
        }

        public SecMenuItemCollection GetMenuItemForCreateRole(int clientID, string username, int systemId, string group)
        {
            return _siteMapDAO.GetMenuItemForCreateRole(clientID, username, systemId, group);
        }

        public SecMenuItemCollection GetMSMenuItems(int clientid, string hierarchyLevel,
            string hierarchyCode, int languageId)
        {
            return _siteMapDAO.GetMSMenuItems(clientid, hierarchyLevel, hierarchyCode, languageId);
        }

        public SecMenuItemCollection GetMenuItemsByUser(int clientid, string username, int hierarchyid, int languageId, bool manageMode)
        {
            return _siteMapDAO.GetMenuItemsByUser(clientid, username, hierarchyid, languageId, manageMode);
        }

        public SecMenuItemCollection GetMenuItemsByUserSSO(int clientid, string username, int hierarchyid, int ssoLevel, int languageId, bool manageMode)
        {
            return _siteMapDAO.GetMenuItemsByUserSSO(clientid, username, hierarchyid, ssoLevel, languageId, manageMode);
        }

        public void InsertExcludeMenuForUser(int clientID, string username, int menuId, DateTime startDate, DateTime endDate)
        {
            _siteMapDAO.InsertExcludeMenuForUser(clientID, username, menuId, startDate, endDate);

        }
        public void DeleteExcludeMenuForUser(int clientID, string username, int menuId)
        {
            _siteMapDAO.DeleteExcludeMenuForUser(clientID, username, menuId);
        }
    }
}