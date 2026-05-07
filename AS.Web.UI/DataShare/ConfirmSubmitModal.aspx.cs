using System;

public partial class ConfirmSubmitModal : NonReportPage
{
    public string BuildMsg()
    {
        string msg = string.Empty;
        if (!string.IsNullOrEmpty(Request.Params["IsUpdate"]) && Request.Params["IsUpdate"].ToBoolean())
        {
            msg = GetLocalResourceObject("ltlConfirmUpdate").ToString();
        }
        else
        {
            if (!string.IsNullOrEmpty(Request.Params["NumberOfFile"])
                && int.Parse(Request.Params["NumberOfFile"].ToString()) > 1)
            {
                msg = GetLocalResourceObject("ltlConfirmResource.Text").ToString();
            }
            else {
                msg = GetLocalResourceObject("ltlConfirmCreateSingle").ToString();
            }
        }

        return msg;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.PageType = SecurePageType.Modal;
     
        ltlConfirm.Text = BuildMsg();
    }
}
