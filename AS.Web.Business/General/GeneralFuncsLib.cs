using AS.Security.WS.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AS.Web.Business.General
{
    public static partial class GeneralFuncsLib
    {
        public static string NvlString(object val)
        {
            return (val == null || val == DBNull.Value ? string.Empty : val.ToString());
        }

        public static string[] GetRDRColumnNames(string page)
        {
            switch (page)
            {
                case "ChargebacksDetail":
                    return new string[] { "FirstChargebackRDRAmount", "PostChargebackRDRAmount" };

                case "rm_MCF_RetCb":
                    return new string[]
                    {
                        "FirstChargebackRDRCount", "FirstChargebackRDRAmount", "PostChargebackRDRCount", "PostChargebackRDRAmount",
                        "ThirtyDaysFirstChargebackRDRCount", "ThirtyDaysFirstChargebackRDRAmount", "ThirtyDaysPostChargebackRDRCount", "ThirtyDaysPostChargebackRDRAmount",
                        "NinetyDaysFirstChargebackRDRCount", "NinetyDaysFirstChargebackRDRAmount", "NinetyDaysPostChargebackRDRCount", "NinetyDaysPostChargebackRDRAmount",
                    };

                default:
                    return new string[] { "FirstChargebackRDRCount", "FirstChargebackRDRAmount", "PostChargebackRDRCount", "PostChargebackRDRAmount" };
            }
        }

        public static T GetValue<T>(DataRow dataRow, string columnName)
        {
            if (!string.IsNullOrEmpty(columnName) && dataRow.Table.Columns.Contains(columnName))
            {
                var value = dataRow[columnName];
                if (value != null && value != DBNull.Value)
                    return (T)Convert.ChangeType(value, typeof(T));
            }

            return default;
        }
        public static PermissionCollection BuildPermissionCollectionWithOrderByGroupName(PermissionCollection pers)
        {
            if (pers == null) return new PermissionCollection();
            PermissionCollection collection = new PermissionCollection();
            var lstOrderBy = (from Permission a in pers.Cast<Permission>() select a)
                .OrderBy(x => x.GroupFuncName)
                .ThenBy(x => x.NodeOrder);
            foreach (Permission item in lstOrderBy)
            {
                collection.Add(item);
            }
            return collection;
        }

        public static string EncryptComment(string comment, string hdCardDetected)
        {
            if (string.IsNullOrEmpty(comment) || string.IsNullOrEmpty(hdCardDetected))
            {
                return comment;
            }
            string lstCard = hdCardDetected.TrimEnd(',');
            if (!string.IsNullOrEmpty(lstCard))
            {
                foreach (var item in lstCard.Split(','))
                {
                    string realItem = item.Replace(" ", "").Replace("-", "");
                    string replaceItem = realItem.Substring(0, 6) + "xxxxxx" + realItem.Substring(realItem.Length - 4, 4);
                    comment = comment.Replace(item, replaceItem);
                }
            }

            return comment;
        }

        public static bool CheckUserPermissions(string currentUserPermissions, char separator, List<string> codePermissions)
        {
            if (string.IsNullOrEmpty(currentUserPermissions) || codePermissions == null || codePermissions.Count  <= 0)
            {
                return false;
            }
            var lstCurrentUserPermissions = SplitToArray(currentUserPermissions, separator);
            var result = lstCurrentUserPermissions
                .Exists(x => codePermissions.Exists(y => y.Equals(x, StringComparison.OrdinalIgnoreCase)));
            return result;
        }
    }
}
