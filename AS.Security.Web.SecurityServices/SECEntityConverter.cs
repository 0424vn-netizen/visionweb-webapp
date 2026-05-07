using System;
using System.Collections.Generic;
using System.Text;

using AS.Security.WS.Entities;
using ASUser = AS.Security.WS.Entities.User;
using ASUserCollection = AS.Security.WS.Entities.UserCollection;
namespace AS.Security.Web.SecurityServices
{
    public static class SecEntityConverter
    {
        public static ASUser ConvertASUser(SecService.User item)
        {
            if (item == null) return null;
            ASUser newUser = new ASUser();
            newUser.RecId = item.RecId;
            newUser.ASClient = item.ASClient;
            newUser.SiteID = item.SiteID;
            newUser.UserID = item.UserID;
            newUser.UserSecRole = item.UserSecRole == null ? string.Empty : item.UserSecRole;
            newUser.UserNameFirst = item.UserNameFirst;
            newUser.UserNameLast = item.UserNameLast;
            newUser.UserNameFull = item.UserNameFull;
            newUser.UserPassword = item.UserPassword;
            newUser.UserPasswordType = item.UserPasswordType;
            newUser.Email = item.Email;
            newUser.LoginQuestionIndex = item.LoginQuestionIndex;
            newUser.LoginQuestionAnswer = item.LoginQuestionAnswer;
            newUser.ActvStat = item.ActvStat;
            newUser.EntityID = item.EntityID;
            newUser.EntityType = item.EntityType;
            newUser.InitialID = item.InitialID;
            newUser.UserType = item.UserType;
            newUser.PrevLoginDTS = item.PrevLoginDTS;
            newUser.LoginAttempts = item.LoginAttempts;

            newUser.LastLoginDTS = item.LastLoginDTS;
            newUser.OriginalUserID = item.OriginalUserID;
            newUser.SalesRepCode = item.SalesRepCode;
            newUser.PhoneForSMS = item.PhoneForSMS;
            newUser.ContactEmail = item.ContactEmail;
            SecurityService srv = new SecurityService();
            srv.AddRequestHeader("ClientId", item.ASClient.ToString());
            PermissionCollection pers = srv.GetPermissionsForUser(newUser.ASClient, newUser.UserID);
            for (int i = 0; i < pers.Count; i++)
            {
                if (pers[i].PermissionCode == "MSOptMerch")
                {
                    newUser.OptInOut = 'Y';
                }
                if (pers[i].PermissionCode == "MSResetMerchPW")
                {
                    newUser.RstMerPw = 'Y';
                }
            }
            return newUser;
        }

        public static ASUserCollection BuildASUserCollection(SecService.User[] list)
        {
            if (list == null) return new ASUserCollection();
            ASUserCollection collection = new ASUserCollection();
            foreach (SecService.User item in list)
            {
                collection.Add(ConvertASUser(item));
            }
            return collection;
        }
        public static ASUserCollection BuildASUserCollection(SecService.User[] list, bool withCustomProperties)
        {
            if (list == null) return new ASUserCollection();
            ASUserCollection collection = new ASUserCollection();
            foreach (SecService.User item in list)
            {

                collection.Add(ConvertASUser(item));
            }
            return collection;
        }

        public static Hierarchy ConvertHierarchy(SecService.Hierarchy item)
        {
            if (item == null) return null;
            var result = new Hierarchy();
            result.HierarchyID = item.HierarchyID;
            result.SystemId = item.SystemId;
            result.HierarchyName = item.HierarchyName;
            result.HierarchyParent = item.HierarchyParent;
            result.HierarchyDescription = item.HierarchyDescription;
            result.ActvStatus = item.ActvStatus;
            result.ClientId = item.ClientId;
            result.HierarchyCode = item.HierarchyCode;
            result.UserRoleType = item.UserRoleType;
            result.HierarchyLevel = item.HierarchyLevel;
            result.UserCount = item.UserCount;
            result.IsPredefined = item.IsPredefined;
            result.CreatedDate = item.CreatedDate;

            return result;

        }

        public static HierarchyCollection BuildHierarchyCollection(SecService.Hierarchy[] list)
        {
            if (list == null) return new HierarchyCollection();
            HierarchyCollection collection = new HierarchyCollection();
            foreach (SecService.Hierarchy item in list)
            {
                collection.Add(ConvertHierarchy(item));
            }
            return collection;
        }


        public static Permission ConvertPermission(SecService.Permission item)
        {
            if (item == null) return null;

            var result = new Permission()
            {
                PermissionId = item.PermissionId,
                SystemId = item.SystemId,
                PermissionCode = item.PermissionCode,
                Description = item.Description,
                DateCreated = item.DateCreated,
                Group = item.Group,
                GroupFuncName = item.GroupFuncName,
                NodeOrder = item.NodeOrder,
                PermissionCodesRequire = item.PermissionCodesRequire
            };

            return result;
        }

        public static PermissionCollection BuildPermissionCollection(SecService.Permission[] list)
        {
            if (list == null) return new PermissionCollection();
            PermissionCollection collection = new PermissionCollection();
            foreach (SecService.Permission item in list)
            {
                collection.Add(ConvertPermission(item));
            }
            return collection;
        }

        public static SsoPermissionXlat ConvertSsoPermissionXlat(SecService.SsoPermissionXlat item)
        {
            if (item == null) return null;
            return new SsoPermissionXlat(item.SecurityLevelId, item.SecurityLevelName, item.PermissionId, item.IsExcluded, item.ClientId, item.PartnerId, item.PermissionCode);
        }


        public static SsoPermissionXlatCollection BuildSsoPermissionXlatCollection(SecService.SsoPermissionXlat[] list)
        {
            if (list == null) return new SsoPermissionXlatCollection();
            SsoPermissionXlatCollection collection = new SsoPermissionXlatCollection();
            foreach (SecService.SsoPermissionXlat item in list)
            {
                collection.Add(ConvertSsoPermissionXlat(item));
            }
            return collection;
        }

        public static SecMenuItem ConvertSecMenuItem(SecService.SecMenuItem item)
        {
            if (item == null) return null;

            var result = new SecMenuItem(item.SiteMapId, item.SystemId, item.Url, item.Title, item.Description, item.Permissions, item.Parent);
            result.NodeOrder = item.NodeOrder;
            result.DisplayType = item.DisplayType;

            return result;
        }

        public static SecMenuItemCollection BuildSECMenuCollection(SecService.SecMenuItem[] list)
        {
            if (list == null) return new SecMenuItemCollection();
            SecMenuItemCollection collection = new SecMenuItemCollection();
            foreach (SecService.SecMenuItem item in list)
            {
                collection.Add(ConvertSecMenuItem(item));
            }
            return collection;
        }

        public static RefTableValue ConvertRefTableValue(SecService.RefTableValue item)
        {
            if (item == null) return null;

            var result = new RefTableValue()
            {
                ASClient = item.ASClient,
                RefTblName = item.RefTblName,
                RefTblKey = item.RefTblKey,
                RefTblKey1 = item.RefTblKey1,
                RefTblLang = item.RefTblLang,
                EffStartDTS = item.EffStartDTS,
                EffEndDTS = item.EffEndDTS,
                RefTblCols = item.RefTblCols,
                ActvStatus = item.ActvStatus
            };

            return result;
        }

        public static RefTableValueCollection BuildReTableValueCollection(SecService.RefTableValue[] list)
        {
            if (list == null) return new RefTableValueCollection();
            RefTableValueCollection collection = new RefTableValueCollection();
            foreach (SecService.RefTableValue item in list)
            {
                collection.Add(ConvertRefTableValue(item));
            }
            return collection;
        }
        public static ASTheme ConvertASThemes(SecService.ASTheme item)
        {
            if (item == null) return null;
            return new ASTheme(item.ThemeId, item.ThemeName, item.ThemeImage, item.Description, item.DateCreated, item.ActvStatus);

        }

        public static ASThemeCollection BuildASThemesCollection(SecService.ASTheme[] list)
        {
            if (list == null) return new ASThemeCollection();
            ASThemeCollection collection = new ASThemeCollection();
            foreach (SecService.ASTheme item in list)
            {
                collection.Add(ConvertASThemes(item));
            }
            return collection;
        }

        public static AppConfig ConvertAppConfig(SecService.AppConfig item)
        {
            if (item == null) return null;
            return new AppConfig(item.AppCode, item.KeyName, item.KeyValue, item.SortSeq);

        }

        public static List<AppConfig> BuildConvertAppConfigCollection(SecService.AppConfig[] list)
        {
            if (list == null) return new List<AppConfig>();
            List<AppConfig> collection = new List<AppConfig>();
            foreach (SecService.AppConfig item in list)
            {
                collection.Add(ConvertAppConfig(item));
            }
            return collection;
        }
        public static HierarchyAccessCollection BuildHierarchyAccessCollection(AS.Security.Web.SecurityServices.SecService.HierarchyAccess[] list)
        {
            if (list == null) return new HierarchyAccessCollection();
            HierarchyAccessCollection collection = new HierarchyAccessCollection();
            foreach (AS.Security.Web.SecurityServices.SecService.HierarchyAccess item in list)
            {
                collection.Add(ConvertHierarchyAccess(item));
            }
            return collection;
        }
        public static HierarchyAccess ConvertHierarchyAccess(AS.Security.Web.SecurityServices.SecService.HierarchyAccess item)
        {
            if (item == null) return null;

            var result = new HierarchyAccess() 
            {
                RecId = item.RecId,
                HierarchyId = item.HierarchyId,
                EntityNumber = item.EntityNumber,
                Type = item.Type,
                UserIDCreate = item.UserIDCreate,
                CreateDate = item.CreateDate,
                DisplayText = item.DisplayText,
                Login = item.Login,
                ClientName = item.ClientName,
            };

            return result;
        }

    }
}
