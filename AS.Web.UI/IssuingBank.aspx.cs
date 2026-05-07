using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AS.Controls.Pages;
using System.Data;
using AS.Common;
using System.Text.RegularExpressions;
[PagePermission("IssuingBankRpt,MSIssuingBankRpt")]
public partial class IssuingBank : NonReportPage
{
    enum PostBackAction
    {
        SearchReport,
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsIntruderDetected) return;
    }

    protected void uxSearchReport_Click(object sender, EventArgs e)
    {
        var text = uxBinNumber.Text.Trim();
        if ((text.Length != 6 && text.Length != 8) || string.IsNullOrEmpty(text) || !Regex.IsMatch(text, @"^\d+$"))
        {
            Literal1.Visible = true;
            return;
        }
        Literal1.Visible = false;
        OnPostBackActions(PostBackAction.SearchReport);
    }

    protected override void OnPostBackActions(Enum type, object sender)
    {
        switch ((PostBackAction)type)
        {
            case PostBackAction.SearchReport:
                uxIssuingBank.Visible = true;
                var binNumer = uxBinNumber.Text.Trim();
               
                uxIssuingBank.BinNumber = Int64.Parse(binNumer);
                uxIssuingBank.BinNumberText = VeraCodeSolution.DoVeraCode(binNumer);
                uxIssuingBank.Search_Click();
                break;
        }
    }
}
