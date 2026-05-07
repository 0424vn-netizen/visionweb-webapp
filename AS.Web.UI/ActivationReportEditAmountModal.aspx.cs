using AS.Common.DBManager;
using AS.Controls.Global;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ActivationReportEditAmountModal : ReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
        PageType = SecurePageType.Modal;
        if (!IsPostBack)
        {
            ltDolar.Text = SessionManager.CurrencySymbol;
            FilterParameterCollection _parames = new FilterParameterCollection();
            _parames.AddLoggedInUserReportingParams(true);
            DataTable dt = WebServices.SecurityServices.GetReports("spa_SEC_GetMinimumBatchAmount", _parames);
            var batchAmount = AS.Common.Formater.FormatData.FormatCurrency(dt.Rows[0]["MinimumBatchAmount"], SessionManager.CurrencyFortmat);
            var amount = AS.Common.Formater.FormatData.FormatCurrency(dt.Rows[0]["MinimumBatchAmount"], SessionManager.CurrencyFortmat);
            uxAmount.Text = amount.Replace(SessionManager.CurrencySymbol, "").Replace(",", "");
        }
        //46652 - AW - Multi-Currency Transaction Display
        ltDolar.Text = ltDolar.Text.ToCurrencySymbol();
    }
    protected void uxApply_Click(object sender, EventArgs e)
    {
        if (ValidateInput())
        {
            double amount = 0;
            double.TryParse(uxAmount.Text, out amount);

            FilterParameterCollection _parames = new FilterParameterCollection();
            _parames.AddLoggedInUserReportingParams(true);
            _parames.Add(new FilterParameter("@MinimumBatchAmount", amount, DbType.Double));
            FilterParameterCollection _paramesOut = new FilterParameterCollection();
            WebServices.CsReportServices.ExecuteNonQueryCommand("spa_SEC_UpdateMinimumBatchAmount", _parames, out _paramesOut);
            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "doneSubmit", "ClosePopupModal();", true);
        }
    }

    private bool ValidateInput()
    {
        if (uxAmount.Text.IsNullOrEmpty())
        {
            ShowMessageBox(Resources.ValMsg.Required, uxAmountMsg);
            return false;
        }
        if (uxAmount.Text.Length > 11)
        {
            ShowMessageBox(string.Format(Resources.ValMsg.MaxLength, 11), uxAmountMsg);
            return false;
        }
        return true;
    }

    private void ShowMessageBox(string message, ValidatorMessage msgctrl)
    {
        ShowServerErrorMessage(msgctrl, message);
    }
}