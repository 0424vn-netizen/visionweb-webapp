using AS.Common.DBManager;
using System.Collections.Generic;
using System.Data;
using BuGeneralFuncsLib =AS.Web.Business.General.GeneralFuncsLib;

/// <summary>
/// Summary description for ParameterService
/// </summary>
public static class ParameterService
{
    public static void SaveStagging(int mode, string assignmentID, List<ParameterFE> parameters)
    {
        SaveStagging(mode, assignmentID, parameters, false);
    }

    public static void SaveStagging(int mode, string assignmentID, List<ParameterFE> parameters, bool isMCFRisk)
    {
        List<RiskParameter> paramList = GetAllParameterFromUI(parameters);
        if (paramList != null)
        {
            var paramStr = isMCFRisk ? paramList.ToListXmlString(true) : paramList.ToListXmlString();
            var filterParameters = new FilterParameterCollection();
            var paramOut = new FilterParameterCollection();
            filterParameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
            filterParameters.Add("@AssignmentID", WebServices.SecurityServices.DecryptText(assignmentID), DbType.Int32);
            filterParameters.Add("@ParameterList", paramStr, DbType.Xml);
            filterParameters.Add("@Mode", mode, DbType.Int32);
            string spa = isMCFRisk ? "spa_RM_MCF_SaveParameterListByAssignment" : "spa_rm_cs_SaveParameterListByAssignment";
            WebServices.RiskServices.ExecuteNonQueryCommand(spa, filterParameters, out paramOut);
        }
    }

    public static string GetTransactionCode(string mode, string assignmentID, string paramKey, string filterID)
    {
        return GetTransactionCode(mode, assignmentID, paramKey, filterID, false)["Label"];
    }

    public static Dictionary<string, string> GetTransactionCode(string mode, string assignmentID, string paramKey, string filterID, bool isMCFRisk)
    {
        FilterParameterCollection parames = new FilterParameterCollection();
        parames.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parames.Add(new FilterParameter("@AssignmentID", WebServices.SecurityServices.DecryptText(assignmentID), DbType.Int32));
        parames.Add(new FilterParameter("@ParameterID", paramKey, DbType.String));
        parames.Add(new FilterParameter("@Option", WebSiteEnums.AssignmentFilterModes.Assigned, DbType.Int32));
        parames.Add(new FilterParameter("@Mode", mode.ToInt(), DbType.Int32));
        string spa = isMCFRisk ? GetSpaNameByParameterId(paramKey, SpaType.Get) : "spa_rm_ParameterFilter_AuthTransactionCode_Get";
        DataTable dt = WebServices.RiskServices.GetReports(spa, parames);
        return ConvertTable2String(dt, paramKey);
    }

    public static void RemoveParametersToStagging(string assignmentID, string paramKey)
    {
        FilterParameterCollection paramesIn = new FilterParameterCollection();
        paramesIn.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        paramesIn.Add(new FilterParameter("@AssignmentID", WebServices.SecurityServices.DecryptText(assignmentID), DbType.Int32));
        paramesIn.Add(new FilterParameter("@ParameterID", WebServices.SecurityServices.DecryptText(paramKey), DbType.String));
        paramesIn.Add(new FilterParameter("@FilterValues", string.Empty, DbType.String));

        FilterParameterCollection paramesOut = new FilterParameterCollection();
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_RM_MCF_ParameterFilter_ModelType_Save", paramesIn, out paramesOut);
    }

    private static List<RiskParameter> GetAllParameterFromUI(List<ParameterFE> Parameters)
    {
        var list = new List<RiskParameter>();
        foreach (var parameter in Parameters)
        {
            RiskParameter p = new RiskParameter();
            p.GroupID = parameter.GroupID.ToInt();
            p.IsSourceParam = parameter.IsSourceParam == "1";

            switch (parameter.ParameterThresholdType)
            {
                case "LowHigh":
                    if (!parameter.ThresholdLow.IsNullOrEmpty())
                    {
                        double cT = double.Parse(parameter.ThresholdLow);
                        if (!parameter.IsThresholdNegative.IsNullOrEmpty() && parameter.IsThresholdNegative == "True" && cT > 0)
                            p.ThresholdValue = -cT;
                        else
                            p.ThresholdValue = cT;
                    }
                    else
                    {
                        p.ThresholdValue = null;
                    }

                    if (!parameter.ThresholdHigh.IsNullOrEmpty())
                    {
                        double cT = double.Parse(parameter.ThresholdHigh);
                        if (!parameter.IsThresholdNegative.IsNullOrEmpty() && parameter.IsThresholdNegative == "True" && cT > 0)
                            p.ThresholdHigh = -cT;
                        else
                            p.ThresholdHigh = cT;
                    }
                    else
                    {
                        p.ThresholdHigh = null;
                    }

                    //NRT Threshold
                    if (!parameter.ReAlertThresholdLow.IsNullOrEmpty())
                    {
                        double cTNRT = double.Parse(parameter.ReAlertThresholdLow);
                        if (!parameter.IsThresholdNegative.IsNullOrEmpty() && parameter.IsThresholdNegative == "True" && cTNRT > 0)
                            p.ReAlertThresholdValue = -cTNRT;
                        else
                            p.ReAlertThresholdValue = cTNRT;
                    }
                    else
                    {
                        p.ReAlertThresholdValue = null;
                    }

                    if (!parameter.ReAlertParameterThresholdHigh.IsNullOrEmpty())
                    {
                        double cTNRT = double.Parse(parameter.ReAlertParameterThresholdHigh);
                        if (!parameter.IsThresholdNegative.IsNullOrEmpty() && parameter.IsThresholdNegative == "True" && cTNRT > 0)
                            p.ReAlertThresholdHigh = -cTNRT;
                        else
                            p.ReAlertThresholdHigh = cTNRT;
                    }
                    else
                    {
                        p.ReAlertThresholdHigh = null;
                    }

                    break;
                default:
                    if (!parameter.Threshold.IsNullOrEmpty())
                    {
                        double cT = double.Parse(parameter.Threshold);
                        if (!parameter.IsThresholdNegative.IsNullOrEmpty() && parameter.IsThresholdNegative == "True" && cT > 0)
                            p.ThresholdValue = -cT;
                        else
                            p.ThresholdValue = cT;
                        //p.ThresholdValue = cT;
                    }
                    else
                    {
                        p.ThresholdValue = null;
                    }

                    if (!parameter.ReAlertParameterThreshold.IsNullOrEmpty())
                    {
                        double cTNRT = double.Parse(parameter.ReAlertParameterThreshold);
                        if (!parameter.IsThresholdNegative.IsNullOrEmpty() && parameter.IsThresholdNegative == "True" && cTNRT > 0)
                            p.ReAlertThresholdValue = -cTNRT;
                        else
                            p.ReAlertThresholdValue = cTNRT;
                        //p.ThresholdValue = cT;
                    }
                    else
                    {
                        p.ReAlertThresholdValue = null;
                    }

                    break;
            }

            p.Key = WebServices.SecurityServices.DecryptText(parameter.ParameterKey);
            if (parameter.IsParameterValueVisible)//(criteriaNumeric.Visible)
            {
                if (!parameter.ParameterValue.IsNullOrEmpty())
                {
                    double cN = double.Parse(parameter.ParameterValue.Replace("(", string.Empty).Replace(")", string.Empty));

                    if (!parameter.IsIndicatorNegative.IsNullOrEmpty() && parameter.IsIndicatorNegative == "True" && cN > 0)
                        p.IndicatorValue = -cN;
                    else
                        p.IndicatorValue = cN;
                    //p.IndicatorValue = cN;
                }
                else
                {
                    p.IndicatorValue = null;
                }
            }
            else
            {
                if (!parameter.From.IsNullOrEmpty() && !parameter.To.IsNullOrEmpty())
                {
                    p.IndicatorValue = double.Parse(parameter.From);
                    p.IndicatorHigh = double.Parse(parameter.To);
                }
                else
                {
                    p.IndicatorValue = null;
                    p.IndicatorHigh = null;
                }
            }

            //NRT Indicator

            if (parameter.IsParameterValueVisible)//(criteriaNumeric.Visible)
            {
                if (!parameter.ReAlertParameterIndicator.IsNullOrEmpty())
                {
                    double cNNRT = double.Parse(parameter.ReAlertParameterIndicator.Replace("(", string.Empty).Replace(")", string.Empty));

                    if (!parameter.IsIndicatorNegative.IsNullOrEmpty() && parameter.IsIndicatorNegative == "True" && cNNRT > 0)
                        p.ReAlertIndicatorValue = -cNNRT;
                    else
                        p.ReAlertIndicatorValue = cNNRT;
                    //p.IndicatorValue = cN;
                }
                else
                {
                    p.ReAlertIndicatorValue = null;
                }
            }
            else
            {
                if (!parameter.FromNRT.IsNullOrEmpty() && !parameter.ToNRT.IsNullOrEmpty())
                {
                    p.ReAlertIndicatorValue = double.Parse(parameter.FromNRT);
                    p.ReAlertIndicatorHigh = double.Parse(parameter.ToNRT);
                }
                else
                {
                    p.ReAlertIndicatorValue = null;
                    p.ReAlertIndicatorHigh = null;
                }
            }

            p.PrecisionValue = parameter.ParameterPrecision.ToInt();
            list.Add(p);
        }
        return list;
    }

    private static Dictionary<string, string> ConvertTable2String(DataTable dt, string paramKey)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        string result = string.Empty;
        string tracking = string.Empty;

        if (dt == null || dt.Rows.Count == 0)
        {
            result = tracking = "N/A";
            data.Add("Label", result);
            data.Add("Tracking", tracking);
            return data;
        }
        var isModelType = IsModelTypeInConfig(paramKey);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            var dataText = isModelType ? BuGeneralFuncsLib.SliptByCapitalLetter(dt.Rows[i]["DataText"].ToString()) : dt.Rows[i]["DataText"];
            result += dataText + ", ";
            tracking += dt.Rows[i]["DataKey"] + ", ";
        }

        result = result.Trim().TrimEnd(',');
        tracking = result.Trim().TrimEnd(',');

        data.Add("Label", result);
        data.Add("Tracking", tracking);

        return data;
    }
    private static string GetSpaNameByParameterId(string paramID, SpaType spaType)
    {
        var isModelType = IsModelTypeInConfig(paramID);
        switch (spaType)
        {
            case SpaType.Get:
                if (isModelType)
                    return "spa_RM_MCF_ParameterFilter_ModelType_Get";
                return "spa_RM_MCF_ParameterFilter_AuthTransactionCode_Get";
            default:
                return null;
        }
    }
    private static bool IsModelTypeInConfig(string paramID)
    {
        return BuGeneralFuncsLib.CheckExistsInSplitToArray(WebSiteSettings.ParametersAllowDecimal, ',', paramID);
    }
    
}