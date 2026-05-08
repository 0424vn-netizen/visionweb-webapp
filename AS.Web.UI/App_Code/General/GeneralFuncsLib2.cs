using AS.Common;
using AS.Common.DBManager;
using AS.Common.WebUI;
using AS.Controls.Grid;
using AS.Controls.Pages;
using AS.Core.Common.Utilities;
using AS.Security.WS.Entities;
using AS.Web.Business.Shared.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Serialization;
using Telerik.Web.UI;

/// <summary>
/// Summary description for GeneralFuncsLib2
/// </summary>
public static partial class GeneralFuncsLib
{
    public const string RISK_OLD_URL_RULE = "/Risk/rm_";
    public const string RISK_MCF_URL_RULE = "/risk_MCF/rm_MCF_";
    public static int DataShare_MaximumFileSize = Convert.ToInt16(ConfigurationManager.AppSettings["DataShare_MaximumFileSize"] ?? "50");
    public static int DataShare_MaximumFiles = Convert.ToInt16(ConfigurationManager.AppSettings["DataShare_MaximumFiles"] ?? "5");
    public static string DataShare_AllowFileType = Convert.ToString(ConfigurationManager.AppSettings["DataShare_AllowFileType"] ?? "bmp,doc,docx,gif,jpeg,jpg,msg,pdf,png,rtf,tif,txt,xls,xlsx,csv,zip,7zip,rar");

    #region Client Extended Settings
    /// <summary>
    /// Get extended settings list by client
    /// </summary>
    /// <param name="xmlFilePath"></param>
    /// <returns></returns>
    public static DataTable ExtendedClientSettings
    {
        get
        {
            string fileName = "~/App_Data/ClientExtendedSettings.xml";
            if (!string.IsNullOrEmpty(SessionManager.ClientSettingFilePostfix))
            {
                fileName = "~/App_Data/ClientExtendedSettings" + SessionManager.ClientSettingFilePostfix + ".xml";
            }

            DataTable extendedSettings = GetDataByXML(HttpContext.Current.Server.MapPath("~/App_Data/ClientExtendedSettings.xml"));
            var extendedSettingsListByClient = extendedSettings.Rows.Cast<DataRow>().Where(row => row["clientId"].ToString().Equals(SessionManager.CurrentClient.ToString()));
            if (extendedSettingsListByClient.Count() > 0)
            {
                return ConvertDataAccordingCurrentLanguage(extendedSettingsListByClient.CopyToDataTable());
            }
            return null;
        }
    }



    public static bool IsEnhancementFeature()
    {
        var enhancement = GeneralFuncsLib.GetDataOfExtendedSetting("HasEnhancementFeatures");
        bool isEnhancement = !string.IsNullOrEmpty(enhancement) && Convert.ToBoolean(enhancement);
        return isEnhancement;
    }


    public static bool IsAlertNotification()
    {
        var alertNotification = GeneralFuncsLib.GetDataOfExtendedSetting("HasAlertNotification");
        bool isNotification = !string.IsNullOrEmpty(alertNotification) && Convert.ToBoolean(alertNotification);
        return isNotification;
    }

    private static DataTable ConvertDataAccordingCurrentLanguage(DataTable table)
    {
        if (table != null)
        {
            foreach (DataRow row in table.Rows)
            {
                if (table.Columns.Contains("reskey") && !string.IsNullOrEmpty(row["reskey"].ToString()))
                {
                    ResourceSet resources = Resources.Template.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
                    row["data"] = resources.GetObject(row["reskey"].ToString());
                }
            }
        }
        return table;
    }

    /// <summary>
    /// Check has excluded functions list by client?
    /// </summary>
    /// <param name="xmlFilePath"></param>
    /// <returns></returns>
    public static bool HasExtendedSetting(string settingName)
    {
        if (ExtendedClientSettings.HasData())
        {
            DataRow extendedSetting = ExtendedClientSettings.Rows.Cast<DataRow>().SingleOrDefault(row => row["settingName"].ToString().ToLower().Equals(settingName.ToLower()));
            if (extendedSetting != null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Get Data of excluded functions list by client?
    /// </summary>
    /// <param name="xmlFilePath"></param>
    /// <returns></returns>
    public static string GetDataOfExtendedSetting(string settingName)
    {
        if (ExtendedClientSettings.HasData())
        {
            DataRow extendedSetting = ExtendedClientSettings.Rows.Cast<DataRow>().SingleOrDefault(row => row["settingName"].ToString().ToLower().Equals(settingName.ToLower()));

            if (extendedSetting != null)
            {
                return extendedSetting["data"].ToString();
            }
        }
        return string.Empty;
    }

    public static string GetDataOfExtendedSetting(string settingName, string clientID)
    {
        DataTable td = new DataTable();

        string fileName = "~/App_Data/ClientExtendedSettings.xml";
        if (!string.IsNullOrEmpty(SessionManager.ClientSettingFilePostfix))
        {
            fileName = "~/App_Data/ClientExtendedSettings" + SessionManager.ClientSettingFilePostfix + ".xml";
        }

        DataTable extendedSettings = GetDataByXML(HttpContext.Current.Server.MapPath("~/App_Data/ClientExtendedSettings.xml"));
        var extendedSettingsListByClient = extendedSettings.Rows.Cast<DataRow>().Where(row => row["clientId"].ToString().Equals(clientID));
        if (extendedSettingsListByClient.Count() > 0)
        {
            td = extendedSettingsListByClient.CopyToDataTable();
        }

        if (td.HasData())
        {
            DataRow extendedSetting = td.Rows.Cast<DataRow>().SingleOrDefault(row => row["settingName"].ToString().ToLower().Equals(settingName.ToLower()));

            if (extendedSetting != null)
            {
                return extendedSetting["data"].ToString();
            }
        }
        return string.Empty;
    }



    public static ClientExtendedSetting GetClientExtendedSetting(string settingName)
    {
        ClientExtendedSetting clientExtendedSetting = new ClientExtendedSetting();
        if (ExtendedClientSettings.HasData())
        {
            DataRow extendedSetting = ExtendedClientSettings.Rows.Cast<DataRow>().SingleOrDefault(row => row["settingName"].ToString().ToLower().Equals(settingName.ToLower()));
            if (extendedSetting != null)
            {
                clientExtendedSetting.SettingName = settingName;
                clientExtendedSetting.ClientId = extendedSetting["clientId"].ToString().Trim();
                clientExtendedSetting.Data = extendedSetting["data"].ToString().Trim();
            }
        }
        return clientExtendedSetting;
    }

    public static DataTable GetDataByXML(string xmlFilePath)
    {
        //DataSet serversList = new DataSet();
        //serversList.ReadXml(xmlFilePath);
        //return serversList.Tables[0];

        DataTable data = new DataTable();
        if (System.IO.File.Exists(xmlFilePath))
        {
            if (HttpRuntime.Cache[xmlFilePath] == null)
            {
                DataSet serversList = new DataSet();
                serversList.ReadXml(xmlFilePath);
                HttpRuntime.Cache.Add(xmlFilePath, serversList.Tables[0], new CacheDependency(xmlFilePath), Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Default, null);
            }
            data = HttpRuntime.Cache[xmlFilePath] as DataTable;
        }

        return data;
    }

    public static bool HasData(this DataTable list)
    {
        if (list != null && list.Rows.Count > 0)
            return true;
        return false;
    }

    /// <summary>
    /// Convert XML UTF8 string to object
    /// </summary>
    /// <param name="xml"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    /// @Author: HungDinh
    public static object ConvertXMLToObject(string xml, Type type)
    {
        XmlSerializer ser = new XmlSerializer(type);
        using (MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xml)))
        {
            object obj = ser.Deserialize(memStream);
            return obj;
        }
    }

    /// <summary>
    /// Convert object to XML UTF8 string 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    /// @Author:HungDinh
    public static string ConvertObjectToXML(object obj, Type type)
    {
        XmlSerializer ser = new XmlSerializer(type);
        using (MemoryStream memStream = new MemoryStream())
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = false;
            settings.NewLineChars = string.Empty;
            settings.ConformanceLevel = ConformanceLevel.Document;
            using (XmlWriter xmlWriter = XmlTextWriter.Create(memStream, settings))
            {
                ser.Serialize(xmlWriter, obj);
                return Encoding.UTF8.GetString(memStream.ToArray());
            }
        }
    }

    public static void SyncUserWithPCIForLockAccount(int ASClient, string userId, bool isActivate)
    {

        FilterParameterCollection parameterIn = new FilterParameterCollection();

        parameterIn.Add(new FilterParameter("@ASClient", ASClient, System.Data.DbType.Int32));
        parameterIn.Add(new FilterParameter("@UserID", userId, System.Data.DbType.String));
        parameterIn.Add(new FilterParameter("@isActive", isActivate, System.Data.DbType.Boolean));
        var _outParam = new FilterParameterCollection();
        PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_UpdateOptInOut", parameterIn, out _outParam);


    }

    # region PCI Process
    static int SYSTEM_CS = 1;
    public static int GetPCIRole(string username)
    {
        int hierachyInPCI = 0;
        FilterParameterCollection paras = new FilterParameterCollection();
        if (GeneralFuncsLib.GetDataOfExtendedSetting("AllowChainAccessToPCIWithOldestMerchant") == "true" && SessionManager.CurrentUser.EntityType == 11)
        {
            FilterParameterCollection parameters = new FilterParameterCollection();
            parameters.Add(new FilterParameter("@ASclient", SessionManager.CurrentClient, DbType.Int32));
            parameters.Add(new FilterParameter("@UserName", username, DbType.String));
            parameters.Add(new FilterParameter("@SystemId", 2, DbType.Int32));
            DataTable hierarchyUser = WebServices.SecurityServices.GetReports("spa_SEC_GetHierarchysForUser", parameters);
            int hierarchyIDUser = 0;
            if (hierarchyUser != null && hierarchyUser.Rows.Count > 0)
            {
                int.TryParse(hierarchyUser.Rows[0]["HierarchyId"].ToString(), out hierarchyIDUser);
            }
            else
            {
                hierarchyIDUser = SessionManager.CurrentHierarchyId;
            }
            paras.Add(new FilterParameter("@HierarchyID", hierarchyIDUser, DbType.Int32));
        }
        else
        {
            paras.Add(new FilterParameter("@HierarchyID", SessionManager.CurrentHierarchyId, DbType.Int32));
        }
        paras.Add(new FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int32));
        DataTable hierarchyList = WebServices.SecurityServices.GetReports("spa_SEC_GetHierarchyInPCIById", paras);
        if (hierarchyList.Rows.Count > 0)
        {
            int.TryParse(hierarchyList.Rows[0]["HierarchyID_PCI"].ToString(), out hierachyInPCI);
        }
        else if (SessionManager.CurrentSystem == SYSTEM_CS) // CS Users    
        {
            // When a role has not updated. That role does not existed in SEC_Hierarchy_HierarchyInPCI --> We will use default role
            hierachyInPCI = int.Parse(GeneralFuncsLib.GetDataOfExtendedSetting("PCI_DEFAULT_HIERARCHY_ID_CS"));
        }
        else if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Hierarchy && SessionManager.CurrentUser.EntityType != 11)
        {
            // When user is hierarchy secondary user We will use default role
            //hierachyInPCI = int.Parse(GeneralFuncsLib.GetDataOfExtendedSetting("PCI_DEFAULT_HIERARCHY_ID_HU_SEC"));

            bool isHierarchySecondary = SessionManager.CurrentUser.UserSecRole.Contains("SEC");

            FilterParameterCollection param = new FilterParameterCollection();
            param.Add(new FilterParameter("@ASclient", SessionManager.CurrentClient, DbType.Int32));
            param.Add(new FilterParameter("@IsPrimaryUser", !isHierarchySecondary, DbType.Boolean));

            DataTable dt = PciWebServices.PciReportServices.GetReports("spa_SEC_GetHierarchyIDFromHierarchyUser", param);
            if (dt.Rows.Count > 0)
            {
                hierachyInPCI = int.Parse(dt.Rows[0]["HierarchyID"].ToString());
            }
        }
        return hierachyInPCI;
    }
    public static int GetThemeOfPCIUser(string username, int hierarchyId)
    {
        int themeId = 0;
        FilterParameterCollection parameters = new FilterParameterCollection();

        parameters.Add("@ASClient", SessionManager.CurrentClient, DbType.Int32);
        parameters.Add("@UserName", username, DbType.String);
        parameters.Add("@HierarchyId", hierarchyId, DbType.Int32);
        DataTable dt = PciWebServices.PciReportServices.GetReports("spa_SEC_GetThemeOfUser", parameters);
        if (dt.Rows.Count > 0)
        {
            themeId = int.Parse(dt.Rows[0]["ThemeID"].ToString());
        }
        return themeId;
    }

    /// <summary>
    /// Create PCI user if not exist
    /// </summary>
    /// <param name="user">User demographic from VS</param>
    /// <param name="newPassword">password</param>
    public static void SyncUserWithPCI(User user, string newPassword)
    {
        int hierachyInPCI = 0;
        bool isHierarchyPCIAccess = GeneralFuncsLib.GetDataOfExtendedSetting("PCI_HIERARCHY_ACCESS").Equals("true") ? true : false;

        //user.UserSecRole
        // FIS: Synch CS & Hierarchy Users
        // The others: Only synch CS Users
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.Add(new FilterParameter("@HierarchyID", Convert.ToInt32(SessionManager.CurrentUserRoles[0].HierarchyID), DbType.Int32));
        paras.Add(new FilterParameter("@ASClientID", SessionManager.CurrentClient, DbType.Int32));

        DataTable hierarchyList = WebServices.SecurityServices.GetReports("spa_SEC_GetHierarchyInPCIById", paras);
        if (hierarchyList.Rows.Count > 0)
        {
            int.TryParse(hierarchyList.Rows[0]["HierarchyID_PCI"].ToString(), out hierachyInPCI);
        }
        else if (SessionManager.CurrentSystem == SYSTEM_CS) // CS Users           
        {
            // Old roles that does not exitsted in SEC_Hierarchy_HierarchyInPCI
            // Because we have not updated that roles yet.
            hierachyInPCI = int.Parse(GeneralFuncsLib.GetDataOfExtendedSetting("PCI_DEFAULT_HIERARCHY_ID_CS"));
        }
        else // MS Users
        {
            if (isHierarchyPCIAccess && SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Hierarchy)
            {
                //hierachyInPCI = int.Parse(GeneralFuncsLib.GetDataOfExtendedSetting("PCI_DEFAULT_HIERARCHY_ID_HU_SEC"));

                bool isHierarchySecondary = SessionManager.CurrentUser.UserSecRole.Contains("SEC");

                FilterParameterCollection param = new FilterParameterCollection();
                param.Add(new FilterParameter("@ASclient", SessionManager.CurrentClient, DbType.Int32));
                param.Add(new FilterParameter("@IsPrimaryUser", !isHierarchySecondary, DbType.Boolean));

                DataTable dt = PciWebServices.PciReportServices.GetReports("spa_SEC_GetHierarchyIDFromHierarchyUser", param);
                if (dt.Rows.Count > 0)
                {
                    hierachyInPCI = int.Parse(dt.Rows[0]["HierarchyID"].ToString());
                }
            }
            else
            {
                return;
            }
        }

        var username = user.OriginalUserID;
        if (GeneralFuncsLib.GetDataOfExtendedSetting("PCI_Site_Jump_MappingPrimaryUser") == "true" && GeneralFuncsLib.GetEntityTypeMapping())
        {
            FilterParameterCollection _param = new FilterParameterCollection();
            _param.Add(new FilterParameter("@ASClient", SessionManager.CurrentUser.ASClient, DbType.Int32));
            _param.Add(new FilterParameter("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString));
            DataTable dtNewUserId = WebServices.SecurityServices.GetReports("spa_cs_GetSpecial_OriginalUserID ", _param);
            if (dtNewUserId != null && dtNewUserId.Rows.Count > 0)
            {
                username = dtNewUserId.Rows[0]["OriginalUserID"].ToString();
            }
        }

        FilterParameterCollection parameterIn = new FilterParameterCollection();
        FilterParameterCollection parameterOut = new FilterParameterCollection();
        parameterIn = new FilterParameterCollection();
        parameterOut = new FilterParameterCollection();
        parameterIn.Add(new FilterParameter("@ASClient", user.ASClient, System.Data.DbType.Int32));
        parameterIn.Add(new FilterParameter("@UserName", username, System.Data.DbType.String));
        DataTable dtCheckUser = PciWebServices.PciReportServices.GetReports("spa_SEC_CheckUserName", parameterIn);

        string actvStatus = "1";
        if (dtCheckUser.Rows.Count == 0)
        {
            user.RecId = Guid.Empty;
            //if (IsInPCIAccessPermission)
            //{
            //Do create user
            parameterIn.Clear();
            parameterOut.Clear();
            parameterIn.Add(new FilterParameter("@UserId", Guid.Empty, DbType.Guid));
            parameterIn.Add(new FilterParameter("@ASClient", user.ASClient, System.Data.DbType.Int32));
            parameterIn.Add(new FilterParameter("@UserName", username, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@UserNameFirst", user.UserNameFirst, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@UserNameLast", user.UserNameLast, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@UserNameFull", user.UserNameFull, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@UserPassword", newPassword, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@UserPasswordType", user.UserPasswordType, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@Email", null, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@LoginQuestionIndex", user.LoginQuestionIndex, System.Data.DbType.Int32));
            parameterIn.Add(new FilterParameter("@LoginQuestionAnswer", user.LoginQuestionAnswer, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@ActiveStatus", actvStatus, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@HierarchyIds", hierachyInPCI.ToString(), System.Data.DbType.AnsiString));
            parameterIn.Add(new FilterParameter("@CreatedBy", user.CreatedBy, System.Data.DbType.Guid));

            //parameterOut.Add(new FilterParameter("@UserId", Guid.Empty, DbType.Guid));
            parameterIn[0].IsOutParameter = true;
            PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_CreateDDSUser", parameterIn, out parameterOut);
            if (parameterOut.Count > 0)
                user.RecId = new Guid(parameterOut[0].ParameterValue.ToString());

            //}
        }
        else
        {
            user.RecId = new Guid(dtCheckUser.Rows[0]["RecId"].ToString());
            //Do update user info
            parameterIn.Clear();
            parameterOut.Clear();
            parameterIn.Add(new FilterParameter("@RecId", user.RecId, System.Data.DbType.Guid));
            parameterIn.Add(new FilterParameter("@UserName", user.OriginalUserID, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@ASClient", user.ASClient, System.Data.DbType.Int32));
            parameterIn.Add(new FilterParameter("@SystemID", 2, System.Data.DbType.Int32));
            parameterIn.Add(new FilterParameter("@UserNameFirst", user.UserNameFirst, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@UserNameLast", user.UserNameLast, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@UserNameFull", user.UserNameFull, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@UserPasswordType", user.UserPasswordType, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@Email", null, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@LoginQuestionIndex", user.LoginQuestionIndex, System.Data.DbType.Int32));
            parameterIn.Add(new FilterParameter("@LoginQuestionAnswer", user.LoginQuestionAnswer, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@ActvStatus", actvStatus, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@HierarchyIds", hierachyInPCI.ToString(), System.Data.DbType.AnsiString));

            PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_UpdateDDSUser", parameterIn, out parameterOut);
        }

        if (user.RecId != Guid.Empty)
        {
            //Do update user
            //Do update sec roles
            parameterIn.Clear();
            parameterOut.Clear();
            parameterIn.Add(new FilterParameter("@ASClient", user.ASClient, System.Data.DbType.Int32));
            parameterIn.Add(new FilterParameter("@UserID", user.RecId, System.Data.DbType.Guid));
            parameterIn.Add(new FilterParameter("@RoleId", hierachyInPCI, System.Data.DbType.Int32));
            PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_UpdSecRoleByUserID", parameterIn, out parameterOut);

            //Do update theme          
            int themeID = GeneralFuncsLib.GetThemeOfPCIUser(SessionManager.CurrentUser.UserID, SessionManager.CurrentUserRoles[0].HierarchyID);
            parameterIn.Clear();
            parameterOut.Clear();
            parameterIn.Add(new FilterParameter("@ASClient", user.ASClient, System.Data.DbType.Int32));
            parameterIn.Add(new FilterParameter("@UserName", username, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@HierarchyId", null, System.Data.DbType.Int32));
            parameterIn.Add(new FilterParameter("@ThemeId", themeID, System.Data.DbType.Int32));
            PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_InsUserInTheme", parameterIn, out parameterOut);

            ////Delete call position from exclude list
            parameterIn.Clear();
            parameterOut.Clear();
            parameterIn.Add(new FilterParameter("@ClientId", user.ASClient, System.Data.DbType.Int32));
            parameterIn.Add(new FilterParameter("@UserName", username, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@PermissionId", 33, System.Data.DbType.Int32)); //CallDisposition
            PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_DelExcludePermissionItem", parameterIn, out parameterOut);

            parameterIn.Clear();
            parameterOut.Clear();
            parameterIn.Add(new FilterParameter("@ClientId", user.ASClient, System.Data.DbType.Int32));
            parameterIn.Add(new FilterParameter("@UserName", username, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@PermissionId", 37, System.Data.DbType.Int32)); //AdminCallDisposition
            PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_DelExcludePermissionItem", parameterIn, out parameterOut);
        }
    }



    public static void SynchUserToPCI(DataTable dtUser, Guid userid, int asclient, string username)
    {
        bool isSynchToPCI = GeneralFuncsLib.GetDataOfExtendedSetting("PCI_USER_SYNCH").Equals("true") ? true : false;
        //bool isHierarchyPCIAccess = GeneralFuncsLib.GetDataOfExtendedSetting("PCI_HIERARCHY_ACCESS").Equals("true") ? true : false;
        //isHierarchyPCIAccess = isHierarchyPCIAccess && SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Hierarchy;

        if (isSynchToPCI)
        {
            int hierachyInPCI = GeneralFuncsLib.GetPCIRole(username);
            if (hierachyInPCI <= 0) return;

            FilterParameterCollection parameterIn = new FilterParameterCollection();
            FilterParameterCollection parameterOut = new FilterParameterCollection();
            parameterIn.Add(new FilterParameter("@RecId", userid, System.Data.DbType.Guid));
            parameterIn.Add(new FilterParameter("@UserName", username, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@ASClient", asclient, System.Data.DbType.Int32));
            parameterIn.Add(new FilterParameter("@SystemID", 2, System.Data.DbType.Int32));

            parameterIn.Add(new FilterParameter("@UserNameFirst", dtUser.Rows[0]["UserNameFirst"].ToSafeString(), System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@UserNameLast", dtUser.Rows[0]["UserNameLast"].ToSafeString(), System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@UserNameFull", dtUser.Rows[0]["UserNameFull"].ToSafeString(), System.Data.DbType.String));

            if (dtUser.Rows[0]["UserPasswordType"] != DBNull.Value)
                parameterIn.Add(new FilterParameter("@UserPasswordType", dtUser.Rows[0]["UserPasswordType"], System.Data.DbType.String));
            if (dtUser.Rows[0]["LoginQuestionIndex"] != DBNull.Value)
                parameterIn.Add(new FilterParameter("@LoginQuestionIndex", dtUser.Rows[0]["LoginQuestionIndex"], System.Data.DbType.Int32));
            if (dtUser.Rows[0]["LoginQuestionAnswer"] != DBNull.Value)
                parameterIn.Add(new FilterParameter("@LoginQuestionAnswer", dtUser.Rows[0]["LoginQuestionAnswer"], System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@Email", null, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@ActvStatus", null, System.Data.DbType.String));
            parameterIn.Add(new FilterParameter("@HierarchyIds", hierachyInPCI, System.Data.DbType.AnsiString));
            PciWebServices.PciReportServices.ExecuteNonQueryCommand("spa_SEC_UpdateDDSUser", parameterIn, out parameterOut);

        }
    }
    public static string GeneratePassword()
    {
        return AS.Common.DataProtection.Cryptophy.SHA1(DateTime.Now.Ticks.ToString());
    }
    public static System.Data.DataTable BuildUserDataTableForSynch(AS.VW.PCI.Api.Client.Models.Responses.GetUsersResponse user)
    {
        System.Data.DataTable dt = new System.Data.DataTable();
        dt.Columns.Add("UserNameFirst");
        dt.Columns.Add("UserNameLast");
        dt.Columns.Add("UserNameFull");
        dt.Columns.Add("UserPasswordType");
        dt.Columns.Add("LoginQuestionIndex");
        dt.Columns.Add("LoginQuestionAnswer");
        System.Data.DataRow row = dt.NewRow();
        row["UserNameFirst"] = (object)user.FirstName ?? System.DBNull.Value;
        row["UserNameLast"] = (object)user.LastName ?? System.DBNull.Value;
        row["UserNameFull"] = (object)user.FullName ?? System.DBNull.Value;
        row["UserPasswordType"] = (object)user.PasswordType ?? System.DBNull.Value;
        row["LoginQuestionIndex"] = System.DBNull.Value;
        row["LoginQuestionAnswer"] = System.DBNull.Value;
        dt.Rows.Add(row);
        return dt;
    }
    #endregion

    public static void TransferFileToClient(this Page page, byte[] buffer, string fileName)
    {
        page.Response.ClearContent();
        try
        {
            page.Response.ClearHeaders();
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Error("ClearHeaders: GeneranlFuncsLib2 - TransferFileToClient:\n" + ex.ToString());
        }
        page.Response.ContentType = "application/octet-stream";
        page.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);
        page.Response.BinaryWrite(buffer);
        page.Response.End();
    }
    public static void TransferFileToClient(this Page page, Stream fileStream, string fileName)
    {
        const int bufferSize = 64 * 1024;

        page.Response.ClearContent();
        try
        {
            page.Response.ClearHeaders();
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Error("ClearHeaders: TransferFileToClient:\n" + ex);
        }

        page.Response.ContentType = "application/octet-stream";
        page.Response.AppendHeader("Content-Disposition", "attachment; filename=" + fileName);

        byte[] buffer = new byte[bufferSize];
        int bytesRead;

        try
        {
            while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                page.Response.OutputStream.Write(buffer, 0, bytesRead);
                page.Response.Flush();
            }
        }
        catch (Exception ex)
        {
            AS.Common.Logger.LoggerManager.Error("Stream write error in TransferFileToClient:\n" + ex);
        }
        finally
        {
            fileStream.Close();
            page.Response.End();
        }
    }

    public static string GetIEBrowserMode()
    {
        string mode = string.Empty;
        string userAgent = HttpContext.Current.Request.UserAgent == null ? string.Empty : HttpContext.Current.Request.UserAgent;
        string browser = HttpContext.Current.Request.Browser.Type == null ? string.Empty : HttpContext.Current.Request.Browser.Type;

        if (userAgent.Contains("Trident/7."))
        {
            if (browser == "IE7")
            {
                mode = "IE11 Compatibility View";
            }
            else
            {
                mode = "IE11 Standard";
            }
        }
        else if (userAgent.Contains("Trident/6.0"))
        {
            if (browser == "IE7")
            {
                mode = "IE10 Compatibility View";
            }
            else
            {
                mode = "IE10 Standard";
            }
        }
        else if (userAgent.Contains("Trident/5.0"))
        {
            if (browser == "IE7")
            {
                mode = "IE9 Compatibility View";
            }
            else
            {
                mode = "IE9 Standard";
            }
        }
        else if (userAgent.Contains("Trident/4.0"))
        {
            if (browser == "IE7")
            {
                mode = "IE8 Compatibility View";
            }
            else
            {
                mode = "IE8 Standard";
            }
        }
        else if (!userAgent.Contains("Trident"))
        {
            mode = browser;
        }
        return mode;
    }

    public static void SwitchIECompatibilityView(Page page)
    {
        string ieCompatibilityView = "Edge";
        string browser = GeneralFuncsLib.GetIEBrowserMode();

        if (browser.IndexOf("IE10") >= 0 || browser.IndexOf("IE11") >= 0)
        {
            ieCompatibilityView = "IE=9";
        }
        else
        {
            ieCompatibilityView = "IE=Edge";
        }
        HtmlMeta meta = new HtmlMeta();
        meta.HttpEquiv = "X-UA-Compatible";
        meta.Content = ieCompatibilityView;
        page.Header.Controls.AddAt(0, meta);
    }

    public static void SetCssClass(this HtmlForm form)
    {
        if (form != null)
        {
            string browser = GeneralFuncsLib.GetIEBrowserMode();
            if (browser.IndexOf("IE9") >= 0)
            {
                form.Attributes.Add("class", "ie9");
            }
            else if (browser.IndexOf("IE8") >= 0)
            {
                form.Attributes.Add("class", "ie8");
            }
            else if (browser.IndexOf("IE7") >= 0)
            {
                form.Attributes.Add("class", "ie7");
            }
            else if (browser.IndexOf("IE10") >= 0)
            {
                form.Attributes.Add("class", "ie10");
            }
        }
    }

    public static bool ToBoolean(this object org)
    {
        if (org == null || org == DBNull.Value) return false;
        return bool.Parse(org.ToString());
    }
    public static int ToInt(this object org)
    {
        if (org == null || org == DBNull.Value) return 0;
        return int.Parse(org.ToString());
    }

    public static long ToLong(this object value, long defaultValue = 0)
    {
        return (value == null || value == DBNull.Value || String.IsNullOrEmpty(value.ToString())) ? defaultValue : Convert.ToInt64(value);
    }

    public static string ToSafeString(this object org)
    {
        if (org == null || org == DBNull.Value) return string.Empty;
        return org.ToString();
    }

    public static void ExecuteClientScript(this Page page, string script)
    {
        page.ClientScript.RegisterStartupScript(typeof(NonReportPage), Guid.NewGuid().ToString().Replace("-", ""), script, true);
    }
    public static void DisplayEntityNameWhenExport(this ASGrid grid)
    {
        ReportPage page = (ReportPage)grid.Page;
        GridColumn col = grid.Columns.FindByUniqueName("EntityName");
        if (col != null)
        {
            col.Visible = true;
        }
    }
    public static void ExportToExcel(this SecurePage page, Control exportHolder, string fileName)
    {
        page.IsNoCache = false;
        //bind data to data bound controls and do other stuff
        page.Response.Clear(); //this clears the Response of any headers or previous output
        page.Response.Buffer = true; //make sure that the entire output is rendered simultaneously
        page.Response.ContentType = "application/vnd.ms-excel";
        StringWriter stringWriter = new StringWriter(); //System.IO namespace should be used
        HtmlTextWriter htmlTextWriter = new HtmlTextWriter(stringWriter);

        HttpContext.Current.Response.AppendHeader("content-disposition", VeraCodeSolution.RemoveCRLF(string.Format("attachment; filename=\"{0}.xls\"", fileName)));
        exportHolder.RenderControl(htmlTextWriter);
        string htmlContent = stringWriter.ToString();
        htmlContent = htmlContent.Replace(@"<pre>", @"<pre style='font-family:Courier;'>");
        Regex rx = new Regex(@"<link\s*rel='stylesheet'\s*type='text/css'.*/>");
        if (rx.IsMatch(htmlContent))
        {
            htmlContent = rx.Replace(htmlContent, string.Empty);
        }

        Regex arx = new Regex(@"<a.*href=.*>");
        if (arx.IsMatch(htmlContent))
        {
            htmlContent = arx.Replace(htmlContent, string.Empty);
            htmlContent = htmlContent.Replace("</a>", string.Empty);
        }
        page.Response.Write(GetOriginalContent(htmlContent));
        page.Response.End();
    }
    public static string GetOriginalContent(string content)
    {
        content = content.Replace("<div style='page-break-before:always;'>&nbsp;", "");
        content = content.Replace("</div>&nbsp;", "<br/>&nbsp;");
        content = @"<html>
                    <head>
                        <style type='text/css'>
                            td
                            {
                                border-style: solid;
                                border-width: thin;
                            }
                            tr.borderBottom td
                            {
                                border-bottom-style: solid;
                                border-bottom-width: thin;
                            }
                            tr.borderTop td
                            {
                                border-top-style: solid;
                                border-top-width: thin;
                            }
                        </style>
                    </head>
                    <body>" + content + "</body></html>";
        return content;
    }
    public static void RegisterCssFile(SecurePage page, bool isModal = false)
    {
        string mobileBrowserPrefix = AS.Web.SharedSession.General.IsMobileBrowser() ? ".noresponsive" : "";

        //Register bootstrap.css
        Literal bootstrapLink = new Literal();
        bootstrapLink.Text = String.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}res/css/bootstrap/bootstrap{1}.min.css\" />", page.ResolveUrl("~/"), mobileBrowserPrefix);
        page.Header.Controls.AddAt(0, bootstrapLink);

        //Register master.css
        string csslink = string.Empty;
        if (isModal)
        {
            csslink = String.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}res/css/master{1}.min.css?v=modal\" />", page.ResolveUrl("~/"), mobileBrowserPrefix);
        }
        else
        {
            csslink = String.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}res/css/master{1}.min.css\" />", page.ResolveUrl("~/"), mobileBrowserPrefix);
        }
        csslink += String.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}res/css/header{1}.min.css\" />", page.ResolveUrl("~/"), mobileBrowserPrefix);
        //Register Theme site
        if (isModal)
        {
            csslink += String.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}App_Themes/{1}/branding_master.min.css?v=modal\"/>", page.ResolveUrl("~/"), SessionManager.CurrentUserThemeName);
        }
        else
        {
            csslink += String.Format("<link rel=\"stylesheet\" type=\"text/css\" href=\"{0}App_Themes/{1}/branding_master.min.css\"/>", page.ResolveUrl("~/"), SessionManager.CurrentUserThemeName);
        }

        Literal link = new Literal();
        link.Text = csslink;
        page.Header.Controls.Add(link);
        RegisterJSMessageParameter(page, isModal);
    }

    public static void RegisterJSMessageParameter(SecurePage page, bool isModal = false)
    {

        string ClientScript = string.Empty;

        ClientScript += "<script type=\"text/javascript\">" + Environment.NewLine;

        //Regist javascript
        //string path = HttpContext.Current.Server.MapPath("~/App_GlobalResources/LanguageResource.resx");

        //XDocument ResourceFile = XDocument.Load(@path);
        //var data = ResourceFile.Element("root");

        //IEnumerable<XElement> FieldList = ResourceFile.Descendants("data");

        //foreach (var field in FieldList)
        //{
        //    string name = field.Attribute("name").Value;
        //    if (name.Contains("_Msg_"))
        //    {
        //        ClientScript += "var " + name + " = '" + HttpContext.GetGlobalResourceObject("LanguageResource", name).ToString() + "';" + Environment.NewLine;
        //    }
        //}
        ResourceSet resources = Resources.LanguageResource.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

        foreach (DictionaryEntry field in resources)
        {
            string key = field.Key.ToString();
            if (key.Contains("_Msg_"))
            {
                ClientScript += "var " + key + " = '" + field.Value.ToString() + "';" + Environment.NewLine;
            }
        }

        //ClientScript += "var AS_CommonJS_Msg_PrintPage_Alert = '" + HttpContext.GetGlobalResourceObject("LanguageResource", "AS_CommonJS_Msg_PrintPage_Alert").ToString() + "';" + Environment.NewLine;
        //ClientScript += "var AS_ReportFilterJS_Msg_Date_FromThanTo = '" + HttpContext.GetGlobalResourceObject("LanguageResource", "AS_ReportFilterJS_Msg_Date_FromThanTo").ToString() + "';" + Environment.NewLine;
        //ClientScript += "var AS_ReportFilterJS_Msg_Date_Invalid = '" + HttpContext.GetGlobalResourceObject("LanguageResource", "AS_ReportFilterJS_Msg_Date_Invalid").ToString() + "';" + Environment.NewLine;
        //ClientScript += "var AS_ReportFilterJS_Msg_Date_MoreThanToday = '" + HttpContext.GetGlobalResourceObject("LanguageResource", "AS_ReportFilterJS_Msg_Date_MoreThanToday").ToString() + "';" + Environment.NewLine;
        //ClientScript += "var AS_ReportFilterJS_Msg_TheBeginDate = '" + HttpContext.GetGlobalResourceObject("LanguageResource", "AS_ReportFilterJS_Msg_TheBeginDate").ToString() + "';" + Environment.NewLine;
        //ClientScript += "var AS_ReportFilterJS_Msg_TheDate = '" + HttpContext.GetGlobalResourceObject("LanguageResource", "AS_ReportFilterJS_Msg_TheDate").ToString() + "';" + Environment.NewLine;
        //ClientScript += "var AS_ReportFilterJS_Msg_TheDateFormat = '" + HttpContext.GetGlobalResourceObject("LanguageResource", "AS_ReportFilterJS_Msg_TheDateFormat").ToString() + "';" + Environment.NewLine;
        //ClientScript += "var AS_ReportFilterJS_Msg_TheEndDate = '" + HttpContext.GetGlobalResourceObject("LanguageResource", "AS_ReportFilterJS_Msg_TheEndDate").ToString() + "';" + Environment.NewLine;
        //ClientScript += "var AS_RiskParameterJS_Msg_ERR_ERR_MINEXCEED = '" + HttpContext.GetGlobalResourceObject("LanguageResource", "AS_RiskParameterJS_Msg_ERR_ERR_MINEXCEED").ToString() + "';" + Environment.NewLine;
        //ClientScript += "var AS_RiskParameterJS_Msg_ERR_MAXEXCEED = '" + HttpContext.GetGlobalResourceObject("LanguageResource", "AS_RiskParameterJS_Msg_ERR_MAXEXCEED").ToString() + "';" + Environment.NewLine;


        ClientScript += "</script>";

        Literal link = new Literal();
        link.Text = ClientScript;
        page.Header.Controls.AddAt(1, link);


    }

    public static void RenderScriptSetting(Page page)
    {
        var settings = PersonalDataHelper.GetUserUISettings();
        if (settings == null) return;

        var settingBuilder = new StringBuilder();
        settingBuilder.Append("<script type=\"text/javascript\">");
        settingBuilder.Append("var UserSetting = {");
        foreach (var setting in settings)
        {
            settingBuilder.AppendFormat("{0}:'{1}', ", setting.Key, setting.Value);
        }
        settingBuilder.Append("};</script>");

        Literal link = new Literal();
        link.Text = settingBuilder.ToString();
        page.Header.Controls.AddAt(1, link);
    }

    public static string GetDefaultPageOfLoggedInUser(SecurePage page)
    {
        if (SessionManager.DefaultLangdingPage == null)
        {
            string url = "~/Default.aspx";
            SecMenuItem defaultMenu = GetDefaultMenuItem(page);

            if (defaultMenu != null)
            {
                //38605 - Enable NRT Risk module
                url = defaultMenu.Url.Replace(RISK_OLD_URL_RULE, RISK_MCF_URL_RULE);
            }
            SessionManager.DefaultLangdingPage = url;
            return page.ResolveUrl(url);
        }
        else
        {
            return page.ResolveUrl(SessionManager.DefaultLangdingPage);
        }
    }

    //42589 – VW – FIS - Password Reset and Expiration Issues
    public static SecMenuItem GetDefaultMenuItem(SecurePage page)
    {
        SecMenuItem res = null;

        DataTable table = GetDefaultLandingPage(SessionManager.CurrentUserRoles[0].HierarchyID);

        //39251 – VW - Add Default Landing Page On Update My Profile Page
        SecMenuItem defaultLandingPage = PersonalDataHelper.GetJSONConfig<SecMenuItem>(UserConfigNames.CONFIG_USER_PROFILE_DEFAULT_LANDING_PAGE);

        if (defaultLandingPage != null && page.IsUserWithPermission(defaultLandingPage.Permissions))
        {
            res = defaultLandingPage;
        }
        else if (table != null && table.Rows.Count > 0 && page.IsUserWithPermission(table.Rows[0]["PermissionCodes"].ToString()))
        {
            res = new SecMenuItem(Convert.ToInt32(table.Rows[0]["SiteMapID"].ToString()), 0, table.Rows[0]["Url"].ToString(), string.Empty, string.Empty, table.Rows[0]["PermissionCodes"].ToString(), 0);
        }
        else if (page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DASHBOARD) || page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_DASHBOARD_MS))
        {
            if (SessionManager.CurrentMenuItems != null)
            {
                DataRow dr = SessionManager.CurrentMenuItems.Select("MenuUrl = '~/Dashboard.aspx'").FirstOrDefault();
                if (dr != null)
                {
                    res = new SecMenuItem(Convert.ToInt32(dr["MenuID"].ToString()), 0, dr["MenuUrl"].ToString(), string.Empty, string.Empty, dr["MenuPermissions"].ToString(), 0);
                }
            }
            else
                res = new SecMenuItem(0, 0, "~/Dashboard.aspx", string.Empty, string.Empty, string.Empty, 0);
        }
        else if (page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_USER_PROF) || page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_USER_PROF_MS))
        {
            if (SessionManager.CurrentMenuItems != null)
            {
                DataRow dr = SessionManager.CurrentMenuItems.Select("MenuUrl = '~/ManageProfile.aspx'").FirstOrDefault();
                if (dr != null)
                {
                    res = new SecMenuItem(Convert.ToInt32(dr["MenuID"].ToString()), 0, dr["MenuUrl"].ToString(), string.Empty, string.Empty, dr["MenuPermissions"].ToString(), 0);
                }
            }
            else
                res = new SecMenuItem(0, 0, "~/ManageProfile.aspx", string.Empty, string.Empty, string.Empty, 0);
        }
        else
        {
            SecMenuItemCollection MenuItems;
            if (SessionManager.SingleSignOnMenuMod)
            {
                MenuItems = WebServices.SecurityServices.GetMenuItemsByUserSSO(SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, SessionManager.CurrentUserRoles[0].HierarchyID, SessionManager.SSOSecurityLevel, SessionManager.CurrentLanguage, false);
            }
            else
                MenuItems = WebServices.SecurityServices.GetMenuItemsByUser(SessionManager.CurrentUser.ASClient, SessionManager.CurrentUser.UserID, SessionManager.CurrentUserRoles[0].HierarchyID, SessionManager.CurrentLanguage, false);
            if (MenuItems.Count > 0)
            {
                res = MenuItems[0];
            }
        }

        return res;
    }

    public static Color ToForceColor(this object htmlColor)
    {
        if (htmlColor == null || htmlColor == DBNull.Value)
            return Color.Empty;
        else if (htmlColor.ToString().Trim().Trim('#') == "000000" || htmlColor.ToString().Trim().Trim('#') == "000")
        {
            return Color.Empty;
        }
        return ColorTranslator.FromHtml(htmlColor.ToString());

    }

    public static bool EnableTerminalCombobox()
    {
        var enableTerminalSetting = GeneralFuncsLib.GetClientExtendedSetting("EnableTerminalComboBox");
        return enableTerminalSetting.Data != null && enableTerminalSetting.Data.ToLower().Equals("true");
    }

    public static string GetStatementReportDateFormat()
    {
        return GeneralFuncsLib.GetDataOfExtendedSetting("STATEMENT_REPORT_DATE_FORMAT");
    }

    public static bool HasMSProductEnvironment()
    {
        return GeneralFuncsLib.GetClientExtendedSetting(WebSiteConstants.PRODUCT_ENVIRONMENT_KEY)
            .Data.Contains(WebSiteEnums.ProductEnvironment.MS.ToString());
    }

    public static string GetStatementBEProcessor()
    {
        return GeneralFuncsLib.GetDataOfExtendedSetting("STATEMENT_BACK_END_PROCESSOR");
    }

    public static string GetStatementDisplayFormat()
    {
        return GeneralFuncsLib.GetDataOfExtendedSetting("STATEMENT_DISPLAY_FORMAT");
    }

    public static string GetInvisibleHyperlinkByEntityTypeIDs()
    {
        return GeneralFuncsLib.GetDataOfExtendedSetting("InvisibleHyperlinkByEntityTypeIDs");
    }

    public static bool HasOptInOutPermission(SecurePage page)
    {
        return page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_OPTINOUT)
            || page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_OPTINOUT);
    }

    public static bool HasRelationshipManagerPermission(ReportPage page)
    {
        return page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RELATIONSHIP_MANAGER)
            || page.IsUserWithPermission(WebSiteConstants.SEC_PERMISSION_RELATIONSHIP_MANAGER_MS);
    }


    public static bool HideSessionAddEditChain()
    {
        return GetDataOfExtendedSetting("MIF_Profile_HideSessionAddEditChain").ToLower().Equals("true")
            ? true : false;
    }

    public static bool HasServicedByFieldInRskRpt()
    {
        var hasServicedBy = GeneralFuncsLib.GetClientExtendedSetting("HasServicedByFieldInRskRpt");
        return hasServicedBy.Data != null
            && hasServicedBy.Data.ToLower().Equals("true");
    }

    public static bool HasEmailFieldInRskRpt()
    {
        var hasEmail = GeneralFuncsLib.GetClientExtendedSetting("HasEmailFieldInRskRpt");
        return hasEmail.Data != null
            && hasEmail.Data.ToLower().Equals("true");
    }

    public static string DefaultReportFilterMode()
    {
        // Format: UserMode1,HierarchyMode1;UserMode2,HierarchyMode2
        // That means when a user with user mode 'UserMode1' login to the system
        // , the default hierarchy filter mode is HierarchyMode1
        string defaultFilterModes = GeneralFuncsLib.GetDataOfExtendedSetting("DefaultReportFilterMode");
        if (!string.IsNullOrEmpty(defaultFilterModes))
        {
            string[] mappings = defaultFilterModes.Split(';');
            foreach (string mapping in mappings)
            {
                if (!string.IsNullOrEmpty(mapping))
                {
                    string[] details = mapping.Split(',');
                    if (details[0].Trim().Equals(GeneralFuncsLib.GetHierarchyInfo(SessionManager.CurrentUser.EntityType).UserMode, StringComparison.OrdinalIgnoreCase))
                    {
                        return details[1].Trim();// Hierarchy Filter Mode
                    }
                }
            }
        }
        return string.Empty;
    }

    public static bool IsSynchPCIRole()
    {
        return GetDataOfExtendedSetting("PCI_ROLE_SYNCH").Equals("true")
            ? true : false;
    }

    public static bool IsCompliassureRoleSynch()
    {
        return GetDataOfExtendedSetting("COMPLIASSURE_ROLE_SYNCH").Equals("true")
            ? true : false;
    }

    public static bool IsSynchPCIUser()
    {
        return GetDataOfExtendedSetting("PCI_USER_SYNCH").Equals("true")
            ? true : false;
    }

    public static bool IsCompliassureUserSynch()
    {
        return GetDataOfExtendedSetting("COMPLIASSURE_USER_SYNCH").Equals("true")
            ? true : false;
    }

    public static bool IsShowRelationshipManager()
    {
        return GetDataOfExtendedSetting("SHOW_RELATIONSHIP_MANAGER").Equals("true")
            ? true : false;
    }

    public static bool CreateMsChainUserWhenAddingNewChain()
    {
        return GetDataOfExtendedSetting("CREATE_MS_CHAIN_USER_WHEN_ADDING_NEW_CHAIN").Equals("true")
            ? true : false;
    }

    public static bool HasSSOLogin()
    {
        return GetDataOfExtendedSetting("HAS_SSO_LOGIN").Equals("true")
            ? true : false;
    }

    public static bool IsDepositHistory()
    {
        return (
             GeneralFuncsLib.GetDataOfExtendedSetting("IS_DEPOSIT_HISTORY").Equals("true")
             || (GeneralFuncsLib.GetDataOfExtendedSetting("IS_DEPOSIT_HISTORY_MS").Equals("true") && SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS));
    }

    public static string ReturnReportTitle()
    {
        return GeneralFuncsLib.GetDataOfExtendedSetting("REPORT_RETURN_TITLE").ToString();
    }

    public static string GetCardTypeOrder()
    {
        return GeneralFuncsLib.GetDataOfExtendedSetting("SET_ORDER_CARD_TYPE").ToString();
    }

    public static string GetMetricsDebitText()
    {
        string debitText = GeneralFuncsLib.GetDataOfExtendedSetting("METRICS_DEBIT_TEXT").ToString();
        return debitText.IsNullOrEmpty() ? "DEBIT" : debitText.Trim().ToUpper();
    }

    public static string GetContactUsMessage()
    {
        return GeneralFuncsLib.GetDataOfExtendedSetting("CONTACT_US_MESSAGE").ToString();
    }

    public static string GetClientContactInfoKey()
    {
        if (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS)
            return GeneralFuncsLib.GetDataOfExtendedSetting("CLIENT_CONTACT_INFO_KEY_MS").ToString();
        else if (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS)
            return GeneralFuncsLib.GetDataOfExtendedSetting("CLIENT_CONTACT_INFO_KEY_CS").ToString();

        return string.Empty;
    }

    private static string[] SplitStringFromClientSetting(string settingName)
    {
        string columns = GeneralFuncsLib.GetDataOfExtendedSetting(settingName).ToString();
        return columns.IsNullOrEmpty() ? new string[0]
            : columns.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    }

    private static string[] GetHiddenColumnsInMsSite(string settingName)
    {
        if (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS)
        {
            return SplitStringFromClientSetting(settingName);
        }
        else
        {
            return new string[0];
        }
    }

    public static string[] GetHiddenColumnsOfBatchHistory()
    {
        return GetHiddenColumnsInMsSite("HIDE_COLUMNS_BATCH_HISTORY_MS");
    }

    public static string[] GetHiddenColumnsOfBatchDetailModal()
    {
        return GetHiddenColumnsInMsSite("HIDE_COLUMNS_BATCH_DETAIL_MS");
    }

    public static string[] GetHiddenColumnsOfTransactionSearch()
    {
        return GetHiddenColumnsInMsSite("HIDE_COLUMNS_TRANS_SEARCH_MS");
    }

    public static string[] GetHiddenColumnsOfCardHistoryBelongToCardModal()
    {
        return GetHiddenColumnsInMsSite("HIDE_COLUMNS_CARD_MODAL_CARD_HISTORY");
    }

    private static bool IsHQMerchant(string merchantNumber)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add("@UserID", SessionManager.CurrentUser.UserID, DbType.AnsiString);
        parameters.Add("@ASClientID", SessionManager.CurrentClient, DbType.AnsiString);
        parameters.Add("@SiteID", SessionManager.CurrentUser.SiteID, DbType.AnsiString);
        parameters.Add("@MerchantNumber", merchantNumber, DbType.AnsiString);
        var dt = WebServices.SecurityServices.GetReports("spa_GetMerchantTypeByMID", parameters);
        if (dt != null && dt.Rows.Count > 0)
            return dt.Rows[0]["ISHQMerchant"].ToBoolean();
        return false;
    }

    public static bool CheckPermissionSyn1099K(bool IsMSUser, User user)
    {
        var isSyn1099K = GetClientExtendedSetting("COMPLIASSURE_SYNCH_CUSTOM").Data;
        if (isSyn1099K != null && isSyn1099K.Equals("true"))
        {
            if (!IsMSUser) return true;
            else if (SessionManager.CurrentUserType == WebSiteEnums.UserHierarchyMode.Merchant) return IsHQMerchant(user.UserID);
        }

        return true;
    }

    /// <summary>
    /// Get merchant profile navigator by client
    /// </summary>
    /// <param name="xmlFilePath"></param>
    /// <returns></returns>
    public static Dictionary<string, string> GetMerchantProfileNavigator(string clientID)
    {
        Dictionary<string, string> navigatorDic = new Dictionary<string, string>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(HttpContext.Current.Server.MapPath("~/App_Data/MerchantProfileNavigator.xml"));
        XmlNodeList nodeList = xmlDoc.SelectNodes("//MerchantProfileNavigator/Client[@Id='" + clientID + "']/Item");
        ResourceSet resources = Resources.Template.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
        if (nodeList != null)
        {
            foreach (XmlNode node in nodeList)
            {
                string id = node.Attributes["Id"].Value;
                if (!navigatorDic.ContainsKey(id))
                {
                    if (resources.GetObject(node.Attributes["Text"].Value) != null && !string.IsNullOrEmpty(resources.GetObject(node.Attributes["Text"].Value).ToString()))
                    {
                        navigatorDic.Add(id, resources.GetObject(node.Attributes["Text"].Value).ToString());
                    }
                    else
                    {
                        navigatorDic.Add(id, node.Attributes["Text"].Value);
                    }
                }
            }
        }
        return navigatorDic;
    }

    public static bool HasStatementMessageSection()
    {
        return GeneralFuncsLib.GetDataOfExtendedSetting("HAS_STATEMENT_MESSAGE_SECTION").Equals("true") ? true : false;
    }

    //TK 32673
    public static bool Has12MonthsChart()
    {
        var config = GeneralFuncsLib.GetDataOfExtendedSetting("Has12MonthsChart");
        return string.IsNullOrEmpty(config) ? false : config.Equals("true");
    }

    #endregion

    public static string GetCurrentCulture()
    {
        //default is US culture
        string _culture = WebSiteConstants.USCulture;
        if (SessionManager.CurrentLanguage != null && SessionManager.CurrentLanguage == (int)WebSiteEnums.LanguageCode.English)
        {
            _culture = WebSiteConstants.USCulture;
        }
        else if (SessionManager.CurrentLanguage != null && SessionManager.CurrentLanguage == (int)WebSiteEnums.LanguageCode.Spanish)
        {
            _culture = WebSiteConstants.SpanishCulture;
        }
        else
        {
            _culture = WebSiteConstants.USCulture;
        }
        return _culture;
    }
    public static decimal ToSafeDecimal(this object source)
    {
        if (source == null || source == DBNull.Value)
        {
            return source.ToSafeDecimal(0);
        }
        return decimal.Parse(source.ToString());
    }
    public static decimal ToSafeDecimal(this object source, decimal defaultValue)
    {
        if (source == null || source == DBNull.Value)
        {
            return defaultValue;
        }
        return decimal.Parse(source.ToString());
    }

    public static string DateFormatMMMYY(this object source, string culture = null)
    {
        if (culture == null)
            culture = GetCurrentCulture();

        if (source == null || source == DBNull.Value)
        {
            return string.Empty;
        }
        var str = source.ToString();
        var parts = str.Split(' ');
        if (parts.Length != 2)
        {
            return source.ToString();
        }
        str = string.Format("{0} {1}", "01", str);
        try
        {
            var date = DateTime.ParseExact(str, "dd MMM yy", CultureInfo.InvariantCulture);
            return date.ToString("MMM yy", CultureInfo.CreateSpecificCulture(culture));
        }
        catch
        {
            return str;
        }

    }

    public static bool IsDisableChangeMSRoleByEntities(string entityTypeId)
    {
        return IsExcludeHierarchy(entityTypeId, "DisableChangeMSRoleByEntities");
    }

    public static bool IsDisableSecondaryUserRoleByEntities(string entityTypeId)
    {
        return IsExcludeHierarchy(entityTypeId, "DisableSecondaryUserRoleByEntities");
    }

    public static bool IsExcludeHierarchy(string entityTypeId, string key)
    {
        bool result = false;
        string excludeHierachies = GeneralFuncsLib.GetDataOfExtendedSetting(key);
        if (!string.IsNullOrEmpty(excludeHierachies))
        {
            excludeHierachies = string.Format(",{0},", excludeHierachies);
            if (excludeHierachies.Contains(string.Format(",{0},", entityTypeId)))
            {
                result = true;
            }
        }
        return result;
    }

    public static string GetFinancialInstitutions()
    {       
        return string.IsNullOrEmpty(SessionManager.ClientInfo.FinancialIntitutions) ?
            string.Empty :
            SessionManager.ClientInfo.FinancialIntitutions;
    }

    public static string GetDirectMerchants()
    {
        return string.IsNullOrEmpty(SessionManager.ClientInfo.DirectMerchants) ?
            string.Empty :
            SessionManager.ClientInfo.DirectMerchants;
    }

    //43842 - FD to TSYS Merchant Migration
    public static bool GetEnabledStatementAPI()
    {
        string statApi = GetDataOfExtendedSetting("EnableStatementAPI");
        return !string.IsNullOrEmpty(statApi) && statApi.ToLower() == "true";
    }

    //43842 - FD to TSYS Merchant Migration
    //42782 – VW – CAYAN - Implement New TSYS Processing Platform - modify
    public static bool EnabledStatementAPI(string merchantNumber)
    {
        if (GetEnabledStatementAPI())
        {
            var checkData = CheckMerchantIsConvertion(merchantNumber);
            if (checkData.HasData())
            {
                return checkData.Rows[0]["IsConvertion"].ToBoolean();
            }
        }

        return false;
    }

    //43842 - FD to TSYS Merchant Migration
    public static DataTable CheckMerchantIsConvertion(string merchantNumber)
    {
        string spaName = "spa_cs_CheckMerchantIsConvertion";
        FilterParameterCollection paras = new FilterParameterCollection();
        paras.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paras.Add(new FilterParameter("@MerchantNumber", merchantNumber, DbType.String));

        return WebServices.CsReportServices.GetReports(spaName, paras);
    }

    public static bool EnabledTSYSConversion()
    {
        return GetDataOfExtendedSetting("ENABLED_TSYS_CONVERSION").ToLower().Equals("true") ? true : false;
    }

    public static bool IsUserSignOn()
    {
        return GetDataOfExtendedSetting("IS_CHANGE_USERNAME_MS_SITE").ToLower().Equals("true")
            && GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_USER_ID_ON_MIF").ToUpper().Contains(WebSiteSettings.WebSiteType.ToUpper()); ;
    }

    public static void VisibleHierarchyNameByFilteringMode(string hierarchyMode, string hierarchyValue, ASGrid gridName, string hierarchyName)
    {
        bool isVisibleHierarchyName = false;
        bool isAssociation = hierarchyName.Equals(Resources.LanguageResource.Hierarchy_Association);
        if (!string.IsNullOrEmpty(GetDataOfExtendedSetting("VISIBLE_HIERARCHYNAME_BY_FILTERING_MODE").ToLower()))
        {
            isVisibleHierarchyName = (GetDataOfExtendedSetting("VISIBLE_HIERARCHYNAME_BY_FILTERING_MODE").ToLower().Equals("true") && isAssociation);
        }
        if (isVisibleHierarchyName)
        {
            gridName.Columns.FindByUniqueName(WebSiteConstants.HIERARCHYNAME).HeaderText = GetHierarchyColumnNameByFilteringMode(hierarchyName);
            gridName.Columns.FindByUniqueName(WebSiteConstants.HIERARCHYNAME).HeaderTooltip = GetHierarchyColumnNameByFilteringMode(hierarchyName);
            gridName.Columns.FindByUniqueName(WebSiteConstants.HIERARCHYNAME).Visible = true;
        }
        else
        {
            gridName.Columns.FindByUniqueName(WebSiteConstants.HIERARCHYNAME).Visible = false;
        }
    }

    public static string GetHierarchyColumnNameByFilteringMode(string hierarchyName)
    {
        string hierachyName = string.Format(Resources.LanguageResource.Hierarchy_Association + " Name");
        if (!(SessionManager.CurrentLanguage == 1))
        {
            hierachyName = string.Format("Nombre de la " + Resources.LanguageResource.Hierarchy_Association);
        }
        return hierachyName;
    }

    // TK 38053
    public static string GetCurrentEnvironment(bool isLoginPage = true)
    {
        if (ConfigurationManager.AppSettings["CurrentEnvironment"] != null)
        {
            string environment = ConfigurationManager.AppSettings["CurrentEnvironment"].ToString();
            if (isLoginPage)
            {
                if (environment.IndexOf(",") != -1)
                    return environment.Split(',')[0];
                else
                    return environment;
            }
            else
            {
                if (environment.IndexOf(",") != -1)
                    return environment.Split(',')[1];
                else
                    return environment;
            }
        }
        return string.Empty;
    }

    public static bool ShowEnvironmentIndicator(bool isLoginPage = false)
    {
        if (!isLoginPage)
        {
            var showEnvironmentIndicator = PersonalDataHelper.GetJSONConfig<string>(UserConfigNames.CONFIG_USER_PROFILE_SHOW_ENVIRONMENT_INDICATOR);
            if (showEnvironmentIndicator == null)
            {
                return true;
            }
            else
            {
                if (!string.IsNullOrEmpty(GetCurrentEnvironment(false)) && !GetCurrentEnvironment(false).Equals("PROD", StringComparison.OrdinalIgnoreCase))
                {
                    return showEnvironmentIndicator.Equals("Yes", StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return !string.IsNullOrEmpty(GetCurrentEnvironment()) && !GetCurrentEnvironment().Equals("PROD", StringComparison.OrdinalIgnoreCase);
        }
    }

    public static void RegisterHtmlMeta(Page page)
    {
        if (!AS.Web.SharedSession.General.IsMobileBrowser())
        {
            HtmlMeta meta = new HtmlMeta();
            meta.Name = "viewport";
            meta.Content = "width=device-width,initial-scale=1";
            page.Header.Controls.AddAt(0, meta);
        }
    }

    // Ticket #39613    
    public static string FormatBorderText(string value, Color color, bool checkWhiteColor = false)
    {
        string result = string.Empty;
        if (value.IsNullOrEmpty() || value.Equals("&nbsp;"))
            return result;

        string style;
        if (color.Name.IsNullOrEmpty() || (checkWhiteColor && color.Name.Equals(Color.White.Name)))
        {
            style = string.Empty;
        }
        else
        {
            style = string.Format(" style='border-bottom: 2px solid {0}'", color.Name);
        }
        result = string.Format("<span{0}>{1}</span>", style, value);

        return result;
    }

    //44894 - VW- Merchant Note Default Preferences via User Mgmt Settings
    public static string BuildXmlFilter(params Tuple<string, string>[] tuples)
    {
        return HttpUtility.UrlEncode(Cryptography.EncryptText(BuilXmlFilterNotEncrypt(tuples)));
    }

    public static string BuilXmlFilterNotEncrypt(params Tuple<string, string>[] tuples)
    {
        string result = "<XmlFilter>";
        foreach (var item in tuples)
        {
            result += string.Format("<{0}>{1}</{0}>", item.Item1, item.Item2);
        }
        result += "</XmlFilter>";
        return result;
    }

    // [43454] - Encrypting the comments field in VW Case Management 
    public static string[] DetectSesitiveData(string content)
    {
        // Check valid card
        string regex = @"\b\d{12}\b"
                       + @"|\b\d{13}\b|\b\d{4}[-|\s]\d{4}[-|\s]\d{5}\b"
                       + @"|\b\d{14}\b|\b\d{4}[-|\s]\d{6}[-|\s]\d{4}\b"
                       + @"|\b\d{15}\b|\b\d{4}[-|\s]\d{6}[-|\s]\d{5}\b|\b\d{4}[-|\s]\d{5}[-|\s]\d{6}\b|\b\d{4}[-|\s]\d{7}[-|\s]\d{4}\b"
                       + @"|\b\d{16}\b|\b\d{4}[-|\s]\d{4}[-|\s]\d{4}[-|\s]\d{4}\b"
                       + @"|\b\d{17}\b"
                       + @"|\b\d{18}\b"
                       + @"|\b\d{19}\b|\b\d{4}[-|\s]\d{4}[-|\s]\d{4}[-|\s]\d{4}[-|\s]\d{3}\b|\b\d{6}[-|\s]\d{13}\b";

        MatchCollection mCards = Regex.Matches(content, regex);
        if (mCards.Count > 0)
        {
            Dictionary<string, string> lstCard = new Dictionary<string, string>();
            string lstBankBin = string.Empty;
            foreach (var item in mCards)
            {
                string card = item.ToString();
                if (!lstCard.ContainsKey(card))
                {
                    string bankBin = card.Replace(" ", "").Replace("-", "").Substring(0, 6);
                    lstBankBin = !lstCard.ContainsValue(bankBin) ? lstBankBin + bankBin + ',' : lstBankBin;
                    lstCard.Add(card, bankBin);
                }
            }

            // Check Bank bin valid
            DataTable result = CheckBankBin(lstBankBin.TrimEnd(','));
            if (result != null && result.Rows.Count > 0)
            {
                string[] arrayFilter = result.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
                return lstCard.Where(x => arrayFilter.Contains(x.Value)).Select(pair => pair.Key).ToArray();
            }
        }

        return null;
    }

    private static DataTable CheckBankBin(string lstBinBumber)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ListBinNumber", lstBinBumber, DbType.AnsiString));

        return WebServices.RiskServices.GetReports("spa_CheckBankBin", parameters);
    }
    /// <summary>
    /// Conver DataTable to Json object
    /// </summary>
    /// <param name="dt"></param>
    /// <returns></returns>
    public static object DataTableToJson(DataTable dt)
    {
        var list = new List<Dictionary<string, object>>();
        foreach (DataRow row in dt.Rows)
        {
            var dict = new Dictionary<string, object>();
            dict["ID"] = row[0].ToString();
            dict["Value"] = row[1].ToString();
            list.Add(dict);
        }

        AdvancedJavaScriptSerializer serialize = new AdvancedJavaScriptSerializer();
        return serialize.Serialize(list);
    }

    public static bool IsMCFRisk()
    {
        int modeMCF = RiskSessionManager.Risk_ModeMCF;
        return true;
    }

    private static DataTable GetConfigRiskMCF()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        return WebServices.RiskServices.GetReports("spa_SEC_GetMenuSetting", parameters);
    }

    public static void UpdateDataWhenLogout()
    {
        // 38605: Clear all WIP by me status
        RM_MCF_GeneralFuncsLib.ClearWIPStatus();
    }

    //Sprint 6 - 46652 - AW Multi-currency Transaction Display 
    public static void SetDefaultCurrency()
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(true);
        DataTable dt = WebServices.CsReportServices.GetReports("spa_GetCurrencyInfo", parameters);

        if (dt.HasData())
        {
            SessionManager.CurrencyFortmat = dt.Rows[0]["CurrencyFortmat"].ToString();
            SessionManager.CurrencySymbol = dt.Rows[0]["CurrencySymbol"].ToString();
        }
        else
        {
            SessionManager.CurrencyFortmat = "en-US";
            SessionManager.CurrencySymbol = "$";
        }
    }
    //Sprint 6 - 46652 - AW Multi-currency Transaction Display 
    public static void ShowHideAWTransactionDetail(ASGrid grid)
    {
        if (SessionManager.CurrentClient == WebSiteConstants.ALLIEDWALLET_CLIENT)
        {
            grid.Columns.FindByUniqueName("OriginalTransactionID").Visible = true;
            grid.Columns.FindByUniqueName("CurrencyCode").Visible = true;
            if (grid.Columns.FindAllByDataField("OriginalAuthorizationAmount").Length > 0)
                grid.Columns.FindByUniqueName("OriginalAuthorizationAmount").Visible = true;
            if (grid.Columns.FindAllByDataField("OriginalTransactionAmount").Length > 0)
                grid.Columns.FindByUniqueName("OriginalTransactionAmount").Visible = true;
            grid.Columns.FindByUniqueName("IPAddress").Visible = true;
        }
        else
        {
            grid.Columns.FindByUniqueName("OriginalTransactionID").Visible = false;
            grid.Columns.FindByUniqueName("CurrencyCode").Visible = false;
            if (grid.Columns.FindAllByDataField("OriginalAuthorizationAmount").Length > 0)
                grid.Columns.FindByUniqueName("OriginalAuthorizationAmount").Visible = false;
            if (grid.Columns.FindAllByDataField("OriginalTransactionAmount").Length > 0)
                grid.Columns.FindByUniqueName("OriginalTransactionAmount").Visible = false;
            grid.Columns.FindByUniqueName("IPAddress").Visible = false;
        }
    }
    /// <summary>
    /// Replace $ to CurrencySymbol
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <history>
    /// Sprint 6 - 46652 - AW Multi-currency Transaction Display 
    /// </history>
    public static string ToCurrencySymbol(this string value)
    {
        if (!string.IsNullOrEmpty(value))
            return value.Replace("$", SessionManager.CurrencySymbol);
        return value;
    }

    public static bool SHOW_PROCESSINGDATA_ENTITYNAME()
    {
        return GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_PROCESSINGDATA_ENTITYNAME").ToLower().Equals("true") ? true : false;
    }
    public static string FormatCurrencyTooltip(object currency, string currencyFormat)
    {
        return string.Format("{0}", currency.ToCurrency("C", currencyFormat));
    }

    public static string GetUserMode()
    {
        string userMode = string.Empty;
        switch (SessionManager.CurrentUserType)
        {
            case WebSiteEnums.UserHierarchyMode.CS:
            case WebSiteEnums.UserHierarchyMode.AS:
                userMode = "CSUSER";
                break;
            case WebSiteEnums.UserHierarchyMode.Site:
                userMode = "CLIENT";
                break;
            case WebSiteEnums.UserHierarchyMode.Hierarchy:
            case WebSiteEnums.UserHierarchyMode.Merchant:
            case WebSiteEnums.UserHierarchyMode.Headquarter:
                userMode = GetHierarchyInfo(SessionManager.CurrentUser.EntityType).UserMode;
                break;
        }

        return userMode;
    }

    public static void WriteRequestLog(ILogObject requestTracking)
    {
        var currentRequest = HttpContext.Current.Request;
        if (currentRequest == null)
            return;

        TimeSpan durationRequest = DateTime.Now.Subtract(requestTracking.LogWebServerDts);
        requestTracking.LogSessionId = HttpContext.Current.Session.SessionID;
        requestTracking.LogElapsedTime = (int)durationRequest.TotalMilliseconds;
        requestTracking.LogWebSiteName = currentRequest.Url.DnsSafeHost;

        string clientIPAddress = currentRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];
        if (string.IsNullOrEmpty(clientIPAddress))
            clientIPAddress = currentRequest.ServerVariables["REMOTE_ADDR"];
        requestTracking.LogClientIPAddr = clientIPAddress;

        if (requestTracking.LogHostIPAddr == null)
            requestTracking.LogHostIPAddr = Array.Exists(currentRequest.ServerVariables.AllKeys, x => x == "LOCAL_ADDR") ? currentRequest.ServerVariables["LOCAL_ADDR"] : string.Empty;
        requestTracking.LogBrowserType = currentRequest.UserAgent;

        requestTracking.LogClientId = SessionManager.CurrentClient;
        requestTracking.LogSystemId = SessionManager.CurrentSystem;
        requestTracking.LogFullName = SessionManager.CurrentUser.UserNameFull;
        requestTracking.LogId1 = SessionManager.CurrentUser.UserID;
        requestTracking.LogId2 = SessionManager.SiteJumper;
        requestTracking.LogData4 = SessionManager.JumpFrom;

        if (requestTracking.LogData1 == null)
        {
            requestTracking.LogData1 = currentRequest.Url.AbsolutePath;
            requestTracking.LogData9 = currentRequest.Url.ToString();
        }
        if (requestTracking.LogData2 == null)
            requestTracking.LogData2 = currentRequest.QueryString.ToString();
        if (requestTracking.LogData3 == null)
        {
            requestTracking.LogData3 = requestTracking.LogTxt2 = new System.IO.StreamReader(currentRequest.InputStream).ReadToEnd();
        }

        WebServices.LogServices.InsertASPXTrackingLog(requestTracking);
    }

    public static DataTable GetWarningEditCustomView(int CustomViewID)
    {
        var parameters = new FilterParameterCollection();
        parameters.AddLoggedInUserReportingParams(false);
        parameters.Add(new FilterParameter("@UserID", SessionManager.CurrentUser, DbType.String));
        parameters.Add(new FilterParameter("@CustomViewID", CustomViewID, DbType.Int32));
        parameters.Add(new FilterParameter("@UserRecID", SessionManager.CurrentUser.RecId, DbType.Guid));
        return WebServices.RiskServices.GetReports("spa_RM_MCF_Warning_EditCustomView", parameters);
    }
    public static void BinAssignmentAuditLink(DataTable dt, SecurePage page, string lastModify, PlaceHolder plAudit, Literal ltAutitReportDetail, HiddenField hdLinkAudit)
    {
        if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("LastModifiedDate") && dt.Columns.Contains("LastModifiedBy"))
        {
            var updateBy = dt.Rows[0]["LastModifiedBy"];
            var updateDate = dt.Rows[0]["LastModifiedDate"];
            var userRecId = dt.Rows[0]["UserRecId"];
            var originalAssignmentID = dt.Rows[0]["OriginalAssignmentID"];
            if (!string.IsNullOrEmpty(updateDate.ToASString())
                && (page.IsUserWithPermission("AssignmentAuditReport") || page.IsUserWithPermission("MSAssignmentAuditReport")))
            {
                plAudit.Visible = true;
                ltAutitReportDetail.Text = string.Format(lastModify, Convert.ToDateTime(updateDate).ToString("MM/dd/yyyy - hh:mm tt"), updateBy);
                hdLinkAudit.Value = page.BuildSecureQueryString(string.Format("assignmentId={0}&userchangedby={1}&lastModifyDate={2}", originalAssignmentID, userRecId, updateDate));
            }
        }
    }

    public static bool Show_RoutingAccountNumber
    {
        get
        {
            bool result = false;
            if (GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_ROUTING_ACCOUNT") != null)
            {
                Boolean.TryParse(GeneralFuncsLib.GetDataOfExtendedSetting("SHOW_ROUTING_ACCOUNT"), out result);
            }
            return result;
        }
    }

    public static void StatementTrackingLog(string reportDate, string merchantNumber, string statementName, string docId = "", bool isHierarchy = false)
    {
        // No write log when site jump
        if (SessionManager.IsSiteJump || SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_CS) return;

        if (string.IsNullOrEmpty(statementName))
            statementName = string.Format("{0}-{1}-{2}", merchantNumber, HttpContext.GetGlobalResourceObject("LanguageResource", "Statement_aspx_cs_STMT").ToString(), DateTime.Parse(reportDate).ToString("MM/dd/yyyy"));
        else
        {
            string[] splitName = statementName.Split('.');
            statementName = splitName[0];
        }

        DataTable result = UpdateStatementTracking(reportDate, merchantNumber, statementName, docId, isHierarchy);

        if (result != null && result.Rows.Count > 0)
        {
            var dataRow = result.Rows[0];
            var data = dataRow["Data"] == null ? null : dataRow["Data"].ToString();
            int sourceId = dataRow["SourceId"] == null ? 8 : dataRow["SourceId"].ToInt();
            var categoryId = dataRow["EventCategoryId"] == null ? 1 : dataRow["EventCategoryId"] as int?;
            var eventTypeId = dataRow["EventTypeId"] == null ? null : dataRow["EventTypeId"] as int?;
            var description = eventTypeId.HasValue ? GeneralFuncsLib.GetEnumDescription(eventTypeId, EventType.Unknown) : GeneralFuncsLib.GetEnumDescription(categoryId, EventCategory.Unknown);

            var eventListenerClient = new EventListenerClient();
            eventListenerClient.CreateEvent(SessionManager.CurrentUser.SiteID, categoryId, eventTypeId, description, data, sourceId, SessionManager.CurrentUser.RecId);
            
        }
    }

    private static DataTable UpdateStatementTracking(string reportDate, string merchantNumber, string statementName, string docID, bool isHierarchy)
    {
        string hierarchyValue = string.Empty;
        string hierarchyMode = string.Empty;
         
        if (!string.IsNullOrEmpty(merchantNumber))
        {
            hierarchyValue = merchantNumber;
            hierarchyMode = "MERCHANTNUMBER";
        }
        else if(isHierarchy)
        {
            hierarchyValue = SessionManager.CurrentUser.EntityID;
            hierarchyMode = GetMsSiteWithStatementUseMode(string.Empty);
        }

        FilterParameterCollection parameterList = new FilterParameterCollection();
        parameterList.AddLoggedInUserParamsWithRecId();
        parameterList.AddEntityTypeParam();
        parameterList.Add(new FilterParameter("@DocID", docID, DbType.String));
        parameterList.Add(new FilterParameter("@ReportDate", reportDate, DbType.DateTime));
        parameterList.Add(new FilterParameter("@FileName", statementName, DbType.String));
        parameterList.Add(new FilterParameter("@FromSource", "VW Site", DbType.String));
        parameterList.Add(new FilterParameter("@HierarchyNumber", hierarchyValue, DbType.String));
        parameterList.Add(new FilterParameter("@HierarchyMode", hierarchyMode, DbType.String));

        return WebServices.CsReportServices.GetReports("spa_Statement_TrackingLog", parameterList);
    }
    
    public static string ExtendStatement
    {
        get
        {
            return GeneralFuncsLib.GetDataOfExtendedSetting("ExtendStatement");
        }
    }

    public static Dictionary<string, string> DicExtendStatement
    {
        get
        {
            string extendStatement = GeneralFuncsLib.ExtendStatement;
            var dictionary = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(extendStatement))
            {
                var listStatement = extendStatement.Split(',');

                foreach (var item in listStatement)
                {
                    var arrItem = item.Split('_');
                    dictionary.Add(arrItem[0], arrItem[1]);
                }
            }
            return dictionary;
        }
    }

    public static string GetMsSiteWithStatementUseMode(string filterValue)
    {
        string filterMode = string.Empty;
        if (SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS && string.IsNullOrEmpty(filterValue))
        {
            Dictionary<string, string> dicExtendStatement = GeneralFuncsLib.DicExtendStatement;
            foreach (var item in dicExtendStatement)
            {
                if (item.Key.IndexOf(GeneralFuncsLib.GetUserMode()) != -1)
                {
                    filterMode = item.Key;
                    break;
                }
            }
        }

        return filterMode;
    }

    /// <summary>
    /// Gets the description of an enum value.
    /// </summary>
    /// <typeparam name="T">The type of enum</typeparam>
    /// <param name="value">The enum value.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static string GetEnumDescription<T>(object value, T defaultValue) where T : struct
    {
        T enumValue = Enum.IsDefined(typeof(T), value) ? (T)value : defaultValue;

        return GetEnumDescription(enumValue);
    }

    /// <summary>
    /// Gets the description of an enum value.
    /// </summary>
    /// <typeparam name="T">The type of enum</typeparam>
    /// <param name="value">The enum value.</param>
    /// <returns></returns>
    public static string GetEnumDescription<T>(T value) where T : struct
    {
        var descAttribute =
            value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;

        return descAttribute == null ? value.ToString() : descAttribute.Description;
    }

    public static string BuildCardUrl(SecurePage page, string partialCard, string fullCard, string merchant)
    {
        return BuildCardUrl(page, partialCard, fullCard, merchant, false, false);
    }

    public static string BuildCardUrl(SecurePage page, string partialCard, string fullCard, string merchant, bool isNotInMifParam, bool isNotInMifValue)
    {
        string queryString = string.Empty;
        if(isNotInMifParam)
            queryString = page.BuildSecureQueryString("cn=" + partialCard + "&cnf=" + fullCard + "&merch=" + merchant + "&isNotInMif=" + isNotInMifValue);
        else
            queryString = page.BuildSecureQueryString("cn=" + partialCard + "&cnf=" + fullCard + "&merch=" + merchant);

        string urlCardDetail = "CardHistoryModal.aspx?" + queryString;
        string urlCard = "<a href=\"javascript:void(0)\" onclick=\"openPopupWindow('" + urlCardDetail + "','CardHistoryWindow'); return false;\">";

        return urlCard;
    }

    public static string[] GetAllowedFileTypeArray(string unStandardString)
    {
        return unStandardString.IsNullOrEmpty()
            ? (new string[0])
            : string.Format("{0}", unStandardString.Replace("7zip", "7z")).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string GetAllowedFileTypeStandard(string unStandardString)
    {
        if (unStandardString.IsNullOrEmpty()) return string.Empty;
        string result = string.Format(".{0}", unStandardString.Replace("7zip", "7z").Replace(",", ",."));
        return result;
    }

    public static bool HasUserPermission(string permission)
    {
        if (string.IsNullOrEmpty(permission))
            return true;

        string userPermission = SessionManager.CurrentUserPermissions;

        if (string.IsNullOrEmpty(userPermission))
            return false;

        var permissionList = userPermission.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        var hasPermission = permissionList.Any(x => x.Equals(permission, StringComparison.OrdinalIgnoreCase));
        return hasPermission;
    }

    public static string GetProductnameOfClient(int ASClient)
    {
        string spaName = "spa_SEC_GetClientForUpdate";
        FilterParameterCollection parameters = new FilterParameterCollection();
        parameters.Add(new FilterParameter("@ASClient", ASClient, DbType.Int32));
        DataTable dt = WebServices.SecurityServices.GetReports(spaName, parameters);
        if (dt != null && dt.Rows.Count > 0)
        {
            string productName = VeraCodeSolution.DoVeraCode(dt.Rows[0]["ProductName"].ToString());
            return productName;
        }

        return "";
    }
    public static int GetUserNameMaxLength(int clientId)
    {
        var defaultMaxLength = 10;
        var maxLength = 30;
        var config = GetDataOfExtendedSetting("CUSTOM_CLIENT_MAXLENGTH", "0");
        var customUserNameMaxLength = GetDataOfExtendedSetting("CUSTOM_USERNAME_MAXLENGTH", "0");

        if (string.IsNullOrEmpty(config))
            return defaultMaxLength;

        if (!string.IsNullOrEmpty(customUserNameMaxLength))
            maxLength = int.Parse(customUserNameMaxLength);

        var clientConfig = config.Split(',').ToList();
        if (config.Equals("All", StringComparison.OrdinalIgnoreCase) || clientConfig.Any(x => x.Equals(clientId.ToString())))
        {
            var isChangeUserNameOnMS = GetDataOfExtendedSetting("IS_CHANGE_USERNAME_MS_SITE").ToLower().Equals("true");
            var isMSUser = SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS;

            //for secondary user & not allow change user name on MS site
            if (!(isChangeUserNameOnMS && isMSUser) && SessionManager.UsePrimaryPrefix)
            {
                string prefixName = SessionManager.CurrentUser.EntityID + "-";
                return maxLength - prefixName.Length;
            }
            return maxLength; 
        }

        return defaultMaxLength;
    }
    public static int GetUserNameMaxLengthForUserProfile(int clientId)
    {         
        var maxLength = 30;
        var config = GetDataOfExtendedSetting("CUSTOM_CLIENT_MAXLENGTH", "0");
        var customUserNameMaxLength = GetDataOfExtendedSetting("CUSTOM_USERNAME_MAXLENGTH", "0");

        if (!string.IsNullOrEmpty(config))
        {
            var isChangeUserNameOnMS = GetDataOfExtendedSetting("IS_CHANGE_USERNAME_MS_SITE").ToLower().Equals("true");
            var isMSUser = SessionManager.CurrentSystem == WebSiteConstants.AS_SYSTEM_MS;
            var clientConfig = config.Split(',').ToList();

            if (config.Equals("All", StringComparison.OrdinalIgnoreCase) || clientConfig.Any(x => x.Equals(clientId.ToString())))
            {
                if (!string.IsNullOrEmpty(customUserNameMaxLength))
                    maxLength = int.Parse(customUserNameMaxLength);

                if (!isMSUser)
                {
                    return maxLength;
                }
                //for secondary user & not allow change user name on MS site
                else if (!isChangeUserNameOnMS && SessionManager.UsePrimaryPrefix)
                {
                    string prefixName = SessionManager.CurrentUser.EntityID + "-";
                    return maxLength - prefixName.Length;
                }
                return maxLength;
            }
            else if (isMSUser && !isChangeUserNameOnMS && SessionManager.UsePrimaryPrefix)
            {
                string prefixName = SessionManager.CurrentUser.EntityID + "-";
                return maxLength - prefixName.Length;
            }
        }

        var defaultMaxLength = GetDataOfExtendedSetting("CUSTOM_DEFAULT_MAXLENGTH", "0");
        return int.Parse(string.IsNullOrEmpty(defaultMaxLength)? UserProfileConstants.CUSTOM_DEFAULT_MAXLENGTH.ToString() : defaultMaxLength);
    }
}

public enum ShowRiskMCF
{
    All = 0,
    OldRisk = 1,
    MCFRisk = 2,
}

