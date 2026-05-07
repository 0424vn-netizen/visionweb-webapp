using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// Summary description for MerchantProfileUtils
/// </summary>
public static class MerchantProfileUtils
{
    public static bool CheckPermissionAddEditChain(object siteAccess, bool isAddEditChainWithAdditionData)
    {
        bool hasPermissionAddEditChain = false;
        bool hideAddEdit = GeneralFuncsLib.HideSessionAddEditChain();

        if (!hideAddEdit && GeneralFuncsLib.HasUserPermission("AddEditChain")
            && (MerchantProfileHelper.SiteAccessIsOptInOut(siteAccess) || isAddEditChainWithAdditionData))
        {
            hasPermissionAddEditChain = true;
        }
        return hasPermissionAddEditChain;
    }
    public static bool CheckPermissionAddEditSecondaryAccessChain(object siteAccess)
    {
        bool hasPermissionAddEditChain = false;
        bool hideAddEdit = GeneralFuncsLib.HideSessionAddEditChain();

        if (!hideAddEdit && GeneralFuncsLib.HasUserPermission("AddEditAccessChain")
            && MerchantProfileHelper.SiteAccessIsOptInOut(siteAccess))
        {
            hasPermissionAddEditChain = true;
        }
        return hasPermissionAddEditChain;
    }
    public static List<TemplateConfig> GetLoopKey(string content)
    {
        var result = new List<TemplateConfig>();
        if (string.IsNullOrEmpty(content))
            return result;

        var templateKeys = GetAllBetween(content, "<!--LOOP_START-->", "<!--LOOP_END-->");
        if (templateKeys != null && templateKeys.Count > 0)
        {
            foreach (Match m in templateKeys)
            {
                var originKey = m.Value;
                var value = m.Groups[1].Value;

                var loopName = GetAllBetween(originKey, "<!--Loop:", ":Loop-->");
                var modelName = loopName[0].Groups[1].Value;
                var modelKey = loopName[0].Value;

                if (string.IsNullOrEmpty(modelName))
                    content = content.Replace(originKey, string.Empty);
                else
                {
                    var config = new TemplateConfig()
                    {
                        OriginalKey = originKey,
                        Value = value.Replace(modelKey, string.Empty),
                        Source = modelName,
                        Type = TemplateConfigType.Loop
                    };

                    result.Add(config);
                }
            }
        }

        return result;
    }
    public static List<TemplateConfig> GetPermissionKey(string content, string start, string end)
    {
        var result = new List<TemplateConfig>();
        if (string.IsNullOrEmpty(content))
            return result;

        var templateKeys = GetAllBetween(content, start, end);
        if (templateKeys != null && templateKeys.Count > 0)
        {
            foreach (Match m in templateKeys)
            {
                var originKey = m.Value;
                var value = m.Groups[1].Value;

                var loopName = GetAllBetween(originKey, "<!--Name:", ":Name-->");
                var permissionName = loopName[0].Groups[1].Value;
                var modelKey = loopName[0].Value;

                if (string.IsNullOrEmpty(permissionName))
                    content = content.Replace(originKey, string.Empty);
                else
                {
                    var config = new TemplateConfig()
                    {
                        OriginalKey = originKey,
                        Value = value.Replace(modelKey, string.Empty),
                        Source = permissionName,
                        Type = TemplateConfigType.Permission
                    };

                    result.Add(config);
                }
            }
        }

        return result;
    }
    public static MatchCollection GetAllBetween(string content, string start, string end)
    {
        var pattern = String.Format("{0}(.*?){1}", Regex.Escape(start), Regex.Escape(end));
        var result = new List<string>();
        return Regex.Matches(content, pattern, RegexOptions.Singleline);
    }
}