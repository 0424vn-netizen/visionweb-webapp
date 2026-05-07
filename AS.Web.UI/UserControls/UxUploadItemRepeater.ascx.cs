using AS.Common.DBManager;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI;
using Telerik.Web.UI;

public partial class UserControls_UxUploadItemRepeater : GlobalUserControl
{
    #region Properties

    public delegate void RemoveEventHandler(object sender, UploadRemoveEventArgs removeEventArgs);
    public event RemoveEventHandler RemoveItem;
    public string ToValidateJs { set; get; }
    public bool IsBindData
    {
        get
        {
            if (ViewState["IsBindData"] == null)
                return false;
            return Convert.ToBoolean(ViewState["IsBindData"]);
        }
        set { ViewState["IsBindData"] = value; }
    }

    #endregion

    #region Privates

    private void CreateJSValidate()
    {
        uxValidator.ValidationFunction = uxValidator.ClientID + "_valInput";
        StringBuilder sb = new StringBuilder();
        StringWriter sw = new StringWriter(sb);
        HtmlTextWriter hw = new HtmlTextWriter(sw);
        uxValidator.RenderControl(hw);
        ToValidateJs = string.Format("{0};isItemRepeaterValid={1}() && isItemRepeaterValid; if(isItemRepeaterValid==false) return false;", sb, uxValidator.ValidationFunction);
        ToValidateJs = string.Empty;
    }

    #endregion

    #region Events

    public string FileType
    {
        get;
        set;
    }
    public string FileSize
    {
        get;
        set;
    }

    public string IdFileClient
    {
        get;
        set;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (IsBindData)
            return;

        IsBindData = true;
        CreateJSValidate();
    }

    public bool IsValid()
    {
        return Page.IsValid;
    }

    protected void btnDelete_OnClick(object sender, EventArgs e)
    {
        if (RemoveItem != null)
        {
            var eventArgs = new UploadRemoveEventArgs();
            eventArgs.IdControl = Convert.ToInt32(hdfId.Value);
            RemoveItem(sender, eventArgs);
        }
    }

    #endregion
    protected void txtNote_PreRender(object sender, EventArgs e)
    {
        txtNote.EmptyMessage = GetLocalResourceObject("phDescription").ToString();
    }

    public string FormatFize()
    {
        string result = string.Empty;
        double temp = 0;

        double.TryParse(FileSize, out temp);
        result = temp < 1024 ? temp.ToString() + "KB" :
            ((float)temp / 1024).ToString("0.00") + "MB";

        return result;

    }
}
