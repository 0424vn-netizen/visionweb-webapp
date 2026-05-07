using System;

public partial class DeleteAssignmentModal : NonReportPage
{
    public string MessageDelete
    {
        get
        {
            if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["MessageDelete"]))
                return SecureQueryString["MessageDelete"];
            else
                return string.Empty;
        }
    }

    public bool IsDelete
    {
        get
        {
            if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["IsDelete"]))
                return bool.Parse(SecureQueryString["IsDelete"]);
            else
                return false;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        this.PageType = SecurePageType.Modal;
        ltlConfirm.Text = MessageDelete;
        if (!IsDelete)
        {
            uxOK.Visible = false;
            uxCancel.Text = GetLocalResourceObject("btnConfirm").ToString();
        }
    }
}
