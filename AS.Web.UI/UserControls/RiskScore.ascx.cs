using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Resources;
using AS.Common;
using AS.Common.DBManager;
using Telerik.Web.UI;

public partial class UserControls_RiskScore : GlobalUserControl
{
    #region Properties   
    public event AfterSubmitHandler AfterSubmit;
    public event EventHandler BeforeSubmit;
    public delegate void AfterSubmitHandler(object sender, string param, decimal from, decimal to, decimal threshold, decimal thresholdHigh, int score);
    public delegate void BeforeSubmitHandler(object sender, float? indicatorMin, float? indicatorMax);

    private float? indicatorMin;
    private float? indicatorMax;
    public void SetIndicatorMinMax(float? indicatorMin, float? indicatorMax)
    {
        this.indicatorMin = indicatorMin;
        this.indicatorMax = indicatorMax;
    }

    private string selectedParameterKey = string.Empty;

    public string SelectedParameterKey
    {
        get { return selectedParameterKey; }
        set { selectedParameterKey = value; }
    }

    private int _RiskSite = -1;
    public int RiskSite
    {
        get { return _RiskSite; }
        set { _RiskSite = value; }
    }

    string SelectedParamValue
    {
        get
        {
            return uxParamList.SelectedValue != null ? uxParamList.SelectedValue : string.Empty;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DoBindData(null, null, null, null, null, null, null);
        }

        //uxUpdate.OnClientClick = string.Format("return ValidateData('{0}', true);", uxUpdate.ClientID);

        this.selectedParameterKey = this.SelectedParamValue;
    }
    protected void uxParamList_ItemDataBound(object sender, Telerik.Web.UI.RadComboBoxItemEventArgs e)
    {
        RadComboBoxItem item = e.Item;
        DataRowView row = (DataRowView)e.Item.DataItem;
        item.Attributes["ParameterThresholdEnabled"] = row["ParameterThresholdEnabled"].ToString();
        item.Attributes["ParameterThresholdHigh"] = row["ParameterThresholdType"].ToString();

        item.Attributes["IsThresholdNegative"] = (row["IsThresholdNegative"] != DBNull.Value && row["IsThresholdNegative"].ToString() != string.Empty && ((bool)row["IsThresholdNegative"])) ? "1" : "0";
        item.Attributes["IsIndicatorNegative"] = (row["IsIndicatorNegative"] != DBNull.Value && row["IsIndicatorNegative"].ToString() != string.Empty && ((bool)row["IsIndicatorNegative"])) ? "1" : "0";
        item.Attributes["IsNullParameterIndicator"] = (row["ParameterIndicator"] != DBNull.Value && row["ParameterIndicator"].ToString() != string.Empty) ? "1" : "0";
        //Bug #37829: [QAI][44432]
        //[42397] - Fixbug 37367
        item.Attributes["ParameterPrecision"] = row["ParameterPrecision"].ToString();
    }
    //protected void uxCancel_Click(object sender, EventArgs e)
    //{
    //    this.Visible = false;
    //}
    protected void uxSubmit_Click(object sender, EventArgs e)
    {
        if (BeforeSubmit != null)
            BeforeSubmit(this, EventArgs.Empty);

        int recordID = 0;
        decimal indicatorFrom = 0;
        decimal indicatorTo = 0;
        int indicatorThreshold = 0;
        int indicatorScore = 0;
        //indicator Threshold High
        int indicatorThresholdHigh = 0;

        Int32.TryParse(uxUpdate.CommandArgument, out recordID);
        Decimal.TryParse(VeraCodeSolution.DoVeraCode(uxParameterIndicatorFrom.Text.Trim()), out indicatorFrom);
        Decimal.TryParse(VeraCodeSolution.DoVeraCode(uxParameterIndicatorTo.Text.Trim()), out indicatorTo);
        int.TryParse(VeraCodeSolution.DoVeraCode(uxParameterThreshold.Text.Trim()), out indicatorThreshold);
        Int32.TryParse(VeraCodeSolution.DoVeraCode(uxParameterScore.Text.Trim()), out indicatorScore);
        //thresholdHigh
        int.TryParse(uxParameterThresholdHigh.Text.Trim(), out indicatorThresholdHigh);

        InsertRiskScores(SessionManager.CurrentUser.ASClient, SelectedParamValue,
                indicatorFrom, indicatorTo, indicatorThreshold, indicatorThresholdHigh, indicatorScore, SessionManager.CurrentUser.UserID);
        if (AfterSubmit != null)
        {
            AfterSubmit(this, SelectedParamValue, indicatorFrom, indicatorTo, indicatorThreshold, indicatorThresholdHigh, indicatorScore);
        }
    }
  
    private void InsertRiskScores(int DDSClient, string ParameterKey, decimal ParameterIndicatorFrom, decimal ParameterIndicatorTo, decimal ParameterThreshold, decimal ParameterThresholdHigh, int ParameterScore, string UserId)
    {
        int indicatorNegative = 1;
        int thresholdNegative = 1;
        FilterParameterCollection parameters = new FilterParameterCollection();
        FilterParameterCollection outParameters = new FilterParameterCollection();
        if (uxParamList.SelectedItem.Attributes["IsIndicatorNegative"] == "1")
        {
            indicatorNegative = -1;
        }
        if (uxParamList.SelectedItem.Attributes["IsThresholdNegative"] == "1")
        {
            thresholdNegative = -1;
        }
       
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.Add(new FilterParameter("@ParameterKey", ParameterKey, DbType.AnsiString));
        parameters.Add(new FilterParameter("@ParameterThreshold", ParameterThreshold == 0 ? 0 : Math.Abs(ParameterThreshold) * thresholdNegative, DbType.Int32));
        parameters.Add(new FilterParameter("@ParameterThresholdHigh", ParameterThresholdHigh == 0 ? 0 : Math.Abs(ParameterThresholdHigh) * thresholdNegative, DbType.Int32));
        parameters.Add(new FilterParameter("@ParameterScore", Math.Abs(ParameterScore), DbType.Int32));
        parameters.Add(new FilterParameter("@ParameterIndicatorFrom", Math.Abs(ParameterIndicatorFrom) * indicatorNegative, DbType.Decimal));
        parameters.Add(new FilterParameter("@ParameterIndicatorTo", Math.Abs(ParameterIndicatorTo) * indicatorNegative, DbType.Decimal));
        
        WebServices.RiskServices.ExecuteNonQueryCommand("spa_rm_cs_InsertRiskScores", parameters, out outParameters);
    }
    bool HasInvalidCharacters(string input, string invalid_chars)
    {
        string invalidHexValue = "";
        for (int i = 0; i < invalid_chars.Length; i++)
        {
            invalidHexValue += "\\x" + ((byte)invalid_chars[i]).ToString("X").ToUpper();

        }
        return System.Text.RegularExpressions.Regex.IsMatch(input, "[" + invalidHexValue + "]");
    }

    bool HasOnlyCharacters(string input, string valid_chars)
    {
        string validHexValue = "";
        for (int i = 0; i < valid_chars.Length; i++)
        {
            validHexValue += "\\x" + ((byte)valid_chars[i]).ToString("X").ToUpper();
        }
        System.Text.RegularExpressions.MatchCollection m = System.Text.RegularExpressions.Regex.Matches(input, "[" + validHexValue + "]");
        if (m != null)
        {
            return m.Count == input.Length;
        }
        return false;
    }

    public bool DoValidateInput()
    {
        const string VALID_CHARS = "0123456789.,-";

        uxParameterIndicatorFrom.Text = VeraCodeSolution.DoVeraCode(uxParameterIndicatorFrom.Text.Trim());
        uxParameterIndicatorTo.Text = VeraCodeSolution.DoVeraCode(uxParameterIndicatorTo.Text.Trim());
        uxParameterThreshold.Text = VeraCodeSolution.DoVeraCode(uxParameterThreshold.Text.Trim());
        uxParameterScore.Text = VeraCodeSolution.DoVeraCode(uxParameterScore.Text.Trim());

        if (!HasOnlyCharacters(uxParameterIndicatorFrom.Text, VALID_CHARS))
        {
            (this.Page).IsIntruderDetected = true;
            return false;
        }

        if (!HasOnlyCharacters(uxParameterIndicatorTo.Text, VALID_CHARS))
        {
            (this.Page).IsIntruderDetected = true;
            return false;
        }

        if (uxParameterThreshold.Text.Length > 0)
        {
            if (!HasOnlyCharacters(uxParameterThreshold.Text, VALID_CHARS))
            {
                (this.Page).IsIntruderDetected = true;
                return false;
            }
        }

        if (uxParameterScore.Text.Length > 0)
        {
            if (!HasOnlyCharacters(uxParameterScore.Text, VALID_CHARS))
            {
                (this.Page).IsIntruderDetected = true;
                return false;
            }
        }

        return true;
    }
    public void DoBindData(string paramId, string param, string from, string to, string threshold, string thresholdHigh, string score)
    {
        FilterParameterCollection parameters = new FilterParameterCollection();
       
        parameters.AddLoggedInUserParams(SessionManager.CurrentUser.SiteID);
        parameters.AddLanguageID();
        DataTable table = WebServices.RiskServices.GetReports("spa_rm_cs_GetRiskParameterList", parameters);
        foreach (DataRow row in table.Rows)
        {
            row["ParameterName"] = row["ParameterKey"] + " - " + row["ParameterGroupDescription"] + " - " + row["ParameterName"];
            row["ParameterKey"] = row["ParameterKey"];
        }
        uxParamList.DataTextField = "ParameterName";
        uxParamList.DataValueField = "ParameterKey";
        uxParamList.DataSource = table;
        uxParamList.DataBind();

        uxParamList.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem(GetLocalResourceObject("RiskScore_ascx_cs_SelectParam").ToString(), "0"));

        if (uxParamList.Items.Count > 10)
        {
            uxParamList.Height = Unit.Pixel(220);
        }

        if (param != null)
        {
            uxParamList.SelectedValue = param;
        }

        if (from != null)
        {
            uxParameterIndicatorFrom.Text = VeraCodeSolution.DoVeraCode(from);
        }

        if (to != null)
        {
            uxParameterIndicatorTo.Text = VeraCodeSolution.DoVeraCode(to);
        }

        if (threshold != null)
        {
            uxParameterThreshold.Text = VeraCodeSolution.DoVeraCode(threshold);
        }
        if (thresholdHigh != null)
        {
            uxParameterThresholdHigh.Text = VeraCodeSolution.DoVeraCode(thresholdHigh);
        }

        if (score != null)
        {
            uxParameterScore.Text = VeraCodeSolution.DoVeraCode(score);
        }

        uxUpdate.CommandArgument = paramId;
    }
    public void ResetForm()
    {
        uxParamList.SelectedIndex = 0;
        uxParameterIndicatorFrom.Text = uxParameterIndicatorTo.Text = uxParameterThreshold.Text = uxParameterScore.Text = string.Empty;
    }
    
}
