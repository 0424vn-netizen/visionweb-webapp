using System;

public partial class ConfirmUserGroupModal : NonReportPage
{
    public string DocumentList
    {
        get
        {
            if (IsSecureQueryString && !string.IsNullOrEmpty(SecureQueryString["DocumentList"]))
                return SecureQueryString["DocumentList"];
            else
                return string.Empty;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.PageType = SecurePageType.Modal;
        if (string.IsNullOrEmpty(DocumentList)) return;

        LoadData();
    }

    private void LoadData()
    {
        string[] documents = DocumentList.Split(',');
        string strPartial = string.Empty;
        string strFull = string.Empty;
        int i = 0;
        foreach (var item in documents)
        {
            if (string.IsNullOrEmpty(item)) continue;
            i++;
            strFull += string.Format("<li>{0}</li>", item);
            if (i <= 3)
            {
                strPartial += string.Format("<li>{0}</li>", item);
            }
            if (i == 4)
                strPartial += string.Format("<span>... </span><a href='#' onclick='loadMore()'>{0}</a>", GetLocalResourceObject("loadMore"));
        }

        lstDocument.InnerHtml = strPartial;
        lstDocumentFull.InnerHtml = strFull;
    }
}
