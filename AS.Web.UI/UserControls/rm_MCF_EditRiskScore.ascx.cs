using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_rm_MCF_EditRiskScore : System.Web.UI.UserControl
{
    public string ParameterIndicatorFrom
    {
        set { this.uxParameterIndicatorFrom.Text = value; }
        get { return this.uxParameterIndicatorFrom.Text; }
    }

    public string ParameterIndicatorTo
    {
        set { this.uxParameterIndicatorTo.Text = value; }
        get { return this.uxParameterIndicatorTo.Text; }
    }

    public string ParameterThreshold
    {
        set { this.uxParameterThreshold.Text = value; }
        get { return this.uxParameterThreshold.Text; }
    }

    public string ParameterThresholdLow
    {
        set { this.uxParameterThreshold.Text = value; }
        get { return this.uxParameterThreshold.Text; }
    }

    public string ParameterThresholdHigh
    {
        set { this.uxParameterThresholdHigh.Text = value; }
        get { return this.uxParameterThresholdHigh.Text; }
    }

    public string ParameterScore
    {
        set { this.uxParameterScore.Text = value; }
        get { return this.uxParameterScore.Text; }
    }

    public string ParameterThresholdTypeHigh
    {
        set { this.uxParameterThresholdTypeHigh.Value = value; }
        get { return this.uxParameterThresholdTypeHigh.Value; }
    }

    public string ParameterPrecision
    {
        set { this.uxParameterPrecision.Value = value; }
        get { return this.uxParameterPrecision.Value; }
    }

    public bool ParameterIndicatorNegative
    {
        set { this.uxParameterIndicatorNegative.Value = value ? "1" : "0"; }
        get { return this.uxParameterIndicatorNegative.Value == "1"; }
    }


    public string CommandSubmit
    {
        set
        {
            uxSubmit.CommandName = value;
        }
    }

    public bool IsAllowDecimalValue { get; set; }

    public int PrecisionValue { get; set; }
    private void Page_PreRender(object sender, System.EventArgs e)
    {
        if (uxParameterIsIndicatorNegative.Value == "1")
        {
            uxLabBeginFrom.Visible = true;
            uxLabEndFrom.Visible = true;
            uxLabBeginTo.Visible = true;
            uxLabEndTo.Visible = true;
            uxParameterIndicatorFrom.Style.Add("color", "Red");
            uxParameterIndicatorTo.Style.Add("color", "Red");
        }
        if (uxParameterIsNullIndicator.Value == "0")
        {
            uxParameterIndicatorFrom.Enabled = false;
            uxParameterIndicatorTo.Enabled = false;
        }


        uxParameterThreshold.Enabled = (uxParameterThresholdEnabled.Value == "1");
        if (uxParameterIsThresholdNegative.Value == "1" && uxParameterThreshold.Text != "N/A")
        {
            uxLabBeginThreshold.Visible = true;
            uxLabEndThreshold.Visible = true;
            uxParameterThreshold.Style.Add("color", "Red");

            uxLabBeginThresholdHigh.Visible = true;
            uxLabEndThresholdHigh.Visible = true;
            uxParameterThresholdHigh.Style.Add("color", "Red");

        }

        uxIsAllowDecimalValue.Value = IsAllowDecimalValue.ToString();
        uxPrecisionValue.Value = PrecisionValue.ToString();
        uxParameterIndicatorFrom.Attributes.Add("onkeypress", IsAllowDecimalValue ? "return SearchEnterOnDecimalTextbox(event,'" + uxSubmit.ClientID + "'," + PrecisionValue + ")" : "return SearchEnterOnTextbox(event,'" + uxSubmit.ClientID + "')");
        uxParameterIndicatorTo.Attributes.Add("onkeypress", IsAllowDecimalValue ? "return SearchEnterOnDecimalTextbox(event,'" + uxSubmit.ClientID + "'," + PrecisionValue + ")" : "return SearchEnterOnTextbox(event,'" + uxSubmit.ClientID + "')");

        uxParameterIndicatorFrom.Attributes.Add("onblur", IsAllowDecimalValue ? "checkDecimalInputCharacterScore(this," + PrecisionValue + ")" : " checkInputCharacterScore(this)");
        uxParameterIndicatorTo.Attributes.Add("onblur", IsAllowDecimalValue ? "checkDecimalInputCharacterScore(this," + PrecisionValue + ")" : " checkInputCharacterScore(this)");
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        uxParameterThreshold.Attributes.Add("onkeypress", "return SearchEnterOnTextbox(event,'" + uxSubmit.ClientID + "')");
        uxParameterThresholdHigh.Attributes.Add("onkeypress", "return SearchEnterOnTextbox(event,'" + uxSubmit.ClientID + "')");
        uxParameterScore.Attributes.Add("onkeypress", "return SearchEnterOnTextbox(event,'" + uxSubmit.ClientID + "')");

        uxParameterThreshold.Attributes.Add("onblur", " checkInputCharacterScore(this)");
        uxParameterThresholdHigh.Attributes.Add("onblur", " checkInputCharacterScore(this)");
        uxParameterScore.Attributes.Add("onblur", " checkInputCharacterScore(this)");
    }
}