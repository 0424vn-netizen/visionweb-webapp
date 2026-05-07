using System;
using System.Collections.Generic;
using System.Text;


using AS.Security.WS.Entities;
using AS.Security.WS.Data;
using System.Data;
using AS.Common.DBManager;

namespace AS.Security.WS.Business
{
    public class GenericServices
    {
        readonly GenericDao _Generic;
        readonly LogDao _Log;
        readonly AppConfigDao _AppConfig;

        public GenericServices(string connString)
        {
            _Generic = new GenericDao(connString);
            _Log = new LogDao(connString);
            _AppConfig = new AppConfigDao(connString);
        }
        

        public DataTable GetReports(string spName, FilterParameterCollection paramaters)
        {

            return _Generic.GetReports(spName, paramaters);
        }
        public RefTableValueCollection GetRefTableValues(int clientId, string refTableName, string refTableLang, int LanguageID)
        {
            return _Generic.GetRefTableValues(clientId, refTableName, refTableLang, LanguageID);
        }
        public ASThemeCollection GetASThemeByUser(int clientid, string username, int hierarchyid)
        {
            return (_Generic).GetASThemesByUser(clientid, username, hierarchyid);
        }
        public void InsertUserInTheme(int clientID, string userName, int hierarchyId, int themeId)
        {
            _Generic.InsertUserInTheme(clientID, userName, hierarchyId, themeId);
        }
        public void InsertIntruderLog(Intruders item)
        {
            _Log.Insert(item);
        }
        public int InsertASPXTrackingLog(AspxTracking item)
        {
            return _Log.Insert(item);
        }
        public AppConfigCollection GetAppConfigByAppCode(string appCode)
        {
            return _AppConfig.GetAppConfigByAppCode(appCode);
        }



        public void InsertLOG_WebServerSessionStats(string webSiteIPAddress, string webSiteClient, string webSiteType, string webSiteGroup, string webSiteDNS)
        {
            _Generic.InsertLOG_WebServerSessionStats(webSiteIPAddress, webSiteClient, webSiteType, webSiteGroup, webSiteDNS);
        }
        public void UpdateLOG_WebServerSessionStats(string webSiteIPAddress, int webSiteMaxUsers, int webSiteActiveUsers)
        {
            _Generic.UpdateLOG_WebServerSessionStats(webSiteIPAddress, webSiteMaxUsers, webSiteActiveUsers);
        }
        public void DeleteLOG_WebServerSessionStats(string webSiteIPAddress)
        {
            _Generic.DeleteLOG_WebServerSessionStats(webSiteIPAddress);
        }
        public void DeleteLOG_WebServerSessionLog(string webSiteIPAddress, string sessionID)
        {
            _Generic.DeleteLOG_WebServerSessionLog(webSiteIPAddress, sessionID);
        }
        public void InsertLOG_WebServerSessionLog(string webSiteIPAddress, string sessionID, string userIPAddress, string browserType, string httpCookie)
        {
            _Generic.InsertLOG_WebServerSessionLog(webSiteIPAddress, sessionID, userIPAddress, browserType, httpCookie);
        }
        public void UpdateLOG_WebServerSessionLog(string webSiteIPAddress, string sessionID, string userIPAddress, string browserType, string httpCookie)
        {
            _Generic.UpdateLOG_WebServerSessionLog(webSiteIPAddress, sessionID, userIPAddress, browserType, httpCookie);
        }
        public void UpdateLOG_WebServerSessionLogEnd( string sessionID)
        {
            _Generic.UpdateLOG_WebServerSessionLogEnd(sessionID);
        }

    }
}
