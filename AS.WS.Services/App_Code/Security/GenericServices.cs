using System;
using System.Data;
using System.Web.Services;
using AS.Security.WS.Entities;

using AS.Common.Logger;
using System.Text;
using System.Security.Cryptography;
using DDS.Data;
using System.IO;

public partial class SecurityService
{    
    [WebMethod]
    public DataTable GetReports(string spName, AS.Common.DBManager.FilterParameterCollection _paramaters)
    {
        
        return _GenericService.GetReports(spName, _paramaters);
    }
    #region VeraCode
    [WebMethod]
    public string EncryptText(string str)
    {        
        var encryptText = DataProtection.EncryptTextWithKeyFile(GetKeyPath(), str);
        return encryptText;
    }
    
    [WebMethod]
    public string DecryptText(string str)
    {
        return DataProtection.DecryptTextWithKeyFile(GetKeyPath(), str);
    }
    [WebMethod]
    public string EncryptTextWithKey(string str, string key)
    {
        try
        {
            if (str == null || str == "") return "";
            var des = new AesCryptoServiceProvider();

            if (!string.IsNullOrEmpty(key) && key.Length < 16)
                key = key.PadRight(16, '0');
            var iv = WSConfiguration.DES_VI;
            if (!string.IsNullOrEmpty(iv) && iv.Length < 16)
                iv = iv.PadRight(16, '0');

            des.Mode = CipherMode.CBC;
            des.Key = Encoding.ASCII.GetBytes(key);
            des.IV = Encoding.ASCII.GetBytes(iv);

            byte[] plainbytes = Encoding.ASCII.GetBytes(str);
            ICryptoTransform transform = des.CreateEncryptor(des.Key, des.IV);
            str = Convert.ToBase64String(transform.TransformFinalBlock(plainbytes, 0, plainbytes.Length));
            return str;
        }
        catch (Exception)
        {
            return "";
        }
    }
    [WebMethod]
    public string DecryptTextWithKey(string str, string key)
    {
        try
        {
            if (str == null || str == "") return "";
            var des = new AesCryptoServiceProvider();

            if (!string.IsNullOrEmpty(key) && key.Length < 16)
                key = key.PadRight(16, '0');
            var iv = WSConfiguration.DES_VI;
            if (!string.IsNullOrEmpty(iv) && iv.Length < 16)
                iv = iv.PadRight(16, '0');

            des.Mode = CipherMode.CBC;
            des.Key = Encoding.ASCII.GetBytes(key);
            des.IV = Encoding.ASCII.GetBytes(iv);

            byte[] cryptedbytes = Convert.FromBase64String(str);

            ICryptoTransform transform = des.CreateDecryptor(des.Key, des.IV);
            str = Encoding.ASCII.GetString(transform.TransformFinalBlock(cryptedbytes, 0, cryptedbytes.Length));
            return str;
        }
        catch
        {
            return "";
        }
    }

    [WebMethod(Description = "Veracode: Generate random number")]
    public int GenNum(int min, int max)
    {
        Random r = new Random();
        return r.Next(min, max);
    }
    [WebMethod(Description = "Veracode: Generate tem password")]
    public string GenPwd()
    {
        string p1 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        string p2 = "0123456789";
        string newpass;
        Random rnd = new Random();
        var builderString = new StringBuilder();
        int rndVal;
        for (int i = 0; i < 5; i++)
        {
            rndVal = rnd.Next();
            builderString.Append(p1[rndVal % p1.Length]);
            rndVal = rnd.Next();
            builderString.Append(p2[rndVal % p2.Length]);
        }
        newpass = builderString.ToString();
        return newpass;
    }
    #endregion
    [WebMethod(Description = "Insert an intruder log")]
    public AppConfigCollection GetAppConfigByAppCode(string appCode)
    {
        try
        {
            return _GenericService.GetAppConfigByAppCode(appCode);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("InsertIntruderLog: \n" + ex.ToString());
            return new AppConfigCollection();
        }
    }
    [WebMethod(Description = "Insert an intruder log")]
    public void InsertIntruderLog(DateTime LogWebServerDts, string LogId1, string LogId2, int LogRecordCount, int LogElapsedTime, int LogMenuId, int LogSubMenuId, string LogSessionId, int LogSessionCnt, int LogLoggingMode, int LogSpecialId, int LogClientId, int LogSystemId, string LogFullName, string LogWebSiteName, string LogData1, string LogData2, string LogData3, string LogData4, string LogData5, string LogData6, string LogData7, string LogData8, string LogData9, string LogData10, string LogTxt1, string LogTxt2, string LogClientIPAddr, string LogHostIPAddr, string LogBrowserType)
    {
        try
        {
            Intruders item = new Intruders();

            item.LogWebServerDts = LogWebServerDts;

            item.LogId1 = LogId1;

            item.LogId2 = LogId2;

            item.LogRecordCount = LogRecordCount;

            item.LogElapsedTime = LogElapsedTime;

            item.LogMenuId = LogMenuId;

            item.LogSubMenuId = LogSubMenuId;

            item.LogSessionId = LogSessionId;

            item.LogSessionCnt = LogSessionCnt;

            item.LogLoggingMode = LogLoggingMode;

            item.LogSpecialId = LogSpecialId;

            item.LogClientId = LogClientId;

            item.LogSystemId = LogSystemId;

            item.LogFullName = LogFullName;

            item.LogWebSiteName = LogWebSiteName;

            item.LogData1 = LogData1;

            item.LogData2 = LogData2;

            item.LogData3 = LogData3;

            item.LogData4 = LogData4;

            item.LogData5 = LogData5;

            item.LogData6 = LogData6;

            item.LogData7 = LogData7;

            item.LogData8 = LogData8;

            item.LogData9 = LogData9;

            item.LogData10 = LogData10;

            item.LogTxt1 = LogTxt1;

            item.LogTxt2 = LogTxt2;

            item.LogClientIPAddr = LogClientIPAddr;

            item.LogHostIPAddr = LogHostIPAddr;

            item.LogBrowserType = LogBrowserType;

            
            _GenericService.InsertIntruderLog(item);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("InsertIntruderLog: \n" + ex.ToString());
        }
    }
    [WebMethod(Description = "Insert an aspx tracking log")]
    public int InsertASPXTrackingLog(DateTime LogWebServerDts, string LogId1, string LogId2, int LogRecordCount, int LogElapsedTime, int LogMenuId, int LogSubMenuId, string LogSessionId, int LogSessionCnt, int LogLoggingMode, int LogSpecialId, int LogClientId, int LogSystemId, string LogFullName, string LogWebSiteName, string LogData1, string LogData2, string LogData3, string LogData4, string LogData5, string LogData6, string LogData7, string LogData8, string LogData9, string LogData10, string LogTxt1, string LogTxt2, string LogClientIPAddr, string LogHostIPAddr, string LogBrowserType)
    {
        int result = 0;
        try
        {
            AspxTracking item = new AspxTracking();

            item.LogWebServerDts = LogWebServerDts;

            item.LogId1 = LogId1;

            item.LogId2 = LogId2;

            item.LogRecordCount = LogRecordCount;

            item.LogElapsedTime = LogElapsedTime;

            item.LogMenuId = LogMenuId;

            item.LogSubMenuId = LogSubMenuId;

            item.LogSessionId = LogSessionId;

            item.LogSessionCnt = LogSessionCnt;

            item.LogLoggingMode = LogLoggingMode;

            item.LogSpecialId = LogSpecialId;

            item.LogClientId = LogClientId;

            item.LogSystemId = LogSystemId;

            item.LogFullName = LogFullName;

            item.LogWebSiteName = LogWebSiteName;

            item.LogData1 = LogData1;

            item.LogData2 = LogData2;

            item.LogData3 = LogData3;

            item.LogData4 = LogData4;

            item.LogData5 = LogData5;

            item.LogData6 = LogData6;

            item.LogData7 = LogData7;

            item.LogData8 = LogData8;

            item.LogData9 = LogData9;

            item.LogData10 = LogData10;

            item.LogTxt1 = LogTxt1;

            item.LogTxt2 = LogTxt2;

            item.LogClientIPAddr = LogClientIPAddr;

            item.LogHostIPAddr = LogHostIPAddr;

            item.LogBrowserType = LogBrowserType;


            return _GenericService.InsertASPXTrackingLog(item);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("InsertASPXTrackingLog: \n" + ex.ToString());
        }
        return result;
    }


    [WebMethod(Description = "Get all definition of reference values of a client.")]
    public RefTableValueCollection GetRefTableValues(int clientId, string refTableName, string refTableLang, int LanguageID)
    {
        try
        {
            
            return _GenericService.GetRefTableValues(clientId, refTableName, refTableLang, LanguageID);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetRefTableValues: \n" + ex.ToString());
            return new RefTableValueCollection();
        }        
    }
    [WebMethod(Description = "Get all AS themes by username")]
    public ASThemeCollection GetASThemesByUser(int clientid, string username, int hierarchyid)
    {
        try
        {
            
            return _GenericService.GetASThemeByUser(clientid, username, hierarchyid);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("GetASThemesByUser: \n" + ex.ToString());
            return new ASThemeCollection();
        }
    }
    [WebMethod(Description = "Set a theme to a user")]
    public void AddThemeForUser(int clientID, string userName, int hierarchyId, int themeId)
    {
        try
        {
            
            _GenericService.InsertUserInTheme(clientID, userName, hierarchyId, themeId);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("AddThemeForUser: \n" + ex.ToString());
        }
    }
    [WebMethod(Description = "")]
    public void InsertLOG_WebServerSessionStats(string webSiteIPAddress, string webSiteClient, string webSiteType, string webSiteGroup, string webSiteDNS)
    {
        try
        {
            
            _GenericService.InsertLOG_WebServerSessionStats(webSiteIPAddress, webSiteClient, webSiteType, webSiteGroup, webSiteDNS);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("InsertLOG_WebServerSessionStats: \n" + ex.ToString());
        }
    }
    [WebMethod(Description = "")]
    public void UpdateLOG_WebServerSessionStats(string webSiteIPAddress, int webSiteMaxUsers, int webSiteActiveUsers)
    {
        try
        {
            
            _GenericService.UpdateLOG_WebServerSessionStats(webSiteIPAddress, webSiteMaxUsers, webSiteActiveUsers);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("UpdateLOG_WebServerSessionStats: \n" + ex.ToString());
        }
    }
    [WebMethod(Description = "")]
    public void DeleteLOG_WebServerSessionStats(string webSiteIPAddress)
    {
        try
        {
            
            _GenericService.DeleteLOG_WebServerSessionStats(webSiteIPAddress);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("DeleteLOG_WebServerSessionStats: \n" + ex.ToString());
        }
    }
    [WebMethod(Description = "")]
    public void DeleteLOG_WebServerSessionLog(string webSiteIPAddress, string sessionID)
    {
        try
        {
            
            _GenericService.DeleteLOG_WebServerSessionLog(webSiteIPAddress, sessionID);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("DeleteLOG_WebServerSessionLog: \n" + ex.ToString());
        }
    }
    [WebMethod(Description = "")]
    public void InsertLOG_WebServerSessionLog(string webSiteIPAddress, string sessionID, string userIPAddress, string browserType, string httpCookie)
    {
        try
        {
            
            _GenericService.InsertLOG_WebServerSessionLog(webSiteIPAddress, sessionID, userIPAddress, browserType, httpCookie);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("InsertLOG_WebServerSessionLog: \n" + ex.ToString());
        }
    }
    [WebMethod(Description = "")]
    public void UpdateLOG_WebServerSessionLog(string webSiteIPAddress, string sessionID, string userIPAddress, string browserType, string httpCookie)
    {
        try
        {
            
            _GenericService.UpdateLOG_WebServerSessionLog(webSiteIPAddress, sessionID, userIPAddress, browserType, httpCookie);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("UpdateLOG_WebServerSessionLog: \n" + ex.ToString());
        }
    }
    [WebMethod(Description = "")]
    public void UpdateLOG_WebServerSessionLogEnd(string sessionID)
    {
        try
        {
            
            _GenericService.UpdateLOG_WebServerSessionLogEnd(sessionID);
        }
        catch (Exception ex)
        {
            LoggerManager.Error("UpdateLOG_WebServerSessionLogEnd: \n" + ex.ToString());
        }
    }
    private string GetKeyPath()
    {
        return GeneralFuncsLib.GetKeyPathForClient(AsClientId, Path.Combine(Server.MapPath("~/App_Data/"), "KeyConfig.xml")); 
    }
}
