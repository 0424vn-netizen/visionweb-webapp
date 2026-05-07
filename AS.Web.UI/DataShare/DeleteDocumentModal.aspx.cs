using System;

public partial class DeleteDocumentModal : NonReportPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        this.PageType = SecurePageType.Modal;
    }
}
