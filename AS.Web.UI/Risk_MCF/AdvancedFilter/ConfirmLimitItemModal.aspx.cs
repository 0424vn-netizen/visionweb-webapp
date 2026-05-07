using System;

public partial class ConfirmLimitItemModal : NonReportPage
{
    public int LimitGenerateStatisticsReport
    {
        get
        {
            int result = 0;
            if (GeneralFuncsLib.GetDataOfExtendedSetting("LimitGenerateStatisticsReport") != null)
            {
                Int32.TryParse(GeneralFuncsLib.GetDataOfExtendedSetting("LimitGenerateStatisticsReport"), out result);
            }
            return result;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        PageType = SecurePageType.Modal;
        msgLimitItems.Text = string.Format(GetLocalResourceObject("msgLimitItems").ToString(), LimitGenerateStatisticsReport);
    }
}