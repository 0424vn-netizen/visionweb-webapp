using AS.Controls.Pages;

[PagePermission("TimeSettings")]
public partial class TimeSetting : NonReportPage
{
    protected override void PageInitialize()
    {
        base.PageInitialize();
        this.IsSecureCSRF = true;
    }
}