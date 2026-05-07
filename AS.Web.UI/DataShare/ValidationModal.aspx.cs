using System;

public partial class ValidationModal : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.PageType = SecurePageType.Modal;
    }
}
