using System;
using System.Collections.Generic;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;
using System.Text;
using AS.Common;
using System.Xml.Serialization;
using BuGeneralFuncsLib = AS.Web.Business.General.GeneralFuncsLib;

#region Assignment

[Serializable]
public class HierarchyAssignment
{
    private DataTable _Left = null;
    public DataTable Left { get { return _Left; } set { _Left = value; } }

    private DataTable _Right = null;
    public DataTable Right { get { return _Right; } set { _Right = value; } }

    public string SelectedItems
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            if (_Right != null)
            {
                for (int i = 0; i < _Right.Rows.Count; i++)
                {
                    sb.Append(_Right.Rows[i][WebSiteConstants.DATA_KEY].ToString() + ",");

                }
            }
            return sb.ToString().Trim().TrimEnd(',');
        }
    }

    public string SelectedItemsAfterSorted
    {
        get
        {
            StringBuilder sb = new StringBuilder();
            DataTable list = GeneralFuncsLib.ToSortedDataTable(_Right, WebSiteConstants.DATA_TEXT);
            if (list != null)
            {
                for (int i = 0; i < list.Rows.Count; i++)
                {
                    sb.Append(list.Rows[i][WebSiteConstants.DATA_KEY].ToString() + ",");
                }
            }
            return sb.ToString().Trim().TrimEnd(',');
        }
    }
}

#endregion

#region Parameters

public static class RiskParameterExtensions
{
    #region constants
    private const string PARAM_CODE = "ParameterCode";
    private const string PARAM_ID = "ParameterID";
    private const string PARAM_KEY = "ParameterKey";
    private const string GROUP_NAME = "GroupName";
    private const string PARAM_DESC = "ParameterDescription";
    private const string PARAM_NAMEHIDE = "ParameterNameHide";
    private const string PARAM_DATATYPEHIDE = "ParameterDataTypeHide";
    private const string PARAM_NAME = "ParameterName";
    private const string PARAM_DATATYPE = "ParameterDataType";
    private const string PARAM_VALUE = "ParameterValue";
    private const string PARAM_VALUEHIGH = "ParameterValueHigh";
    private const string PARAM_FILTERTYPE = "ParameterFilter";
    private const string PARAM_THRESHOLD = "ParameterThreshold";
    private const string PARAM_PREC = "ParameterPrecision";
    private const string PARAM_GROUPNAME = "ParameterGroupName";
    private const string IS_ASSIGNED = "IsAssigned";
    private const string IS_SELECTED = "IsSelected";
    private const string PARAM_ROWNUM = "ParameterRowNumber";
    private const string ACTIVITY_STATUS = "ActivityStatus";
    private const string PARAM_GROUPNAMEDUMMY = "ParameterGroupNameDummy";
    private const string PARAM_THRESHOLDHIDE = "ParameterThresholdHide";
    private const string PARAM_THRESHOLDHIDENRT = "ParameterThresholdHideNRT";
    private const string NBSP = "&nbsp;";
    private const string PARAM_CHECKBOX = "ParameterCheckBox";
    private const string IS_NEW = "IsNew";
    private const string REALERT_PARAM_VALUE = "ReAlertParameterIndicator";
    private const string REALERT_PARAM_THRES = "ReAlertParameterThreshold";
    private const string REALERT_PARAM_VALUE_HIGH = "ReAlertParameterIndicatorHigh";
    private const string REALERT_PARAM_THRES_HIGH = "ReAlertParameterThresholdHigh";
    private static readonly Type typeDBNULL = Type.GetType("System.DBNull");
    private const string PARAM_VALUE_IS_EDIT = "IsEditReAlertParameterIndicator";
    private const string PARAM_THRES_IS_EDIT = "IsEditReAlertParameterThreshold";

    #endregion

    /// <summary>
    /// DDS - Get RiskParameter object by Key
    /// </summary>
    /// <param name="list"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static RiskParameter GetRiskParameterByKey(this List<RiskParameter> list, string key)
    {
        if (list == null)
            return null;
        return list.FirstOrDefault<RiskParameter>(p => p.Key == key);
    }
    /// <summary>
    /// DDS - Get RiskParameter object by ID (order number)
    /// </summary>
    /// <param name="list"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static RiskParameter GetRiskParameterByID(this List<RiskParameter> list, int id)
    {
        return list.FirstOrDefault<RiskParameter>(p => p.ID == id);
    }
    /// <summary>
    /// DDS - Get RiskParameter object by Name
    /// </summary>
    /// <param name="list"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static RiskParameter GetRiskParameterByName(this List<RiskParameter> list, string name)
    {
        return list.FirstOrDefault<RiskParameter>(p => p.Name == name);
    }
    /// <summary>
    /// DDS - Get RiskParameters by group names
    /// </summary>
    /// <param name="list"></param>
    /// <param name="groupName"></param>
    /// <returns></returns>
    public static List<RiskParameter> GetRiskParametersByGroupName(this List<RiskParameter> list, string groupName)
    {
        var query = from p in list
                    where p.GroupName == groupName
                    select p;
        return query.ToList<RiskParameter>();
    }
    /// <summary>
    /// DDS - Build RiskParameter structure based on the Telerik GridDataItem
    /// </summary>
    /// <param name="RiskParameter"></param>
    /// <param name="dataItem"></param>
    public static void GetDataFromGridDataItem(this RiskParameter parameter, GridDataItem dataItem, RiskParameter referer)
    {
        var code = dataItem[PARAM_CODE].Text;
        var id = dataItem[PARAM_ID].Text;
        var key = dataItem[PARAM_KEY].Text;
        var name = dataItem[PARAM_NAMEHIDE].Text;
        var indType = dataItem[PARAM_DATATYPEHIDE].Text;
        var groupName = dataItem[GROUP_NAME].Text;
        var desc = dataItem[PARAM_DESC].Text;

        var ParameterThresholdType = dataItem["ParameterThresholdType"].Text;

        bool isAssigned = false;

        //============= is assigned =================================
        HtmlInputCheckBox chkBox = dataItem[PARAM_CHECKBOX].FindControl("chkRowIndex") as HtmlInputCheckBox;
        isAssigned = chkBox.Checked;

        //============ indicator value ===============================
        double? indVal = null;
        double? indValHigh = null;
        TextBox txt = dataItem[PARAM_VALUE].FindControl("txtParameterValue") as TextBox;
        if (txt != null && txt.Visible)
        {
            string str_indVal = txt.Text;
            string precision = txt.Attributes["Precision"];
            if (dataItem["IndicatorNegative"].Text.ToString() != "" && dataItem["IndicatorNegative"].Text.ToString() == "Yes")
            {
                indVal = str_indVal.Trim() == string.Empty ? referer.IndicatorValue : -double.Parse(str_indVal.Trim());
            }
            else
            {
                indVal = str_indVal.Trim() == string.Empty ? referer.IndicatorValue : double.Parse(str_indVal.Trim());
            }
            //reupdate the textbox
            if (str_indVal.Trim() == string.Empty)
            {
                //[44432]: Bug #37956
                if (!string.IsNullOrEmpty(precision))
                {
                    if (int.Parse(precision) == 2)
                        txt.Text = VeraCodeSolution.DoVeraCode(referer.IndicatorValue.Value.ToString("#,#0.00"));
                    if (int.Parse(precision) == 4)
                        txt.Text = VeraCodeSolution.DoVeraCode(referer.IndicatorValue.Value.ToString("#,#0.0000"));
                }
                else
                    txt.Text = VeraCodeSolution.DoVeraCode(referer.IndicatorValue.Value.ToString("#,#0"));
            }
            txt.Enabled = isAssigned;
        }
        TextBox txtFrom = dataItem[PARAM_VALUE].FindControl("txtFrom") as TextBox;
        if (txtFrom.Visible)
        {
            string str_indVal = txtFrom.Text;
            indVal = str_indVal.Trim() == string.Empty ? referer.IndicatorValue : double.Parse(str_indVal.Trim());
            //reupdate the textbox
            if (str_indVal.Trim() == string.Empty)
                txtFrom.Text = VeraCodeSolution.DoVeraCode(referer.IndicatorValue.Value.ToString("#,#0"));
            txtFrom.Enabled = isAssigned;
        }
        TextBox txtTo = dataItem[PARAM_VALUE].FindControl("txtTo") as TextBox;
        if (txtTo.Visible)
        {
            string str_indVal = txtTo.Text;
            indValHigh = str_indVal.Trim() == string.Empty ? referer.IndicatorHigh : double.Parse(str_indVal.Trim());
            //reupdate the textbox
            if (str_indVal.Trim() == string.Empty)
                txtTo.Text = VeraCodeSolution.DoVeraCode(referer.IndicatorHigh.Value.ToString("#,#0"));
            txtTo.Enabled = isAssigned;
        }

        //======== threshold ========================================
        double? threshold = null;
        double? thresholdLow = null;
        double? thresholdHigh = null;

        if (dataItem[PARAM_THRESHOLDHIDE].Text.Trim().Replace(NBSP, "") != string.Empty)
        {
            TextBox txtThreshold = dataItem[PARAM_THRESHOLD].FindControl("txtThreshold") as TextBox;
            txtThreshold.Enabled = isAssigned;

            TextBox txtThresholdLow = dataItem[PARAM_THRESHOLD].FindControl("txtThresholdLow") as TextBox;
            txtThresholdLow.Enabled = isAssigned;

            TextBox txtThresholdHigh = dataItem[PARAM_THRESHOLD].FindControl("txtThresholdHigh") as TextBox;
            txtThresholdHigh.Enabled = isAssigned;

            switch (ParameterThresholdType)
            {
                case "LowHigh":
                    thresholdLow = ProcessThresholdFromGrid(dataItem, txtThresholdLow, referer.ThresholdValue);
                    thresholdHigh = ProcessThresholdFromGrid(dataItem, txtThresholdHigh, referer.ThresholdHigh);
                    break;
                default:
                    threshold = ProcessThresholdFromGrid(dataItem, txtThreshold, referer.ThresholdValue);
                    break;
            }

        }

        //===========================================================
        parameter.ActivityStatus = isAssigned ? 1 : 0;
        parameter.ID = int.Parse(id);
        parameter.Code = code.Replace(NBSP, "");
        parameter.Key = key;
        parameter.Name = name;
        parameter.IndicatorType = indType;
        parameter.IndicatorValue = indVal;
        parameter.IndicatorHigh = indValHigh;

        parameter.GroupName = groupName;
        parameter.ExtraInformations = (isAssigned ? "true" : "false");
        parameter.Description = desc;

        if (thresholdHigh != null)// for low-high threshod
        {
            parameter.ThresholdHigh = thresholdHigh;
            parameter.ThresholdValue = thresholdLow;
        }
        else
        {
            parameter.ThresholdValue = threshold;
            parameter.ThresholdHigh = null;
        }
    }

    public static void GetDataFromGridDataItemNRT(this RiskParameter parameter, GridDataItem dataItem, RiskParameter referer, bool IsEditIndicator, bool IsEditThreshold)
    {
        var code = dataItem[PARAM_CODE].Text;
        var id = dataItem[PARAM_ID].Text;
        var key = dataItem[PARAM_KEY].Text;
        var name = dataItem[PARAM_NAMEHIDE].Text;
        var indType = dataItem[PARAM_DATATYPEHIDE].Text;
        var groupName = dataItem[GROUP_NAME].Text;
        var desc = dataItem[PARAM_DESC].Text;
        var ParameterThresholdType = dataItem["ParameterThresholdType"].Text;

        bool isAssigned = false;

        //============= is assigned =================================
        HtmlInputCheckBox chkBox = dataItem[PARAM_CHECKBOX].FindControl("chkRowIndex") as HtmlInputCheckBox;
        isAssigned = chkBox.Checked;

        //============ indicator value ===============================
        double? indVal = null;
        double? indValHigh = null;
        TextBox txt = dataItem[PARAM_VALUE].FindControl("txtParameterValue") as TextBox;
        if (txt != null && txt.Visible)
        {
            string str_indVal = txt.Text;
            string precision = txt.Attributes["Precision"];
            if (dataItem["IndicatorNegative"].Text.ToString() != "" && dataItem["IndicatorNegative"].Text.ToString() == "Yes")
            {
                indVal = str_indVal.Trim() == string.Empty ? referer.IndicatorValue : -double.Parse(str_indVal.Trim());
            }
            else
            {
                indVal = str_indVal.Trim() == string.Empty ? referer.IndicatorValue : double.Parse(str_indVal.Trim());
            }
            //reupdate the textbox
            if (str_indVal.Trim() == string.Empty)
            {
                //[44432]: Bug #37956
                if (!string.IsNullOrEmpty(precision))
                {
                    if (int.Parse(precision) == 2)
                        txt.Text = VeraCodeSolution.DoVeraCode(referer.IndicatorValue.Value.ToString("#,#0.00"));
                    if (int.Parse(precision) == 4)
                        txt.Text = VeraCodeSolution.DoVeraCode(referer.IndicatorValue.Value.ToString("#,#0.0000"));
                    //if (int.Parse(precision) == 0)
                    //    txt.Text = VeraCodeSolution.DoVeraCode(referer.IndicatorValue.Value.ToString("#,#0"));
                }
                else
                    txt.Text = VeraCodeSolution.DoVeraCode(referer.IndicatorValue.Value.ToString("#,#0"));
            }
            txt.Enabled = isAssigned;
        }
        TextBox txtFrom = dataItem[PARAM_VALUE].FindControl("txtFrom") as TextBox;
        if (txtFrom.Visible)
        {
            string str_indVal = txtFrom.Text;
            indVal = str_indVal.Trim() == string.Empty ? referer.IndicatorValue : double.Parse(str_indVal.Trim());
            //reupdate the textbox
            if (str_indVal.Trim() == string.Empty)
                txtFrom.Text = VeraCodeSolution.DoVeraCode(referer.IndicatorValue.Value.ToString("#,#0"));
            txtFrom.Enabled = isAssigned;
        }
        TextBox txtTo = dataItem[PARAM_VALUE].FindControl("txtTo") as TextBox;
        if (txtTo.Visible)
        {
            string str_indVal = txtTo.Text;
            indValHigh = str_indVal.Trim() == string.Empty ? referer.IndicatorHigh : double.Parse(str_indVal.Trim());
            //reupdate the textbox
            if (str_indVal.Trim() == string.Empty)
                txtTo.Text = VeraCodeSolution.DoVeraCode(referer.IndicatorHigh.Value.ToString("#,#0"));
            txtTo.Enabled = isAssigned;
        }


        //============ ReAlert indicator value ===============================
        double? indValNRT = null;
        double? indValHighNRT = null;
        TextBox txtNRT = dataItem[REALERT_PARAM_VALUE].FindControl("txtNRTParameterValue") as TextBox;
        if (txtNRT != null && txtNRT.Visible)
        {
            string str_indVal_NRT = txtNRT.Text;
            string precisionNRT = txtNRT.Attributes["Precision"];
            if (dataItem["IndicatorNegative"].Text.ToString() != "" && dataItem["IndicatorNegative"].Text.ToString() == "Yes")
            {
                if ((dataItem[PARAM_CHECKBOX].FindControl("chkRowIndex") as HtmlInputCheckBox).Checked)
                {
                    indValNRT = string.IsNullOrEmpty(str_indVal_NRT.Trim()) ? double.MinValue : -double.Parse(str_indVal_NRT.Trim());
                }
                else
                {
                    indValNRT = string.IsNullOrEmpty(str_indVal_NRT.Trim()) ? !string.IsNullOrEmpty(referer.ReAlertIndicatorValue.ToString()) ? double.Parse(referer.ReAlertIndicatorValue.ToString()) : double.MinValue
                        : -double.Parse(str_indVal_NRT.Trim());
                }
            }
            else
            {
                if ((dataItem[PARAM_CHECKBOX].FindControl("chkRowIndex") as HtmlInputCheckBox).Checked)
                {
                    indValNRT = str_indVal_NRT.Trim() == string.Empty ? double.MinValue : double.Parse(str_indVal_NRT.Trim());
                }
                else
                {
                    indValNRT = str_indVal_NRT.Trim() == string.Empty ? (!(string.IsNullOrEmpty(referer.ReAlertIndicatorValue.ToString())) ?
                        double.Parse(referer.ReAlertIndicatorValue.ToString()) : double.MinValue) : double.Parse(str_indVal_NRT.Trim());
                }
            }
            //reupdate the textbox
            if (str_indVal_NRT.Trim() == string.Empty)
            {
                //[44432]: Bug #37956
                if (!string.IsNullOrEmpty(precisionNRT))
                {
                    if (int.Parse(precisionNRT) == 2)
                        txtNRT.Text = string.IsNullOrEmpty(referer.ReAlertIndicatorValue.ToString()) ? string.Empty : VeraCodeSolution.DoVeraCode(referer.ReAlertIndicatorValue.Value.ToString("#,#0.00"));
                    if (int.Parse(precisionNRT) == 4)
                        txtNRT.Text = string.IsNullOrEmpty(referer.ReAlertIndicatorValue.ToString()) ? string.Empty : VeraCodeSolution.DoVeraCode(referer.ReAlertIndicatorValue.Value.ToString("#,#0.0000"));
                    //if (int.Parse(precisionNRT) == 0)
                    //    txtNRT.Text = string.IsNullOrEmpty(referer.ReAlertIndicatorValue.ToString()) ? string.Empty : VeraCodeSolution.DoVeraCode(referer.ReAlertIndicatorValue.Value.ToString("#,#0"));
                }
                else
                    txtNRT.Text = VeraCodeSolution.DoVeraCode(referer.ReAlertIndicatorValue.Value.ToString("#,#0"));
            }
            txtNRT.Enabled = isAssigned;
        }
        TextBox txtNRTFrom = dataItem[PARAM_VALUE].FindControl("txtNRTFrom") as TextBox;
        if (txtNRTFrom.Visible)
        {
            string str_indVal_NRT = txtNRTFrom.Text;
            indValNRT = str_indVal_NRT.Trim() == string.Empty ? double.MinValue : double.Parse(str_indVal_NRT.Trim());
            //reupdate the textbox
            if (str_indVal_NRT.Trim() == string.Empty)
                txtNRTFrom.Text = string.Empty;
            txtNRTFrom.Enabled = isAssigned;
        }
        TextBox txtNRTTo = dataItem[REALERT_PARAM_VALUE].FindControl("txtNRTTo") as TextBox;
        if (txtNRTTo.Visible)
        {
            string str_indVal_NRT = txtNRTTo.Text;
            indValHighNRT = str_indVal_NRT.Trim() == string.Empty ? double.MinValue : double.Parse(str_indVal_NRT.Trim());
            //reupdate the textbox
            if (str_indVal_NRT.Trim() == string.Empty)
                txtNRTTo.Text = string.Empty;
            txtNRTTo.Enabled = isAssigned;
        }


        //======== threshold ========================================
        double? threshold = null;
        double? thresholdLow = null;
        double? thresholdHigh = null;

        if (dataItem[PARAM_THRESHOLDHIDE].Text.Trim().Replace(NBSP, "") != string.Empty)
        {
            TextBox txtThreshold = dataItem[PARAM_THRESHOLD].FindControl("txtThreshold") as TextBox;
            txtThreshold.Enabled = isAssigned;

            TextBox txtThresholdLow = dataItem[PARAM_THRESHOLD].FindControl("txtThresholdLow") as TextBox;
            txtThresholdLow.Enabled = isAssigned;

            TextBox txtThresholdHigh = dataItem[PARAM_THRESHOLD].FindControl("txtThresholdHigh") as TextBox;
            txtThresholdHigh.Enabled = isAssigned;

            switch (ParameterThresholdType)
            {
                case "LowHigh":
                    thresholdLow = ProcessThresholdFromGrid(dataItem, txtThresholdLow, referer.ThresholdValue);
                    thresholdHigh = ProcessThresholdFromGrid(dataItem, txtThresholdHigh, referer.ThresholdHigh);
                    break;
                default:
                    threshold = ProcessThresholdFromGrid(dataItem, txtThreshold, referer.ThresholdValue);
                    break;
            }

        }

        //======== ReAlert threshold ========================================
        double? thresholdNRT = null;
        double? thresholdLowNRT = null;
        double? thresholdHighNRT = null;

        if (dataItem[PARAM_THRESHOLDHIDENRT].Text.Trim().Replace(NBSP, "") != string.Empty || IsEditThreshold)
        {
            TextBox txtNRTThreshold = dataItem[REALERT_PARAM_THRES].FindControl("txtNRTThreshold") as TextBox;
            txtNRTThreshold.Enabled = isAssigned;

            TextBox txtNRTThresholdLow = dataItem[REALERT_PARAM_THRES].FindControl("txtNRTThresholdLow") as TextBox;
            txtNRTThresholdLow.Enabled = isAssigned;

            TextBox txtNRTThresholdHigh = dataItem[REALERT_PARAM_THRES].FindControl("txtNRTThresholdHigh") as TextBox;
            txtNRTThresholdHigh.Enabled = isAssigned;

            switch (ParameterThresholdType)
            {
                case "LowHigh":
                    thresholdLowNRT = ProcessThresholdFromGrid(dataItem, txtNRTThresholdLow, referer.ReAlertThresholdValue);
                    thresholdHighNRT = ProcessThresholdFromGrid(dataItem, txtNRTThresholdHigh, referer.ReAlertThresholdHigh);
                    break;
                default:
                    thresholdNRT = ProcessThresholdFromGrid(dataItem, txtNRTThreshold, referer.ReAlertThresholdValue);
                    break;
            }

        }
        //===========================================================
        parameter.ActivityStatus = isAssigned ? 1 : 0;
        parameter.ID = int.Parse(id);
        parameter.Code = code.Replace(NBSP, "");
        parameter.Key = key;
        parameter.Name = name;
        parameter.IndicatorType = indType;
        parameter.IndicatorValue = indVal;
        parameter.ReAlertIndicatorValue = (indValNRT == double.MinValue) ? null : indValNRT;
        parameter.IndicatorHigh = indValHigh;
        parameter.ReAlertIndicatorHigh = (indValHighNRT == double.MinValue) ? null : indValHighNRT;
        parameter.ReAlertThreshold = IsEditThreshold;
        parameter.ReAlertIndicator = IsEditIndicator;
        parameter.GroupName = groupName;
        parameter.ExtraInformations = (isAssigned ? "true" : "false");
        parameter.Description = desc;

        if (thresholdHigh != null)// for low-high threshod
        {
            parameter.ThresholdHigh = thresholdHigh;
            parameter.ThresholdValue = thresholdLow;
        }
        else
        {
            parameter.ThresholdValue = threshold;
            parameter.ThresholdHigh = null;
        }

        // ReAlert
        if (thresholdHighNRT != null || thresholdLowNRT != null)// for low-high threshod
        {
            parameter.ReAlertThresholdHigh = thresholdHighNRT;
            parameter.ReAlertThresholdValue = thresholdLowNRT;
        }
        else
        {
            parameter.ReAlertThresholdValue = thresholdNRT;
            parameter.ReAlertThresholdHigh = null;
        }
    }


    private static double? ProcessThresholdFromGrid(GridDataItem dataItem, TextBox txtThreshold, double? OldThresholdValue)
    {
        double? threshold = null;
        if (txtThreshold.Text.Trim() != string.Empty)
        {
            if (txtThreshold.Visible)
            {
                if (dataItem["ThresholdNegative"].Text.ToString() != "" && dataItem["ThresholdNegative"].Text.ToString() == "Yes" && txtThreshold.Text != "")
                {
                    if (double.Parse(txtThreshold.Text.Trim()) > 0)
                        threshold = -double.Parse(txtThreshold.Text.Trim());
                    else
                    {
                        threshold = double.Parse(txtThreshold.Text.Trim());
                    }
                }
                else
                {
                    threshold = double.Parse(txtThreshold.Text.Trim());
                }
            }
        }
        else if (txtThreshold.Text.Trim() == string.Empty)
        {

            //threshold = double.NaN;    //indicate user set empty for threshold
            if (dataItem[PARAM_THRESHOLDHIDE].Text.Trim().Replace(NBSP, "") == "0dds0")
            {
                txtThreshold.Text = string.Empty;
            }
            else
            {
                if (!txtThreshold.Enabled)
                {    //reload the old values for user to reference.
                    txtThreshold.Text = VeraCodeSolution.DoVeraCode(OldThresholdValue != null ?
                                            !double.IsNaN(OldThresholdValue.Value) ?
                                                OldThresholdValue.Value.ToString("#,#0")
                                                : string.Empty
                                            : string.Empty);
                    if (!string.IsNullOrEmpty(txtThreshold.Text) && double.Parse(txtThreshold.Text) > 0 && dataItem["ThresholdNegative"].Text.ToString() != "" && dataItem["ThresholdNegative"].Text.ToString() == "Yes")
                    {
                        threshold = txtThreshold.Text != string.Empty ? -double.Parse(txtThreshold.Text) : threshold;

                    }
                    else
                    {
                        threshold = txtThreshold.Text != string.Empty ? double.Parse(txtThreshold.Text) : threshold;
                    }
                }
            }
        }
        else
        {
            threshold = null;
        }
        return threshold;
    }
    /// <summary>
    /// DDS - Build Parameter structure based on the Microsoft's DataRow
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="row"></param>
    public static void GetDataFromTableRow(this RiskParameter parameter, DataRow row)
    {
        var code = row[PARAM_CODE].ToString();
        var id = row[PARAM_ID].ToString();
        var key = row[PARAM_KEY].ToString();
        var name = row[PARAM_NAME].ToString();
        var indType = row[PARAM_DATATYPE].ToString();
        var indVal = row[PARAM_VALUE].ToString();
        var indValHigh = row[PARAM_VALUEHIGH].ToString();
        var thresholdType = row[PARAM_THRESHOLD].GetType();
        var threshold = row[PARAM_THRESHOLD].ToString();
        var parameterFilter = row[PARAM_FILTERTYPE].ToString();
        var precision = row[PARAM_PREC].ToString();
        var groupName = row[PARAM_GROUPNAME].ToString();
        var isAssigned = row[IS_ASSIGNED].ToString().ToLower();
        var desc = row[PARAM_DESC].ToString();
        var activitystatus = row[ACTIVITY_STATUS].ToString();
        var thresholdHigh = row["ParameterThresholdHigh"].ToString();

        parameter.ID = int.Parse(id);
        parameter.Name = name;
        parameter.Code = code;
        parameter.Key = key;
        parameter.IndicatorType = indType;

        if (indVal.Trim() != string.Empty)
            parameter.IndicatorValue = double.Parse(indVal);
        else
            parameter.IndicatorValue = null;

        if (indValHigh.Trim() != string.Empty)
            parameter.IndicatorHigh = double.Parse(indValHigh);
        else
            parameter.IndicatorHigh = null;

        if (threshold.Trim() != string.Empty && threshold != "0dds0")
            parameter.ThresholdValue = double.Parse(threshold);
        else
        {
            if (thresholdType != typeDBNULL)
                parameter.ThresholdValue = double.NaN; //indicate empty
            else
                parameter.ThresholdValue = null;
        }

        parameter.ParameterFilter = int.Parse(parameterFilter);
        parameter.PrecisionValue = int.Parse(precision);
        parameter.GroupName = groupName;
        parameter.ExtraInformations = isAssigned;
        parameter.Description = desc;
        parameter.ActivityStatus = int.Parse(activitystatus);

    }

    public static void GetDataFromTableRowNRT(this RiskParameter parameter, DataRow row)
    {
        var code = row[PARAM_CODE].ToString();
        var id = row[PARAM_ID].ToString();
        var key = row[PARAM_KEY].ToString();
        var name = row[PARAM_NAME].ToString();
        var indType = row[PARAM_DATATYPE].ToString();
        var indVal = row[PARAM_VALUE].ToString();
        var indValHigh = row[PARAM_VALUEHIGH].ToString();
        var thresholdType = row[PARAM_THRESHOLD].GetType();
        var threshold = row[PARAM_THRESHOLD].ToString();
        var parameterFilter = row[PARAM_FILTERTYPE].ToString();
        var precision = row[PARAM_PREC].ToString();
        var groupName = row[PARAM_GROUPNAME].ToString();
        var isAssigned = row[IS_ASSIGNED].ToString().ToLower();
        var desc = row[PARAM_DESC].ToString();
        var activitystatus = row[ACTIVITY_STATUS].ToString();
        var thresholdHigh = row["ParameterThresholdHigh"].ToString();

        var isEditIndicatorNRT = row[PARAM_VALUE_IS_EDIT].ToBoolean();
        var isEditThresholdNRT = row[PARAM_THRES_IS_EDIT].ToBoolean();
        var indValNRT = row[REALERT_PARAM_VALUE].ToString();
        var indValHighNRT = row[REALERT_PARAM_VALUE_HIGH].ToString();
        var thresholdNRT = row[REALERT_PARAM_THRES].ToString();
        var thresholdHighNRT = row[REALERT_PARAM_THRES_HIGH].ToString();


        parameter.ID = int.Parse(id);
        parameter.Name = name;
        parameter.Code = code;
        parameter.Key = key;
        parameter.IndicatorType = indType;

        if (indVal.Trim() != string.Empty)
            parameter.IndicatorValue = double.Parse(indVal);
        else
            parameter.IndicatorValue = null;

        if (indValHigh.Trim() != string.Empty)
            parameter.IndicatorHigh = double.Parse(indValHigh);
        else
            parameter.IndicatorHigh = null;

        if (thresholdHigh.Trim() != string.Empty)
            parameter.ThresholdHigh = double.Parse(thresholdHigh);
        else
            parameter.ThresholdHigh = null;

        if (threshold.Trim() != string.Empty && threshold != "0dds0")
            parameter.ThresholdValue = double.Parse(threshold);
        else
        {
            if (thresholdType != typeDBNULL)
                parameter.ThresholdValue = double.NaN; //indicate empty
            else
                parameter.ThresholdValue = null;
        }

        // ReAlert 
        if (indValNRT.Trim() != string.Empty)
            parameter.ReAlertIndicatorValue = double.Parse(indValNRT);
        else
            parameter.ReAlertIndicatorValue = null;

        if (indValHighNRT.Trim() != string.Empty)
            parameter.ReAlertIndicatorHigh = double.Parse(indValHighNRT);
        else
            parameter.ReAlertIndicatorHigh = null;

        if (thresholdNRT.Trim() != string.Empty && thresholdNRT != "0dds0")
            parameter.ReAlertThresholdValue = double.Parse(thresholdNRT);
        else
        {
            if (thresholdType != typeDBNULL)
                parameter.ReAlertThresholdValue = double.NaN; //indicate empty
            else
                parameter.ReAlertThresholdValue = null;
        }

        if (thresholdHighNRT.Trim() != string.Empty)
            parameter.ReAlertThresholdHigh = double.Parse(thresholdHighNRT);
        else
            parameter.ReAlertThresholdHigh = null;

        parameter.ReAlertIndicator = isEditIndicatorNRT;
        parameter.ReAlertThreshold = isEditThresholdNRT;
        parameter.ParameterFilter = int.Parse(parameterFilter);
        parameter.PrecisionValue = int.Parse(precision);
        parameter.GroupName = groupName;
        parameter.ExtraInformations = isAssigned;
        parameter.Description = desc;
        parameter.ActivityStatus = int.Parse(activitystatus);

    }

    /// <summary>
    /// DDS - Build Parameter structure based on the Microsoft's DataRow for just Assignment
    /// </summary>
    /// <param name="parameter"></param>
    /// <param name="row"></param>
    public static void GetDataFromTableRow2(this RiskParameter parameter, DataRow row)
    {
        var code = row[PARAM_CODE].ToString();
        var id = row[PARAM_ID].ToString();
        var key = row[PARAM_KEY].ToString();
        var name = row[PARAM_NAME].ToString();
        var indType = row[PARAM_DATATYPE].ToString();
        var indVal = row[PARAM_VALUE].ToString();
        var indValNRT = row[REALERT_PARAM_VALUE].ToString();
        var indValHigh = row[PARAM_VALUEHIGH].ToString();
        var thresholdType = row[PARAM_THRESHOLD].GetType();
        var parameterFilter = row[PARAM_FILTERTYPE].ToString();
        var threshold = row[PARAM_THRESHOLD].ToString();
        var thresholdNRT = row[REALERT_PARAM_THRES].ToString();
        var precision = row[PARAM_PREC].ToString();
        var groupName = row[PARAM_GROUPNAME].ToString();
        var isAssigned = row[IS_ASSIGNED].ToString().ToLower();
        var desc = row[PARAM_DESC].ToString();
        var activitystatus = row[IS_SELECTED].ToString();
        var thresholdHigh = row["ParameterThresholdHigh"].ToString();
        var isEditIndicatorNRT = row[PARAM_VALUE_IS_EDIT].ToBoolean();
        var isEditThresholdNRT = row[PARAM_THRES_IS_EDIT].ToBoolean();

        parameter.ID = int.Parse(id);
        parameter.Name = name;
        parameter.Code = code;
        parameter.Key = key;
        parameter.IndicatorType = indType;

        if (indValNRT.Trim() != string.Empty)
            parameter.ReAlertIndicatorValue = double.Parse(indValNRT);
        else
            parameter.ReAlertIndicatorValue = null;

        if (indVal.Trim() != string.Empty)
            parameter.IndicatorValue = double.Parse(indVal);
        else
            parameter.IndicatorValue = null;

        if (indValHigh.Trim() != string.Empty)
            parameter.IndicatorHigh = double.Parse(indValHigh);
        else
            parameter.IndicatorHigh = null;

        if (threshold.Trim() != string.Empty && threshold != "0dds0")
            parameter.ThresholdValue = double.Parse(threshold);
        else
        {
            if (thresholdType != typeDBNULL)
                parameter.ThresholdValue = double.NaN; //indicate empty
            else
                parameter.ThresholdValue = null;
        }

        if (thresholdHigh.Trim() != string.Empty && thresholdHigh != "0dds0")
            parameter.ThresholdHigh = double.Parse(thresholdHigh);
        else
        {
            if (thresholdType != typeDBNULL)
                parameter.ThresholdHigh = double.NaN; //indicate empty
            else
                parameter.ThresholdHigh = null;
        }

        // ReAlert Threshold

        if (thresholdNRT.Trim() != string.Empty && thresholdNRT != "0dds0")
            parameter.ReAlertThresholdValue = double.Parse(thresholdNRT);
        else
        {
            if (thresholdType != typeDBNULL)
                parameter.ReAlertThresholdValue = double.NaN; //indicate empty
            else
                parameter.ReAlertThresholdValue = null;
        }

        parameter.ReAlertIndicator = isEditIndicatorNRT;
        parameter.ReAlertThreshold = isEditThresholdNRT;

        parameter.ParameterFilter = int.Parse(parameterFilter);
        parameter.PrecisionValue = int.Parse(precision);
        parameter.GroupName = groupName;
        parameter.ExtraInformations = "FromAssignment";
        parameter.Description = desc;
        parameter.ActivityStatus = int.Parse(activitystatus);

    }

    public static void GetDataFromTableRow2NRT(this RiskParameter parameter, DataRow row)
    {
        var code = row[PARAM_CODE].ToString();
        var id = row[PARAM_ID].ToString();
        var key = row[PARAM_KEY].ToString();
        var name = row[PARAM_NAME].ToString();
        var indType = row[PARAM_DATATYPE].ToString();
        var indVal = row[PARAM_VALUE].ToString();
        var indValHigh = row[PARAM_VALUEHIGH].ToString();
        var thresholdType = row[PARAM_THRESHOLD].GetType();
        var parameterFilter = row[PARAM_FILTERTYPE].ToString();
        var threshold = row[PARAM_THRESHOLD].ToString();
        var precision = row[PARAM_PREC].ToString();
        var groupName = row[PARAM_GROUPNAME].ToString();
        var isAssigned = row[IS_ASSIGNED].ToString().ToLower();
        var desc = row[PARAM_DESC].ToString();
        var activitystatus = row[IS_SELECTED].ToString();
        var thresholdHigh = row["ParameterThresholdHigh"].ToString();

        var isEditIndicatorNRT = row[PARAM_VALUE_IS_EDIT].ToBoolean();
        var isEditThresholdNRT = row[PARAM_THRES_IS_EDIT].ToBoolean();
        var indValNRT = row[REALERT_PARAM_VALUE].ToString();
        var thresholdNRT = row[REALERT_PARAM_THRES].ToString();
        parameter.ID = int.Parse(id);
        parameter.Name = name;
        parameter.Code = code;
        parameter.Key = key;
        parameter.IndicatorType = indType;

        if (indVal.Trim() != string.Empty)
            parameter.IndicatorValue = double.Parse(indVal);
        else
            parameter.IndicatorValue = null;

        if (indValHigh.Trim() != string.Empty)
            parameter.IndicatorHigh = double.Parse(indValHigh);
        else
            parameter.IndicatorHigh = null;

        if (threshold.Trim() != string.Empty && threshold != "0dds0")
            parameter.ThresholdValue = double.Parse(threshold);
        else
        {
            if (thresholdType != typeDBNULL)
                parameter.ThresholdValue = double.NaN; //indicate empty
            else
                parameter.ThresholdValue = null;
        }

        if (thresholdHigh.Trim() != string.Empty && thresholdHigh != "0dds0")
            parameter.ThresholdHigh = double.Parse(thresholdHigh);
        else
        {
            if (thresholdType != typeDBNULL)
                parameter.ThresholdHigh = double.NaN; //indicate empty
            else
                parameter.ThresholdHigh = null;
        }

        // ReAlert 

        if (indValNRT.Trim() != string.Empty)
            parameter.ReAlertIndicatorValue = double.Parse(indValNRT);
        else
            parameter.ReAlertIndicatorValue = null;

        if (thresholdNRT.Trim() != string.Empty && thresholdNRT != "0dds0")
            parameter.ReAlertThresholdValue = double.Parse(thresholdNRT);
        else
        {
            if (thresholdType != typeDBNULL)
                parameter.ReAlertThresholdValue = double.NaN; //indicate empty
            else
                parameter.ReAlertThresholdValue = null;
        }

        parameter.ReAlertIndicator = isEditIndicatorNRT;
        parameter.ReAlertThreshold = isEditThresholdNRT;

        parameter.ParameterFilter = int.Parse(parameterFilter);
        parameter.PrecisionValue = int.Parse(precision);
        parameter.GroupName = groupName;
        parameter.ExtraInformations = "FromAssignment";
        parameter.Description = desc;
        parameter.ActivityStatus = int.Parse(activitystatus);

    }

    /// <summary>
    /// DDS - Convert to string list separated by ";" for passing to stored procedure to process CRUD operations.
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static string ToListString(this List<RiskParameter> list)
    {
        string result = string.Empty;
        foreach (var param in list)
            result += param.ToString() + ";";
        return result.Trim(';');
    }
    /// <summary>
    /// DDS - Convert to string xml for passing to stored procedure to process CRUD operations.
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static string ToListXmlString(this List<RiskParameter> list)
    {

        return ToListXmlString(list, false);
    }

    /// <summary>
    /// DDS - Convert to string xml for passing to stored procedure to process CRUD operations.
    /// </summary>
    /// <param name="list"></param>
    /// <param name="IsReAlert"></param>
    /// <returns></returns>
    public static string ToListXmlString(this List<RiskParameter> list, bool IsReAlert)
    {
        StringBuilder result = new StringBuilder();

        result.Append("<Root><Parameters>");

        foreach (var param in list)
            if (IsReAlert)
            {
                result.Append(param.ToXmlString(true));
            }
            else
            {
                result.Append(param.ToXmlString());
            }


        result.Append("</Parameters></Root>");

        return result.ToString();
    }

    /// <summary>
    /// DDS - Return a DataTable from the list.
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static DataTable ToDataTable(this List<RiskParameter> list)
    {
        DataTable table = new DataTable();
        table.Columns.Add(PARAM_ROWNUM, typeof(string));
        table.Columns.Add(PARAM_ID, typeof(int));
        table.Columns.Add(PARAM_CODE, typeof(string));
        table.Columns.Add(PARAM_KEY, typeof(string));
        table.Columns.Add(PARAM_NAME, typeof(string));
        table.Columns.Add(PARAM_VALUE, typeof(float));
        table.Columns.Add(PARAM_DATATYPE, typeof(string));
        table.Columns.Add(PARAM_THRESHOLD, typeof(float));
        table.Columns.Add(PARAM_PREC, typeof(int));
        table.Columns.Add(PARAM_GROUPNAME, typeof(string));
        table.Columns.Add(IS_ASSIGNED, typeof(bool));
        table.Columns.Add(PARAM_GROUPNAMEDUMMY, typeof(string));
        table.Columns.Add(PARAM_DESC, typeof(string));
        table.Columns.Add(ACTIVITY_STATUS, typeof(int));
        table.Columns.Add(IS_NEW, typeof(bool));

        //does not return the extra information column
        int rowNumber = 0;

        foreach (RiskParameter param in list)
        {
            rowNumber++;

            string dummyCol = string.Empty;


            switch (param.GroupName)
            {
                case "Deposits":
                    dummyCol = "A"; break;
                case "Auth":
                    dummyCol = "B"; break;
                case "Attrition":
                    dummyCol = "C"; break;
                case "Chargeback/Retrievals":
                    dummyCol = "D"; break;
                case "Contract":
                    dummyCol = "E"; break;
                case "Duplicates":
                    dummyCol = "F"; break;
                case "Volume/Batch/Ticket":
                    dummyCol = "G"; break;
                case "Credits":
                    dummyCol = "H"; break;
                case "ACH Rejects":
                    dummyCol = "I"; break;
                case "BIN":
                    dummyCol = "J"; break;
                case "Merchant Filter":
                    dummyCol = "K"; break;
                default: break;
            }

            table.Rows.Add(rowNumber, param.ID, param.Code, param.Key, param.Name, param.IndicatorValue,
                param.IndicatorType, param.ThresholdValue, param.PrecisionValue,
                param.GroupName, param.ExtraInformations == "true", dummyCol, param.Description,
                param.ActivityStatus, false);
        }
        return table;

    }
}

/// <summary>
/// RiskParameter class for user control uxParameter
/// </summary>
/// 
[Serializable]
public class RiskParameter : IEquatable<RiskParameter>, ICloneable
{
    /// <summary>
    /// Gets or sets the activity status.
    /// </summary>
    /// <value>The activity status.</value>
    public int ActivityStatus { get; set; }

    /// <summary>
    /// Gets or sets the ID.
    /// </summary>
    /// <value>The ID.</value>
    public int ID { get; set; }

    /// <summary>
    /// Gets or sets the code.
    /// </summary>
    /// <value>The code.</value>
    public string Code { get; set; }
    /// <summary>
    /// Key of RiskParameter
    /// </summary>
    public string Key { get; set; }
    /// <summary>
    /// RiskParameter name.
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Indicator value of RiskParameter
    /// </summary>
    public double? IndicatorValue { get; set; }

    public double? IndicatorHigh { get; set; }
    /// <summary>
    /// Indicator type of RiskParameter (#,%,$,days)
    /// </summary>
    public string IndicatorType { get; set; }
    /// <summary>
    /// Threshold value of RiskParameter
    /// </summary>
    public double? ThresholdValue { get; set; }

    public double? ThresholdHigh { get; set; }

    /// <summary>
    /// Filter type of Parameter
    /// </summary>
    public int? ParameterFilter { get; set; }

    /// <summary>
    /// Precision value of RiskParameter
    /// </summary>
    public int? PrecisionValue { get; set; }
    /// <summary>
    /// Group name of RiskParameter
    /// </summary>
    public string GroupName { get; set; }
    /// <summary>
    /// Extra informations, used to save any minor data for this RiskParameter if u want like this.
    /// </summary>
    public string ExtraInformations { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    /// <value>The description.</value>
    public string Description { get; set; }

    /// <summary>
    /// Gets or sets source param
    /// </summary>
    public bool IsSourceParam { get; set; }

    /// <summary>
    /// Gets or sets group id
    /// </summary>
    public int GroupID { get; set; }

    /// <summary>
    /// ReAlertIndicatorValue value of RiskParameter
    /// </summary>
    public double? ReAlertIndicatorValue { get; set; }

    /// <summary>
    /// ReAlertIndicatorHigh value of RiskParameter
    /// </summary>
    public double? ReAlertIndicatorHigh { get; set; }

    /// <summary>
    /// ReAlertThresholdValue value of RiskParameter
    /// </summary>
    public double? ReAlertThresholdValue { get; set; }

    /// <summary>
    /// ReAlertThresholdHigh value of RiskParameter
    /// </summary>
    public double? ReAlertThresholdHigh { get; set; }

    /// <summary>
    /// ReAlertThreshold value of RiskParameter
    /// </summary>
    public bool ReAlertThreshold { get; set; }

    /// <summary>
    /// ReAlertIndicator value of RiskParameter
    /// </summary>
    public bool ReAlertIndicator { get; set; }
    public RiskParameter()
    {
        ActivityStatus = this.ActivityStatus;
        ID = -1;
        Code = string.Empty;
        Key = string.Empty;
        Name = string.Empty;
        IndicatorValue = null;
        IndicatorType = string.Empty;
        ThresholdValue = null;
        ThresholdHigh = null;
        ParameterFilter = 0;
        PrecisionValue = 0;
        GroupName = string.Empty;
        Description = string.Empty;
        GroupID = 0;
        IsSourceParam = true;
        ReAlertIndicatorHigh = null;
        ReAlertIndicatorValue = null;
        ReAlertThresholdValue = null;
        ReAlertThresholdHigh = null;
    }

    public RiskParameter(string key)
        : this()
    {
        this.Key = key;
    }

    #region IEquatable<RiskParameter> Members
    /// <summary>
    /// Compare equal using ID
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(RiskParameter other)
    {
        return this.ID == other.ID;
    }
    /// <summary>
    /// Compare equal using Key
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool EqualsByKey(RiskParameter other)
    {
        return this.Key == other.Key;
    }
    /// <summary>
    /// Compare equal using Name
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool EqualsByName(RiskParameter other)
    {
        return this.Name.Trim() == other.Name.Trim();
    }
    #endregion

    public override string ToString()
    {
        string key = this.Key;
        string indVal = "null";

        if (this.IndicatorValue != null)
        {
            if (this.IndicatorType == "days" || this.IndicatorType == "#")
            {
                string temp = ((decimal)this.IndicatorValue.Value).ToString();
                if (temp.Contains('.'))
                    indVal = temp.Substring(0, temp.IndexOf("."));
                else
                    indVal = temp;
            }
            else
                indVal = this.IndicatorValue.ToString();
        }

        string thresholdVal = string.Empty;
        if (this.ThresholdValue == null)
            thresholdVal = "null";
        else if (double.IsNaN(this.ThresholdValue.Value))
            thresholdVal = "empty";
        else
            thresholdVal = ((decimal)this.ThresholdValue.Value).ToString();

        string precisionVal = string.Empty;
        if (this.PrecisionValue == null)
            precisionVal = "0";
        else
            precisionVal = ((int)this.PrecisionValue.Value).ToString();

        return string.Format("{0},{1},{2},{3},{4}", key, indVal, thresholdVal, precisionVal, this.ActivityStatus);
    }

    public string RemoveOrAddAtrrReAlert(string param, bool isReAlert)
    {
        if (isReAlert)
        {
            string reAlert = string.Empty;
            string indValNRT = string.Empty;
            if (this.ReAlertIndicatorValue != null)
            {
                if (this.IndicatorType == "days" || this.IndicatorType == "#")
                {
                    string temp = ((decimal)this.ReAlertIndicatorValue.Value).ToString();
                    if (temp.Contains('.'))
                        indValNRT = temp.Substring(0, temp.IndexOf("."));
                    else
                        indValNRT = temp;
                }
                else
                    indValNRT = this.ReAlertIndicatorValue.ToString();
            }

            string indValHighNRT = string.Empty;
            if (this.ReAlertIndicatorHigh != null)
            {
                string temp = ((decimal)this.ReAlertIndicatorHigh.Value).ToString();
                if (temp.Contains('.'))
                    indValHighNRT = temp.Substring(0, temp.IndexOf("."));
                else
                    indValHighNRT = temp;
            }

            string thresholdValNRT = string.Empty;
            if (this.ReAlertThresholdValue != null && !double.IsNaN(this.ReAlertThresholdValue.Value))
                thresholdValNRT = ((decimal)this.ReAlertThresholdValue.Value).ToString();

            string ThresholdHighNRT = string.Empty;
            if (this.ReAlertThresholdHigh != null && !double.IsNaN(this.ReAlertThresholdHigh.Value))
                ThresholdHighNRT = ((decimal)this.ReAlertThresholdHigh.Value).ToString();

            reAlert += (string.IsNullOrEmpty(indValNRT) ? string.Empty : string.Format(" ReAlertParameterIndicator = '{0}'", indValNRT))
                + (string.IsNullOrEmpty(indValHighNRT) ? string.Empty : string.Format(" ReAlertParameterIndicatorHigh = '{0}'", indValHighNRT))
                + (string.IsNullOrEmpty(thresholdValNRT) ? string.Empty : string.Format(" ReAlertParameterThreshold = '{0}'", thresholdValNRT)
                + string.Format(" ReAlertParameterThresholdMin = '{0}'", thresholdValNRT))
                + (string.IsNullOrEmpty(ThresholdHighNRT) ? string.Empty : string.Format(" ReAlertParameterThresholdHigh = '{0}'", ThresholdHighNRT));

            param = param.Replace("[ReAlert]", reAlert);
        }
        else
        {
            param = param.Replace("[ReAlert]", string.Empty);
        }
        return param;
    }

    public string ToXmlString()
    {
        return ToXmlString(false);
    }

    public string ToXmlString(bool isReAlert)
    {
        string key = this.Key;
        string xmlResult = string.Empty;
        string indVal = string.Empty;
        if (this.IndicatorValue != null)
        {
            string temp = ((decimal)this.IndicatorValue.Value).ToString();
            if (IsDecimalInConfig(key))
            {
                indVal = temp;
            }
            else if (this.IndicatorType == "days" || this.IndicatorType == "#")
            {
                if (temp.Contains('.'))
                    indVal = temp.Substring(0, temp.IndexOf("."));
                else
                    indVal = temp;
            }
            else
                indVal = this.IndicatorValue.ToString();
        }
        string indValHigh = string.Empty;
        if (this.IndicatorHigh != null)
        {
            string temp = ((decimal)this.IndicatorHigh.Value).ToString();
            if (IsDecimalInConfig(key))
            {
                indValHigh = temp;
            }
            else if (temp.Contains('.'))
                indValHigh = temp.Substring(0, temp.IndexOf("."));
            else
                indValHigh = temp;
        }
        string thresholdVal = string.Empty;
        if (this.ThresholdValue != null && !double.IsNaN(this.ThresholdValue.Value))
            thresholdVal = ((decimal)this.ThresholdValue.Value).ToString();

        string ThresholdHigh = string.Empty;
        if (this.ThresholdHigh != null && !double.IsNaN(this.ThresholdHigh.Value))
            ThresholdHigh = ((decimal)this.ThresholdHigh.Value).ToString();

        string precisionVal = string.Empty;
        if (this.PrecisionValue != null && !float.IsNaN(this.PrecisionValue.Value))
            precisionVal = ((int)this.PrecisionValue.Value).ToString();

        if (indVal == string.Empty && thresholdVal == string.Empty)
        {
            if (this.ExtraInformations != "FromAssignment")
                xmlResult = string.Format("<Parameter Key='{0}' Precision='{1}' ActivityStatus='{2}' GroupID='{3}' IsSourceParam = '{4}' [ReAlert] />",
                                            key, precisionVal, this.ActivityStatus, this.GroupID, this.IsSourceParam);
            else
                xmlResult = string.Format("<Parameter Key='{0}' Precision='{1}' IsSelected='{2}' GroupID='{3}' IsSourceParam = '{4}' [ReAlert] />",
                                            key, precisionVal, this.ActivityStatus, this.GroupID, this.IsSourceParam);

        }
        else if (!indValHigh.IsNullOrEmpty() && thresholdVal == string.Empty)
        {
            if (this.ExtraInformations != "FromAssignment")
                xmlResult = string.Format("<Parameter Key='{0}' Indicator='{1}' Precision='{2}' ActivityStatus='{3}' IndicatorHigh='{4}' GroupID='{5}' IsSourceParam = '{6}' [ReAlert] />",
                         key, indVal, precisionVal, this.ActivityStatus, indValHigh, this.GroupID, this.IsSourceParam);
            else
                xmlResult = string.Format("<Parameter Key='{0}' Indicator='{1}' Precision='{2}' IsSelected='{3}' IndicatorHigh='{4}' GroupID='{5}' IsSourceParam = '{6}' [ReAlert] />",
                        key, indVal, precisionVal, this.ActivityStatus, indValHigh, this.GroupID, this.IsSourceParam);
        }
        else if (!indValHigh.IsNullOrEmpty())
        {
            if (!ThresholdHigh.IsNullOrEmpty())
            {
                if (this.ExtraInformations != "FromAssignment")
                    xmlResult = string.Format("<Parameter Key='{0}' Indicator='{1}' Threshold='{2}' Precision='{3}' ActivityStatus='{4}' IndicatorHigh='{5}' ThresholdHigh='{6}' GroupID='{7}' IsSourceParam = '{8}' [ReAlert] />",
                         key, indVal, thresholdVal, precisionVal, this.ActivityStatus, indValHigh, this.ThresholdHigh, this.GroupID, this.IsSourceParam);
                else
                    xmlResult = string.Format("<Parameter Key='{0}' Indicator='{1}' Threshold='{2}' Precision='{3}' IsSelected='{4}' IndicatorHigh='{5}' ThresholdHigh='{6}' GroupID='{7}' IsSourceParam = '{8}' [ReAlert] />",
                            key, indVal, thresholdVal, precisionVal, this.ActivityStatus, indValHigh, this.ThresholdHigh, this.GroupID, this.IsSourceParam);
            }
            else
            {
                if (this.ExtraInformations != "FromAssignment")
                    xmlResult = string.Format("<Parameter Key='{0}' Indicator='{1}' Threshold='{2}' Precision='{3}' ActivityStatus='{4}' IndicatorHigh='{5}' GroupID='{6}' IsSourceParam = '{7}' [ReAlert] />",
                         key, indVal, thresholdVal, precisionVal, this.ActivityStatus, indValHigh, this.GroupID, this.IsSourceParam);
                else
                    xmlResult = string.Format("<Parameter Key='{0}' Indicator='{1}' Threshold='{2}' Precision='{3}' IsSelected='{4}' IndicatorHigh='{5}' GroupID='{6}' IsSourceParam = '{7}' [ReAlert] />",
                            key, indVal, thresholdVal, precisionVal, this.ActivityStatus, indValHigh, this.GroupID, this.IsSourceParam);
            }
        }
        else if (indVal == string.Empty)
        {
            if (this.ExtraInformations != "FromAssignment")
                xmlResult = string.Format("<Parameter Key='{0}' Threshold='{1}' Precision='{2}' ActivityStatus='{3}' GroupID='{4}' IsSourceParam = '{5}' [ReAlert] />",
                                    key, thresholdVal, precisionVal, this.ActivityStatus, this.GroupID, this.IsSourceParam);
            else
                xmlResult = string.Format("<Parameter Key='{0}' Threshold='{1}' Precision='{2}' IsSelected='{3}' GroupID='{4}' IsSourceParam = '{5}' [ReAlert] />",
                                    key, thresholdVal, precisionVal, this.ActivityStatus, this.GroupID, this.IsSourceParam);

        }
        else if (!ThresholdHigh.IsNullOrEmpty())
        {
            if (this.ExtraInformations != "FromAssignment")
                xmlResult = string.Format("<Parameter Key='{0}' Indicator='{1}' Threshold='{2}' Precision='{3}' ActivityStatus='{4}' ThresholdHigh='{5}' GroupID='{6}' IsSourceParam = '{7}' [ReAlert] />",
                         key, indVal, thresholdVal, precisionVal, this.ActivityStatus, this.ThresholdHigh, this.GroupID, this.IsSourceParam);
            else
                xmlResult = string.Format("<Parameter Key='{0}' Indicator='{1}' Threshold='{2}' Precision='{3}' IsSelected='{4}' ThresholdHigh='{5}' GroupID='{6}' IsSourceParam = '{7}' [ReAlert] />",
                        key, indVal, thresholdVal, precisionVal, this.ActivityStatus, this.ThresholdHigh, this.GroupID, this.IsSourceParam);
        }
        else if (thresholdVal == string.Empty)
        {
            if (this.ExtraInformations != "FromAssignment")
                xmlResult = string.Format("<Parameter Key='{0}' Indicator='{1}' Precision='{2}' ActivityStatus='{3}' GroupID='{4}' IsSourceParam = '{5}' [ReAlert] />",
                        key, indVal, precisionVal, this.ActivityStatus, this.GroupID, this.IsSourceParam);
            else
                xmlResult = string.Format("<Parameter Key='{0}' Indicator='{1}' Precision='{2}' IsSelected='{3}' GroupID='{4}' IsSourceParam = '{5}' [ReAlert] />",
                        key, indVal, precisionVal, this.ActivityStatus, this.GroupID, this.IsSourceParam);
        }
        else if (this.ExtraInformations != "FromAssignment")
            xmlResult = string.Format("<Parameter Key='{0}' Indicator='{1}' Threshold='{2}' Precision='{3}' ActivityStatus='{4}' GroupID='{5}' IsSourceParam = '{6}' [ReAlert] />",
                    key, indVal, thresholdVal, precisionVal, this.ActivityStatus, this.GroupID, this.IsSourceParam);
        else
            xmlResult = string.Format("<Parameter Key='{0}' Indicator='{1}' Threshold='{2}' Precision='{3}' IsSelected='{4}' GroupID='{5}' IsSourceParam = '{6}' [ReAlert] />",
                    key, indVal, thresholdVal, precisionVal, this.ActivityStatus, this.GroupID, this.IsSourceParam);

        if (isReAlert)
        {
            xmlResult = RemoveOrAddAtrrReAlert(xmlResult, true);
        }
        else
        {
            xmlResult = RemoveOrAddAtrrReAlert(xmlResult, false);
        }
        return xmlResult;
    }

    #region ICloneable Members

    public object Clone()
    {
        RiskParameter p = new RiskParameter
        {
            ID = this.ID,
            Key = this.Key,
            Code = this.Code,
            Name = this.Name,
            GroupName = this.GroupName,
            IndicatorValue = this.IndicatorValue,
            IndicatorType = this.IndicatorType,
            ThresholdValue = this.ThresholdValue,
            ThresholdHigh = this.ThresholdHigh,
            PrecisionValue = this.PrecisionValue,
            ActivityStatus = this.ActivityStatus,
            GroupID = this.GroupID,
            IsSourceParam = this.IsSourceParam
        };
        return p;
    }
    /// <summary>
    /// Clone a RiskParameter
    /// </summary>
    /// <returns></returns>
    public RiskParameter RiskParameterClone()
    {
        return this.Clone() as RiskParameter;
    }

    #endregion

    /// <summary>
    /// Compare for value changes (indicator and threshold).
    /// Note: this is not compare the extra informations of RiskParameter
    /// </summary>
    /// <param name="p"></param>
    /// <returns>true mean that value is equal, other is false.</returns>
    public bool IsContentEquals(RiskParameter p)
    {
        if (this.ID != p.ID)
            return false;
        if (this.ThresholdValue != p.ThresholdValue)
            return false;
        if (this.IndicatorValue != p.IndicatorValue)
            return false;
        return true;
    }

    private bool IsDecimalInConfig(string key)
    {
        return BuGeneralFuncsLib.CheckExistsInSplitToArray(WebSiteSettings.ParametersAllowDecimal, ',', key);
    }

}

public class ParameterFE
{
    public string ParameterKey { get; set; }
    public string ParameterPrecision { get; set; }
    public string IsThresholdNegative { get; set; }
    public string IsIndicatorNegative { get; set; }
    public string ParameterThresholdType { get; set; }
    public string ThresholdLow { get; set; }
    public string ThresholdHigh { get; set; }
    public string Threshold { get; set; }
    public string ParameterValue { get; set; }
    public string From { get; set; }
    public string To { get; set; }
    public string GroupID { get; set; }
    public string IsSourceParam { get; set; }
    public string ReAlertParameterIndicator { get; set; }
    public string ReAlertParameterThreshold { get; set; }
    public string ReAlertThresholdLow { get; set; }
    public string ReAlertParameterThresholdHigh { get; set; }
    public string FromNRT { get; set; }
    public string ToNRT { get; set; }
    public bool IsParameterValueVisible { get; set; }
}
#endregion


#region Detection Queues

[Serializable]
public class DetectionQueue
{
    public DetectionQueue() { }

    private WebSiteEnums.AssignmentFilterTypes _AssignmentOption = WebSiteEnums.AssignmentFilterTypes.User;
    public WebSiteEnums.AssignmentFilterTypes AssignmentOption { get { return _AssignmentOption; } set { _AssignmentOption = value; } }

    private int _AssignmentID = 0;
    public int AssignmentID { get { return _AssignmentID; } set { _AssignmentID = value; } }

    private WebSiteEnums.AssignmentType _AssignmentType = WebSiteEnums.AssignmentType.DetectionQueue;
    public WebSiteEnums.AssignmentType AssignmentType { get { return _AssignmentType; } set { _AssignmentType = value; } }

    private string _AssignmentName = string.Empty;
    public string AssignmentName { get { return _AssignmentName; } set { _AssignmentName = value; } }

    private DateTime _ReportDate = DateTime.Today;
    public DateTime ReportDate { get { return _ReportDate; } set { _ReportDate = value; } }

    private string _OrderBy = string.Empty;
    public string OrderBy { get { return _OrderBy; } set { _OrderBy = value; } }

    private string _AndCondition = string.Empty;
    public string AndCondition { get { return _AndCondition; } set { _AndCondition = value; } }

    private WebSiteEnums.RiskReportType _ReportType = WebSiteEnums.RiskReportType.RainbowReport;
    public WebSiteEnums.RiskReportType ReportType { get { return _ReportType; } set { _ReportType = value; } }

    private WebSiteEnums.MerchantWorkedType _MerchantWorkedType = WebSiteEnums.MerchantWorkedType.All;
    public WebSiteEnums.MerchantWorkedType MerchantWorkedType { get { return _MerchantWorkedType; } set { _MerchantWorkedType = value; } }

    public string Header { get { return string.Format("{0} {1} ({2})", Resources.LanguageResource.RiskEntities_Header_Assignment, _AssignmentName, _ReportDate.ToString("MM/dd/yyyy")); } }

    private int _PageSize = 10;
    public int PageSize { get { return _PageSize; } set { _PageSize = value; } }

    private int _PageIndex = 1;
    public int PageIndex { get { return _PageIndex; } set { _PageIndex = value; } }

    private string _SortExpression = string.Empty;
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    private string _SortOrder = string.Empty;
    public string SortOrder { get { return _SortOrder; } set { _SortOrder = value; } }

    //43784 Queue Enhancements
    private string _AggregateSortExpression = string.Empty;
    public string AggregateSortExpression { get { return _AggregateSortExpression; } set { _AggregateSortExpression = value; } }

    private string _AggregateSortOrder = string.Empty;
    public string AggregateSortOrder { get { return _AggregateSortOrder; } set { _AggregateSortOrder = value; } }
}


[Serializable]
public class DetectionQueue_RequeuedAssignment
{
    public DetectionQueue_RequeuedAssignment()
    {
        TotalAlertMerchant = 0;
        IsRequeueAll = false;
        RequeuedMerchantList = new HashSet<string>();
        TotalVolume = 0;
    }

    public int TotalAlertMerchant { get; set; }
    public int CurrentRequeuedMerchants
    {
        get
        {
            if (IsRequeueAll)
                return TotalAlertMerchant;
            else
                return RequeuedMerchantList.Count();
        }
    }
    public int RequeueSessionID { get; set; }
    public bool IsRequeueAll { get; set; }
    public decimal TotalVolume { get; set; }
    public HashSet<string> RequeuedMerchantList { get; set; }
}
#endregion

#region MCF Detection Queues

[Serializable]
public class MCF_DetectionQueue
{
    public MCF_DetectionQueue() { }

    private WebSiteEnums.AssignmentFilterTypes _AssignmentOption = WebSiteEnums.AssignmentFilterTypes.User;
    public WebSiteEnums.AssignmentFilterTypes AssignmentOption { get { return _AssignmentOption; } set { _AssignmentOption = value; } }

    private int _AssignmentID = 0;
    public int AssignmentID { get { return _AssignmentID; } set { _AssignmentID = value; } }

    private WebSiteEnums.AssignmentType _AssignmentType = WebSiteEnums.AssignmentType.DetectionQueue;
    public WebSiteEnums.AssignmentType AssignmentType { get { return _AssignmentType; } set { _AssignmentType = value; } }

    private string _AssignmentName = string.Empty;
    public string AssignmentName { get { return _AssignmentName; } set { _AssignmentName = value; } }

    private DateTime _ReportDate = DateTime.Today;
    public DateTime ReportDate { get { return _ReportDate; } set { _ReportDate = value; } }

    private string _OrderBy = string.Empty;
    public string OrderBy { get { return _OrderBy; } set { _OrderBy = value; } }

    private string _AndCondition = string.Empty;
    public string AndCondition { get { return _AndCondition; } set { _AndCondition = value; } }

    private WebSiteEnums.RiskReportType _ReportType = WebSiteEnums.RiskReportType.RainbowReport;
    public WebSiteEnums.RiskReportType ReportType { get { return _ReportType; } set { _ReportType = value; } }

    private WebSiteEnums.MerchantWorkedType _MerchantWorkedType = WebSiteEnums.MerchantWorkedType.All;
    public WebSiteEnums.MerchantWorkedType MerchantWorkedType { get { return _MerchantWorkedType; } set { _MerchantWorkedType = value; } }

    public string Header { get { return string.Format("{0} {1} ({2})", Resources.LanguageResource.RiskEntities_Header_Assignment, _AssignmentName, _ReportDate.ToString("MM/dd/yyyy")); } }

    private int _PageSize = 10;
    public int PageSize { get { return _PageSize; } set { _PageSize = value; } }

    private int _PageIndex = 1;
    public int PageIndex { get { return _PageIndex; } set { _PageIndex = value; } }

    private string _SortExpression = string.Empty;
    public string SortExpression { get { return _SortExpression; } set { _SortExpression = value; } }

    private string _SortOrder = string.Empty;
    public string SortOrder { get { return _SortOrder; } set { _SortOrder = value; } }

    //43784 Queue Enhancements
    private string _AggregateSortExpression = string.Empty;
    public string AggregateSortExpression { get { return _AggregateSortExpression; } set { _AggregateSortExpression = value; } }

    private string _AggregateSortOrder = string.Empty;
    public string AggregateSortOrder { get { return _AggregateSortOrder; } set { _AggregateSortOrder = value; } }

    private string _ApplyFilterId = string.Empty;
    public string ApplyFilterId { get { return _ApplyFilterId; } set { _ApplyFilterId = value; } }
}


[Serializable]
public class MCF_DetectionQueue_RequeuedAssignment
{
    public MCF_DetectionQueue_RequeuedAssignment()
    {
        TotalAlertMerchant = 0;
        IsRequeueAll = false;
        RequeuedMerchantList = new HashSet<string>();
        TotalVolume = 0;
    }

    public int TotalAlertMerchant { get; set; }
    public int CurrentRequeuedMerchants
    {
        get
        {
            if (IsRequeueAll)
                return TotalAlertMerchant;
            else
                return RequeuedMerchantList.Count();
        }
    }
    public int RequeueSessionID { get; set; }
    public bool IsRequeueAll { get; set; }
    public decimal TotalVolume { get; set; }
    public HashSet<string> RequeuedMerchantList { get; set; }
    public int TotalRequeueMerchants { get; set; }
    public int TotalAllMerchants { get; set; }
    public int TotalMerchantsOtherFilter { get; set; }
}
#endregion


#region RiskQueue Entity
[Serializable]
public class RiskQueue
{

    public string MerchantNumber
    {
        get;
        set;
    }
    public List<string> MerchantRanges
    {
        get;
        set;
    }
    public List<decimal> RiskScores
    {
        get;
        set;
    }
    public string Parameters
    {
        get;
        set;
    }
    public bool IsMatchAllParameters
    {
        get;
        set;
    }

    public DateTime ReportDate
    {
        get;
        set;
    }

    public int ReportType
    {
        get;
        set;
    }

    public bool CheckedRiskScore
    {
        get;
        set;
    }
    public string FilterList
    {
        get;
        set;
    }
    public int AssignmentID
    {
        get;
        set;
    }
    public string AdhocName
    {
        get;
        set;
    }
}

enum CurrentFilter
{
    Unknown = 0,
    Group = 1,
    TPO = 2,
    Assoc_Chain = 3,
    SIC = 4,
    State = 5,
    ZIP = 6,
    Profiles = 7,
    MerchantRanges = 8,
    MerchantNumber = 9,
    NewMerchants = 10,
    MerchantsOnWatch = 11
}
#endregion

#region ReferrerInfo Entity
[Serializable]
public class ReferrerInfo
{
    public ReferrerInfo()
    {
    }

    public ReferrerInfo(string key, string url, string title)
    {
        this.Key = key;
        this.Url = url;
        this.Title = title;
    }

    public string Key
    {
        get;
        set;
    }

    public string Url
    {
        get;
        set;
    }

    public string Title
    {
        get;
        set;
    }
}

#endregion


#region MerchantRange
public class MerchantRange
{
    public string MerchantRangeFrom { get; set; }
    public string MerchantRangeTo { get; set; }
    #region Constructors

    public MerchantRange() { }

    public MerchantRange(
            string merchantRangeFrom,
            string merchantRangeTo)
    {
        MerchantRangeFrom = merchantRangeFrom;
        MerchantRangeTo = merchantRangeTo;
    }

    #endregion
}
#endregion

#region Retrieval Chargebacks

[Serializable]
public class RetrievalsChargebacks
{
    public RetrievalsChargebacks() { }

    private string _hierarchyMode = string.Empty;
    private string _hierarchyValue = string.Empty;
    private WebSiteEnums.WorkTypes _isWorked = WebSiteEnums.WorkTypes.All;
    private int _pageSize = 10;
    private int _pageIndex = 1;
    private string _sortExpression = string.Empty;
    private string _sortOrder = string.Empty;


    public string HierarchyMode
    {
        get { return _hierarchyMode; }
        set { _hierarchyMode = value; }
    }

    public string HierarchyValue
    {
        get { return _hierarchyValue; }
        set { _hierarchyValue = value; }
    }

    public WebSiteEnums.WorkTypes WorkType
    {
        get { return _isWorked; }
        set { _isWorked = value; }
    }

    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = value; }
    }

    public int PageIndex
    {
        get { return _pageIndex; }
        set { _pageIndex = value; }
    }

    public string SortExpression
    {
        get { return _sortExpression; }
        set { _sortExpression = value; }
    }

    public string SortOrder
    {
        get { return _sortOrder; }
        set { _sortOrder = value; }
    }
}

[Serializable]
public class MCF_RetrievalsChargebacks
{
    public MCF_RetrievalsChargebacks() { }

    private string _hierarchyMode = string.Empty;
    private string _hierarchyValue = string.Empty;
    private WebSiteEnums.ReviewedTypes _isReviewed = WebSiteEnums.ReviewedTypes.All;
    private int _pageSize = 10;
    private int _pageIndex = 1;
    private string _sortExpression = string.Empty;
    private string _sortOrder = string.Empty;


    public string HierarchyMode
    {
        get { return _hierarchyMode; }
        set { _hierarchyMode = value; }
    }

    public string HierarchyValue
    {
        get { return _hierarchyValue; }
        set { _hierarchyValue = value; }
    }

    public WebSiteEnums.ReviewedTypes ReviewedType
    {
        get { return _isReviewed; }
        set { _isReviewed = value; }
    }

    public int PageSize
    {
        get { return _pageSize; }
        set { _pageSize = value; }
    }

    public int PageIndex
    {
        get { return _pageIndex; }
        set { _pageIndex = value; }
    }

    public string SortExpression
    {
        get { return _sortExpression; }
        set { _sortExpression = value; }
    }

    public string SortOrder
    {
        get { return _sortOrder; }
        set { _sortOrder = value; }
    }
}
#endregion

#region Attribute Operand and Metric

public enum AttributeRiskScoreMode
{
    None = 0,
    FromTo = 1,
    Operand = 2,
    OperandMetric = 3,
    SingleSelect = 4,
    OperandIfEmpty = 5,
}


[Serializable]
[XmlRoot("Attribute")]
public class AttributeInformation
{
    //public OperandCollection OperandCollection { get; set; }
    [XmlElement("Operands")]
    public OperandCollection Operands { get; set; }

    //public OperandCollection OperandCollection { get; set; }
    [XmlElement("Metrics")]
    public MetricCollection Metrics { get; set; }
}

[Serializable]
public class OperandCollection
{
    [XmlElement("Operand")]
    public List<Operand> Items { get; set; }

    public bool HasValues
    {
        get
        {
            return Items != null && Items.Any();
        }
    }
}

[Serializable]
public class Operand
{
    [XmlAttribute("Value")]
    public string Value { get; set; }

    [XmlAttribute("Text")]
    public string Text { get; set; }

    [XmlAttribute("FieldType")]
    public string FieldType { get; set; }


}

[Serializable]
public class MetricCollection
{

    [XmlElement("Metric")]
    public List<Metric> Items { get; set; }

    public bool HasValues
    {
        get
        {
            return Items != null && Items.Any();
        }
    }

    public List<Metric> GetMetricByOperand(string operandID)
    {
        if (HasValues)
        {
            return Items.Where(t => t.Filter.Split(',').Contains(operandID)).ToList();
        }
        else
            return new List<Metric>();

    }
}

[Serializable]
public class Metric
{

    [XmlAttribute("Filter")]
    public string Filter { get; set; }

    [XmlAttribute("Value")]
    public string Value { get; set; }

    [XmlAttribute("Text")]
    public string Text { get; set; }

    [XmlAttribute("Tooltip")]
    public string Tooltip { get; set; }

}


[Serializable]
[XmlRoot("Attributes")]
public class ClassificationAttribute
{
    //public OperandCollection OperandCollection { get; set; }
    [XmlElement("Attribute")]
    public List<Attribute> Attributes { get; set; }

}


[Serializable]
public class Attribute
{

    [XmlAttribute("From")]
    public string From { get; set; }

    [XmlAttribute("Value")]
    public string Value { get; set; }

    [XmlAttribute("To")]
    public string To { get; set; }

    [XmlAttribute("Operand")]
    public string Operand { get; set; }

    [XmlAttribute("Metrics")]
    public string Metrics { get; set; }

    public Attribute()
    {
        this.From = "";
        this.To = "";
        this.Operand = "";
        this.Metrics = "";
    }
}
#endregion